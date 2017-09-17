using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using IntranetTG;
using IntranetTG.Functions;
using IntranetTG.Objects;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using PdfSharp;
using PdfSharp.Drawing;
using TGTheraS4.Objects;
using TheraS5.Objects;
using TheraS5.Services;
using Application = System.Windows.Forms.Application;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GeneralService _generalService;
        private List<Contacts> allContacts = new List<Contacts>();

        private List<Service> AllHouses = new List<Service>();
        public List<Klienten_Berichte> Berichte = new List<Klienten_Berichte>();
        private SQLCommands c;

        private readonly bool cmbKMGset;
        private string colhd = "";
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        private bool contactMode;
        private bool contactSearch;
        private string contID = "";
        private bool contSearch1;
        private bool contSearch2;
        private bool contSearch3;
        public string cs = "DbLinqProvider=Sqlite;Data Source=converted.sqlite";

        public string current_client_medi = "";
        private string[] databaseString;
        private DateTime dataTime;
        public DataContext db;
        public List<Document> doclist = new List<Document>();

        public bool edit_medi;

        private bool firsttimer = true;
        private bool funcset = false;
        public Funktionszugehoerigkeit funcz;

        private int gload = 0;
        private bool gmailFirst = true;
        private List<WorkingTime> holidaydata;

        public bool isloggedin;
        public bool isOnline;
        private bool kbrekstopp;

        private int load = 0;

        private List<Medicaments> m;

        private string medihelpname;

        public string[] medis;
        public MySqlConnection myConnection;


        private bool newUser;

        public List<string> nonWorkingDays = new List<string>();

        public List<string> obsoleteMedis;
        public bool offline;
        private List<PadMas> pad = new List<PadMas>();
        public string pdf_haben;
        public string pdf_Krank;

        public string pdf_soll;
        public string pdf_uber;
        public string pdf_uberganz;
        public string pdf_url;
        private DataGridRow rowBeingEdited;

        private string selectedContact; // zwischenspeicherung des sozialarbeiters 
        private List<Service> SelectedHouses = new List<Service>();

        public bool set;
        private List<Contacts> sozialarbeiter = new List<Contacts>();
        private DataTable table;
        private List<WorkingTime> time;
        private User u;
        public List<User> userlist = new List<User>();
        private string version;
        private int wahl;
        private bool webmailFirst = true;
        public List<WikiDoc> wiki = new List<WikiDoc>();
        private bool wikiLoaded = false;

        /*
        Table<clients> db_clients = new Table<clients>();
        Table<clientsmedications> db_clientsmedications = new Table<clientsmedications>();
        Table<clientstoservices> db_clientstoservices = new Table<clientstoservices>();
        Table<medicaments> db_medicamtens = new Table<medicaments>();
        Table<services> db_services = new Table<services>();
        Table<users> db_users = new Table<users>();
        Table<userstoservices> db_userstoservices = new Table<userstoservices>();
        Table<clientsdailydocs> db_clientsdailydocs = new Table<clientsdailydocs>();
        */
        public MainWindow()
        {
            InitializeComponent();

            _generalService = new GeneralService("0", "0", "0"); // SystemID is currently Hardcoded...

            isOnline = _generalService.NetworkCheck();

            if (Width > SystemParameters.PrimaryScreenWidth ||
                Height > SystemParameters.PrimaryScreenHeight)
            {
                Width = SystemParameters.PrimaryScreenWidth;
                Height = SystemParameters.PrimaryScreenHeight;
                MessageBox.Show(
                    "Achtung! \nIhre Bildschirmauflösung entspricht nicht den empfohlenen Mindestanforderungen von \n \n1600*900\n Ich werde die Fenstergröße für Sie anpassen"
                    + "Dies kann jedoch Probleme bei der Skalierung hervorrufen", "Bildschirmauflösung",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            DeleteOldTempFiles();


            cmb_Month.SelectedIndex = DateTime.Now.Month - 1;
            cmb_Year.SelectedIndex = DateTime.Now.Year - 2000;


            cmbKMGMonth.SelectedIndex = DateTime.Now.Month - 1;
            cmbKMGYear.SelectedIndex = DateTime.Now.Year - 2000;

            cmbKMGset = true;

            var con = new SQLiteConnection(cs);
            con.Open();

            db = new DataContext(con);
        }

        public int SelectedIndex
        {
            get => SelectedIndex1;
            set => SelectedIndex1 = value;
        }

        public int SelectedIndex1 { get; set; }


        private void fillShoutBox(List<Shout> shouts)
        {
            FillColumnInTable(new string[] { "Shoutbox", "Erstellt", "Name"});

            foreach (var shout in shouts)
            {
                DataRow row = table.NewRow();
                row[0] = shout.Message;
                row[1] = shout.Created.ToShortDateString();
                row[2] = shout.CreatedUserFirstname + " " + shout.CreatedUserLastname;
                table.Rows.Add(row);
            }

            var dataset = new DataSet();

            dataset.Tables.Add(table);

            dgShouts.ItemsSource = dataset.Tables[0].DefaultView;
            dgShouts.DataContext = dataset.Tables[0];
        }

        private void AfterLogin()
        {
            if (isOnline)
            {
                AutoUpdate();

                /*string[] shouts = {"Shoutbox", "Erstellt", "Name"};
                FillMoreData(dgShouts, c.getShouts(), shouts);*/
                fillShoutBox(c.GetShouts());

                string[] names = {"Id", "Name", "Bezeichnung", "Prozent"};

                txtUser.Focus();
                tabHome.IsSelected = true;
            }
            else
            {
                offline = true;
                tabHome.IsEnabled = false;
                tabPass.IsEnabled = false;
                tabKassabuch.IsEnabled = false;
                tabKontakte.IsEnabled = false;
                tabVitaldaten.IsEnabled = false;
                tabAppointments.IsEnabled = false;
                tabTasks.IsEnabled = false;
                tabWorkingTime.IsEnabled = false;
                tabKmGeld.IsEnabled = false;
                tabAdmin.IsEnabled = false;
                tabWiki.IsEnabled = false;

                tabDokumentation.IsSelected = true;
            }
        }


        public void DeleteOldTempFiles()
        {
            var fileList = Directory.GetFiles(".\\", @"Temp*.*");
            foreach (var file in fileList)
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                }
            }
        }


        public void AutoUpdate() //#update
        {
            var ftp = new FtpHandler();
            ftp.DownloadFile("logo.tg", "logo.tg");
        }

        public void SetAUsercmb()
        {
            try
            {
                var items = c.getWorkingtimeUsers().Split('%');
                User tmp;
                string[] line;
                var index = 0;
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i] != "")
                    {
                        line = items[i].Split('$');
                        tmp = new User(line[0], line[1], line[2], Convert.ToInt32(line[3]));

                        cmbAdminUsers.Items.Add(tmp.Firstname + " " + tmp.Lastname);
                        userlist.Add(tmp);
                        if (items[i].Equals(u.Username))
                        {
                            index = i;
                        }
                        cmbAdminUsers.Text = "";
                    }
                }
                cmbAdminUsers.SelectedIndex = index + 1;
            }
            catch
            {
            }
        }


        public void SetAdmintoolWorkingTime()
        {
            userlist = new List<User>();
            if (u.IsAdmin == false)
            {
                cmbworkingTimeUser.Visibility = Visibility.Hidden;

                cmbworkingTimeUser.Items.Add("Alle User");
                var items = c.getWorkingtimeUsers().Split('%');
                User tmp;
                string[] line;
                var index = 0;
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i] != "")
                    {
                        line = items[i].Split('$');
                        tmp = new User(line[0], line[1], line[2], Convert.ToInt32(line[3]));
                        cmbworkingTimeUser.Items.Add(tmp.Firstname + " " + tmp.Lastname);
                        userlist.Add(tmp);
                        if (line[0].Trim().Equals(u.Username.Trim()))
                        {
                            index = i;
                        }
                    }
                }
                cmbworkingTimeUser.SelectedIndex = index + 1;
            }
            else
            {
                cmbworkingTimeUser.Items.Add("Alle User");
                var items = c.getWorkingtimeUsers().Split('%');
                User tmp;
                string[] line;
                var index = 0;
                for (var i = 0; i < items.Length; i++)
                {
                    if (items[i] != "")
                    {
                        line = items[i].Split('$');
                        tmp = new User(line[0], line[1], line[2], Convert.ToInt32(line[3]));
                        cmbworkingTimeUser.Items.Add(tmp.Firstname + " " + tmp.Lastname);
                        userlist.Add(tmp);
                        if (line[0].Trim().Equals(u.Username.Trim()))
                        {
                            index = i;
                        }
                    }
                }
                cmbworkingTimeUser.SelectedIndex = index + 1;
            }
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            Login();

            AfterLogin();

            SetAdmintoolWorkingTime();
            setdgvWorkingTime();
            setdgvMedication();
            SetAUsercmb();
            cmbAdminUsers.Text = "";

            if (u.IsAdmin)
            {
                btnUrlaubBest.Visibility = Visibility.Visible;
                btnUrlaubAbl.Visibility = Visibility.Visible;
                chk_allow.Visibility = Visibility.Hidden;
            }
            else
            {
                btnPDFExport.Visibility = Visibility.Hidden;
                chk_allow.Visibility = Visibility.Hidden;
                btnAllUsersPDF.Visibility = Visibility.Hidden;
            }


            hideContact();
            deleteContactText();
            allContacts = c.fillContacts();
            dgvContact.ItemsSource = allContacts;
            fillContactCmb();

            lblContSearch.Visibility = Visibility.Hidden;
            btnContSearch1Minus.Visibility = Visibility.Hidden;
            btnContSearch2Plus.Visibility = Visibility.Hidden;
            btnContSearch2Minus.Visibility = Visibility.Hidden;
            btnContSearch3Plus.Visibility = Visibility.Hidden;
            btnContSearch3Minus.Visibility = Visibility.Hidden;
            cmbContSearch1.Visibility = Visibility.Hidden;
            cmbContSearch2.Visibility = Visibility.Hidden;
            cmbContSearch3.Visibility = Visibility.Hidden;
            txtContSearch1.Visibility = Visibility.Hidden;
            txtContSearch2.Visibility = Visibility.Hidden;
            txtContSearch3.Visibility = Visibility.Hidden;
        }

        private void txtPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                Login();

                AfterLogin();

                SetAdmintoolWorkingTime();
                setdgvWorkingTime();
                setdgvMedication();
                SetAUsercmb();
                cmbAdminUsers.Text = "";

                if (u.IsAdmin)
                {
                    btnUrlaubBest.Visibility = Visibility.Visible;
                    btnUrlaubAbl.Visibility = Visibility.Visible;
                    chk_allow.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnPDFExport.Visibility = Visibility.Hidden;
                    chk_allow.Visibility = Visibility.Hidden;
                    btnAllUsersPDF.Visibility = Visibility.Hidden;
                }
            }
        }


        private void Login()
        {
            _generalService.Password = txtPW.Password;
            _generalService.UserName = txtUser.Text;

            c = new SQLCommands(_generalService.GetMySqlConnectionInformation());

            u = new User(txtUser.Text, txtPW.Password, c.getPW(c.getUserID(txtUser.Text)));

            if (Functions.checkUser(u))
            {
                isloggedin = true;
                Background = Brushes.White;
                u = Functions.ActualUserFromList;

                u.Id = c.getUserID(u.Username);
                u.Services = c.userToService(u.Id);

                FillKidsIntoCombo();
                btnLogin.Visibility = Visibility.Hidden;
                tabMain.Visibility = Visibility.Visible;
                imgLogo.Visibility = Visibility.Hidden;
                Functions.EmployeeList = FillFullUser(c.getEmployees());
                Functions.ProjectList = FillProject(c.getProjects());
                Functions.ServiceList = FillService(c.getServices());
                u.IsAdmin = c.isAdmin(u.Id);
                refreshNewestDokus();
                refreshAllTasks();
                refreshAllInstructions();
                RefreshService();
                refreshKmG(u.Id);
                fillcmbUserKmG(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

                if (!u.IsAdmin)
                {
                    btnNewInstruction.Visibility = Visibility.Hidden; // instruction tab
                    btnNS.Visibility = Visibility.Hidden;
                    btnNK.Visibility = Visibility.Hidden;
                    tabAdmin.Visibility = Visibility.Hidden;
                    cmbUserKmG.Visibility = Visibility.Hidden;
                    btnWikiUp.Visibility = Visibility.Hidden;
                    btnWikiReplace.Visibility = Visibility.Hidden;
                    btnWikiDel.Visibility = Visibility.Hidden;
                    btnWikiReNam.Visibility = Visibility.Hidden;
                }
                if (u.IsAdmin)
                {
                    btnUrlaubBest.Visibility = Visibility.Visible;
                    btnUrlaubAbl.Visibility = Visibility.Visible;
                    chk_allow.Visibility = Visibility.Hidden;
                    cmbworkingTimeUser.Visibility = Visibility.Visible;
                    btnPDFExport.Visibility = Visibility.Visible;
                    btnAllUsersPDF.Visibility = Visibility.Visible;
                    btnWikiUp.Visibility = Visibility.Visible;
                    btnWikiReplace.Visibility = Visibility.Visible;
                    btnWikiDel.Visibility = Visibility.Visible;
                    btnWikiReNam.Visibility = Visibility.Visible;
                }
                else
                {
                    btnPDFExport.Visibility = Visibility.Hidden;
                    chk_allow.Visibility = Visibility.Hidden;
                    btnAllUsersPDF.Visibility = Visibility.Hidden;
                    btnAllUsersPDF.Visibility = Visibility.Hidden;
                    btnWikiDel.Visibility = Visibility.Hidden;
                    btnWikiUp.Visibility = Visibility.Hidden;
                    btnWikiReNam.Visibility = Visibility.Hidden;
                    btnWikiReplace.Visibility = Visibility.Hidden;
                }

                var arbeit = c.getWorkingDays(u.Id);

                if (arbeit == "5")
                {
                    u.WorkingTimeType = 1;
                }
                else
                {
                    u.WorkingTimeType = 2;
                }
                var haeuser = c.getKBHaeuser();
                cmbBxKBHaus.ItemsSource = haeuser;
                cmbBxKBHaus.DisplayMemberPath = "name";
                cmbBxKBHaus.SelectedValuePath = "id";
                dtPckrKBDat.SelectedDate = DateTime.Now;
                dtPckrKBFiltVon.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtPckrKBFiltBis.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                fillArt();
                loadWiki();
            }

            updatecmbUserKmG(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            c.setlogin();
        }


        private void loadWiki()
        {
            var docs = c.GetWikiDocs();
            wiki = new List<WikiDoc>();
            foreach (var doc in docs)
            {
                wiki.Add(doc);
            }
            dgvWikiDocs.ItemsSource = wiki;
        }

        private List<Project> FillProject(string data)
        {
            var empList = new List<Project>();

            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length - 1; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                var emp = new Project();
                emp.Id = databaseRow[0];
                emp.Name = databaseRow[1];
                empList.Add(emp);
            }
            return empList;
        }

        private List<Service> FillService(string data)
        {
            var empList = new List<Service>();

            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length - 1; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                var emp = new Service();
                emp.Id = databaseRow[0];
                emp.Name = databaseRow[1];
                empList.Add(emp);
            }
            return empList;
        }

        private List<Employee> FillFullUser(string data)
        {
            var empList = new List<Employee>();

            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                var emp = new Employee();
                emp.Id = databaseRow[0];
                emp.FullName = databaseRow[1];
                empList.Add(emp);
            }
            return empList;
        }

        private void FillKidsIntoCombo()
        {
            cmbKlient.Items.Clear();
            cmbTg.Items.Clear();
            cmbGuG.Items.Clear();
            cmbPad.Items.Clear();
            foreach (var service in u.Services)
            {
                var temp = new List<string>();

                foreach (var cc in c.WgToClients(service, SQLCommands.ClientFilter.NotLeft))
                {
                    temp.Add(cc.Key + "$" + cc.Value);
                }

                var temp2 = temp;

                var temp3 = new List<string>();

                foreach (var cc in c.WgToClients(service, SQLCommands.ClientFilter.Left))
                {
                    temp3.Add(cc.Key + "$" + cc.Value);
                }

                foreach (var name in temp)
                {
                    var namen = name.Split('$');
                    if (namen.Length > 1)
                    {
                        if (namen[0].Contains(' '))
                        {
                            namen[0] = namen[0].Replace(' ', ',');
                        }
                        if (namen[1].Contains(' '))
                        {
                            namen[1] = namen[1].Replace(' ', ',');
                        }
                        if (!cmbKlient.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbKlient.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbTg.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbTg.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbGuG.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbGuG.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbFVGClient.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbFVGClient.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbMedicClient.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbMedicClient.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbMA.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbMA.Items.Add(namen[0] + " " + namen[1]);
                        }
                        cmbPad.Items.Add(namen[0] + " " + namen[1]);
                    }
                }
                //Kinder die schon verlassen haben
                foreach (var name in temp2)
                {
                    var namen = name.Split('$');
                    if (namen.Length > 1)
                    {
                        if (namen[0].Contains(' '))
                        {
                            namen[0] = namen[0].Replace(' ', ',');
                        }
                        if (namen[1].Contains(' '))
                        {
                            namen[1] = namen[1].Replace(' ', ',');
                        }
                        if (!cmbKlientAuswaehlen.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbKlientAuswaehlen.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmb_Klient_Doc.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmb_Klient_Doc.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmb_Klient_Foto.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmb_Klient_Foto.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbFVGClient.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbFVGClient.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbTg.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbTg.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbGuG.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbGuG.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbMedicClient.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbMedicClient.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmbMA.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbMA.Items.Add(namen[0] + " " + namen[1]);
                        }
                        cmbPad.Items.Add(namen[0] + " " + namen[1]);
                    }
                }
                foreach (var name in temp3)
                {
                    var namen = name.Split('$');
                    if (namen.Length > 1)
                    {
                        if (namen[0].Contains(' '))
                        {
                            namen[0] = namen[0].Replace(' ', ',');
                        }
                        if (namen[1].Contains(' '))
                        {
                            namen[1] = namen[1].Replace(' ', ',');
                        }

                        if (!cmbKlientArchivAuswaehlen.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmbKlientArchivAuswaehlen.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmb_Klient_Doc.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmb_Klient_Doc.Items.Add(namen[0] + " " + namen[1]);
                        }

                        if (!cmb_Klient_Foto.Items.Contains(namen[0] + " " + namen[1]))
                        {
                            cmb_Klient_Foto.Items.Add(namen[0] + " " + namen[1]);
                        }
                    }
                }
            }
        }

        private void FillColumnInTable(string[] columnnames)
        {
            table = new DataTable();

            var i = 0;
            foreach (var column in columnnames)
            {
                table.Columns.Add(column);
                table.Columns[i].ColumnName = column;
                i++;
            }
        }

        /// <summary>
        ///     Base fill more data method
        /// </summary>
        /// <param name="wg">base fill method for more data</param>
        /// <returns>All column</returns>
        private void FillMoreData(DataGrid dgv, string data, string[] columnnames)
        {
            DataRow row = null;
            FillColumnInTable(columnnames);

            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                for (var u = 0; u < databaseRow.Length; u++)
                {
                    if (u < columnnames.Length)
                    {
                        if (u == 0)
                        {
                            row = table.NewRow();
                        }
                        if (DateTime.TryParse(databaseRow[u], out dataTime))
                        {
                            row[u] = dataTime.ToShortDateString();
                        }
                        else
                        {
                            row[u] = databaseRow[u];
                        }
                    }
                }

                if (row != null)
                {
                    table.Rows.Add(row);
                }
            }
            var dataset = new DataSet();

            dataset.Tables.Add(table);

            dgv.ItemsSource = dataset.Tables[0].DefaultView;
            dgv.DataContext = dataset.Tables[0];

            if (table.Columns.Count > 0)
            {
                if (table.Columns[0].ColumnName == "Id")
                {
                    // dgv.Columns[0].Visibility = Visibility.Hidden;
                }
            }
        }

        //for all grids with rowedit. rowedit implementieren
        private void FillGridUserControl(GridUserControl usc, string data, string[] columnnames)
        {
            var table = new DataTable();
            DataRow row = null;
            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length - 1; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                for (var u = 0; u < databaseRow.Length; u++)
                {
                    if (u < columnnames.Length)
                    {
                        if (i == 0)
                        {
                            table.Columns.Add(columnnames[u]);
                            table.Columns[u].ColumnName = columnnames[u];
                        }
                        else
                        {
                            if (u == 0)
                            {
                                row = table.NewRow();
                            }
                            if (DateTime.TryParse(databaseRow[u], out dataTime))
                            {
                                row[u] = dataTime.ToShortDateString();
                            }
                            else
                            {
                                row[u] = databaseRow[u];
                            }
                        }
                    }
                }

                if (row != null)
                {
                    table.Rows.Add(row);
                }
            }
            var dataset = new DataSet();

            dataset.Tables.Add(table);
            usc.dgEdit.ItemsSource = dataset.Tables[0].DefaultView;
            usc.dgEdit.DataContext = dataset.Tables[0];
        }

        //for all grids with editdialog
        private void FillEditUserControl(EditUserControl usc, string data, string[] columnnames)
        {
            var table = new DataTable();
            DataRow row = null;
            databaseString = data.Split('%');

            for (var i = 0; i < databaseString.Length - 1; i++)
            {
                var databaseRow = databaseString[i].Split('$');

                for (var u = 0; u < databaseRow.Length; u++)
                {
                    if (u < columnnames.Length)
                    {
                        if (i == 0)
                        {
                            table.Columns.Add(columnnames[u]);
                            table.Columns[u].ColumnName = columnnames[u];
                        }
                        else
                        {
                            if (u == 0)
                            {
                                row = table.NewRow();
                            }
                            if (DateTime.TryParse(databaseRow[u], out dataTime))
                            {
                                row[u] = dataTime.ToShortDateString();
                            }
                            else
                            {
                                row[u] = databaseRow[u];
                            }
                        }
                    }
                }

                if (row != null)
                {
                    table.Rows.Add(row);
                }
            }
            var dataset = new DataSet();

            dataset.Tables.Add(table);
            usc.dgEdit.ItemsSource = dataset.Tables[0].DefaultView;
            usc.dgEdit.DataContext = dataset.Tables[0];
        }

        private void tabMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabMain.SelectedIndex == 0)
            {
                /*string[] shouts = {"Shoutbox", "Erstellt", "Name"};
                FillMoreData(dgShouts, c.getShouts(), shouts);*/
                fillShoutBox(c.GetShouts());
            }
            if (tabMain.SelectedIndex == 6)
            {
                //Zeiterfassungstab
            }
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtDokuKoerperlichAnzeige.Text = "";
                txtDokuSchulischAnzeige.Text = "";
                txtDokuPsychischAnzeige.Text = "";
                txtDokuAußenkontaktAnzeige.Text = "";
                txtDokuPflichteAnzeige.Text = "";
                txtDokuCreatedUser.Text = "";
                if ((bool) !chkMultiday.IsChecked)
                {
                    if (dateDoku.ToString() != "" && cmbKlient.SelectedValue.ToString() != null)
                    {
                        fillMedi(cmbKlient.SelectedValue.ToString(), (DateTime) dateDoku.SelectedDate);
                        var currDoku = c.getDoku(cmbKlient.SelectedValue.ToString(),
                            (DateTime) dateDoku.SelectedDate);
                        var temps = currDoku.Split('%');
                        var temp = currDoku.Split('$');
                        if (temp.Length > 5)
                        {
                            if (temps.Length <= 2)
                            {
                                txtDokuKoerperlichAnzeige.Text = temp[0];
                                txtDokuSchulischAnzeige.Text = temp[1];
                                txtDokuPsychischAnzeige.Text = temp[2];
                                txtDokuAußenkontaktAnzeige.Text = temp[3];
                                txtDokuPflichteAnzeige.Text = temp[4];
                                txtDokuCreatedUser.Text = c.getNameByID(temp[5], false);
                                txtDokuAußenkontaktAnzeige.IsReadOnly = true;
                                txtDokuKoerperlichAnzeige.IsReadOnly = true;
                                txtDokuPflichteAnzeige.IsReadOnly = true;
                                txtDokuPsychischAnzeige.IsReadOnly = true;
                                txtDokuSchulischAnzeige.IsReadOnly = true;
                                txtDokuAußenkontakt.IsEnabled = true;
                                txtDokuKoerperlich.IsEnabled = true;
                                txtDokuPflichte.IsEnabled = true;
                                txtDokuPsychisch.IsEnabled = true;
                                txtDokuSchulisch.IsEnabled = true;
                            }
                            else
                            {
                                bool[] firsts = {true, true, true, true, true};
                                for (var i = 0; i < temps.Length; i++)
                                {
                                    if (temps[i] != "")
                                    {
                                        var temp_doku = temps[i].Split('$');
                                        if (i == 0)
                                        {
                                            txtDokuCreatedUser.Text = c.getNameByID(temp_doku[5], false);
                                        }

                                        if (temp_doku[0] != "")
                                        {
                                            if (!firsts[0])
                                            {
                                                txtDokuKoerperlichAnzeige.Text =
                                                    txtDokuKoerperlichAnzeige.Text + "\r\n\r\n";
                                            }
                                            txtDokuKoerperlichAnzeige.Text =
                                                txtDokuKoerperlichAnzeige.Text + temp_doku[0];
                                            firsts[0] = false;
                                        }
                                        if (temp_doku[1] != "")
                                        {
                                            if (!firsts[1])
                                            {
                                                txtDokuSchulischAnzeige.Text =
                                                    txtDokuSchulischAnzeige.Text + "\r\n\r\n";
                                            }
                                            txtDokuSchulischAnzeige.Text = txtDokuSchulischAnzeige.Text + temp_doku[1];
                                            firsts[1] = false;
                                        }
                                        if (temp_doku[2] != "")
                                        {
                                            if (!firsts[2])
                                            {
                                                txtDokuPsychischAnzeige.Text =
                                                    txtDokuPsychischAnzeige.Text + "\r\n\r\n";
                                            }
                                            txtDokuPsychischAnzeige.Text = txtDokuPsychischAnzeige.Text + temp_doku[2];
                                            firsts[2] = false;
                                        }
                                        if (temp_doku[3] != "")
                                        {
                                            if (!firsts[3])
                                            {
                                                txtDokuAußenkontaktAnzeige.Text =
                                                    txtDokuAußenkontaktAnzeige.Text + "\r\n\r\n";
                                            }
                                            txtDokuAußenkontaktAnzeige.Text =
                                                txtDokuAußenkontaktAnzeige.Text + temp_doku[3];
                                            firsts[3] = false;
                                        }
                                        if (temp_doku[4] != "")
                                        {
                                            if (!firsts[4])
                                            {
                                                txtDokuPflichteAnzeige.Text = txtDokuPflichteAnzeige.Text + "\r\n\r\n";
                                            }
                                            txtDokuPflichteAnzeige.Text = txtDokuPflichteAnzeige.Text + temp_doku[4];
                                            firsts[4] = false;
                                        }
                                    }
                                }

                                txtDokuAußenkontaktAnzeige.IsReadOnly = true;
                                txtDokuKoerperlichAnzeige.IsReadOnly = true;
                                txtDokuPflichteAnzeige.IsReadOnly = true;
                                txtDokuPsychischAnzeige.IsReadOnly = true;
                                txtDokuSchulischAnzeige.IsReadOnly = true;
                                txtDokuAußenkontakt.IsEnabled = true;
                                txtDokuKoerperlich.IsEnabled = true;
                                txtDokuPflichte.IsEnabled = true;
                                txtDokuPsychisch.IsEnabled = true;
                                txtDokuSchulisch.IsEnabled = true;
                            }
                        }
                        else
                        {
                            txtDokuAußenkontaktAnzeige.Text = "";
                            txtDokuKoerperlichAnzeige.Text = "";
                            txtDokuPflichteAnzeige.Text = "";
                            txtDokuPsychischAnzeige.Text = "";
                            txtDokuSchulischAnzeige.Text = "";
                            txtDokuCreatedUser.Text = "";
                            txtDokuAußenkontaktAnzeige.IsReadOnly = true;
                            txtDokuKoerperlichAnzeige.IsReadOnly = true;
                            txtDokuPflichteAnzeige.IsReadOnly = true;
                            txtDokuPsychischAnzeige.IsReadOnly = true;
                            txtDokuSchulischAnzeige.IsReadOnly = true;
                            txtDokuAußenkontakt.IsEnabled = true;
                            txtDokuKoerperlich.IsEnabled = true;
                            txtDokuPflichte.IsEnabled = true;
                            txtDokuPsychisch.IsEnabled = true;
                            txtDokuSchulisch.IsEnabled = true;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void fillMedi(string p, DateTime dt)
        {
            m = new List<Medicaments>();

            string[] fillings;

            if (c.medicationIsConfirmed(p, dt))
            {
                fillings = c.getMedicationForClient(p, dt, false, true).Split('%');
            }
            else
            {
                fillings = c.getMedicationForClient(p, dt, false, false).Split('%');
            }

            var fillings2 = c.getMedicationConfirmations(p, dt).Split('%');
            foreach (var s in fillings)
            {
                var temp = s.Split('$');
                if (temp.Length > 5)
                {
                    var mo = "";
                    var mi = "";
                    var ev = "";
                    var ni = "";
                    var crea = "";
                    string cancelled = null;
                    foreach (var s2 in fillings2)
                    {
                        var temp2 = s2.Split('$');
                        if (temp2.Length > 6)
                        {
                            if (temp2[0] == temp[5])
                            {
                                mo = temp2[2];
                                mi = temp2[3];
                                ev = temp2[4];
                                ni = temp2[5];
                                crea = temp2[6];
                                cancelled = temp2[7];
                            }
                        }
                    }

                    if (crea != "")
                    {
                        if (temp[1] == "0" && temp[2] == "0" && temp[3] == "0" && temp[4] == "0")
                        {
                            m.Add(new Medicaments(temp[0] + " (Bei Bedarf)", temp[1], temp[2], temp[3], temp[4],
                                converter(mo), converter(mi), converter(ev), converter(ni), temp[5], temp[6], crea));
                        }
                        else
                        {
                            m.Add(new Medicaments(temp[0], temp[1], temp[2], temp[3], temp[4], converter(mo),
                                converter(mi), converter(ev), converter(ni), temp[5], temp[6], crea));
                        }
                    }
                    else
                    {
                        crea = DateTime.Today.ToString();
                        if (temp[1] == "0" && temp[2] == "0" && temp[3] == "0" && temp[4] == "0")
                        {
                            m.Add(new Medicaments(temp[0] + " (Bei Bedarf)", temp[1], temp[2], temp[3], temp[4],
                                converter(mo), converter(mi), converter(ev), converter(ni), temp[5], temp[6], crea));
                        }
                        else
                        {
                            m.Add(new Medicaments(temp[0], temp[1], temp[2], temp[3], temp[4], converter(mo),
                                converter(mi), converter(ev), converter(ni), temp[5], temp[6], crea));
                        }
                    }
                }
            }

            var newlist = new List<Medicaments>();
            var found = false;
            foreach (var item in m)
            {
                foreach (var temp in newlist)
                {
                    if (item.name == temp.name)
                    {
                        if (item.created > temp.created)
                        {
                            newlist.Remove(temp);
                            newlist.Add(item);
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    newlist.Add(item);
                    found = false;
                }
            }
            if ((bool) chk_noMedi.IsChecked)
            {
                chk_noMedi.IsChecked = false;
            }

            dgvMedi.ItemsSource = new List<Medicaments>();
            dgvMedi.ItemsSource = m;
            if (c.medicationIsConfirmed(p, dt))
            {
                dgvMedi.IsEnabled = false;
                chk_noMedi.IsEnabled = false;
            }
            else
            {
                dgvMedi.IsEnabled = true;
                chk_noMedi.IsEnabled = true;
            }
        }

        private void dgvMedi_Loaded(object sender, RoutedEventArgs e)
        {
            for (var i = 0; i < dgvMedi.Items.Count; i++)
            {
                if (((Medicaments) dgvMedi.Items[i]).morning == "1")
                {
                }
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            var CheckBoxSender = sender as CheckBox;
            if (sender != null && firsttimer)
            {
                firsttimer = false;
                CheckBoxSender.IsEnabled = CheckBoxSender.Content.ToString() == "1";
            }
            firsttimer = false;
        }

        public bool converter(string a)
        {
            if (a == "1")
            {
                return true;
            }
            return false;
        }

        public Visibility visConverter(string a)
        {
            if (a == "1")
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        private void btnSetDoku_Click(object sender, RoutedEventArgs e)
        {
            if (!cmbKlient.SelectionBoxItem.Equals(""))
            {
                if ((bool) !chkMultiday.IsChecked)
                {
                    btnRecoverDoku.Visibility = Visibility.Visible;
                    txtDokuAußenkontakt.Background = Brushes.White;
                    txtDokuKoerperlich.Background = Brushes.White;
                    txtDokuPflichte.Background = Brushes.White;
                    txtDokuPsychisch.Background = Brushes.White;
                    txtDokuSchulisch.Background = Brushes.White;
                    txtDokuTempAussenkontakt.Text = txtDokuAußenkontakt.Text;
                    txtDokuTempKoerperlich.Text = txtDokuKoerperlich.Text;
                    txtDokuTempPflichten.Text = txtDokuPflichte.Text;
                    txtDokuTempPsychisch.Text = txtDokuPsychisch.Text;
                    txtDokuTempSchulisch.Text = txtDokuSchulisch.Text;

                    var no_post = false;

                    foreach (Medicaments medic in dgvMedi.Items)
                    {
                        if (medic.morning == "1" && medic.morningConfirmed == false ||
                            medic.midday == "1" && medic.middayConfirmed == false ||
                            medic.evening == "1" && medic.eveningConfirmed == false ||
                            medic.night == "1" && medic.nightConfirmed == false)
                        {
                            if (!(bool) chk_noMedi.IsChecked)
                            {
                                var mq = new MediQuestion(medic.name);
                                mq.ShowDialog();
                                if (mq.getIt() != "")
                                {
                                    c.setMediForDay(cmbKlient.Text, (DateTime) dateDoku.SelectedDate, medic, u.Id,
                                        mq.getIt());
                                }
                                else
                                {
                                    no_post = true;
                                    break;
                                }
                            }
                            dgvMedi.IsEnabled = false;
                        }
                        else
                        {
                            c.setMediForDay(cmbKlient.Text, (DateTime) dateDoku.SelectedDate, medic, u.Id, "");
                            dgvMedi.IsEnabled = false;
                        }
                    }
                    if ((bool) chk_noMedi.IsChecked)
                    {
                        chk_noMedi.IsChecked = false;
                        dgvMedi.IsEnabled = true;
                    }


                    if (!no_post)
                    {
                        if (txtDokuKoerperlichAnzeige.Text == "" && txtDokuSchulischAnzeige.Text == "" &&
                            txtDokuPsychischAnzeige.Text == "" && txtDokuPflichteAnzeige.Text == "" &&
                            txtDokuAußenkontaktAnzeige.Text == "")
                        {
                            c.setDoku(cmbKlient.Text, (DateTime) dateDoku.SelectedDate, txtDokuKoerperlich.Text,
                                txtDokuSchulisch.Text, txtDokuPsychisch.Text, txtDokuPflichte.Text,
                                txtDokuAußenkontakt.Text, u.Id);

                            txtDokuKoerperlichAnzeige.Text = txtDokuKoerperlich.Text;
                            txtDokuSchulischAnzeige.Text = txtDokuSchulisch.Text;
                            txtDokuPsychischAnzeige.Text = txtDokuPsychisch.Text;
                            txtDokuPflichteAnzeige.Text = txtDokuPflichte.Text;
                            txtDokuAußenkontaktAnzeige.Text = txtDokuAußenkontakt.Text;
                        }
                        else
                        {
                            var dokuKoerperlichMerged = txtDokuKoerperlich.Text;
                            var dokuSchulischMerged = txtDokuSchulisch.Text;
                            var dokuPsychischMerged = txtDokuPsychisch.Text;
                            var dokuPflichteMerged = txtDokuPflichte.Text;
                            var dokuAußenkontaktMerged = txtDokuAußenkontakt.Text;
                            if (txtDokuKoerperlich.Text != "")
                            {
                                dokuKoerperlichMerged = "---NACHTRAG (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                                                        ") VON: " + c.getNameByID(u.Id, false) + "---\r\n\r\n" +
                                                        txtDokuKoerperlich.Text;
                                txtDokuKoerperlichAnzeige.Text =
                                    txtDokuKoerperlichAnzeige.Text + "\r\n\r\n---NACHTRAG (" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " + c.getNameByID(u.Id, false) +
                                    "---\r\n\r\n" + txtDokuKoerperlich.Text;
                            }
                            if (txtDokuSchulisch.Text != "")
                            {
                                dokuSchulischMerged = "---NACHTRAG (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                                                      ") VON: " + c.getNameByID(u.Id, false) + "---\r\n\r\n" +
                                                      txtDokuSchulisch.Text;
                                txtDokuSchulischAnzeige.Text =
                                    txtDokuSchulischAnzeige.Text + "\r\n\r\n---NACHTRAG (" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " + c.getNameByID(u.Id, false) +
                                    "---\r\n\r\n" + txtDokuSchulisch.Text;
                            }
                            if (txtDokuPsychisch.Text != "")
                            {
                                dokuPsychischMerged = "---NACHTRAG (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                                                      ") VON: " + c.getNameByID(u.Id, false) + "---\r\n\r\n" +
                                                      txtDokuPsychisch.Text;
                                txtDokuPsychischAnzeige.Text =
                                    txtDokuPsychischAnzeige.Text + "\r\n\r\n---NACHTRAG (" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " + c.getNameByID(u.Id, false) +
                                    "---\r\n\r\n" + txtDokuPsychisch.Text;
                            }
                            if (txtDokuPflichte.Text != "")
                            {
                                dokuPflichteMerged = "---NACHTRAG (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") +
                                                     ") VON: " + c.getNameByID(u.Id, false) + "---\r\n\r\n" +
                                                     txtDokuPflichte.Text;
                                txtDokuPflichteAnzeige.Text =
                                    txtDokuPflichteAnzeige.Text + "\r\n\r\n---NACHTRAG (" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " + c.getNameByID(u.Id, false) +
                                    "---\r\n\r\n" + txtDokuPflichte.Text;
                            }
                            if (txtDokuAußenkontakt.Text != "")
                            {
                                dokuAußenkontaktMerged =
                                    "---NACHTRAG (" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " +
                                    c.getNameByID(u.Id, false) + "---\r\n\r\n" + txtDokuAußenkontakt.Text;
                                txtDokuAußenkontaktAnzeige.Text =
                                    txtDokuAußenkontaktAnzeige.Text + "\r\n\r\n---NACHTRAG (" +
                                    DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ") VON: " + c.getNameByID(u.Id, false) +
                                    "---\r\n\r\n" + txtDokuAußenkontakt.Text;
                            }

                            c.setDoku(cmbKlient.Text, (DateTime) dateDoku.SelectedDate, dokuKoerperlichMerged,
                                dokuSchulischMerged, dokuPsychischMerged, dokuPflichteMerged, dokuAußenkontaktMerged,
                                u.Id);
                        }

                        txtDokuAußenkontakt.Text = "";
                        txtDokuKoerperlich.Text = "";
                        txtDokuPflichte.Text = "";
                        txtDokuPsychisch.Text = "";
                        txtDokuSchulisch.Text = "";
                    }
                }
                else
                {
                    var temp = c.readDokuOverTime(cmbKlient.Text, (DateTime) dateDoku.SelectedDate,
                        (DateTime) dateDokuTo.SelectedDate);
                    txtDokuKoerperlichAnzeige.Text = temp[0];
                    txtDokuSchulischAnzeige.Text = temp[1];
                    txtDokuPsychischAnzeige.Text = temp[2];
                    txtDokuAußenkontaktAnzeige.Text = temp[3];
                    txtDokuPflichteAnzeige.Text = temp[4];

                    txtDokuAußenkontaktAnzeige.IsReadOnly = true;
                    txtDokuKoerperlichAnzeige.IsReadOnly = true;
                    txtDokuPflichteAnzeige.IsReadOnly = true;
                    txtDokuPsychischAnzeige.IsReadOnly = true;
                    txtDokuSchulischAnzeige.Text = "";

                    txtDokuAußenkontaktAnzeige.IsEnabled = true;
                    txtDokuKoerperlichAnzeige.IsEnabled = true;
                    txtDokuPflichteAnzeige.IsEnabled = true;
                    txtDokuPsychischAnzeige.IsEnabled = true;
                    txtDokuSchulischAnzeige.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Kein Klient ausgewählt!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditShout_Click(object sender, RoutedEventArgs e)
        {
            var shout = new EditShoutDialog(c);
            shout.ShowDialog();

            /*string[] shouts = {"Shoutbox", "Erstellt", "Name"};
            FillMoreData(dgShouts, c.getShouts(), shouts);*/
            fillShoutBox(c.GetShouts());
        }

        private void btnNewBericht_Click(object sender, RoutedEventArgs e)
        {
            var b = new Bericht(u, c);
            b.ShowDialog();
        }

        /**
         * Erstellen einer neuen Aufgabe
         * */
        private void btnNewTask_Click(object sender, RoutedEventArgs e)
        {
            var et = new EditTask(u.Id, c);
            et.Show();
            refreshAllTasks();
        }

        private void dgTasksForUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var t = dgTaskForUser.SelectedItem as Task;
                var et = new EditTask(true, true, t.von, t.zu, t.startdate, t.enddate, t.desc, c);
                et.Show();
                refreshAllTasks();
            }
            catch (Exception)
            {
            }
        }

        private void dgInstructionsFromUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var i = dgInstructionsFromUser.SelectedItem as Instruction;
                var ei = new EditInstruction(u.IsAdmin, u.Id, i.date, i.title, i.desc, i.name, c);
                ei.Show();
            }
            catch
            {
            }
        }

        private void btnGoToInstructions_Click(object sender, RoutedEventArgs e)
        {
            tabMain.SelectedIndex = 4;
            refreshAllInstructions();
        }

        private void refreshKmG(string id)
        {
            var month = "0";
            var year = "0";

            try
            {
                month = (int.Parse(cmbKMGMonth.SelectedIndex.ToString()) + 1).ToString();
                year = cmbKMGYear.Text;
            }
            catch (Exception)
            {
            }

            var KmGlist = c.getKilometerGeld(id, month, year);
            var KmGlist2 = new List<KmG>();

            foreach (var tmp in KmGlist)
            {
                if (DateTime.Parse(tmp.Zeitvon).Month.ToString() == month &&
                    DateTime.Parse(tmp.Zeitvon).Year.ToString() == year)
                {
                    if (DateTime.Parse(tmp.Zeitbis).Month.ToString() == month &&
                        DateTime.Parse(tmp.Zeitbis).Year.ToString() == year)
                    {
                        tmp.Verfasser = c.getNameByID(tmp.Verfasser, false);
                        KmGlist2.Add(tmp);
                    }
                }
            }
            dgKmG.ItemsSource = KmGlist2;
        }

        private string RefreshKmG_PDF(string id)
        {
            var KmGlist = c.getKilometerGeld(id);
            var KmGlist2 = new List<KmG>();

            foreach (var tmp in KmGlist)
            {
                tmp.Verfasser = c.getNameByID(tmp.Verfasser, false);
                KmGlist2.Add(tmp);
            }

            double zahl = 0;

            foreach (var tmp2 in KmGlist2)
            {
                zahl += Convert.ToDouble(tmp2.Summe);
            }

            return zahl.ToString();
        }

        private string RefreshKmG_PDF_Month(string id, string month, string year)
        {
            var KmGlist = c.getKilometerGeld_Month(id, month, year);
            var KmGlist2 = new List<KmG>();

            foreach (var tmp in KmGlist)
            {
                tmp.Verfasser = c.getNameByID(tmp.Verfasser, false);
                KmGlist2.Add(tmp);
            }

            double zahl = 0;

            foreach (var tmp2 in KmGlist2)
            {
                zahl += Convert.ToDouble(tmp2.Summe);
            }

            return zahl.ToString();
        }

        public void RefreshService()
        {
            var serviceList = c.getServicesVital();
            cmbZugehoerigkeit.ItemsSource = serviceList;
            cmbZugehoerigkeit.DisplayMemberPath = "Name";
            cmbZugehoerigkeit.SelectedValuePath = "Id";
            AllHouses = serviceList;

            dgHouseAll.ItemsSource = AllHouses;
        }

        private void refreshInstructions()
        {
            var instructionList = c.getInstruction();
            dgInstructionsFromUser.ItemsSource = instructionList;
        }

        private void refreshUnreadInstructions()
        {
            var unreadinstructionList = c.getUnreadInstruction(u.Id);
            dgUnreadInstructions.ItemsSource = unreadinstructionList;
        }

        private void refreshTaskfromUser()
        {
            var tasklist = c.getTasksfromUser(u.Id);
            dgTaskFromUser.ItemsSource = tasklist;
        }

        private void refreshUrgentTaskfromUser()
        {
            var urgenttasklist = c.getUrgentTasksfromUser(u.Id);
            dgImportantTasks.ItemsSource = urgenttasklist;
        }

        private void refreshTaskforUser()
        {
            var taskforlist = c.getTasksforUser(u.Id);
            dgTaskForUser.ItemsSource = taskforlist;
        }

        private void refreshCreatedTasks()
        {
            var createdTasks = c.getCreatedTasksforUser(u.Id);
            dgCreatedTasks.ItemsSource = createdTasks;
        }

        private void refreshNewestDokus()
        {
            var ts = TimeSpan.FromTicks(TimeSpan.TicksPerDay * 7);
            var dt = DateTime.Now - ts;
            var sql_date = dt.ToString("yyyy-MM-dd");
            var newestDokusList = c.getDokusByWgsAndDate(u.Services, sql_date);
            dgNewestDokus.ItemsSource = newestDokusList;
        }

        private void refreshAllTasks()
        {
            refreshTaskforUser();
            refreshTaskfromUser();
            refreshUrgentTaskfromUser();
            refreshCreatedTasks();
        }

        private void refreshAllInstructions()
        {
            refreshInstructions();
            refreshUnreadInstructions();
        }

        private void btnGoToTasks_Click(object sender, RoutedEventArgs e)
        {
            tabMain.SelectedIndex = 10;
            refreshAllTasks();
        }

        private void Gmail_LoadCompleted(object sender, NavigationEventArgs e)
        {
        }

        /**
         * Erstellt eine neue Dienstanweisung
         * */
        private void btnNewInstruction_Click(object sender, RoutedEventArgs e)
        {
            var name = c.getNameByID(u.Id, false);
            var newinstruction = new EditInstruction(name, c);
            newinstruction.ShowDialog();
            refreshAllInstructions();
        }

        private void dgAppointmentsForUser_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Cancel = true;
            }
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            dateDokuTo.Visibility = Visibility.Visible;
            lblTo.Visibility = Visibility.Visible;
            lblDateOrTo.Content = "Von";
            btnSetDoku.Content = "Doku von... bis... anfordern";
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            dateDokuTo.Visibility = Visibility.Hidden;
            lblTo.Visibility = Visibility.Hidden;
            lblDateOrTo.Content = "Datum";
            btnSetDoku.Content = "Dokumentation speichern";
        }

        private void button1_Click_1(object sender, RoutedEventArgs e) // Fun Knopf
        {
            Functions.fun();
        }

        private void btnLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login();
        }

        private void bnt_Reload_Click(object sender, RoutedEventArgs e)
        {
            bnt_Reload.IsEnabled = false;
            setdgvWorkingTime();
            foreach (var day in c.getNonWorkingDays().Split('%'))
            {
                nonWorkingDays.Add(day);
            }
            bnt_Reload.IsEnabled = true;
        }


        public string[] getWorkinhDataForUserOida(User a, DateTime month)
        {
            var derOberKillaShit = new string[5];

            /*
             * [0] = Sollstunden
             * [1] = Überstunden
             * [2] = Urlaubsstunden
             * [3] = Habenstunden
             * [4] = Krankenstand 
             */
            try
            {
                a.Kostalid = Convert.ToInt32(a.Id);

                string[] fillings;
                fillings = c.getWorkingtime(month, a.Kostalid, u.IsAdmin).Split('%');

                var arbeit = c.getWorkingDays(a.Id);

                if (arbeit == "5")
                {
                    a.WorkingTimeType = 1;
                }
                else
                {
                    a.WorkingTimeType = 2;
                }

                //fw hdmi ifschl pozkern

                IFormatProvider culture = new CultureInfo("de-DE", true);
                time = new List<WorkingTime>();
                holidaydata = new List<WorkingTime>();
                foreach (var s in fillings)
                {
                    var temp = s.Split('$');
                    if (temp.Length == 7)
                    {
                        if (temp[2] == "Urlaub")
                        {
                            if ((temp[6] == "True") | (temp[6] == "1"))
                            {
                                time.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                    true));
                            }
                            else if ((temp[6] == "False") | (temp[6] == "0"))
                            {
                                holidaydata.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4],
                                    temp[5], false));
                            }
                        }
                        else
                        {
                            time.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                false));
                        }
                    }
                }


                //soll-stunden
                double sollstd = 0;
                //Hier werden die Feiertage von der Datenbank geholt
                var holidaysstring = c.getNonWorkingDays();
                if (holidaysstring == null)
                {
                    return null;
                }
                var holidays = holidaysstring.Split('%');
                //Mit dieser Variable wird geprüft, ob der ausgewählte Tag ein Feiertag ist
                var isholday = false;
                DateTime sampleday;
                if (a.WorkingTimeType == 1)
                {
                    var days = DateTime.DaysInMonth(month.Year, month.Month);
                    DateTime dt;
                    for (var counter = 1; counter <= days; counter++)
                    {
                        dt = new DateTime(month.Year, month.Month, counter);
                        //Wenn der Tag kein Samstag oder Sontag ist zähle!
                        if (dt.ToString("ddd") != "Sa." && dt.ToString("ddd") != "So.")
                        {
                            //Alle Feiertage werden durch gegangen
                            for (var i = 0; i < holidays.Length; i++)
                            {
                                if (holidays[i] != "")
                                {
                                    sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                    if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                        sampleday.Year == dt.Year)
                                    {
                                        //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                        isholday = true;
                                    }
                                }
                            }
                            if (!isholday)
                            {
                                //Sollstunden werden erhöht, wenn es kein Feiertag ist
                                sollstd += 1;
                            }
                        }
                        //Die Variable wird zurück gesetzt
                        isholday = false;
                    }
                    sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 5);
                    var tempusvaletsemper = sollstd.ToString();
                    if (tempusvaletsemper.Contains(','))
                    {
                        try
                        {
                            tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    derOberKillaShit[0] = tempusvaletsemper;
                }
                else if (a.WorkingTimeType == 2)
                {
                    var days = DateTime.DaysInMonth(month.Year, month.Month);
                    DateTime dt;
                    for (var counter = 1; counter <= days; counter++)
                    {
                        dt = new DateTime(month.Year, month.Month, counter);
                        if (dt.ToString("ddd") != "So.")
                        {
                            for (var i = 0; i < holidays.Length; i++)
                            {
                                if (holidays[i] != "")
                                {
                                    sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                    if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                        sampleday.Year == dt.Year)
                                    {
                                        isholday = true;
                                    }
                                }
                            }
                            if (!isholday)
                            {
                                sollstd += 1;
                            }
                        }
                        isholday = false;
                    }
                    sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 6);
                    var tempusvaletsemper = sollstd.ToString();
                    if (tempusvaletsemper.Contains(','))
                    {
                        try
                        {
                            tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    derOberKillaShit[0] = tempusvaletsemper;
                }

                //haben-stunden

                double habenstd = 0;
                var urlaub = c.getUrlaubstime(Convert.ToInt32(a.Id));
                double urlaubstd = 0;
                if (urlaub != "NULL")
                {
                    urlaubstd = Convert.ToInt32(urlaubstd);
                }
                double workinghours = 0;
                if (u.WorkingTimeType == 1)
                {
                    workinghours = c.getWorkinghoursperWeek(a.Id) / 5;
                }
                else
                {
                    workinghours = c.getWorkinghoursperWeek(a.Id) / 6;
                }
                double krankenstunden = 0;
                foreach (var wt in time)
                {
                    var worktime = getworkingtime(wt);
                    var urlaubtime = geturlaubtime(wt, workinghours);
                    urlaubstd -= geturlaubsTage(wt); //urlaubtime.TotalHours;
                    habenstd += urlaubtime.TotalHours;
                    habenstd += worktime.TotalHours;
                    if (wt.comment.StartsWith("Krankenstand - "))
                    {
                        krankenstunden += worktime.TotalHours;
                    }
                }
                double uberstd = 0;
                if (sollstd - habenstd <= 0)
                {
                    uberstd = (sollstd - habenstd) * -1;
                    habenstd = habenstd - uberstd;
                }
                else
                {
                    uberstd = habenstd - sollstd;
                }
                var tmp2 = uberstd.ToString();
                if (tmp2.Contains(','))
                {
                    try
                    {
                        tmp2 = tmp2.Substring(0, tmp2.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                derOberKillaShit[1] = tmp2;
                var tmp = urlaubstd.ToString();
                if (urlaubstd.ToString().Contains(','))
                {
                    try
                    {
                        tmp = urlaubstd.ToString().Substring(0, urlaubstd.ToString().IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }

                if (a.WorkingTimeType == 1)
                {
                }
                else if (a.WorkingTimeType == 2)
                {
                }

                derOberKillaShit[2] = tmp;
                var tmp4 = habenstd.ToString();
                if (tmp4.Contains(','))
                {
                    try
                    {
                        tmp4 = tmp4.Substring(0, tmp4.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                derOberKillaShit[3] = tmp4;

                var tmp5 = krankenstunden.ToString();
                if (tmp5.Contains(','))
                {
                    try
                    {
                        tmp5 = tmp5.Substring(0, tmp5.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                derOberKillaShit[4] = tmp5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return derOberKillaShit;
        }

        public void setdgvWorkingTime()
        {
            try
            {
                var date =
                    new DateTime(
                        Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                            .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1, 1);

                var a = new User();
                User b;
                if (cmbworkingTimeUser.Text != "Alle User")
                {
                    var temps = cmbworkingTimeUser.Text.Split(' ');
                    for (var i = 0; i < userlist.Count; i++)
                    {
                        b = userlist[i];
                        if (temps[0].Trim() == b.Firstname && temps[1].Trim() == b.Lastname)
                        {
                            a = userlist[i];
                            a.Id = userlist[i].Kostalid.ToString();
                            a.Kostalid = userlist[i].Kostalid;
                            break;
                        }
                    }
                }
                else
                {
                    a.Kostalid = 0;
                    a.Id = "0";
                }

                string[] fillings;
                fillings = c.getWorkingtime(date, a.Kostalid, u.IsAdmin).Split('%');

                var arbeit = c.getWorkingDays(a.Id);

                if (arbeit == "5")
                {
                    a.WorkingTimeType = 1;
                }
                else
                {
                    a.WorkingTimeType = 2;
                }


                var tab = new DataTable();
                var columnnames = new string[5] {"User", "Art", "Von-Datum", "Bis-Datum", "Kommentar"};
                IFormatProvider culture = new CultureInfo("de-DE", true);
                time = new List<WorkingTime>();
                holidaydata = new List<WorkingTime>();
                foreach (var s in fillings)
                {
                    var temp = s.Split('$');
                    if (temp.Length == 7)
                    {
                        if (temp[2] == "Urlaub")
                        {
                            if ((temp[6] == "True") | (temp[6] == "1"))
                            {
                                time.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                    true));
                            }
                            else if ((temp[6] == "False") | (temp[6] == "0"))
                            {
                                holidaydata.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4],
                                    temp[5], false));
                            }
                        }
                        else
                        {
                            time.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                false));
                        }
                    }
                }

                foreach (var workingt in time)
                {
                    workingt.datetimefrom2 = workingt.datetimefrom.ToString("dd.MM.yyyy HH:mm");
                    workingt.datetimeto2 = workingt.datetimeto.ToString("dd.MM.yyyy HH:mm");
                }


                dgvZeiterfassung.ItemsSource = time;

                foreach (var workingt in holidaydata)
                {
                    workingt.datetimefrom2 = workingt.datetimefrom.ToString("dd.MM.yyyy HH:mm");
                    workingt.datetimeto2 = workingt.datetimeto.ToString("dd.MM.yyyy HH:mm");
                }


                dgvUrlaub.ItemsSource = holidaydata;

                //soll-stunden
                double sollstd = 0;
                //Hier werden die Feiertage von der Datenbank geholt
                var holidaysstring = c.getNonWorkingDays();
                if (holidaysstring == null)
                {
                    return;
                }
                var holidays = holidaysstring.Split('%');
                //Mit dieser Variable wird geprüft, ob der ausgewählte Tag ein Feiertag ist
                var isholday = false;
                DateTime sampleday;
                if (a.WorkingTimeType == 1)
                {
                    var days = DateTime.DaysInMonth(
                        Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                            .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1);
                    DateTime dt;
                    for (var counter = 1; counter <= days; counter++)
                    {
                        dt = new DateTime(
                            Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                            cmb_Month.SelectedIndex + 1, counter);
                        //Wenn der Tag kein Samstag oder Sontag ist zähle!
                        if (dt.ToString("ddd") != "Sa." && dt.ToString("ddd") != "So.")
                        {
                            //Alle Feiertage werden durch gegangen
                            for (var i = 0; i < holidays.Length; i++)
                            {
                                if (holidays[i] != "")
                                {
                                    sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                    if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                        sampleday.Year == dt.Year)
                                    {
                                        //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                        isholday = true;
                                    }
                                }
                            }
                            if (!isholday)
                            {
                                //Sollstunden werden erhöht, wenn es kein Feiertag ist
                                sollstd += 1;
                            }
                        }
                        //Die Variable wird zurück gesetzt
                        isholday = false;
                    }
                    sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 5);
                    var tempusvaletsemper = sollstd.ToString();
                    if (tempusvaletsemper.Contains(','))
                    {
                        try
                        {
                            tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    SollStunden.Content = tempusvaletsemper;
                }
                else if (a.WorkingTimeType == 2)
                {
                    var days = DateTime.DaysInMonth(
                        Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                            .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1);
                    DateTime dt;
                    for (var counter = 1; counter <= days; counter++)
                    {
                        dt = new DateTime(
                            Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                            cmb_Month.SelectedIndex + 1, counter);
                        if (dt.ToString("ddd") != "So.")
                        {
                            for (var i = 0; i < holidays.Length; i++)
                            {
                                if (holidays[i] != "")
                                {
                                    sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                    if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                        sampleday.Year == dt.Year)
                                    {
                                        isholday = true;
                                    }
                                }
                            }
                            if (!isholday)
                            {
                                sollstd += 1;
                            }
                        }
                        isholday = false;
                    }
                    sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 6);
                    var tempusvaletsemper = sollstd.ToString();
                    if (tempusvaletsemper.Contains(','))
                    {
                        try
                        {
                            tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    SollStunden.Content = tempusvaletsemper;
                }

                //haben-stunden

                double habenstd = 0;
                var urlaub = c.getUrlaubstime(Convert.ToInt32(a.Id));
                double urlaubstd = 0;
                if (urlaub != "NULL")
                {
                    try
                    {
                        urlaubstd = Convert.ToDouble(urlaub.Substring(0, urlaub.IndexOf(',') + 2));
                    }
                    catch
                    {
                        //MessageBox.Show(ex.ToString());
                        try
                        {
                            urlaubstd = Convert.ToInt32(urlaub.Substring(0, urlaub.IndexOf(',')));
                        }
                        catch
                        {
                            /**/
                            /**/
                        }
                    }
                }
                double workinghours = 0;
                if (a.WorkingTimeType == 1)
                {
                    workinghours = c.getWorkinghoursperWeek(a.Id) / 5;
                }
                else
                {
                    workinghours = c.getWorkinghoursperWeek(a.Id) / 6;
                }
                double krankenstunden = 0;
                foreach (var wt in time)
                {
                    var worktime = getworkingtime(wt);
                    var urlaubtime = geturlaubtime(wt, workinghours);
                    urlaubstd -= geturlaubsTage(wt); //urlaubtime.TotalHours;
                    habenstd += urlaubtime.TotalHours;
                    habenstd += worktime.TotalHours;
                    if (wt.comment.StartsWith("Krankenstand - "))
                    {
                        krankenstunden += worktime.TotalHours;
                    }
                }
                double uberstd = 0;
                if (sollstd - habenstd <= 0)
                {
                    uberstd = (sollstd - habenstd) * -1;
                    habenstd = habenstd - uberstd;
                }
                else
                {
                    uberstd = habenstd - sollstd;
                }
                var tmp2 = uberstd.ToString();
                if (tmp2.Contains(','))
                {
                    try
                    {
                        tmp2 = tmp2.Substring(0, tmp2.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                Uberstunden.Content = tmp2;
                var tmp = urlaubstd.ToString();
                if (urlaubstd.ToString().Contains(','))
                {
                    try
                    {
                        tmp = urlaubstd.ToString().Substring(0, urlaubstd.ToString().IndexOf(',') + 5);
                    }
                    catch
                    {
                    }
                }
                lblUrlaub.Content = tmp;
                var tmp4 = habenstd.ToString();
                if (tmp4.Contains(','))
                {
                    try
                    {
                        tmp4 = tmp4.Substring(0, tmp4.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                HabenStunden.Content = tmp4;

                var tmp5 = krankenstunden.ToString();
                if (tmp5.Contains(','))
                {
                    try
                    {
                        tmp5 = tmp5.Substring(0, tmp5.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                lblKrank.Content = tmp5;
                //test area
                var tempp = c.getUberstdges(a.Kostalid);
                if (string.IsNullOrEmpty(tempp) | string.IsNullOrWhiteSpace(tempp))
                {
                    tempp = "0";
                }
                var tmp3 = (uberstd + Convert.ToDouble(tempp)).ToString();
                //string tmp3 = (uberstd + Convert.ToDouble(c.getUberstdges(a.Kostalid))).ToString();
                //test area
                if (tmp3.Contains(','))
                {
                    try
                    {
                        tmp3 = tmp3.Substring(0, tmp3.IndexOf(',') + 3);
                    }
                    catch
                    {
                    }
                }
                uberstdges.Content = tmp3;

                if (a.Kostalid == 0)
                {
                    lblUrlaub.Content = "0";
                    HabenStunden.Content = "0";
                    SollStunden.Content = "0";
                    Uberstunden.Content = "0";
                    lblKrank.Content = "0";
                    uberstdges.Content = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public int getSonnUndFeiertag(int userid)
        {
            return machDieBerechnung(userid);
        }

        private int machDieBerechnung(int userid)
        {
            var sollstunden = 0;
            if (Feiertegeundso() == 5 * Math.Sin(3 * gibmirSonntage()))
            {
                return sollstunden;
            }
            return 0;
        }

        private double Feiertegeundso()
        {
            return 5.0;
        }

        private int gibmirSonntage()
        {
            return 30;
        }

        public int geturlaubsTage(WorkingTime wt)
        {
            var tage = 0;
            if (wt.art == "Urlaub")
            {
                var a = wt.datetimeto - wt.datetimefrom;
                tage = a.Days + 1;
            }

            return tage;
        }

        public TimeSpan geturlaubtime(WorkingTime wt, double workinghours)
        {
            var tmp = new TimeSpan();
            try
            {
                if (wt.art == "Urlaub")
                {
                    if (workinghours != 0) //test#nohomo
                    {
                        var a = workinghours.ToString().Split(',');
                        try
                        {
                            if (a[1].Length > 2)
                            {
                                a[1] = a[1].Substring(0, 2);
                            }
                        }
                        catch
                        {
                        }


                        tmp = wt.datetimeto - wt.datetimefrom;

                        if (tmp.Days == 0)
                        {
                            //tmp = new TimeSpan(Convert.ToInt32(a[0].ToString()), Convert.ToInt32(Math.Round((Convert.ToDecimal(a[1].ToString()) / 100) * 60)), 0);
                            tmp = TimeSpan.FromHours(workinghours);
                        }

                        else
                        {
                            //tmp = new TimeSpan(Convert.ToInt32(a[0].ToString()), Convert.ToInt32(Math.Round((Convert.ToDecimal(a[1].ToString()) / 100) * 60)), 0);
                            tmp = TimeSpan.FromHours(workinghours);
                            var tmp2 = tmp;
                            for (var i = 0; i < (wt.datetimeto - wt.datetimefrom).Days; i++)
                            {
                                tmp = tmp + tmp2;
                            }
                        }
                    }
                    else
                    {
                        return new TimeSpan(0, 0, 0);
                    }
                    return tmp;
                }
                return new TimeSpan(0, 0, 0);
            }
            catch
            {
                return new TimeSpan(0, 0, 0);
            }
        }

        public TimeSpan getworkingtime(WorkingTime wt)
        {
            var tmp = new TimeSpan();


            if (wt.art == "Tagdienst")
            {
                tmp = wt.datetimeto - wt.datetimefrom;
            }
            else if (wt.art == "Nachtdienst")
            {
                tmp = new TimeSpan((wt.datetimeto - wt.datetimefrom).Ticks / 2);
            }
            else if (wt.art != "Urlaub")
            {
                tmp = wt.datetimeto - wt.datetimefrom;
            }

            return tmp;
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btnZeitEdit_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new EditWorkingTimeDialog(cmb_Month.SelectedIndex + 1,
                Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                    .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), chk_allow.IsChecked.Value,
                Convert.ToInt32(u.Id));
            dialog.ShowDialog();

            setdgvWorkingTime();
        }

        private void BtnZeiterfassung_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnChangePW_Click(object sender, RoutedEventArgs e)
        {
            var isCorrect = false;
            var source = pwdOld.Password;
            using (var md5Hash = MD5.Create())
            {
                var hash = Functions.GetMd5Hash(md5Hash, source);

                if (Functions.VerifyMd5Hash(md5Hash, source, c.getPW(u.Id)))
                {
                    isCorrect = true;
                }
                else
                {
                    isCorrect = false;
                }
            }
            if (isCorrect)
            {
                if (pwdNew.Password == pwdNewConfirm.Password)
                {
                    source = pwdNew.Password;
                    using (var md5Hash = MD5.Create())
                    {
                        c.setNewPW(Functions.GetMd5Hash(md5Hash, source), u.Id);
                        pwdNew.Password = "";
                        pwdNewConfirm.Password = "";
                        pwdOld.Password = "";
                        MessageBox.Show("Passwort wurde erfolgreich geändert", "Erfolg!", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Sie müssen 2 mal das Selbe Passwort eingeben um die eingabe zu bestätigen",
                        "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Sie müssen bei altes Password das Korrekte alte Passwort eintragen!", "Achtung",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void imgLogo_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void dgvZeiterfassung_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void tabMain_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void tabWorkingTime_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void tabTasks_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tabMain.Visibility = Visibility.Hidden;
        }

        private void dgTaskFromUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var t = dgTaskFromUser.SelectedItem as Task;
                var et = new EditTask(false, false, t.von, t.zu, t.startdate, t.enddate, t.desc, c);
                et.Show();
                refreshAllTasks();
            }
            catch
            {
            }
        }

        private void dgUnreadInstructions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var i = dgUnreadInstructions.SelectedItem as Instruction;
                var ei = new EditInstruction(u.IsAdmin, u.Id, i.date, i.title, i.desc, i.name, c);
                ei.Show();
                refreshAllInstructions();
            }
            catch
            {
            }
        }

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            cmbAdminUsers.Visibility = Visibility.Hidden;
            newUser = true;
            txtAccountnr.Text = "";
            txtAdminFirstname.Text = "";
            txtAdminGeb.Text = "";
            txtAdminLastname.Text = "";
            txtBankname.Text = "";
            txtBLZ.Text = "";
            txtCity.Text = "";
            txtFax.Text = "";
            txtMailPw.Text = "";
            txtMailUser.Text = "";
            txtPw.Text = "";
            txtStreet.Text = "";
            txtSVNr.Text = "";
            txtTel.Text = "";
            txtUsername.Text = "";
            txtWeeklyhours.Text = "";
            txtWorkingdaysWeekly.Text = "";
            txtZip.Text = "";
            setadminenadisa(true);
            btnFunkZugeh.IsEnabled = false;
        }

        private bool checkName(string name)
        {
            var temp = name.Trim();

            if (temp.Contains(" "))
            {
                return false;
            }
            return true;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            txtAdminLastname.Text.Replace(' ', '-');
            txtAdminFirstname.Text.Replace(' ', '-');

            txtAccountnr.Background = color1;
            txtAdminFirstname.Background = color1;
            txtAdminGeb.Background = color1;
            txtAdminLastname.Background = color1;
            txtBankname.Background = color1;
            txtBLZ.Background = color1;
            txtCity.Background = color1;
            txtFax.Background = color1;
            txtMailPw.Background = color1;
            txtMailUser.Background = color1;
            txtPw.Background = color1;
            txtStreet.Background = color1;
            txtSVNr.Background = color1;
            txtTel.Background = color1;
            txtUsername.Background = color1;
            txtWeeklyhours.Background = color1;
            txtWorkingdaysWeekly.Background = color1;
            txtZip.Background = color1;
            dateAnfang.Background = color1;
            dateEnde.Background = color1;
            dgHouseSelected.Background = color1;
            cmbAdminUsers.Background = color1;

            if (newUser)
            {
                newUser = !newUser;

                if (txtWorkingdaysWeekly.Text != null && (int.Parse(txtWorkingdaysWeekly.Text) == 5 ||
                                                          int.Parse(txtWorkingdaysWeekly.Text) == 6))
                {
                    var source = txtPw.Text;
                    using (var md5Hash = MD5.Create())
                    {
                        if (checkName(txtAdminFirstname.Text) && checkName(txtAdminLastname.Text) &&
                            dgHouseSelected.Items.Count > 0)
                        {
                            DateTime geb;
                            try
                            {
                                geb = Convert.ToDateTime(txtAdminGeb.Text);
                            }
                            catch
                            {
                                MessageBox.Show("Geben Sie ein gültiges Geburtsdatum ein!");
                                return;
                            }

                            c.setNewUser(txtAccountnr.Text, txtAdminFirstname.Text.Trim(), txtAdminLastname.Text.Trim(),
                                txtBankname.Text, txtBLZ.Text, txtCity.Text, txtFax.Text, txtMailPw.Text,
                                txtMailUser.Text, txtStreet.Text, txtSVNr.Text, txtTel.Text, txtUsername.Text,
                                txtWeeklyhours.Text, txtWorkingdaysWeekly.Text, txtZip.Text,
                                Functions.GetMd5Hash(md5Hash, source), u.Id, txtAdminGeb.Text,
                                rdbAdminYes.IsChecked.ToString(), (DateTime) dateAnfang.SelectedDate);
                            c.AddUserstoServices(
                                Convert.ToInt32(
                                    c.getUserIDbyFullname(txtAdminFirstname.Text.Trim() + " " +
                                                          txtAdminLastname.Text.Trim())), SelectedHouses);
                            var dp = new Dropbox(txtAdminFirstname.Text.Trim(), txtAdminLastname.Text.Trim(),
                                txtMailUser.Text);
                            dp.ShowDialog();
                            cmbAdminUsers.Visibility = Visibility.Visible;
                            txtAccountnr.Text = "";
                            txtAdminFirstname.Text = "";
                            txtAdminGeb.Text = "";
                            txtAdminLastname.Text = "";
                            txtBankname.Text = "";
                            txtBLZ.Text = "";
                            txtCity.Text = "";
                            txtFax.Text = "";
                            txtMailPw.Text = "";
                            txtMailUser.Text = "";
                            txtPw.Text = "";
                            txtStreet.Text = "";
                            txtSVNr.Text = "";
                            txtTel.Text = "";
                            txtUsername.Text = "";
                            txtWeeklyhours.Text = "";
                            txtWorkingdaysWeekly.Text = "";
                            txtZip.Text = "";
                            dateAnfang.SelectedDate = null;
                            dateEnde.SelectedDate = null;
                            MessageBox.Show("Erfolg", "Der Neue User wurde erfolgreich eingefügt", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                        else
                        {
                            txtAdminFirstname.Background = color2;
                            dgHouseSelected.Background = color2;
                            txtAdminLastname.Background = color2;
                            MessageBox.Show("Namen dürfen keine Leerzeichen enthalten! Ein Haus muss ausgewählt sein!",
                                "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }
                else
                {
                    txtWorkingdaysWeekly.Background = color2;
                    MessageBox.Show("Fehler!", "Arbeitstage pro Woche dürfen nur 5 oder 6 Tage sein!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                if (!cmbAdminUsers.Text.Equals(""))
                {
                    var a = new User();
                    User b;


                    var temps = cmbAdminUsers.Text.Split(' ');
                    for (var i = 0; i < userlist.Count; i++)
                    {
                        b = userlist[i];
                        if (temps[0] == b.Firstname && temps[1] == b.Lastname)
                        {
                            a = userlist[i];
                            break;
                        }
                    }
                    var source = txtPw.Text;
                    try
                    {
                        //Formatierung des Datums
                        var hodenkrebs = txtAdminGeb.Text;
                        var Ladekabel = hodenkrebs.Split(' ');
                        var Spatzi = Ladekabel[0].Split('.');
                        if (Spatzi[1].Length == 1)
                        {
                            Spatzi[1] = "0" + Spatzi[1];
                        }
                        if (Spatzi[2].Length == 1)
                        {
                            Spatzi[2] = "0" + Spatzi[2];
                        }
                        if (Spatzi[3].Length == 1)
                        {
                            Spatzi[3] = "0" + Spatzi[3];
                        }
                        hodenkrebs = Spatzi[2] + "." + Spatzi[1] + "." + Spatzi[0] + " " + Ladekabel[1];

                        txtAdminGeb.Text = hodenkrebs;
                    }
                    catch (Exception exception)
                    {
                        var text = exception.Message;
                    }

                    if (!txtWorkingdaysWeekly.Text.Equals(""))
                    {
                        try
                        {
                            if (int.Parse(txtWorkingdaysWeekly.Text) == 5 ||
                                int.Parse(txtWorkingdaysWeekly.Text) == 6)
                            {
                                if (source != "")
                                {
                                    using (var md5Hash = MD5.Create())
                                    {
                                        c.updateUser(txtAccountnr.Text, txtAdminFirstname.Text.Trim(),
                                            txtAdminLastname.Text.Trim(), txtBankname.Text, txtBLZ.Text, txtCity.Text,
                                            txtFax.Text, txtMailPw.Text, txtMailUser.Text, txtStreet.Text, txtSVNr.Text,
                                            txtTel.Text, txtUsername.Text, txtWeeklyhours.Text,
                                            txtWorkingdaysWeekly.Text, txtZip.Text,
                                            Functions.GetMd5Hash(md5Hash, source), u.Id, txtAdminGeb.Text, a.Kostalid,
                                            rdbAdminYes.IsChecked.ToString(), Functions.DateConverter(dateEnde.Text),
                                            Functions.DateConverter(dateAnfang.Text));
                                        c.updateUsertoService(
                                            Convert.ToInt32(
                                                c.getUserIDbyFullname(txtAdminFirstname.Text.Trim() + " " +
                                                                      txtAdminLastname.Text.Trim())), SelectedHouses);
                                    }
                                }
                                else
                                {
                                    if (dateEnde.SelectedDate == null)
                                    {
                                        c.updateUserNoPw(txtAccountnr.Text, txtAdminFirstname.Text.Trim(),
                                            txtAdminLastname.Text.Trim(), txtBankname.Text, txtBLZ.Text, txtCity.Text,
                                            txtFax.Text, txtMailPw.Text, txtMailUser.Text, txtStreet.Text, txtSVNr.Text,
                                            txtTel.Text, txtUsername.Text, txtWeeklyhours.Text,
                                            txtWorkingdaysWeekly.Text, txtZip.Text, u.Id, txtAdminGeb.Text,
                                            rdbAdminYes.IsChecked.ToString(), a.Kostalid,
                                            (DateTime) dateAnfang.SelectedDate);
                                    }
                                    else
                                    {
                                        c.updateUserNoPw(txtAccountnr.Text, txtAdminFirstname.Text.Trim(),
                                            txtAdminLastname.Text.Trim(), txtBankname.Text, txtBLZ.Text, txtCity.Text,
                                            txtFax.Text, txtMailPw.Text, txtMailUser.Text, txtStreet.Text, txtSVNr.Text,
                                            txtTel.Text, txtUsername.Text, txtWeeklyhours.Text,
                                            txtWorkingdaysWeekly.Text, txtZip.Text, u.Id, txtAdminGeb.Text,
                                            rdbAdminYes.IsChecked.ToString(), a.Kostalid,
                                            (DateTime) dateEnde.SelectedDate, (DateTime) dateAnfang.SelectedDate);
                                    }
                                    c.updateUsertoService(
                                        Convert.ToInt32(
                                            c.getUserIDbyFullname(txtAdminFirstname.Text.Trim() + " " +
                                                                  txtAdminLastname.Text.Trim())), SelectedHouses);
                                }

                                cmbAdminUsers.Visibility = Visibility.Visible;
                                txtAccountnr.Text = "";
                                txtAdminFirstname.Text = "";
                                txtAdminGeb.Text = "";
                                txtAdminLastname.Text = "";
                                txtBankname.Text = "";
                                txtBLZ.Text = "";
                                txtCity.Text = "";
                                txtFax.Text = "";
                                txtMailPw.Text = "";
                                txtMailUser.Text = "";
                                txtPw.Text = "";
                                txtStreet.Text = "";
                                txtSVNr.Text = "";
                                txtTel.Text = "";
                                txtUsername.Text = "";
                                txtWeeklyhours.Text = "";
                                txtWorkingdaysWeekly.Text = "";
                                txtZip.Text = "";
                                dateAnfang.SelectedDate = null;
                                dateEnde.SelectedDate = null;
                                MessageBox.Show("Der User wurde erfolgreich bearbeitet", "Erfolg", MessageBoxButton.OK,
                                    MessageBoxImage.Information);

                                c.setEmailPass(a.Kostalid.ToString(), txtMailPw.Text);
                            }
                            else
                            {
                                txtWorkingdaysWeekly.Background = color2;
                                MessageBox.Show("Arbeitstage pro Woche dürfen nur 5 oder 6 Tage sein!", "Fehler!",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            txtWorkingdaysWeekly.Background = color2;
                            MessageBox.Show("Dieser Wert ist ungültig");
                            return;
                        }
                    }
                }
                else
                {
                    cmbAdminUsers.Background = color2;
                    MessageBox.Show("Kein Benutzer ausgewählt!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            dgHouseSelected.ItemsSource = new List<Service>();
            dgHouseAll.ItemsSource = new List<Service>();

            SelectedHouses = new List<Service>();
            AllHouses = c.getServicesVital();

            dgHouseSelected.ItemsSource = SelectedHouses;
            dgHouseAll.ItemsSource = AllHouses;
        }

        private void btn_Workingtimeprint_Click(object sender, RoutedEventArgs e)
        {
        }

        public void setadminenadisa(bool show)
        {
            txtAdminFirstname.IsEnabled = show;
            txtAdminLastname.IsEnabled = show;
            txtAdminGeb.IsEnabled = show;
            txtZip.IsEnabled = show;
            txtCity.IsEnabled = show;
            txtStreet.IsEnabled = show;
            txtTel.IsEnabled = show;
            txtFax.IsEnabled = show;

            txtUsername.IsEnabled = show;
            txtPw.IsEnabled = show;
            txtMailUser.IsEnabled = show;
            txtMailPw.IsEnabled = show;
            txtWeeklyhours.IsEnabled = show;
            txtWorkingdaysWeekly.IsEnabled = show;
            btnFunkZugeh.IsEnabled = show;
            rdbAdminYes.IsEnabled = show;
            rdbAdminNo.IsEnabled = show;

            btnAdd.IsEnabled = show;
            btnDel.IsEnabled = show;

            txtSVNr.IsEnabled = show;
            txtBankname.IsEnabled = show;
            txtAccountnr.IsEnabled = show;
            txtBLZ.IsEnabled = show;
            dateAnfang.IsEnabled = show;
            dateEnde.IsEnabled = show;

            button4.IsEnabled = show;
        }

        private void btnGetAdminData_Click(object sender, RoutedEventArgs e)
        {
            setadminenadisa(true);
            var a = new User();
            User b;


            var temps = cmbAdminUsers.Text.Split(' ');
            for (var i = 0; i < userlist.Count; i++)
            {
                b = userlist[i];
                if (temps[0] == b.Firstname && temps[1] == b.Lastname)
                {
                    a = userlist[i];
                    break;
                }
            }


            var temp = c.getStammdatenById(a.Kostalid);
            var allInfo = temp.Split('$');
            try
            {
                txtAccountnr.Text = allInfo[11];
                txtAdminFirstname.Text = allInfo[0];
                txtAdminLastname.Text = allInfo[1];
                txtBankname.Text = allInfo[9];
                txtBLZ.Text = allInfo[10];
                txtCity.Text = allInfo[6];
                txtFax.Text = allInfo[8];
                txtMailPw.Text = allInfo[13];
                txtMailUser.Text = allInfo[12];
                txtStreet.Text = allInfo[4];
                txtSVNr.Text = allInfo[3];
                txtTel.Text = allInfo[7];
                txtUsername.Text = allInfo[2];
                txtWeeklyhours.Text = allInfo[14];
                txtWorkingdaysWeekly.Text = allInfo[15];
                txtZip.Text = allInfo[5];
                txtAdminGeb.Text = allInfo[17];
                dateAnfang.SelectedDate = Convert.ToDateTime(allInfo[18]);
                try
                {
                    dateEnde.SelectedDate = Convert.ToDateTime(allInfo[19]);
                }
                catch
                {
                }
                if (allInfo[16] == "true")
                {
                    rdbAdminYes.IsChecked = true;
                    rdbAdminNo.IsChecked = false;
                }
                else
                {
                    rdbAdminYes.IsChecked = false;
                    rdbAdminNo.IsChecked = true;
                }

                var services = c.getServicesIdByUserId(a.Kostalid).Split('$');
                AllHouses = c.getServicesVital();
                SelectedHouses = new List<Service>();
                foreach (var house in services)
                {
                    foreach (var serv in AllHouses)
                    {
                        if (serv.Id == house)
                        {
                            AllHouses.Remove(serv);
                            SelectedHouses.Add(serv);
                            break;
                        }
                    }
                }

                dgHouseAll.ItemsSource = new List<Service>();
                dgHouseSelected.ItemsSource = new List<Service>();

                dgHouseAll.ItemsSource = AllHouses;
                dgHouseSelected.ItemsSource = SelectedHouses;

                setadminenadisa(true);
            }
            catch
            {
            }
        }

        private void dgImportantTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var t = dgImportantTasks.SelectedItem as Task;
                var et = new EditTask(false, false, t.von, t.zu, t.startdate, t.enddate, t.desc, c);
                et.Show();
                refreshAllTasks();
            }
            catch
            {
            }
        }

        private void AktualisierenAll_Click(object sender, RoutedEventArgs e)
        {
            refreshAllTasks();
        }

        private void btnAktualisierenDienstanweisungen_Click(object sender, RoutedEventArgs e)
        {
            refreshAllInstructions();
        }

        private void btnAktualisierenAufgaben_Click(object sender, RoutedEventArgs e)
        {
            refreshAllTasks();
        }

        private void dgCreatedTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var t = dgCreatedTasks.SelectedItem as Task;
                var et = new EditTask(true, false, t.von, t.zu, t.startdate, t.enddate, t.desc, c);
                et.Show();
                refreshAllTasks();
            }
            catch
            {
            }
        }

        private void btnGetKlientDaten_Click(object sender, RoutedEventArgs e)
        {
            sozialarbeiter = c.getSozialarbeiter();
            cmbContacts.ItemsSource = sozialarbeiter;
            cmbContacts.DisplayMemberPath = "Fullname";
            cmbContacts.SelectedIndex = 0;

            btnVitalSave.Visibility = Visibility.Hidden;
            cmbZugehoerigkeit.IsEnabled = false;
            rbman.IsEnabled = false;
            rbwoman.IsEnabled = false;
            txtVorname.IsEnabled = false;
            txtNachname.IsEnabled = false;
            dpAufnahmedatum.IsEnabled = false;
            dpAustrittsdatum.IsEnabled = false;
            dpGeburtsdatum.IsEnabled = false;
            txtStaatsbuergerschaft.IsEnabled = false;
            txtZuständigeBezirkshauptmannschaft.IsEnabled = false;
            txtVersicherungsträger.IsEnabled = false;
            txtICDCode.IsEnabled = false;
            txtGeburtsort.IsEnabled = false;
            txtSozialversicherungsnummer.IsEnabled = false;
            txtMitversichertBei.IsEnabled = false;
            cmbContacts.IsEnabled = false;
            rdbIntern.IsEnabled = false;
            rdbExtern.IsEnabled = false;
            rdbGericht.IsEnabled = false;
            rdbFrei.IsEnabled = false;
            btnGetKlientDaten.IsEnabled = true;

            rbman.IsChecked = false;
            rbwoman.IsChecked = false;
            txtVorname.Text = "";
            txtNachname.Text = "";
            dpAufnahmedatum.Text = "";
            dpAustrittsdatum.Text = "";
            dpGeburtsdatum.Text = "";
            txtStaatsbuergerschaft.Text = "";
            txtZuständigeBezirkshauptmannschaft.Text = "";
            txtVersicherungsträger.Text = "";
            txtICDCode.Text = "";
            txtGeburtsort.Text = "";
            txtSozialversicherungsnummer.Text = "";
            txtMitversichertBei.Text = "";
            rdbIntern.IsChecked = false;
            rdbExtern.IsChecked = false;
            rdbGericht.IsChecked = false;
            rdbFrei.IsChecked = false;
            cmbContacts.Text = "";
            cmbZugehoerigkeit.Text = "";

            try
            {
                string vid;
                if (cmbKlientAuswaehlen.Text != "")
                {
                    vid = c.getIdbyNameClients(cmbKlientAuswaehlen.SelectedValue.ToString());
                }
                else if (cmbKlientArchivAuswaehlen.Text != "")
                {
                    vid = c.getIdbyNameClients(cmbKlientArchivAuswaehlen.SelectedValue.ToString());
                }
                else
                {
                    MessageBox.Show("Wählen Sie einen Klienten aus!");
                    return;
                }


                var values = c.getVital(vid);
                if (values[0] == "0")
                {
                    rbman.IsChecked = true;
                }
                else
                {
                    rbwoman.IsChecked = true;
                }
                txtVorname.Text = values[1];
                txtNachname.Text = values[2];
                if (values[3] != "")
                {
                    dpAufnahmedatum.SelectedDate = DateTime.Parse(values[3]);
                }
                if (values[4] != "")
                {
                    dpGeburtsdatum.SelectedDate = DateTime.Parse(values[4]);
                }
                txtStaatsbuergerschaft.Text = values[5];
                txtZuständigeBezirkshauptmannschaft.Text = values[6];
                txtVersicherungsträger.Text = values[7];
                if (values[8] != "")
                {
                    dpAustrittsdatum.SelectedDate = DateTime.Parse(values[8]);
                }
                txtICDCode.Text = values[9];
                txtGeburtsort.Text = values[10];
                txtSozialversicherungsnummer.Text = values[11];
                txtMitversichertBei.Text = values[12];

                var sele = 0;
                foreach (var con in sozialarbeiter)
                {
                    if (con.id == values[13].Trim())
                    {
                        cmbContacts.SelectedIndex = sele;
                        break;
                    }
                    sele++;
                }


                selectedContact = values[13];
                cmbZugehoerigkeit.SelectedValue = c.getServiceIdbyClientId(vid);

                if (values[14] == "0")
                {
                    rdbIntern.IsChecked = true;
                }
                else if (values[14] == "1")
                {
                    rdbExtern.IsChecked = true;
                }

                if (values[15] == "0")
                {
                    rdbGericht.IsChecked = true;
                }
                else if (values[15] == "1")
                {
                    rdbFrei.IsChecked = true;
                }

                if (u.IsAdmin)
                {
                    cmbZugehoerigkeit.IsEnabled = true;
                    dpAustrittsdatum.IsEnabled = true;
                    btnNK.Visibility = Visibility.Visible;
                }
                rbman.IsEnabled = true;
                rbwoman.IsEnabled = true;
                txtVorname.IsEnabled = true;
                txtNachname.IsEnabled = true;
                dpAufnahmedatum.IsEnabled = true;
                dpGeburtsdatum.IsEnabled = true;
                txtStaatsbuergerschaft.IsEnabled = true;
                txtZuständigeBezirkshauptmannschaft.IsEnabled = true;
                txtVersicherungsträger.IsEnabled = true;
                txtICDCode.IsEnabled = true;
                txtGeburtsort.IsEnabled = true;
                txtSozialversicherungsnummer.IsEnabled = true;
                txtMitversichertBei.IsEnabled = true;
                cmbContacts.IsEnabled = true;
                rdbIntern.IsEnabled = true;
                rdbExtern.IsEnabled = true;
                rdbGericht.IsEnabled = true;
                rdbFrei.IsEnabled = true;
                btnSaveChanges.Visibility = Visibility.Visible;

                tbBericht.IsEnabled = true;


                tbDok.IsEnabled = true;
                tbFotos.IsEnabled = true;
                tbGuG.IsEnabled = true;
                tbMed.IsEnabled = true;
                tbMedAkt.IsEnabled = true;
                tbTg.IsEnabled = true;
                tbPad.IsEnabled = true;
            }
            catch
            {
            }

            try
            {
                if (cmbKlientAuswaehlen.SelectedIndex == -1)
                {
                    if (cmbKlientArchivAuswaehlen.SelectedIndex == -1)
                    {
                        MessageBox.Show("Bitte wählen Sie einen Klienten aus!", "Achtung!", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return;
                    }
                }

                if (cmbKlientAuswaehlen.SelectedIndex != -1)
                {
                    cmbFVGClient.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbTg.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbFVGClient.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbGuG.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmb_Klient_Doc.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmb_Klient_Foto.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbMA.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbMedicClient.SelectedItem = cmbKlientAuswaehlen.SelectedItem;
                    cmbPad.SelectedItem = cmbKlientAuswaehlen.SelectedItem;

                    try
                    {
                        cmbFVGDoc.Items.Clear();
                        Berichte = new List<Klienten_Berichte>();


                        var namen = cmbKlientAuswaehlen.SelectedItem.ToString().Split(' ');

                        foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 3))
                        {
                            Berichte.Add(item);
                            cmbFVGDoc.Items.Add(item.name);
                        }
                        foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 3))
                        {
                            Berichte.Add(item);
                            cmbFVGDoc.Items.Add(item.name);
                        }
                    }
                    catch
                    {
                    }
                }
                else if (cmbKlientArchivAuswaehlen.SelectedIndex != -1)
                {
                    cmbFVGClient.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbTg.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbFVGClient.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbGuG.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmb_Klient_Doc.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmb_Klient_Foto.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbMA.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbMedicClient.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;
                    cmbPad.SelectedItem = cmbKlientArchivAuswaehlen.SelectedItem;

                    try
                    {
                        cmbFVGDoc.Items.Clear();
                        Berichte = new List<Klienten_Berichte>();


                        var namen = cmbKlientAuswaehlen.SelectedItem.ToString().Split(' ');

                        foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 3))
                        {
                            Berichte.Add(item);
                            cmbFVGDoc.Items.Add(item.name);
                        }
                        foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 3))
                        {
                            Berichte.Add(item);
                            cmbFVGDoc.Items.Add(item.name);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnUrlaub_Click(object sender, RoutedEventArgs e)
        {
            var edit = new EditHolidayDialog(cmb_Month.SelectedIndex + 1,
                Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                    .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), Convert.ToInt32(u.Id));
            edit.setuser(u);
            edit.ShowDialog();
            if (edit.getSaved())
            {
                setdgvWorkingTime();
            }
        }

        private void btnUrlaubBest_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgvUrlaub.SelectedIndex;
            if (selected == -1)
            {
                MessageBox.Show("Kein Datensatz ausgewählt!");
            }
            else
            {
                try
                {
                    var wt = (WorkingTime) dgvUrlaub.SelectedItem;
                    var uid = Convert.ToInt32(c.getUserIDbyFullname(wt.username));
                    c.setHolidayverfied(uid, wt.datetimefrom, wt.datetimeto, 1, Convert.ToInt32(u.Id));
                    setdgvWorkingTime();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnWtLoeschen_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgvZeiterfassung.SelectedIndex;
            if (selected == -1)
            {
                MessageBox.Show("Kein Datensatz ausgewählt!");
            }
            else
            {
                try
                {
                    var wt = (WorkingTime) dgvZeiterfassung.SelectedItem;
                    var uid = Convert.ToInt32(c.getUserIDbyFullname(wt.username));
                    if (wt.datetimefrom.Year == DateTime.Now.Year && wt.datetimefrom.Month == DateTime.Now.Month)
                    {
                        c.delWorkingtime(uid, wt.datetimefrom, wt.datetimeto, wt.comment);
                        setdgvWorkingTime();
                    }
                    else
                    {
                        MessageBox.Show("Nicht im aktuellen Monat!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnUrlaubAbl_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgvUrlaub.SelectedIndex;
            if (selected == -1)
            {
                MessageBox.Show("Kein Datensatz ausgewählt!");
            }
            else
            {
                try
                {
                    var wt = (WorkingTime) dgvUrlaub.SelectedItem;

                    var uid = Convert.ToInt32(c.getUserIDbyFullname(wt.username));
                    c.setHolidayverfied(uid, wt.datetimefrom, wt.datetimeto, -1, Convert.ToInt32(u.Id));
                    setdgvWorkingTime();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnNS_Click(object sender, RoutedEventArgs e)
        {
            var ec = new EditContacts(u.Id, true);
            ec.ShowDialog();
            if (selectedContact != null)
            {
                cmbContacts.SelectedValue = selectedContact;
            }
        }


        public void setKlientsBrushes(Brush b)
        {
            txtVorname.Background = b;
            txtNachname.Background = b;
            txtStaatsbuergerschaft.Background = b;
            txtGeburtsort.Background = b;
            txtZuständigeBezirkshauptmannschaft.Background = b;
            dpGeburtsdatum.Background = b;
            rdbIntern.Background = b;
            rdbExtern.Background = b;
            cmbContacts.Background = b;
            txtICDCode.Background = b;
            txtSozialversicherungsnummer.Background = b;
            txtVersicherungsträger.Background = b;
            txtMitversichertBei.Background = b;
            dpAufnahmedatum.Background = b;
            dpAustrittsdatum.Background = b;
            rdbGericht.Background = b;
            rdbFrei.Background = b;
        }

        private void btnNK_Click(object sender, RoutedEventArgs e)
        {
            btnVitalSave.Visibility = Visibility.Visible;
            setKlientsBrushes(color1);
            cmbKlientArchivAuswaehlen.SelectedIndex = -1;
            cmbKlientArchivAuswaehlen.IsEnabled = false;
            cmbKlientAuswaehlen.SelectedIndex = -1;
            cmbKlientAuswaehlen.IsEnabled = false;

            cmbZugehoerigkeit.IsEnabled = true;
            rbman.IsEnabled = true;
            rbwoman.IsEnabled = true;
            txtVorname.IsEnabled = true;
            txtNachname.IsEnabled = true;
            dpAufnahmedatum.IsEnabled = true;
            dpAustrittsdatum.IsEnabled = true;
            dpGeburtsdatum.IsEnabled = true;
            txtStaatsbuergerschaft.IsEnabled = true;
            txtZuständigeBezirkshauptmannschaft.IsEnabled = true;
            txtVersicherungsträger.IsEnabled = true;
            txtICDCode.IsEnabled = true;
            txtGeburtsort.IsEnabled = true;
            txtSozialversicherungsnummer.IsEnabled = true;
            txtMitversichertBei.IsEnabled = true;
            cmbContacts.IsEnabled = true;
            rdbIntern.IsEnabled = true;
            rdbExtern.IsEnabled = true;
            rdbGericht.IsEnabled = true;
            rdbFrei.IsEnabled = true;
            cmbKlientAuswaehlen.IsEnabled = false;
            btnGetKlientDaten.IsEnabled = false;
            cmbKlientAuswaehlen.IsEnabled = false;

            rbman.IsChecked = false;
            rbwoman.IsChecked = false;
            txtVorname.Text = "";
            txtNachname.Text = "";
            dpAufnahmedatum.Text = "";
            dpAustrittsdatum.Text = "";
            dpGeburtsdatum.Text = "";
            txtStaatsbuergerschaft.Text = "";
            txtZuständigeBezirkshauptmannschaft.Text = "";
            txtVersicherungsträger.Text = "";
            txtICDCode.Text = "";
            txtGeburtsort.Text = "";
            txtSozialversicherungsnummer.Text = "";
            txtMitversichertBei.Text = "";
            rdbIntern.IsChecked = false;
            rdbExtern.IsChecked = false;
            rdbGericht.IsChecked = false;
            rdbFrei.IsChecked = false;
            cmbKlientAuswaehlen.Text = "";
            cmbContacts.Text = "";
            cmbZugehoerigkeit.Text = "";

            btnVitalSave.Visibility = Visibility.Visible;
            btnSaveChanges.Visibility = Visibility.Hidden;


            sozialarbeiter = c.getSozialarbeiter();
            cmbContacts.ItemsSource = sozialarbeiter;
            cmbContacts.DisplayMemberPath = "Fullname";
            cmbContacts.SelectedIndex = 0;

            rbman.IsChecked = true;
            cmbZugehoerigkeit.SelectedIndex = 0;
        }

        private void btnVitalSave_Click(object sender, RoutedEventArgs e)
        {
            var vitalschalter = true;
            var status = "";
            var assignment = "";
            var fail = false;

            txtVorname.Text.Replace(' ', '-');
            txtNachname.Text.Replace(' ', '-');

            var sex = "0";
            if (rbwoman.IsChecked == true)
            {
                sex = "1";
            }
            var vorname = "";
            txtVorname.Background = color1;
            if (!string.IsNullOrEmpty(txtVorname.Text))
            {
                vorname = txtVorname.Text;
            }
            else
            {
                txtVorname.Background = color2;
                fail = true;
            }
            var nachname = "";
            if (!string.IsNullOrEmpty(txtNachname.Text))
            {
                nachname = txtNachname.Text;
                txtNachname.Background = color1;
            }
            else
            {
                txtNachname.Background = color2;
                fail = true;
            }
            var aufnahmedatum = "";
            if (dpAufnahmedatum.SelectedDate != null)
            {
                aufnahmedatum = dpAufnahmedatum.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                dpAufnahmedatum.Background = color1;
            }
            else
            {
                dpAufnahmedatum.Background = color2;
                fail = true;
            }
            var geburtsdatum = "";
            if (dpGeburtsdatum.SelectedDate != null)
            {
                geburtsdatum = dpGeburtsdatum.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                dpGeburtsdatum.Background = color1;
            }
            else
            {
                dpGeburtsdatum.Background = color2;
                fail = true;
            }
            var staatsbuergerschaft = "";
            if (!string.IsNullOrEmpty(txtStaatsbuergerschaft.Text))
            {
                staatsbuergerschaft = txtStaatsbuergerschaft.Text;
                txtStaatsbuergerschaft.Background = color1;
            }
            else
            {
                txtStaatsbuergerschaft.Background = color2;
                fail = true;
            }
            var zustaendig = "";
            if (!string.IsNullOrEmpty(txtZuständigeBezirkshauptmannschaft.Text))
            {
                zustaendig = txtZuständigeBezirkshauptmannschaft.Text;
                txtZuständigeBezirkshauptmannschaft.Background = color1;
            }
            else
            {
                txtZuständigeBezirkshauptmannschaft.Background = color2;
                fail = true;
            }
            var versicherung = "";
            if (!string.IsNullOrEmpty(txtVersicherungsträger.Text))
            {
                versicherung = txtVersicherungsträger.Text;
                txtVersicherungsträger.Background = color1;
            }
            else
            {
                txtVersicherungsträger.Background = color2;
                fail = true;
            }
            var icd = "";
            if (!string.IsNullOrEmpty(txtICDCode.Text))
            {
                icd = txtICDCode.Text;
                txtICDCode.Background = color1;
            }
            var sozialversicherungsnummer = "";
            if (!string.IsNullOrEmpty(txtSozialversicherungsnummer.Text))
            {
                sozialversicherungsnummer = txtSozialversicherungsnummer.Text;
                txtSozialversicherungsnummer.Background = color1;
            }
            else
            {
                txtSozialversicherungsnummer.Background = color2;
                fail = true;
            }
            var mitversichert = "";
            if (!string.IsNullOrEmpty(txtMitversichertBei.Text))
            {
                mitversichert = txtVersicherungsträger.Text;
                txtMitversichertBei.Background = color1;
            }
            else
            {
                txtMitversichertBei.Background = color2;
                fail = true;
            }
            var geburtsort = "";
            if (!string.IsNullOrEmpty(txtGeburtsort.Text))
            {
                geburtsort = txtGeburtsort.Text;
                txtGeburtsort.Background = color1;
            }
            else
            {
                txtGeburtsort.Background = color2;
                fail = true;
            }
            var contacs = "null";
            if (cmbContacts.SelectedValue != null)
            {
                var con = (Contacts) cmbContacts.SelectedItem;
                contacs = con.id;
                cmbContacts.Background = color1;
            }
            else
            {
                cmbContacts.Background = color2;
                fail = true;
            }
            var zugehoerigkeit = "";
            if (cmbZugehoerigkeit.SelectedValue != null)
            {
                var serv = (Service) cmbZugehoerigkeit.SelectedItem;
                zugehoerigkeit = serv.Id;
                cmbZugehoerigkeit.Background = color1;
            }
            else
            {
                cmbZugehoerigkeit.Background = color2;
                fail = true;
                vitalschalter = false;
            }

            if (rdbIntern.IsChecked == true || rdbExtern.IsChecked == true)
            {
                if (rdbIntern.IsChecked == true)
                {
                    status = "0";
                    rdbIntern.Background = color1;
                }
                else
                {
                    status = "1";
                    rdbExtern.Background = color1;
                }
                rdbIntern.Background = color1;
                rdbExtern.Background = color1;
            }
            else
            {
                rdbExtern.Background = color2;
                rdbIntern.Background = color2;
                fail = true;
            }

            if (rdbGericht.IsChecked == true || rdbFrei.IsChecked == true)
            {
                if (rdbGericht.IsChecked == true)
                {
                    assignment = "0";
                    rdbGericht.Background = color1;
                }
                else
                {
                    assignment = "1";
                    rdbFrei.Background = color1;
                }
                rdbGericht.Background = color1;
                rdbFrei.Background = color1;
            }
            else
            {
                rdbFrei.Background = color2;
                rdbGericht.Background = color2;
                fail = true;
            }

            if (vitalschalter && status != "" && assignment != "" && fail == false)
            {
                if (c.setVital(sex, vorname, nachname, aufnahmedatum, geburtsdatum, staatsbuergerschaft, zustaendig,
                    versicherung, icd, geburtsort, sozialversicherungsnummer, mitversichert, contacs, zugehoerigkeit,
                    status, assignment))
                {
                    rbman.IsChecked = false;
                    rbwoman.IsChecked = false;
                    txtVorname.Text = "";
                    txtNachname.Text = "";
                    dpAufnahmedatum.Text = "";
                    dpAustrittsdatum.Text = "";
                    dpGeburtsdatum.Text = "";
                    txtStaatsbuergerschaft.Text = "";
                    txtZuständigeBezirkshauptmannschaft.Text = "";
                    txtVersicherungsträger.Text = "";
                    txtICDCode.Text = "";
                    txtGeburtsort.Text = "";
                    txtSozialversicherungsnummer.Text = "";
                    txtMitversichertBei.Text = "";
                    rdbIntern.IsChecked = false;
                    rdbExtern.IsChecked = false;
                    rdbGericht.IsChecked = false;
                    rdbFrei.IsChecked = false;
                    cmbKlientAuswaehlen.Text = "";
                    cmbContacts.Text = "";
                    cmbZugehoerigkeit.Text = "";
                    btnVitalSave.Visibility = Visibility.Hidden;
                    btnSaveChanges.Visibility = Visibility.Visible;
                    FillKidsIntoCombo();
                    cmbAdminUsers.Text = "";
                    btnGetKlientDaten.IsEnabled = true;
                    cmbKlientAuswaehlen.IsEnabled = true;

                    cmbKlientArchivAuswaehlen.SelectedIndex = -1;
                    cmbKlientArchivAuswaehlen.IsEnabled = true;
                    cmbZugehoerigkeit.IsEnabled = false;
                    rbman.IsEnabled = false;
                    rbwoman.IsEnabled = false;
                    txtVorname.IsEnabled = false;
                    txtNachname.IsEnabled = false;
                    dpAufnahmedatum.IsEnabled = false;
                    dpAustrittsdatum.IsEnabled = false;
                    dpGeburtsdatum.IsEnabled = false;
                    txtStaatsbuergerschaft.IsEnabled = false;
                    txtZuständigeBezirkshauptmannschaft.IsEnabled = false;
                    txtVersicherungsträger.IsEnabled = false;
                    txtICDCode.IsEnabled = false;
                    txtGeburtsort.IsEnabled = false;
                    txtSozialversicherungsnummer.IsEnabled = false;
                    txtMitversichertBei.IsEnabled = false;
                    cmbContacts.IsEnabled = false;
                    rdbIntern.IsEnabled = false;
                    rdbExtern.IsEnabled = false;
                    rdbGericht.IsEnabled = false;
                    rdbFrei.IsEnabled = false;
                    cmbKlientAuswaehlen.IsEnabled = true;
                    btnGetKlientDaten.IsEnabled = true;
                    cmbKlientAuswaehlen.IsEnabled = true;

                    rbman.IsChecked = false;
                    rbwoman.IsChecked = false;
                    txtVorname.Text = "";
                    txtNachname.Text = "";
                    dpAufnahmedatum.Text = "";
                    dpAustrittsdatum.Text = "";
                    dpGeburtsdatum.Text = "";
                    txtStaatsbuergerschaft.Text = "";
                    txtZuständigeBezirkshauptmannschaft.Text = "";
                    txtVersicherungsträger.Text = "";
                    txtICDCode.Text = "";
                    txtGeburtsort.Text = "";
                    txtSozialversicherungsnummer.Text = "";
                    txtMitversichertBei.Text = "";
                    rdbIntern.IsChecked = false;
                    rdbExtern.IsChecked = false;
                    rdbGericht.IsChecked = false;
                    rdbFrei.IsChecked = false;
                    cmbKlientAuswaehlen.Text = "";
                    cmbContacts.Text = "";
                    cmbZugehoerigkeit.Text = "";
                }
                else
                {
                    MessageBox.Show("Fehler beim Speichern!\n\nVersuchen Sie es in wenigen Momenten bitte erneut.",
                        "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (!vitalschalter && status != "" && assignment != "")
            {
                cmbKlientArchivAuswaehlen.SelectedIndex = -1;
                cmbKlientArchivAuswaehlen.IsEnabled = true;
                cmbZugehoerigkeit.IsEnabled = false;
                rbman.IsEnabled = false;
                rbwoman.IsEnabled = false;
                txtVorname.IsEnabled = false;
                txtNachname.IsEnabled = false;
                dpAufnahmedatum.IsEnabled = false;
                dpAustrittsdatum.IsEnabled = false;
                dpGeburtsdatum.IsEnabled = false;
                txtStaatsbuergerschaft.IsEnabled = false;
                txtZuständigeBezirkshauptmannschaft.IsEnabled = false;
                txtVersicherungsträger.IsEnabled = false;
                txtICDCode.IsEnabled = false;
                txtGeburtsort.IsEnabled = false;
                txtSozialversicherungsnummer.IsEnabled = false;
                txtMitversichertBei.IsEnabled = false;
                cmbContacts.IsEnabled = false;
                rdbIntern.IsEnabled = false;
                rdbExtern.IsEnabled = false;
                rdbGericht.IsEnabled = false;
                rdbFrei.IsEnabled = false;
                cmbKlientAuswaehlen.IsEnabled = true;
                btnGetKlientDaten.IsEnabled = true;
                cmbKlientAuswaehlen.IsEnabled = true;

                rbman.IsChecked = false;
                rbwoman.IsChecked = false;
                txtVorname.Text = "";
                txtNachname.Text = "";
                dpAufnahmedatum.Text = "";
                dpAustrittsdatum.Text = "";
                dpGeburtsdatum.Text = "";
                txtStaatsbuergerschaft.Text = "";
                txtZuständigeBezirkshauptmannschaft.Text = "";
                txtVersicherungsträger.Text = "";
                txtICDCode.Text = "";
                txtGeburtsort.Text = "";
                txtSozialversicherungsnummer.Text = "";
                txtMitversichertBei.Text = "";
                rdbIntern.IsChecked = false;
                rdbExtern.IsChecked = false;
                rdbGericht.IsChecked = false;
                rdbFrei.IsChecked = false;
                cmbKlientAuswaehlen.Text = "";
                cmbContacts.Text = "";
                cmbZugehoerigkeit.Text = "";
            }
            if (fail)
            {
                MessageBox.Show("Es müssen alle Pflichtfelder ausgefüllt werden!", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var vitalschalter = true;
            var status = "";
            var assignment = "";
            var fail = false;

            var sex = "0";
            if (rbwoman.IsChecked == true)
            {
                sex = "1";
            }
            var vorname = "";
            txtVorname.Background = color1;
            if (!string.IsNullOrEmpty(txtVorname.Text))
            {
                vorname = txtVorname.Text;
            }
            else
            {
                txtVorname.Background = color2;
                fail = true;
            }
            var nachname = "";
            if (!string.IsNullOrEmpty(txtNachname.Text))
            {
                nachname = txtNachname.Text;
                txtNachname.Background = color1;
            }
            else
            {
                txtNachname.Background = color2;
                fail = true;
            }
            var aufnahmedatum = "";
            if (dpAufnahmedatum.SelectedDate != null)
            {
                aufnahmedatum = dpAufnahmedatum.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                dpAufnahmedatum.Background = color1;
            }
            else
            {
                dpAufnahmedatum.Background = color2;
                fail = true;
            }
            var verlassendatum = "";
            if (dpAustrittsdatum.SelectedDate != null)
            {
                verlassendatum = dpAustrittsdatum.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                dpAustrittsdatum.Background = color1;
            }
            var geburtsdatum = "";
            if (dpGeburtsdatum.SelectedDate != null)
            {
                geburtsdatum = dpGeburtsdatum.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm");
                dpGeburtsdatum.Background = color1;
            }
            else
            {
                dpGeburtsdatum.Background = color2;
                fail = true;
            }
            var staatsbuergerschaft = "";
            if (!string.IsNullOrEmpty(txtStaatsbuergerschaft.Text))
            {
                staatsbuergerschaft = txtStaatsbuergerschaft.Text;
                txtStaatsbuergerschaft.Background = color1;
            }
            else
            {
                txtStaatsbuergerschaft.Background = color2;
                fail = true;
            }
            var zustaendig = "";
            if (!string.IsNullOrEmpty(txtZuständigeBezirkshauptmannschaft.Text))
            {
                zustaendig = txtZuständigeBezirkshauptmannschaft.Text;
                txtZuständigeBezirkshauptmannschaft.Background = color1;
            }
            else
            {
                txtZuständigeBezirkshauptmannschaft.Background = color2;
                fail = true;
            }
            var versicherung = "";
            if (!string.IsNullOrEmpty(txtVersicherungsträger.Text))
            {
                versicherung = txtVersicherungsträger.Text;
                txtVersicherungsträger.Background = color1;
            }
            else
            {
                txtVersicherungsträger.Background = color2;
                fail = true;
            }
            var icd = "";
            if (!string.IsNullOrEmpty(txtICDCode.Text))
            {
                icd = txtICDCode.Text;
                txtICDCode.Background = color1;
            }
            var sozialversicherungsnummer = "";
            if (!string.IsNullOrEmpty(txtSozialversicherungsnummer.Text))
            {
                sozialversicherungsnummer = txtSozialversicherungsnummer.Text;
                txtSozialversicherungsnummer.Background = color1;
            }
            else
            {
                txtSozialversicherungsnummer.Background = color2;
                fail = true;
            }
            var mitversichert = "";
            if (!string.IsNullOrEmpty(txtMitversichertBei.Text))
            {
                mitversichert = txtVersicherungsträger.Text;
                txtMitversichertBei.Background = color1;
            }
            else
            {
                txtMitversichertBei.Background = color2;
                fail = true;
            }
            var geburtsort = "";
            if (!string.IsNullOrEmpty(txtGeburtsort.Text))
            {
                geburtsort = txtGeburtsort.Text;
                txtGeburtsort.Background = color1;
            }
            else
            {
                txtGeburtsort.Background = color2;
                fail = true;
            }
            var contacs = "null";
            if (cmbContacts.SelectedValue != null)
            {
                var con = (Contacts) cmbContacts.SelectedItem;
                contacs = con.id;
                cmbContacts.Background = color1;
            }
            else
            {
                cmbContacts.Background = color2;
                fail = true;
            }
            var zugehoerigkeit = "";
            if (cmbZugehoerigkeit.SelectedValue != null)
            {
                var serv = (Service) cmbZugehoerigkeit.SelectedItem;
                zugehoerigkeit = serv.Id;
                cmbZugehoerigkeit.Background = color1;
            }
            else
            {
                cmbZugehoerigkeit.Background = color2;
                fail = true;
                vitalschalter = false;
            }

            if (rdbIntern.IsChecked == true || rdbExtern.IsChecked == true)
            {
                if (rdbIntern.IsChecked == true)
                {
                    status = "0";
                    rdbIntern.Background = color1;
                }
                else
                {
                    status = "1";
                    rdbExtern.Background = color1;
                }
                rdbIntern.Background = color1;
                rdbExtern.Background = color1;
            }
            else
            {
                rdbExtern.Background = color2;
                rdbIntern.Background = color2;
                fail = true;
            }

            if (rdbGericht.IsChecked == true || rdbFrei.IsChecked == true)
            {
                if (rdbGericht.IsChecked == true)
                {
                    assignment = "0";
                    rdbGericht.Background = color1;
                }
                else
                {
                    assignment = "1";
                    rdbFrei.Background = color1;
                }
                rdbGericht.Background = color1;
                rdbFrei.Background = color1;
            }
            else
            {
                rdbFrei.Background = color2;
                rdbGericht.Background = color2;
                fail = true;
            }


            if (vitalschalter && status != "" && assignment != "" && fail == false)
            {
                var vid = c.getIdbyNameClients(cmbKlientAuswaehlen.SelectedValue.ToString());

                c.changeVital(vid, sex, vorname, nachname, aufnahmedatum, geburtsdatum, staatsbuergerschaft, zustaendig,
                    versicherung, icd, geburtsort, sozialversicherungsnummer, mitversichert, contacs, zugehoerigkeit,
                    verlassendatum, status, assignment);

                rbwoman.IsEnabled = false;
                txtVorname.IsEnabled = false;
                txtNachname.IsEnabled = false;
                dpAufnahmedatum.IsEnabled = false;
                dpGeburtsdatum.IsEnabled = false;
                txtStaatsbuergerschaft.IsEnabled = false;
                txtZuständigeBezirkshauptmannschaft.IsEnabled = false;
                txtVersicherungsträger.IsEnabled = false;
                dpAustrittsdatum.IsEnabled = false;
                txtICDCode.IsEnabled = false;
                txtGeburtsort.IsEnabled = false;
                txtSozialversicherungsnummer.IsEnabled = false;
                txtMitversichertBei.IsEnabled = false;
                cmbContacts.IsEnabled = false;
                cmbZugehoerigkeit.IsEnabled = false;
                rdbIntern.IsEnabled = false;
                rdbExtern.IsEnabled = false;
                rdbGericht.IsEnabled = false;
                rdbFrei.IsEnabled = false;
            }
            if (fail)
            {
                MessageBox.Show("Es müssen alle Pflichtfelder ausgefüllt werden!", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void btnGetTg_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTg.SelectedValue != null)
            {
                var tgKlient = c.getIdbyNameClients(cmbTg.SelectedValue.ToString());
                txtTg.Text = c.getTaschengeld(tgKlient);
                var tglist = c.getTaschengeldDoku(tgKlient);
                dgTaschengeld.ItemsSource = tglist;
            }
            else
            {
                MessageBox.Show("Es muss ein Kient ausgewählt sein.", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void btnChangeTg_Click(object sender, RoutedEventArgs e)
        {
            if (txtTgDiff.Text != "")
            {
                string diff;
                var zeichen = '-';
                var conv = "";
                var temp = 10.2;
                var id = "";

                conv = txtTgDiff.Text.Replace(',', '.');
                conv = conv.Replace('+', ' ');
                conv = conv.Replace('-', ' ');
                diff = conv.Trim();


                try
                {
                    temp = double.Parse(diff, CultureInfo.InvariantCulture.NumberFormat);
                    if (!txtTgKommentar.Text.Equals(""))
                    {
                        if (rdbTgP.IsChecked == true)
                        {
                            zeichen = '+';
                        }
                        else
                        {
                            zeichen = '-';
                        }
                        id = c.getIdbyNameClients(cmbTg.SelectedValue.ToString());
                        c.setTaschengeld(id, diff, zeichen, txtTgKommentar.Text, c.getNameByID(u.Id, false));

                        txtTgDiff.Text = "";
                        txtTgKommentar.Text = "";

                        var tgKlient = c.getIdbyNameClients(cmbTg.SelectedValue.ToString());
                        txtTg.Text = c.getTaschengeld(tgKlient);
                        var tglist = c.getTaschengeldDoku(tgKlient);
                        dgTaschengeld.ItemsSource = tglist;
                    }
                    else
                    {
                        MessageBox.Show("Es muss ein Grund angegeben sein.", "Achtung", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Es dürfen nur Zahlen eingetragen werden.", "Achtung", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Es muss eine Differenz eingetragen sein.", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private bool isDecimal(double num, int nachkomma)
        {
            var eingabe = num.ToString();
            eingabe = eingabe.Replace('.', ',');
            var prove = eingabe.Split(',');

            if (prove.Length == 2)
            {
                if (prove[1].Length == nachkomma)
                {
                    return true;
                }
            }
            return false;
        }

        public DateTime getdatum()
        {
            var picker = new PDFDatePicker();
            picker.ShowDialog();
            if (picker.abb)
            {
                return new DateTime(2001, 1, 1);
            }
            return picker.datum;
        }

        private void btnPDFExport_Click(object sender, RoutedEventArgs e)
        {
            var datum = new DateTime(1990, 1, 1);
            while (datum.Year < 2000)
            {
                datum = getdatum();
            }
            if (datum.Year == 2001)
            {
                return;
            }

            pdfShit(datum, true);
        }

        public void pdfShit(DateTime DAAAAATUM, bool show)
        {
            //71 Zeilen pro pdf
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF(*.pdf)|*.pdf";
            saveFileDialog1.Title = "PDF Speichern";
            saveFileDialog1.FileName = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                       DateTime.Now.Day + "_Abrechnung";
            var result = saveFileDialog1.ShowDialog();
            if (!((saveFileDialog1.FileName == null) | (saveFileDialog1.FileName == "")) & (result == true))
            {
                var cafpdf = new CreateAFuckingPDF();
                var userIds = c.getUsers_working().ToArray();
                var pdfShizzle = "";
                pdfShizzle += DAAAAATUM.Month + "." + DAAAAATUM.Year + "\n";
                pdfShizzle +=
                    "Name                          |  Über | S & F | Nacht | Urlaub | Krank | Soll | Haben \n";
                foreach (var us in userIds)
                {
                    /*
             * [0] = Sollstunden
             * [1] = Überstunden
             * [2] = Urlaubsstunden
             * [3] = Habenstunden
             * [4] = Krankenstand 
             */
                    var tempUserData = getWorkinhDataForUserOida(us, DAAAAATUM);
                    pdfShizzle += c.getNameByID(us.Id, true);
                    for (var i = c.getNameByID(us.Id, false).Count(); i < 30; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += "|";

                    for (var i = tempUserData[1].Count(); i < 7; i++)
                    {
                        pdfShizzle += " ";
                    }


                    pdfShizzle += tempUserData[1];


                    pdfShizzle += "|";

                    var sf = sonUfeiertagsinterpreter(c.getSonnUFeier(Convert.ToInt32(us.Id), DAAAAATUM));


                    for (var i = sf.ToString().Count(); i < 7; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += sf.ToString();

                    pdfShizzle += "|";

                    for (var i = getnachtdienstetmp(Convert.ToInt32(us.Id), DAAAAATUM).ToString().Count(); i < 7; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += getnachtdienstetmp(Convert.ToInt32(us.Id), DAAAAATUM);


                    pdfShizzle += "|";

                    for (var i = tempUserData[2].Count(); i < 8; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += tempUserData[2];

                    pdfShizzle += "|";

                    for (var i = tempUserData[4].Count(); i < 7; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += tempUserData[4];


                    pdfShizzle += "|";

                    for (var i = tempUserData[0].Count(); i < 6; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += tempUserData[0];

                    pdfShizzle += "|";

                    for (var i = tempUserData[3].Count(); i < 6; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += tempUserData[3]
                                  + "\n";
                }
                cafpdf.text = pdfShizzle;
                cafpdf.show = show;
                cafpdf.pdfTitle = "Arbeitszeit Übersicht";
                cafpdf.truenamebro = saveFileDialog1.FileName;
                var oThread = new Thread(cafpdf.createThisShit2);
                oThread.Start();
            }
        }

        private int sonUfeiertagsinterpreter(string sf)
        {
            double std = 0;
            if (string.IsNullOrEmpty(sf))
            {
                return 0;
            }
            var arr = sf.Split('%');
            foreach (var a in arr)
            {
                if (!string.IsNullOrEmpty(a))
                {
                    var data = a.Split('$');

                    var von = Convert.ToDateTime(data[1]);
                    var bis = Convert.ToDateTime(data[2]);

                    var b = (bis - von).TotalHours;

                    if (data[0] == "Tagdienst")
                    {
                        std += b;
                    }
                    else if (data[0] == "Nachtdienst")
                    {
                        b = b / 2;
                        std += b;
                    }
                }
            }

            return Convert.ToInt32(std);
        }


        private void cmbFVGClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

            cmbFVGDoc.Items.Clear();

            foreach (var item in c.getFVGs(namen[0], namen[1]).Split('$'))
            {
                cmbFVGDoc.Items.Add(item);
            }
        }


        private void btngetGuG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var id = c.getIdbyNameClients(cmbGuG.SelectedValue.ToString());
                var bodylist = c.getBodyInfo(id);
                dgBodyInfo.ItemsSource = bodylist;

                label48.Visibility = Visibility.Visible;
                label49.Visibility = Visibility.Visible;
                btnSaveBodyInfo.Visibility = Visibility.Visible;
                txtSize.Visibility = Visibility.Visible;
                txtWeight.Visibility = Visibility.Visible;
            }
            catch
            {
                MessageBox.Show("Kein Klient ausgewählt", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnFvgGet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbFVGClient.SelectedIndex != -1 && cmbFVGDoc.SelectedIndex != -1 && cmbFVGDoc.Text != "")
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');
                    editFVG.ContentHtml = c.getBericht_Content(Berichte.ElementAt(cmbFVGDoc.SelectedIndex));
                    editFVG.IsEnabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Sie müssen sowohl einen Benutzer als auch ein Dokument auswählen!", "Achtung",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnSaveBodyInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSize.Text) || !string.IsNullOrWhiteSpace(txtWeight.Text))
            {
                if (txtSize.Text.Length < 6 || txtWeight.Text.Length < 6)
                {
                    try
                    {
                        var temp1 = double.Parse(txtSize.Text.Replace('.', ','));
                        var size = txtSize.Text.Replace(',', '.');
                        var temp2 = double.Parse(txtWeight.Text.Replace('.', ','));
                        var weight = txtWeight.Text.Replace(',', '.');
                        var id = c.getIdbyNameClients(cmbGuG.SelectedValue.ToString());

                        c.setBodyInfo(id, size, weight);

                        label48.Visibility = Visibility.Hidden;
                        label49.Visibility = Visibility.Hidden;
                        btnSaveBodyInfo.Visibility = Visibility.Hidden;
                        txtSize.Visibility = Visibility.Hidden;
                        txtWeight.Visibility = Visibility.Hidden;

                        var bodylist = c.getBodyInfo(id);
                        dgBodyInfo.ItemsSource = bodylist;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Es dürfen nur Zahlen eingetragen werden.", "Achtung", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Überprüfen Sie die Eingabe", "Achtung", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Es muss was eingetragen werden", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private int getnachtdienstetmp(int userid, DateTime datum)
        {
            var nachtdienste = 0;
            var date = new DateTime(datum.Year, datum.Month, 1);
            var fillings = c.getWorkingtime(date, userid, u.IsAdmin).Split('%');
            if (fillings.Count() == 0)
            {
                return 0;
            }
            for (var i = 0; i < fillings.Count(); i++)
            {
                if (fillings[i].Split('$')[0] != "")
                {
                    if (fillings[i].Split('$')[2] == "Nachtdienst")
                    {
                        if (!(fillings[i].Split('$')[5].StartsWith("Krankenstand") |
                              fillings[i].Split('$')[5].StartsWith("Seminar")))
                        {
                            nachtdienste++;
                        }
                    }
                }
            }

            return nachtdienste / 2;
        }

        private int getnachtdienste(int userid)
        {
            var nachtdienste = 0;

            var date =
                new DateTime(
                    Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                        .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1, 1);
            var fillings = c.getWorkingtime(date, userid, u.IsAdmin).Split('%');
            if (fillings.Count() > 0)
            {
                var index = -1;
                for (var i = 0; i < fillings.Count(); i++)
                {
                    if (fillings[i].Split('$')[2] == "Nachtdienst")
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                {
                    var lastend = Convert.ToDateTime(fillings[index].Split('$')[4]);
                    DateTime nextbegin;
                    for (var i = 0; i < fillings.Count(); i++)
                    {
                        if (fillings[i] != "")
                        {
                            nextbegin = Convert.ToDateTime(fillings[i].Split('$')[3]);
                            var line = fillings[i].Split('$');
                            if (line[2] == "Nachtdienst")
                            {
                                var diff = nextbegin - lastend;
                                MessageBox.Show(diff.TotalMinutes + " = " + nextbegin + " - " +
                                                lastend);
                                if (diff.TotalMinutes > 0)
                                {
                                    nachtdienste++;
                                }
                                lastend = nextbegin;
                            }
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

            return nachtdienste;
        }


        private string getdatensatz(string name, int feiertagstd, int nachtdienst, int urlaub, int krankenstand)
        {
            var temp = "";


            return temp;
        }

        private void btnnewMA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ema = new EditMedicalActions(u.Id, u.Services, cmbMA.SelectedValue.ToString(), c);
                ema.ShowDialog();
                btngetMA.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
            catch
            {
            }
        }


        private void btngetMA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mediid = c.getIdbyNameClients(cmbMA.SelectedValue.ToString());
                medihelpname = cmbMA.SelectedIndex != -1
                    ? cmbMA.SelectedValue.ToString()
                    : cmbKlientArchivAuswaehlen.SelectedValue.ToString();
                var medilist = c.GetClientMed(mediid);
                dgmedicalaction.ItemsSource = medilist;
            }
            catch (Exception)
            {
            }
        }

        private void dgmedicalaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var ma = dgmedicalaction.SelectedItem as MediAkt;
                var et = new EditMedicalActions(medihelpname, ma.date, ma.art, ma.desc, true, c);
                et.ShowDialog();
                if (et.del)
                {
                    c.DeleteMediActionForClient(c.getIdbyNameClients(cmbMA.SelectedValue.ToString()), ma.date, ma.art,
                        ma.desc);
                    dgmedicalaction.Items.Remove(dgmedicalaction.SelectedItem);
                    try
                    {
                        dgmedicalaction.ItemsSource = new List<MediAkt>();
                        var mediid = c.getIdbyNameClients(cmbMA.SelectedValue.ToString());
                        medihelpname = cmbMA.SelectedIndex != -1
                            ? cmbMA.SelectedValue.ToString()
                            : cmbKlientArchivAuswaehlen.SelectedValue.ToString();
                        var medilist = c.GetClientMed(mediid);
                        dgmedicalaction.ItemsSource = medilist;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btngetKmG_Click(object sender, RoutedEventArgs e)
        {
            var month = "0";
            var year = "0";

            try
            {
                month = (int.Parse(cmbKMGMonth.SelectedIndex.ToString()) + 1).ToString();
                year = cmbKMGYear.Text;
            }
            catch (Exception)
            {
            }

            if (!u.IsAdmin)
            {
                cmbUserKmG.Text = c.getNameByID(u.Id, false);
                cmbUserKmG.SelectedValue = u.Id;
            }

            if (cmbUserKmG.Text != "")
            {
                if (cmbKMGMonth.Text != "")
                {
                    if (cmbKMGYear.Text != "")
                    {
                        try
                        {
                            refreshKmG(cmbUserKmG.SelectedValue.ToString());
                            if (cmbUserKmG.Text.EndsWith("*"))
                            {
                                lblKMGSumme.Content =
                                    c.getKMGSumme(c.getIdbyName(cmbUserKmG.Text.Remove(cmbUserKmG.Text.Count() - 2)),
                                        month, year);
                            }
                            else
                            {
                                lblKMGSumme.Content = c.getKMGSumme(c.getIdbyName(cmbUserKmG.Text), month, year);
                            }
                            lblKMGSumme_eur.Content =
                                Convert.ToDouble(c.getKMGSumme(c.getIdbyName(cmbUserKmG.Text), month, year)) / 0.42;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es muss ein Jahr ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Es muss ein Monat ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Es muss ein Benutzer ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            btnKMGActiveEx.IsEnabled = true;
        }

        public void updatecmbUserKmG(string year, string month)
        {
            new Thread(() => { }).Start();
            cmbUserKmG.ItemsSource = null;
            if (Functions.EmployeeList != null)
            {
                var newlist = new List<Employee>();
                foreach (var em in Functions.EmployeeList)
                {
                    var n = new Employee(em.FirstName, em.LastName);
                    n.FullName = em.FullName;
                    n.Id = em.Id;
                    n.LastLogin = em.LastLogin;
                    n.Start = em.Start;
                    n.User = em.User;
                    newlist.Add(n);
                }

                foreach (var e in newlist)
                {
                    var a = e.FullName.Trim().Split(' ');
                    var Firstname = a[0];
                    var Lastname = a[1];


                    if (Convert.ToDouble(c.getKMGSumme(c.getIdbyName(e.FullName), month, year)) > 0)
                    {
                        e.FullName += "*";
                    }
                    else
                    {
                        if (e.FullName.EndsWith("*"))
                        {
                            e.FullName.Replace('*', ' ');
                            e.FullName.Trim();
                        }
                    }
                }

                cmbUserKmG.ItemsSource = newlist;
            }
            else
            {
                cmbUserKmG.ItemsSource = Functions.EmployeeList;
            }
            cmbUserKmG.DisplayMemberPath = "FullName";
            cmbUserKmG.SelectedValuePath = "Id";
        }

        private void fillcmbUserKmG(string year, string month)
        {
            new Thread(() => { }).Start();
            cmbUserKmG.ItemsSource = null;
            if (Functions.EmployeeList != null)
            {
                var newlist = new List<Employee>();
                foreach (var em in Functions.EmployeeList)
                {
                    var n = new Employee(em.FirstName, em.LastName);
                    n.FullName = em.FullName;
                    n.Id = em.Id;
                    n.LastLogin = em.LastLogin;
                    n.Start = em.Start;
                    n.User = em.User;
                    newlist.Add(n);
                }

                foreach (var e in newlist)
                {
                    var a = e.FullName.Trim().Split(' ');
                    var Firstname = a[0];
                    var Lastname = a[1];


                    if (Convert.ToDouble(c.getKMGSumme(c.getIdbyName(e.FullName), month, year)) > 0)
                    {
                        e.FullName += "*";
                    }
                    else
                    {
                        if (e.FullName.EndsWith("*"))
                        {
                            e.FullName.Replace('*', ' ');
                            e.FullName.Trim();
                        }
                    }
                }

                cmbUserKmG.ItemsSource = newlist;
            }
            else
            {
                cmbUserKmG.ItemsSource = Functions.EmployeeList;
            }
            cmbUserKmG.DisplayMemberPath = "FullName";
            cmbUserKmG.SelectedValuePath = "Id";
        }

        private void btnBerNew_Click(object sender, RoutedEventArgs e)
        {
            lblBerichtVorlage.Content = "Vorlage auswählen";
            btnBerLoad.Content = "Speichern";
        }

        private void btnBerLoad_Click(object sender, RoutedEventArgs e)
        {
            if (btnBerLoad.Content.Equals("Vorlage auswählen"))
            {
                // Ein neuer Bericht soll angelegt werden!
            }
        }

        private void btnSaveKmg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtKilometer.Background = color1;
                txtKilometer.Background = color1;
                dpZeitbis.Background = color1;
                dpZeitvon.Background = color1;
                var zeitvon = dpZeitvon.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + txtZeitvon.Text;
                var zeitbis = dpZeitbis.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + txtZeitbis.Text;
                if (int.Parse(txtKilometer.Text) <= 0)
                {
                    MessageBox.Show("Die Kilometer dürfen nicht negativ oder 0 sein", "Achtung", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    txtKilometer.Background = color2;
                    return;
                }
                if (dpZeitvon.SelectedDate > dpZeitbis.SelectedDate)
                {
                    MessageBox.Show("Die von Zeit darf nicht größer als die bis Zeit sein", "Achtung",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    dpZeitbis.Background = color2;
                    dpZeitvon.Background = color2;
                    return;
                }
                var summe = (double.Parse(txtKilometer.Text) * 0.42).ToString();
                summe = summe.Replace(",", ".");

                c.setKilometerGeld(u.Id, txtKennzeichen.Text, txtOrtvon.Text, txtOrtbis.Text, zeitvon, zeitbis, summe,
                    txtKilometer.Text);

                txtKilometer.Text = null;
                txtOrtbis.Text = null;
                txtOrtvon.Text = null;
                txtZeitbis.Text = null;
                txtZeitvon.Text = null;
                txtVerfasser.Text = null;
                dpZeitbis.SelectedDate = null;
                dpZeitvon.SelectedDate = null;

                refreshKmG(u.Id);
            }
            catch (FormatException)
            {
                MessageBox.Show("Es wurde keine Zahl im Kilometer Feld eingetragen", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtKilometer.Background = color2;
            }
            catch (Exception)
            {
                MessageBox.Show("Ein Feld wurde nicht richtig befüllt", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void button3_Click_2(object sender, RoutedEventArgs e)
        {
            c.InsertFirstPMoney(u.Id);
        }

        private void bttnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        public List<WorkingTime> setpdfWorkingTime(DateTime date, User a)
        {
            try
            {
                set = true;

                string[] fillings;
                fillings = c.getWorkingtime(date, a.Kostalid, u.IsAdmin).Split('%');

                var arbeit = c.getWorkingDays(a.Id);

                if (arbeit == "5")
                {
                    a.WorkingTimeType = 1;
                }
                else
                {
                    a.WorkingTimeType = 2;
                }


                var tab = new DataTable();
                var columnnames = new string[5] {"User", "Art", "Von-Datum", "Bis-Datum", "Kommentar"};
                IFormatProvider culture = new CultureInfo("de-DE", true);
                var time2 = new List<WorkingTime>();
                holidaydata = new List<WorkingTime>();
                foreach (var s in fillings)
                {
                    var temp = s.Split('$');
                    if (temp.Length == 7)
                    {
                        if (temp[2] == "Urlaub")
                        {
                            if ((temp[6] == "True") | (temp[6] == "1"))
                            {
                                time2.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                    true));
                            }
                            else if ((temp[6] == "False") | (temp[6] == "0"))
                            {
                                //holidaydata.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5], false));
                            }
                        }
                        else
                        {
                            time2.Add(new WorkingTime(temp[0] + " " + temp[1], temp[2], temp[3], temp[4], temp[5],
                                false));
                        }
                    }
                }

                //dgvZeiterfassung.ItemsSource = time;
                //dgvUrlaub.ItemsSource = holidaydata;

                //soll-stunden
                double sollstd = 0;
                //Hier werden die Feiertage von der Datenbank geholt
                var holidaysstring = c.getNonWorkingDays();
                if (holidaysstring != null)
                {
                    var holidays = holidaysstring.Split('%');
                    //Mit dieser Variable wird geprüft, ob der ausgewählte Tag ein Feiertag ist
                    var isholday = false;
                    DateTime sampleday;
                    if (a.WorkingTimeType == 1)
                    {
                        var days = DateTime.DaysInMonth(
                            Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                            cmb_Month.SelectedIndex + 1);
                        DateTime dt;
                        for (var counter = 1; counter <= days; counter++)
                        {
                            dt = new DateTime(
                                Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                    .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                                cmb_Month.SelectedIndex + 1, counter);
                            //Wenn der Tag kein Samstag oder Sontag ist zähle!
                            if (dt.ToString("ddd") != "Sa." && dt.ToString("ddd") != "So.")
                            {
                                //Alle Feiertage werden durch gegangen
                                for (var i = 0; i < holidays.Length; i++)
                                {
                                    if (holidays[i] != "")
                                    {
                                        sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                        if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                            sampleday.Year == dt.Year)
                                        {
                                            //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                            isholday = true;
                                        }
                                    }
                                }
                                if (!isholday)
                                {
                                    //Sollstunden werden erhöht, wenn es kein Feiertag ist
                                    sollstd += 1;
                                }
                            }
                            //Die Variable wird zurück gesetzt
                            isholday = false;
                        }
                        sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 5);
                        var tempusvaletsemper = sollstd.ToString();
                        if (tempusvaletsemper.Contains(','))
                        {
                            try
                            {
                                tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                            }
                            catch
                            {
                            }
                        }
                        pdf_soll = tempusvaletsemper;
                    }
                    else if (a.WorkingTimeType == 2)
                    {
                        var days = DateTime.DaysInMonth(
                            Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                            cmb_Month.SelectedIndex + 1);
                        DateTime dt;
                        for (var counter = 1; counter <= days; counter++)
                        {
                            dt = new DateTime(
                                Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                                    .Substring(cmb_Year.SelectedValue.ToString().Length - 5)),
                                cmb_Month.SelectedIndex + 1, counter);
                            if (dt.ToString("ddd") != "So.")
                            {
                                for (var i = 0; i < holidays.Length; i++)
                                {
                                    if (holidays[i] != "")
                                    {
                                        sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                        if (sampleday.Day == dt.Day && sampleday.Month == dt.Month &&
                                            sampleday.Year == dt.Year)
                                        {
                                            isholday = true;
                                        }
                                    }
                                }
                                if (!isholday)
                                {
                                    sollstd += 1;
                                }
                            }
                            isholday = false;
                        }
                        sollstd = sollstd * (c.getWorkinghoursperWeek(a.Id) / 6);
                        var tempusvaletsemper = sollstd.ToString();
                        if (tempusvaletsemper.Contains(','))
                        {
                            try
                            {
                                tempusvaletsemper = tempusvaletsemper.Substring(0, tempusvaletsemper.IndexOf(',') + 3);
                            }
                            catch
                            {
                            }
                        }
                        pdf_soll = tempusvaletsemper;
                    }

                    //haben-stunden

                    double habenstd = 0;
                    var urlaub = c.getUrlaubstime(Convert.ToInt32(a.Id));
                    double urlaubstd = 0;
                    if (urlaub != "NULL")
                    {
                        try
                        {
                            urlaubstd = Convert.ToDouble(urlaub.Substring(0, urlaub.IndexOf(',') + 2));
                        }
                        catch
                        {
                            /**/
                            /**/
                            try
                            {
                                urlaubstd = Convert.ToInt32(urlaub.Substring(0, urlaub.IndexOf(',')));
                            }
                            catch
                            {
                            }
                        }
                    }
                    double workinghours = 0;
                    if (a.WorkingTimeType == 1)
                    {
                        workinghours = c.getWorkinghoursperWeek(a.Id) / 5;
                    }
                    else
                    {
                        workinghours = c.getWorkinghoursperWeek(a.Id) / 6;
                    }
                    double krankenstunden = 0;
                    foreach (var wt in time)
                    {
                        var worktime = getworkingtime(wt);
                        var urlaubtime = geturlaubtime(wt, workinghours);
                        urlaubstd -= geturlaubsTage(wt); //urlaubtime.TotalHours;
                        habenstd += urlaubtime.TotalHours;
                        habenstd += worktime.TotalHours;
                        if (wt.comment.StartsWith("Krankenstand - "))
                        {
                            krankenstunden += worktime.TotalHours;
                        }
                    }
                    double uberstd = 0;
                    if (sollstd - habenstd <= 0)
                    {
                        uberstd = (sollstd - habenstd) * -1;
                        habenstd = habenstd - uberstd;
                    }
                    else
                    {
                        uberstd = habenstd - sollstd;
                    }
                    var tmp2 = uberstd.ToString();
                    if (tmp2.Contains(','))
                    {
                        try
                        {
                            tmp2 = tmp2.Substring(0, tmp2.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    pdf_uber = tmp2;
                    var tmp = urlaubstd.ToString();
                    if (urlaubstd.ToString().Contains(','))
                    {
                        try
                        {
                            tmp = urlaubstd.ToString().Substring(0, urlaubstd.ToString().IndexOf(',') + 5);
                        }
                        catch
                        {
                        }
                    }
                    pdf_url = tmp;
                    var tmp4 = habenstd.ToString();
                    if (tmp4.Contains(','))
                    {
                        try
                        {
                            tmp4 = tmp4.Substring(0, tmp4.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    pdf_haben = tmp4;

                    var tmp5 = krankenstunden.ToString();
                    if (tmp5.Contains(','))
                    {
                        try
                        {
                            tmp5 = tmp5.Substring(0, tmp5.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    pdf_Krank = tmp5;
                    //test area
                    var tempp = c.getUberstdges(a.Kostalid);
                    if (string.IsNullOrEmpty(tempp) | string.IsNullOrWhiteSpace(tempp))
                    {
                        tempp = "0";
                    }
                    var tmp3 = (uberstd + Convert.ToDouble(tempp)).ToString();
                    //string tmp3 = (uberstd + Convert.ToDouble(c.getUberstdges(a.Kostalid))).ToString();
                    //test area
                    if (tmp3.Contains(','))
                    {
                        try
                        {
                            tmp3 = tmp3.Substring(0, tmp3.IndexOf(',') + 3);
                        }
                        catch
                        {
                        }
                    }
                    pdf_uberganz = tmp3;
                }
                return time2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }


        public void pdfShitIndividual(DateTime DAAAAATUM, List<WorkingTime> wt, bool show)
        {
            if (wt.Count == 0)
            {
                MessageBox.Show("Keine Einträge");
                return;
            }
            //71 Zeilen pro pdf
            var saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PDF(*.pdf)|*.pdf";
            saveFileDialog1.Title = "PDF Speichern";
            var eins = wt[0];
            saveFileDialog1.FileName = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                       DateTime.Now.Day + "_" + eins.username.Replace(' ', '_') +
                                       "_Abrechnung";
            var result = saveFileDialog1.ShowDialog();
            if (!((saveFileDialog1.FileName == null) | (saveFileDialog1.FileName == "")) & (result == true))
            {
                var cafpdf = new CreateAFuckingPDF();
                var pdfShizzle = "";
                pdfShizzle += DAAAAATUM.Month + "." + DAAAAATUM.Year + " - " + eins.username +
                              "\n";
                pdfShizzle += "Art     | Von                  | Bis                  | Kommentar \n";
                foreach (var item in wt)
                {
                    /*
                    pdfShizzle += item.username;
                    for (int i = item.username.Count(); i < 30; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += "|";
                    */
                    for (var i = item.art.Count(); i < 6; i++)
                    {
                        pdfShizzle += " ";
                    }

                    if (item.art.StartsWith("Tag"))
                    {
                        pdfShizzle += "Tag     ";
                    }
                    else if (item.art.StartsWith("Nacht"))
                    {
                        pdfShizzle += "Nacht   ";
                    }
                    else if (item.art.StartsWith("Krank"))
                    {
                        pdfShizzle += "Krank   ";
                    }
                    else if (item.art.StartsWith("Semi"))
                    {
                        pdfShizzle += "Seminar ";
                    }
                    else if (item.art.StartsWith("Url"))
                    {
                        pdfShizzle += "Urlaub  ";
                    }


                    pdfShizzle += "| ";

                    for (var i = item.datetimefrom.ToString().Count(); i < 19; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += item.datetimefrom.ToString();

                    pdfShizzle += "  | ";

                    for (var i = item.datetimeto.ToString().Count(); i < 19; i++)
                    {
                        pdfShizzle += " ";
                    }

                    pdfShizzle += item.datetimeto.ToString();


                    pdfShizzle += "  | ";

                    if (item.comment.Count() <= 90)
                    {
                        pdfShizzle += item.comment + "\n";
                    }
                    else
                    {
                        pdfShizzle += item.comment.Substring(0, 90) + "\n";
                        pdfShizzle += item.comment.Substring(91) + "\n";
                    }
                }
                if (set) //ein benutzer
                {
                    pdfShizzle += "\n";
                    pdfShizzle += "Soll-Stunden: " + pdf_soll + "\n";
                    pdfShizzle += "Haben-Stunden: " + pdf_haben + "\n";
                    pdfShizzle += "Überstunden: " + pdf_uber + "\n";
                    pdfShizzle += "Urlaubstage: " + pdf_url + "\n";
                    pdfShizzle += "Krankenstandsstunden: " + pdf_Krank + "\n";
                    pdfShizzle += "Gesamt Überstunden: " + pdf_uberganz;
                    set = false;
                }
                else //alle benutzer
                {
                    pdfShizzle += "\n";
                    pdfShizzle += "Soll-Stunden: " + SollStunden.Content + "\n";
                    pdfShizzle += "Haben-Stunden: " + HabenStunden.Content + "\n";
                    pdfShizzle += "Überstunden: " + Uberstunden.Content + "\n";
                    pdfShizzle += "Urlaubstage: " + lblUrlaub.Content + "\n";
                    pdfShizzle += "Krankenstandsstunden: " + lblKrank.Content + "\n";
                    pdfShizzle += "Gesamt Überstunden: " + uberstdges.Content;
                }
                cafpdf.text = pdfShizzle;
                cafpdf.pdfTitle = "Arbeitszeit (" + eins.username + ")";
                cafpdf.show = show;
                cafpdf.truenamebro = saveFileDialog1.FileName;
                cafpdf.show = show;
                var oThread = new Thread(cafpdf.createThisShit2);
                oThread.Start();
                //cafpdf.createThisShit(pdfShizzle, saveFileDialog1.FileName); // getnachtdienste (userid), getSonnUndFeiertage (userid),
            }
        }

        private void btnUserPDF_Click(object sender, RoutedEventArgs e)
        {
            if (cmbworkingTimeUser.SelectedIndex != 0)
            {
                var date =
                    new DateTime(
                        Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                            .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1, 1);
                pdfShitIndividual(date, time, true);
            }
        }

        private void btnAllUsersPDF_Click(object sender, RoutedEventArgs e)
        {
            var datum = new DateTime(1990, 1, 1);
            while (datum.Year < 2000)
            {
                datum = getdatum();
            }
            if (datum.Year == 2001)
            {
                return;
            }

            var ids = c.getUsers_working();
            var asdasd = new List<User>();
            foreach (var us in ids)
            {
                //184
                var name = c.getNameByID(us.Id, false);
                var arr = name.Split(' ');
                if (!((arr[0] == "") | (arr[0] == " ") | (arr[1] == "") | (arr[1] == " ")))
                {
                    us.Firstname = arr[0];
                    us.Lastname = arr[1];
                    us.Kostalid = Convert.ToInt32(us.Id);
                    asdasd.Add(us);
                }
            }
            for (var i = 1; i < asdasd.Count; i++)
            {
                var usssers = setpdfWorkingTime(datum, asdasd[i]);
                if (usssers.Count != 0)
                {
                    pdfShitIndividual(datum, usssers, false);
                }
                set = false;
            }
            MessageBox.Show("Fertig");
        }

        private void dgvZeiterfassung_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var wt = (WorkingTime) dgvZeiterfassung.SelectedItem;
            if (wt.art != "Urlaub")
            {
                var ts = wt.datetimeto - wt.datetimefrom;
                MessageBox.Show("Stunden: " + ts.TotalHours);
            }
            else
            {
                var uid = Convert.ToInt32(c.getUserIDbyFullname(wt.username));
                var asdsdfsdfsdfvdcb = c.getName(uid, wt.datetimefrom, wt.datetimeto, wt.comment);
                var name = c.getNameByID(asdsdfsdfsdfvdcb, false);
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Genehmigt von: keine Angabe");
                }
                else
                {
                    MessageBox.Show("Genehmigt von: " + name);
                }
            }
        }

        private void DeleteAllCookies()
        {
            //Set the current user cookie to have expired yesterday
            var cookie = string.Format("c_user=; expires={0:R}; path=/; domain=.google.com",
                DateTime.UtcNow.AddDays(-1).ToString("R"));
            System.Windows.Application.SetCookie(new Uri("https://www.google.com/calendar?hl=de"), cookie);
        }

        private void calendarCancelPress()
        {
        }

        private void btn_Doc_Akt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vid = c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString());

                if (!(string.IsNullOrEmpty(vid) | string.IsNullOrWhiteSpace(vid)))
                {
                    //Thread th = new Thread(() => { update_docs(Convert.ToInt32(vid)); });
                    //MessageBox.Show("Bitte warten...");
                    //th.Start();
                    //th.Join();
                    update_docs(Convert.ToInt32(vid));
                    //dgv_Doc_List.ItemsSource = doclist;
                }
            }
            catch
            {
                MessageBox.Show("Wählen Sie einen Klienten aus!");
            }
        }

        public void update_docs(int id)
        {
            var docs = c.GetClientdoku(id, false);
            doclist = new List<Document>();
            foreach (var doc in docs)
            {
                doclist.Add(doc);
            }
            dgv_Doc_List.ItemsSource = doclist;
        }

        private void btnNH_Click(object sender, RoutedEventArgs e)
        {
            var eh = new EditHaus(u.Id, c);
            eh.ShowDialog();
            RefreshService();
        }

        private void btn_Doc_Down_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_Doc_List.SelectedIndex != -1)
            {
                var doc = (Document) dgv_Doc_List.SelectedItem;

                var saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = doc.path.Substring(doc.path.LastIndexOf('/') + 1);
                var result = saveFileDialog1.ShowDialog();
                var tmp = false;
                if (!((saveFileDialog1.FileName == null) | (saveFileDialog1.FileName == "")) & (result == true))
                {
                    var ftp = new FtpHandler();
                    if (ftp.DownloadFile(doc.path, saveFileDialog1.FileName))
                    {
                        if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                tmp = true;
                                Process.Start(saveFileDialog1.FileName).WaitForExit();
                            }
                            catch
                            {
                            }
                        }
                    }
                    if (tmp == false)
                    {
                    }
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btn_Doc_Up_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.ShowDialog();
            var tit = new Title(c);
            tit.ShowDialog();
            if (tit.titel != "-1")
            {
                var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);


                var ftp = new FtpHandler();
                var ergeb = ftp.UploadFile(ofd.FileName,
                    "data/clients/" + Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())) +
                    "/documents/" + name);
                if (ergeb)
                {
                    var a = new FileInfo(ofd.FileName).Length;

                    var b = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / 1024)));


                    c.addpath(name, Convert.ToInt32(u.Id),
                        Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())), tit.titel, b);
                }
                update_docs(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())));
            }
        }

        private void btn_wiki_Up_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            var result = ofd.ShowDialog();
            if (result == true)
            {
                var tit = new Title(c);
                tit.ShowDialog();
                if (tit.titel != "-1")
                {
                    var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);


                    var ftp = new FtpHandler();
                    ftp.UploadFile(ofd.FileName, "data/wiki/" + name);

                    var a = new FileInfo(ofd.FileName).Length;

                    var b = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / 1024)));


                    c.addwiki(name, Convert.ToInt32(u.Id), tit.titel);
                    loadWiki();
                }
            }
        }


        private void btn_Doc_Ers_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_Doc_List.SelectedIndex != -1)
            {
                var doc = (Document) dgv_Doc_List.SelectedItem;

                var ofd = new OpenFileDialog();
                ofd.ShowDialog();
                var tit = new Title(c);
                tit.ShowDialog();
                if (tit.titel != "-1")
                {
                    var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);


                    var ftp = new FtpHandler();
                    ftp.UploadFile(ofd.FileName,
                        "data/clients/" +
                        Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())) + "/documents/" +
                        name);

                    var a = new FileInfo(ofd.FileName).Length;

                    var b = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / 1024)));


                    c.updatePath(doc, name, Convert.ToInt32(u.Id),
                        Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())), tit.titel, b);
                    update_docs(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())));
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btn_Zeit_Kilometer_Click(object sender, RoutedEventArgs e)
        {
            setdgvWorkingTime();

            var date =
                new DateTime(
                    Convert.ToInt32(cmb_Year.SelectedValue.ToString()
                        .Substring(cmb_Year.SelectedValue.ToString().Length - 5)), cmb_Month.SelectedIndex + 1, 1);

            var a = new User();
            User b;
            if (cmbworkingTimeUser.Text != "Alle User")
            {
                var temps = cmbworkingTimeUser.Text.Split(' ');
                for (var i = 0; i < userlist.Count; i++)
                {
                    b = userlist[i];
                    if (temps[0].Trim() == b.Firstname && temps[1].Trim() == b.Lastname)
                    {
                        a = userlist[i];
                        a.Id = userlist[i].Kostalid.ToString();
                        a.Kostalid = userlist[i].Kostalid;
                        break;
                    }
                }
            }
            else
            {
                a.Kostalid = 0;
                a.Id = "0";
            }

            if (a.Kostalid == 0)
            {
                var list = new List<WorkingTime>();
                list = (List<WorkingTime>) dgvZeiterfassung.ItemsSource;
                var namen = new List<string>();
                var ausgabe = "Folgende Mitarbeiter haben im " + cmb_Month.Text + " " + cmb_Year.Text +
                              " Kilometergeld eingetragen: \n";
                var id = "";
                foreach (var wt in list)
                {
                    if (!namen.Contains(wt.username))
                    {
                        namen.Add(wt.username);
                        id = c.getUserIDbyFullname(wt.username);
                        if (c.getKilometerGeldset(Convert.ToInt32(id), date.Year, date.Month))
                        {
                            ausgabe += wt.username + "\n";
                        }
                    }
                }
                MessageBox.Show(ausgabe);
            }
            else if (a.Kostalid > 0)
            {
                var set = c.getKilometerGeldset(a.Kostalid, date.Year, date.Month);
                if (set)
                {
                    MessageBox.Show(cmbworkingTimeUser.Text + " hat im " + cmb_Month.Text + " " + cmb_Year.Text +
                                    " Kilometergeld eingetragen.");
                }
                else
                {
                    MessageBox.Show(cmbworkingTimeUser.Text + " hat im " + cmb_Month.Text + " " + cmb_Year.Text +
                                    " kein Kilometergeld eingetragen.");
                }
            }
        }

        private void setdgvMedication()
        {
        }

        private void btn_GG_PDF_Click(object sender, RoutedEventArgs e)
        {
            //dgBodyInfo
            var text = "";
            var tmp = (List<BodyInfo>) dgBodyInfo.ItemsSource;
            if (tmp != null && tmp.Count > 0 && cmbGuG.Text != "")
            {
                var saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF(*.pdf)|*.pdf";
                saveFileDialog1.Title = "PDF Speichern";
                saveFileDialog1.FileName = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" +
                                           DateTime.Now.Day + "_" + cmbGuG.Text + "_GuG";
                var result = saveFileDialog1.ShowDialog();
                if (!((saveFileDialog1.FileName == null) | (saveFileDialog1.FileName == "")) & (result == true))
                {
                    text += cmbGuG.Text + "\n\n";
                    text += "DATUM      | GRÖßE | GEWICHT\n";
                    foreach (var inf in tmp)
                    {
                        text += inf.datum + " | ";
                        text += inf.groeße + " | ";
                        text += inf.gewicht + "\n";
                    }
                    var cafpdf = new CreateAFuckingPDF();
                    cafpdf.show = true;
                    cafpdf.text = text;
                    cafpdf.truenamebro = saveFileDialog1.FileName;
                    var oThread = new Thread(cafpdf.createThisShit2);
                    oThread.Start();
                }
            }
            else
            {
                MessageBox.Show("Wählen Sie einen Klienten aus und klicken Sie auf 'Daten erhalten'!", "Fehler");
            }
        }

        private void btn_Pic_Akt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vid = c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString());

                if (!(string.IsNullOrEmpty(vid) | string.IsNullOrWhiteSpace(vid)))
                {
                    update_pics(Convert.ToInt32(vid));
                }
            }
            catch
            {
                MessageBox.Show("Wählen Sie einen Klienten aus!");
            }
        }

        public void update_pics(int id)
        {
            var docs = c.GetClientdoku(id, true);
            var oclist = new List<Document>();
            foreach (var doc in docs)
            {
                oclist.Add(doc);
            }
            dataGrid2.ItemsSource = oclist;
        }

        private void dgv_Doc_List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var doc = (Document) dgv_Doc_List.SelectedItem;
            var speicherort = Assembly.GetExecutingAssembly().Location;
            speicherort = speicherort.Substring(0, speicherort.LastIndexOf('\\'));
            speicherort += "\\Temp" + DateTime.Now.Ticks +
                           doc.path.Substring(doc.path.LastIndexOf('.', doc.path.Length - 1));
            var ftp = new FtpHandler();
            if (ftp.DownloadFile(doc.path, speicherort))
            {
                try
                {
                    //Process.Start(speicherort).WaitForExit();
                    //Thread.Sleep(1000);
                    //System.IO.File.Delete(speicherort);

                    //-------------------------------------------------------------------------------------------
                    // 
                    // Bei Windows 8 -> Mit obigem Code NullReference Exception.. gelöscht wirds aber nicht..
                    // Programm wird blockiert solange das File offen ist :/
                    // Beim Start einfach alle Temp Dateien löschen is besser..
                    // 
                    //-------------------------------------------------------------------------------------------

                    Process.Start(speicherort);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void dgv_wiki_List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /* Vorschau -- Deaktiviert
            WikiDoc doc = (WikiDoc)dgvWikiDocs.SelectedItem;
            string speicherort = System.Reflection.Assembly.GetExecutingAssembly().Location;
            speicherort = speicherort.Substring(0, speicherort.LastIndexOf('\\'));
            speicherort += "\\Temp1" + doc.path.Substring(doc.path.LastIndexOf('.', doc.path.Length - 1));
            FTPHandler ftp = new FTPHandler();
            if (ftp.down2(@"/home/.sites/33/site5/web/intranet/app/webroot" + doc.path, speicherort))
            {


                try
                {
                    Process.Start(speicherort).WaitForExit();
                    Thread.Sleep(1000);
                    System.IO.File.Delete(speicherort);
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }

            }
            */
        }

        private void btnTgDoku_Click(object sender, RoutedEventArgs e)
        {
            var id = c.getIdbyNameClients(cmbTg.SelectedValue.ToString());
            var tglist = c.getTaschengeldDoku(id);
            dgTaschengeld.ItemsSource = tglist;
        }

        private void btn_Pic_down_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                var doc = (Document) dataGrid2.SelectedItem;

                var saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.FileName = doc.path.Substring(doc.path.LastIndexOf('/') + 1);
                var result = saveFileDialog1.ShowDialog();
                if (!((saveFileDialog1.FileName == null) | (saveFileDialog1.FileName == "")) & (result == true))
                {
                    var ftp = new FtpHandler();
                    if (ftp.DownloadFile(doc.path, saveFileDialog1.FileName))
                    {
                        if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo,
                                MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                Process.Start(saveFileDialog1.FileName).WaitForExit();
                                update_pics(Convert.ToInt32(
                                    c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())));
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btn_Pic_Up_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter =
                "Image files (*.jpg, *.jpeg, , *.tif, *.png) | *.jpg; *.jpeg; *.tif; *.png"; //"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|TIF Files (*.tif)|*.tif";
            ofd.ShowDialog();
            var tit = new Title(c);
            tit.ShowDialog();
            if (tit.titel != "-1")
            {
                var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);


                var ftp = new FtpHandler();
                ftp.UploadFile(ofd.FileName,
                    "data/clients/" + Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())) +
                    "/documents/" + name);

                var a = new FileInfo(ofd.FileName).Length;

                var b = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / 1024)));


                c.addpath_pics(name, Convert.ToInt32(u.Id),
                    Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())), tit.titel, b);
                update_pics(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())));
            }
        }

        private void btn_Pic_Ers_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                var doc = (Document) dataGrid2.SelectedItem;

                var ofd = new OpenFileDialog();
                ofd.Filter =
                    "Image files (*.jpg, *.jpeg, , *.tif, *.png) | *.jpg; *.jpeg; *.tif; *.png"; //"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|TIF Files (*.tif)|*.tif";
                ofd.ShowDialog();
                var tit = new Title(c);
                tit.ShowDialog();
                if (tit.titel != "-1")
                {
                    var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);


                    var ftp = new FtpHandler();
                    ftp.UploadFile(ofd.FileName,
                        "data/clients/" +
                        Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())) +
                        "/documents/" + name);

                    var a = new FileInfo(ofd.FileName).Length;

                    var b = Convert.ToInt32(Math.Round(Convert.ToDecimal(a / 1024)));


                    c.updatePath_pics(doc, name, Convert.ToInt32(u.Id),
                        Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())), tit.titel, b);
                    update_pics(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())));
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btnKMGeldPDFExport_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<User>();
            var len = 0;
            list = c.getUsers();

            foreach (var usäer in list)
            {
                var s = c.getNameByID(usäer.Id, false);
                if (len < s.Length)
                {
                    len = s.Length;
                }
            }

            var fagit = "Name" + fillSpace(len - "Name".Length, " ") + " | Kilometergeld\n";
            foreach (var asad in list)
            {
                var usornäm = c.getNameByID(asad.Id, false);
                fagit += usornäm + fillSpace(len - usornäm.Length, " ") + " | ";
                fagit += RefreshKmG_PDF(asad.Id);
                fagit += "\n";
            }

            var sfd = new SaveFileDialog();
            sfd.FileName = "Kilometergeld-" + DateTime.Now.Date.ToString().Split(' ')[0];
            sfd.Filter = "PDF-Dokument|*.pdf";
            sfd.Title = "PDF exportieren";
            var kostolpdf = new CreateAFuckingPDF();

            kostolpdf.text = fagit;
            kostolpdf.pdfTitle = "Kilometer Gesamtübersicht";
            if ((bool) sfd.ShowDialog() && sfd.FileName != "")
            {
                kostolpdf.truenamebro = sfd.FileName;
                kostolpdf.createThisShit2();
                if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(sfd.FileName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void btnKMGMonEx_Click(object sender, RoutedEventArgs e)
        {
            //MonthSelector ms = new MonthSelector();
            //ms.ShowDialog();
            //if (!ms.closed || ms.canceled)
            //{
            //    return;
            //}
            var list = new List<User>();
            var len = 0;
            list = c.getUsers();

            foreach (var usäer in list)
            {
                var s = c.getNameByID(usäer.Id, false);
                if (len < s.Length)
                {
                    len = s.Length;
                }
            }

            var fagit = "Name" + fillSpace(len - "Name".Length, " ") + " | Kilometergeld\n";
            foreach (var asad in list)
            {
                var usornäm = c.getNameByID(asad.Id, false);
                fagit += usornäm + fillSpace(len - usornäm.Length, " ") + " | ";
                fagit += RefreshKmG_PDF(asad.Id);
                fagit += "\n";
            }
            var sfd = new SaveFileDialog();
            sfd.FileName = "Kilometergeld-" + DateTime.Now.Date.ToString().Split(' ')[0];
            sfd.Filter = "PDF-Dokument|*.pdf";
            sfd.Title = "PDF exportieren";
            var kostolpdf = new CreateAFuckingPDF();

            kostolpdf.text = fagit;
            kostolpdf.pdfTitle = "Kilometer Monatsstatistik";
            if ((bool) sfd.ShowDialog() && sfd.FileName != "")
            {
                kostolpdf.truenamebro = sfd.FileName;
                kostolpdf.createThisShit2();
                if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(sfd.FileName);
                    }
                    catch
                    {
                    }
                }
            }
        }


        //Fetter Bug Kann keine neuen berichte erstellen

        private void btnFvgSet_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFVGClient.SelectedIndex != -1 && cmbFVGDoc.SelectedIndex != -1 && cmbFVGDoc.Text != "")
            {
                var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');
                //editFVG.ContentHtml = c.getFVGValue2(namen[0], namen[1], cmbFVGDoc.SelectedItem.ToString(), cmbArt.Text);
                var tmp = Berichte.ElementAt(cmbFVGDoc.SelectedIndex);
                tmp.content = editFVG.ContentHtml;
                c.setBericht_Content(tmp, u);
                editFVG.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Sie haben kein Dokument ausgewählt");
            }
        }

        private void bttnMedAktExp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pdf = "";
                var len = 0;
                var mediid = cmbMA.SelectedIndex != -1
                    ? c.getIdbyNameClients(cmbMA.SelectedValue.ToString())
                    : c.getIdbyNameClients(cmbKlientArchivAuswaehlen.SelectedValue.ToString());
                var ms = new MonthSelector();
                ms.ShowDialog();
                if (!ms.closed || ms.canceled)
                {
                    return;
                }
                var medilist = c.GetClientMed_Month(mediid, ms.calMS.DisplayDate.Month.ToString(),
                    ms.calMS.DisplayDate.Year.ToString());
                dgmedicalaction.ItemsSource = medilist;
                for (var i = 0; i < medilist.Count; i++)
                {
                    if (len < medilist[i].art.Length)
                    {
                        len = medilist[i].art.Length;
                    }
                }
                pdf = "Datum      | Art" + fillSpace(len - "Art".Length, " ") + " | Beschreibung\n";
                for (var i = 0; i < medilist.Count; i++)
                {
                    pdf += medilist[i].date + " | " + medilist[i].art + fillSpace(len - medilist[i].art.Length, " ") +
                           " | " + medilist[i].desc + "\n";
                }
                var sfd = new SaveFileDialog();
                sfd.FileName = "Medizinische_Aktionen-" + DateTime.Now.Date.ToString().Split(' ')[0];
                sfd.Filter = "PDF-Dokument|*.pdf";
                sfd.Title = "PDF exportieren";
                var kostolpdf = new CreateAFuckingPDF();

                kostolpdf.text = pdf;
                if ((bool) sfd.ShowDialog() && sfd.FileName != "")
                {
                    kostolpdf.truenamebro = sfd.FileName;
                    kostolpdf.createThisShit2();
                    if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            Process.Start(sfd.FileName);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
                /**/
                /**/
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (dgv_Doc_List.SelectedIndex != -1)
            {
                var doc = (Document) dgv_Doc_List.SelectedItem;


                var ftp = new FtpHandler();
                ftp.DeleteFile(doc.path);


                c.deletePath(doc);
                update_docs(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Doc.SelectedValue.ToString())));
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                var doc = (Document) dataGrid2.SelectedItem;


                var ftp = new FtpHandler();
                ftp.DeleteFile(doc.path);


                c.deletePath_pics(doc);
                update_pics(Convert.ToInt32(c.getIdbyNameClients(cmb_Klient_Foto.SelectedValue.ToString())));
            }
        }

        private void cmbMA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            int index = cmbMA.SelectedIndex;
            cmbKlientArchivAuswaehlen.SelectedIndex = -1;
            cmbMA.SelectedIndex = index;
             */
        }

        public void update_medications()
        {
            var medicList = new List<Medicaments>();
            obsoleteMedis = new List<string>();
            medis = c.getMedicaments().Split('%');
            cmbMedis.Items.Clear();
            foreach (var medi in medis)
            {
                cmbMedis.Items.Add(medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%"));
            }
            //string[] fillings = c.getMedication(cmbMedicClient.SelectedValue.ToString(), DateTime.Today).Split('%');
            var fillings = c.getMedicationForClient(cmbMedicClient.SelectedValue.ToString(), DateTime.Today, true,
                false).Split('%');
            foreach (var s in fillings)
            {
                var temp = s.Split('$');
                if (temp.Length > 5)
                {
                    if (temp[1] == "0" && temp[2] == "0" && temp[3] == "0" && temp[4] == "0")
                    {
                        medicList.Add(new Medicaments(temp[0] + " (Bei Bedarf)", temp[1], temp[2], temp[3], temp[4],
                            false, false, false, false, temp[5], temp[6], DateTime.Today.ToString()));
                    }
                    else
                    {
                        medicList.Add(new Medicaments(temp[0], temp[1], temp[2], temp[3], temp[4], false, false, false,
                            false, temp[5], temp[6], DateTime.Today.ToString()));
                    }
                    if (c.CheckMediIfObsolete(temp[6]))
                    {
                        obsoleteMedis.Add(temp[6] + "§true");
                    }
                    else
                    {
                        obsoleteMedis.Add(temp[6] + "§false");
                    }
                }
            }
            dgvMedikamente.ItemsSource = new List<Medicaments>();
            dgvMedikamente.ItemsSource = medicList;

            cmbMedis.IsEnabled = true;
            txtNewMediMorning.IsEnabled = true;
            txtNewMediMidday.IsEnabled = true;
            txtNewMediEvening.IsEnabled = true;
            txtNewMediNight.IsEnabled = true;
            dtpNewMediFrom.IsEnabled = true;
            dtpNewMediTo.IsEnabled = true;
            btnAddNewMedi.IsEnabled = true;
            btnDeleteSelectedMedi.IsEnabled = true;
            btnDeleteSelectedMedi_Copy.IsEnabled = true;
            btnEditSelectedMedi.IsEnabled = true;
            current_client_medi = c.getIdbyNameClients(cmbMedicClient.SelectedValue.ToString());
        }


        private void button8_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int id = Int32.Parse(c.getIdbyNameClients(cmbMedicClient.SelectedValue.ToString()));


                update_medications();


                //#Unfertig - Es muss noch so gerichtet werden dass das was von getMedicamentsByClient zu einer MedicamentsList umgeformt wird
                //um es in den DataGrid einzufügen

                //String[,] medics = c.getMedicamentsByClient(id);

                //dgvMedikamente.ItemsSource = medicList;
            }
            catch
            {
                MessageBox.Show("Kein Klient ausgewählt", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgvMedikamente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmbKlientAuswaehlen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wahl == 0)
            {
                wahl = 1;
                cmbKlientArchivAuswaehlen.SelectedIndex = -1;
            }

            wahl = 0;
        }

        private void cmbKlientArchivAuswaehlen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wahl == 0)
            {
                wahl = 2;
                cmbKlientAuswaehlen.SelectedIndex = -1;
            }

            wahl = 0;
        }

        private void cmbAdminUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void fillArt()
        {
            cmbArt.Items.Add("Telefonat");
            cmbArt.Items.Add("Vorfallsprotokoll");
            cmbArt.Items.Add("Gesprächsprotokoll");
            cmbArt.Items.Add("Fallverlaufsgespräch");
            cmbArt.Items.Add("Jahresbericht");
            cmbArt.Items.Add("Zwischenbericht");
            cmbArt.SelectedIndex = 3;
        }

        private void cmbArt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //6.11.2014 - in jedem block noch daten aus der 2. tabelle hole

            try
            {
                cmbFVGDoc.Items.Clear();
                Berichte = new List<Klienten_Berichte>();

                if (cmbArt.SelectedIndex == 0) // Telefonat
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 0))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 0))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                else if (cmbArt.SelectedIndex == 1) // Vorfallsprotokoll
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 1))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 1))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                else if (cmbArt.SelectedIndex == 2) // Gesprächsprotokoll
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 2))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 2))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                else if (cmbArt.SelectedIndex == 3) // Fallverlaufsgespräch
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 3))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 3))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                else if (cmbArt.SelectedIndex == 4) // Jahresbericht
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 4))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 4))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                else if (cmbArt.SelectedIndex == 5) // Zwischenbericht
                {
                    var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 5))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                    foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 5))
                    {
                        Berichte.Add(item);
                        cmbFVGDoc.Items.Add(item.name);
                    }
                }

                //zu cmbFVGDoc hinzufügen
            }
            catch
            {
            }

            /*
            try
            {
                cmbFVGDoc.Items.Clear();
                Berichte = new List<Klienten_Berichte>();

                if (cmbArt.SelectedIndex == 0) // Telefonat
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getBericht(namen[0], namen[1]).Split('$'))
                    {
                        //cmbFVGDoc.Items.Add(item);
                        Berichte.Add();
                    }

                    
                }

                else if (cmbArt.SelectedIndex == 1) // Vorfallsprotokoll
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getVorfall(namen[0], namen[1]).Split('$'))
                    {
                        cmbFVGDoc.Items.Add(item);
                    }
                }

                else if (cmbArt.SelectedIndex == 2) // Gesprächsprotokoll
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getGespr(namen[0], namen[1]).Split('$'))
                    {
                        cmbFVGDoc.Items.Add(item);
                    }
                }

                else if (cmbArt.SelectedIndex == 3) // Fallverlaufsgespräch
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getFVGs(namen[0], namen[1]).Split('$'))
                    {
                        cmbFVGDoc.Items.Add(item);
                    }
                }

                else if (cmbArt.SelectedIndex == 4) // Jahresbericht
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getJahres(namen[0], namen[1]).Split('$'))
                    {
                        cmbFVGDoc.Items.Add(item);
                    }
                }

                else if (cmbArt.SelectedIndex == 5) // Zwischenbericht
                {
                    string[] namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                    foreach (string item in c.getZwischen(namen[0], namen[1]).Split('$'))
                    {
                        cmbFVGDoc.Items.Add(item);
                    }
                }
            }
            catch (Exception ex) { }
             */
        }

        private void btnSavePad_Click(object sender, RoutedEventArgs e)
        {
            if (txtPad1.Text != "")
            {
                if (txtPad2.Text != "")
                {
                    if (dpPad1.Text != "")
                    {
                        if (dpPad2.Text != "")
                        {
                            if (dpPad1.SelectedDate <= dpPad2.SelectedDate)
                            {
                                pad = new List<PadMas>();
                                dgPad.ItemsSource = new List<PadMas>();
                                c.setPad(u.Id, c.getIdbyNameClients(cmbPad.Text), txtPad1.Text, txtPad2.Text,
                                    dpPad1.Text, dpPad2.Text);
                                pad = c.getPad(c.getIdbyNameClients(cmbPad.SelectedValue.ToString()));
                                dgPad.ItemsSource = pad;
                            }
                            else
                            {
                                MessageBox.Show("Das Von-Datum muss kleiner als das Bis-Datum sein!", "Achtung!",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Bitte tragen Sie ein Bis-Datum ein!", "Achtung!", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bitte tragen Sie ein Von-Datum ein!", "Achtung!", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Bitte tragen Sie eine Begründung ein!", "Achtung!", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bitte tragen Sie eine Maßnahme ein!", "Achtung!", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void btnGetPad_Click(object sender, RoutedEventArgs e)
        {
            pad = new List<PadMas>();
            dgPad.ItemsSource = new List<PadMas>();
            pad = c.getPad(c.getIdbyNameClients(cmbPad.SelectedValue.ToString()));
            dgPad.ItemsSource = pad;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgHouseAll.SelectedIndex != -1)
            {
                SelectedHouses.Add((Service) dgHouseAll.SelectedItem);
                AllHouses.Remove((Service) dgHouseAll.SelectedItem);

                try
                {
                    dgHouseSelected.ItemsSource = new List<Service>();
                    dgHouseAll.ItemsSource = new List<Service>();
                }
                catch
                {
                }

                dgHouseSelected.ItemsSource = SelectedHouses;
                dgHouseAll.ItemsSource = AllHouses;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgHouseSelected.SelectedIndex != -1)
            {
                AllHouses.Add((Service) dgHouseSelected.SelectedItem);
                SelectedHouses.Remove((Service) dgHouseSelected.SelectedItem);

                try
                {
                    dgHouseSelected.ItemsSource = new List<Service>();
                    dgHouseAll.ItemsSource = new List<Service>();
                }
                catch
                {
                }

                dgHouseSelected.ItemsSource = SelectedHouses;
                dgHouseAll.ItemsSource = AllHouses;
            }
        }

        private void dataGrid2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var doc = (Document) dataGrid2.SelectedItem;
            var speicherort = Assembly.GetExecutingAssembly().Location;
            speicherort = speicherort.Substring(0, speicherort.LastIndexOf('\\'));
            speicherort += "\\Temp2" + doc.path.Substring(doc.path.LastIndexOf('.', doc.path.Length - 1));
            var ftp = new FtpHandler();
            if (ftp.DownloadFile(doc.path, speicherort))
            {
                try
                {
                    Process.Start(speicherort).WaitForExit();
                    Thread.Sleep(1000);
                    File.Delete(speicherort);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnFvgNew_Click(object sender, RoutedEventArgs e)
        {
            var abd = new AddBerichtDialog();
            abd.ShowDialog();
            if (abd.Name != null)
            {
                cmbFVGDoc.SelectedIndex = -1;
                editFVG.ContentHtml = "";
                var tmp = new Klienten_Berichte();
                cmbArt.SelectedIndex = -1;
                tmp.art = abd.art;
                tmp.name = abd.Name;
                tmp.Client_id = Convert.ToInt32(c.getIdbyNameClients(cmbFVGClient.SelectedItem.ToString()));
                tmp.table = 2;
                var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                c.addBericht(tmp, u, date);

                cmbFVGDoc.Items.Clear();
                Berichte = new List<Klienten_Berichte>();


                var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], abd.art))
                {
                    Berichte.Add(item);
                    cmbFVGDoc.Items.Add(item.name);
                }
                foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], abd.art))
                {
                    Berichte.Add(item);
                    cmbFVGDoc.Items.Add(item.name);
                }


                cmbArt.SelectedIndex = abd.art;


                cmbFVGDoc.SelectedIndex = Berichte.Count() - 1;
                editFVG.IsEnabled = true;
            }
        }

        private void btnFvgC_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFVGClient.SelectedIndex != -1 && cmbFVGDoc.SelectedIndex != -1 && cmbFVGDoc.Text != "")
            {
                //editFVG.ContentHtml = c.getFVGValue2(namen[0], namen[1], cmbFVGDoc.SelectedItem.ToString(), cmbArt.Text);
                var tmp = Berichte.ElementAt(cmbFVGDoc.SelectedIndex);
                //Dialog zum fragen der art öffnen
                var dia = new Klient_Berichte_Art_C(cmbArt.SelectedIndex);
                dia.ShowDialog();
                if (dia.set)
                {
                    tmp.art = dia.art;
                    c.setBericht_Art(tmp);
                    try
                    {
                        cmbFVGDoc.Items.Clear();
                        Berichte = new List<Klienten_Berichte>();

                        if (cmbArt.SelectedIndex == 0) // Telefonat
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 0))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 0))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }

                        else if (cmbArt.SelectedIndex == 1) // Vorfallsprotokoll
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 1))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 1))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }

                        else if (cmbArt.SelectedIndex == 2) // Gesprächsprotokoll
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 2))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 2))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }

                        else if (cmbArt.SelectedIndex == 3) // Fallverlaufsgespräch
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 3))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 3))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }

                        else if (cmbArt.SelectedIndex == 4) // Jahresbericht
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 4))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 4))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }

                        else if (cmbArt.SelectedIndex == 5) // Zwischenbericht
                        {
                            var namen = cmbFVGClient.SelectedItem.ToString().Split(' ');

                            foreach (var item in c.getBericht_clientsfvgs(namen[0], namen[1], 5))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                            foreach (var item in c.getBericht_clientsreports(namen[0], namen[1], 5))
                            {
                                Berichte.Add(item);
                                cmbFVGDoc.Items.Add(item.name);
                            }
                        }


                        //zu cmbFVGDoc hinzufügen
                    }
                    catch
                    {
                        /**/
                        /**/
                    }
                }
            }
        }

        private void btnWikiEditClick(object sender, RoutedEventArgs e)
        {
            if (dgvWikiDocs.SelectedIndex != -1)
            {
                if (MessageBox.Show("Wollen Sie wirklich löschen?", "Achtung!", MessageBoxButton.YesNo,
                        MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    var tmp = (WikiDoc) dgvWikiDocs.SelectedItem;
                    var ftp = new FtpHandler();
                    c.delwikiDoc(tmp);
                    try
                    {
                        ftp.DeleteFile(tmp.path);
                    }
                    catch
                    {
                        /**/
                        /**/
                    }


                    loadWiki();
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }


            /*
            //muss noch getestet werden
            //#####TESTEN##
            if (dgvWikiDocs.SelectedIndex != -1)
            {
                WikiDoc tmp = (WikiDoc) dgvWikiDocs.SelectedItem;
                Title tit = new Title();
                tit.ShowDialog();

                c.renameWiki(tmp, tit.titel, u);

                loadWiki();
            }
             */
        }

        private void btnFvgC_Copy_Click(object sender, RoutedEventArgs e)
        {
            var vorlage = new Bericht_Vorlage(c);
            vorlage.ShowDialog();
            if (vorlage.set)
            {
                if (vorlage.name != "")
                {
                    var tmp = new Klienten_Berichte();
                    tmp.content = editFVG.ContentHtml;
                    tmp.name = vorlage.name;

                    c.addBericht_Vorlage(tmp);
                }
                else
                {
                    editFVG.ContentHtml = vorlage.content;
                    editFVG.IsEnabled = true;
                }
            }
        }

        private void TS4_Closing(object sender, CancelEventArgs e)
        {
        }


        private void chk_noMedi_Checked(object sender, RoutedEventArgs e)
        {
            dgvMedi.IsEnabled = false;
        }

        private void chk_noMedi_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvMedi.IsEnabled = true;
        }

        private void dgNewestDokus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void btn_toNewestNews_Click(object sender, RoutedEventArgs e)
        {
            if (dgNewestDokus.SelectedItem != null)
            {
                var ndd = (NewestDokus) dgNewestDokus.SelectedItem;
                var slct_ersteller = ndd.ersteller;
                var slct_art = ndd.art;
                var slct_tag = ndd.tag;
                var slct_name = ndd.name;
                var slct_wg = ndd.wg;
                var slct_id = ndd.id;
                if (slct_art == "Tagesdokumentation")
                {
                    tabMain.SelectedIndex = 6;
                    cmbKlient.SelectedItem = slct_name;
                    dateDoku.SelectedDate = DateTime.Parse(slct_tag);
                }
                else
                {
                    tabMain.SelectedIndex = 7;
                    cmbKlientAuswaehlen.SelectedItem = slct_name;
                    btnGetKlientDaten.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    tabControl1.SelectedIndex = 1;
                    cmbArt.SelectedItem = slct_art;
                    cmbFVGDoc.SelectedItem = slct_id;
                    btnFvgGet.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            }
            else
            {
                MessageBox.Show("Sie müssen eine Dokumentation auswählen!", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnWikiReplace_Click(object sender, RoutedEventArgs e)
        {
            if (dgvWikiDocs.SelectedIndex != -1)
            {
                var doc = (WikiDoc) dgvWikiDocs.SelectedItem;

                var ofd = new OpenFileDialog();

                var result = ofd.ShowDialog();
                if (result == true)
                {
                    var name = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\') + 1);
                    var ftp = new FtpHandler();
                    try
                    {
                        ftp.DeleteFile(doc.path);
                    }
                    catch
                    {
                        /**/
                        /**/
                    }


                    ftp.UploadFile(ofd.FileName, "data/wiki/" + name);

                    c.updatewiki(name, Convert.ToInt32(u.Id), doc);
                    loadWiki();
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btnWikiReNam_Click(object sender, RoutedEventArgs e)
        {
            if (dgvWikiDocs.SelectedIndex != -1)
            {
                var tmp = (WikiDoc) dgvWikiDocs.SelectedItem;
                var tit = new Title(c);
                tit.ShowDialog();
                if (tit.titel != "-1")
                {
                    c.renameWiki(tmp, tit.titel, u);

                    loadWiki();
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt");
            }
        }

        private void btnWikiRate_Click(object sender, RoutedEventArgs e)
        {
            if (dgvWikiDocs.SelectedIndex != -1)
            {
                var rate = new RateWiki();
                rate.ShowDialog();
                if (rate.rate != -1)
                {
                    var tmp = (WikiDoc) dgvWikiDocs.SelectedItem;
                    c.setWikiRating(u.Id, rate.rate, tmp);
                    loadWiki();
                }
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddNewMedi_Click(object sender, RoutedEventArgs e)
        {
            if ((txtNewMediMorning.Text != "0") | (txtNewMediMidday.Text != "0") | (txtNewMediEvening.Text != "0") |
                (txtNewMediNight.Text != "0"))
            {
                if (txtNewMediMorning.Text != "" && txtNewMediMidday.Text != "" && txtNewMediEvening.Text != "" &&
                    txtNewMediNight.Text != "")
                {
                    if (dtpNewMediFrom.SelectedDate != null && dtpNewMediTo.SelectedDate != null)
                    {
                        if (cmbMedis.SelectedValue != null)
                        {
                            if (edit_medi)
                            {
                                var msgboxResult =
                                    MessageBox.Show(
                                        "ACHTUNG!\n\nDas Ändern eines Medikamts bewirkt, dass es in allen Dokumentationen (auch in vergangenen) geändert wird und sollte AUSSCHLIESSLICH bei falsch angelegten Medikationen verwendet werden!\n\nWenn die Dosis oder die Menge des Medikamts geändert wurden, legen Sie bitte eine neue Medikation fest und markieren die alte als \"abgesetzt\"!\n\n\nSoll der Vorgang fortgesetzt werden?",
                                        "Achtung!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (msgboxResult == MessageBoxResult.Yes)
                                {
                                    var medi_id = "";
                                    foreach (var medi in medis)
                                    {
                                        cmbMedis.Items.Add(medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%"));
                                        if (medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%") ==
                                            cmbMedis.SelectedValue.ToString())
                                        {
                                            medi_id = medi.Split('$')[0];
                                        }
                                    }
                                    var mmm = (Medicaments) dgvMedikamente.SelectedItem;
                                    c.updateMedi(medi_id, mmm.cmId, u.Id,
                                        dtpNewMediFrom.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                        dtpNewMediTo.SelectedDate.Value.ToString("yyyy-MM-dd"), txtNewMediMorning.Text,
                                        txtNewMediMidday.Text, txtNewMediEvening.Text, txtNewMediNight.Text);
                                    update_medications();
                                    txtNewMediMorning.Text = "";
                                    txtNewMediMidday.Text = "";
                                    txtNewMediEvening.Text = "";
                                    txtNewMediNight.Text = "";
                                    cmbMedis.SelectedIndex = -1;
                                    dtpNewMediFrom.SelectedDate = null;
                                    dtpNewMediTo.SelectedDate = null;
                                    btnAddNewMedi.Content = "hinzufügen";
                                    btnMediUndoChanges.Visibility = Visibility.Hidden;
                                    edit_medi = false;
                                    MessageBox.Show("Die Medikation wurde geändert!", "Erfolg!", MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                                }
                                else if (msgboxResult == MessageBoxResult.No)
                                {
                                    //NIX
                                }
                            }
                            else
                            {
                                var medi_id = "";
                                foreach (var medi in medis)
                                {
                                    cmbMedis.Items.Add(medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%"));
                                    if (medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%") ==
                                        cmbMedis.SelectedValue.ToString())
                                    {
                                        medi_id = medi.Split('$')[0];
                                    }
                                }
                                if (medi_id != "")
                                {
                                    // client_id, created, modified, creatuser_id, lastuser_id, medicament_id, from, to, morning, midday, evening, night
                                    string[] dataa =
                                    {
                                        current_client_medi, DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                        DateTime.Now.ToString("yyyy-MM-dd HH:mm"), u.Id, u.Id, medi_id,
                                        dtpNewMediFrom.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                        dtpNewMediTo.SelectedDate.Value.ToString("yyyy-MM-dd"), txtNewMediMorning.Text,
                                        txtNewMediMidday.Text, txtNewMediEvening.Text, txtNewMediNight.Text
                                    };
                                    c.addMedicamentForClient(dataa);
                                    update_medications();
                                    MessageBox.Show("Die Medikation wurde angewandt!", "Erfolg!", MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Internal Error! (E1337)", "Fehler!", MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Es muss ein Medikament ausgewählt werden!", "Fehler!", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es müssen sowohl Von-Datum als auch Bis-Datum ausgefüllt werden!", "Fehler!",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Es müssen alle Felder ausgefüllt werden!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                //MessageBox.Show("Diese Eingabe ist sinnlos!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                var msgboxResult =
                    MessageBox.Show(
                        "ACHTUNG!\n\nMit der Dosis-Angabe \"0-0-0-0\" wird die Medikation als \"nur bei Bedarf\" gespeichert!\n\nWenn das Medikament regelmäßig eingenommen wird, korrigieren Sie bitte Ihre Eingabe!\n\n\nSoll der Vorgang fortgesetzt werden?",
                        "Achtung!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msgboxResult == MessageBoxResult.Yes)
                {
                    if (txtNewMediMorning.Text != "" && txtNewMediMidday.Text != "" && txtNewMediEvening.Text != "" &&
                        txtNewMediNight.Text != "")
                    {
                        if (dtpNewMediFrom.SelectedDate != null && dtpNewMediTo.SelectedDate != null)
                        {
                            if (cmbMedis.SelectedValue != null)
                            {
                                if (edit_medi)
                                {
                                    var msgboxResult1 =
                                        MessageBox.Show(
                                            "ACHTUNG!\n\nDas Ändern eines Medikamts bewirkt, dass es in allen Dokumentationen (auch in vergangenen) geändert wird und sollte AUSSCHLIESSLICH bei falsch angelegten Medikationen verwendet werden!\n\nWenn die Dosis oder die Menge des Medikamts geändert wurden, legen Sie bitte eine neue Medikation fest und markieren die alte als \"abgesetzt\"!\n\n\nSoll der Vorgang fortgesetzt werden?",
                                            "Achtung!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                    if (msgboxResult1 == MessageBoxResult.Yes)
                                    {
                                        var medi_id = "";
                                        foreach (var medi in medis)
                                        {
                                            cmbMedis.Items.Add(medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%"));
                                            if (medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%") ==
                                                cmbMedis.SelectedValue.ToString())
                                            {
                                                medi_id = medi.Split('$')[0];
                                            }
                                        }
                                        var mmm = (Medicaments) dgvMedikamente.SelectedItem;
                                        c.updateMedi(medi_id, mmm.cmId, u.Id,
                                            dtpNewMediFrom.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                            dtpNewMediTo.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                            txtNewMediMorning.Text, txtNewMediMidday.Text, txtNewMediEvening.Text,
                                            txtNewMediNight.Text);
                                        update_medications();
                                        txtNewMediMorning.Text = "";
                                        txtNewMediMidday.Text = "";
                                        txtNewMediEvening.Text = "";
                                        txtNewMediNight.Text = "";
                                        cmbMedis.SelectedIndex = -1;
                                        dtpNewMediFrom.SelectedDate = null;
                                        dtpNewMediTo.SelectedDate = null;
                                        btnAddNewMedi.Content = "hinzufügen";
                                        btnMediUndoChanges.Visibility = Visibility.Hidden;
                                        edit_medi = false;
                                        MessageBox.Show("Die Medikation wurde geändert!", "Erfolg!",
                                            MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else if (msgboxResult1 == MessageBoxResult.No)
                                    {
                                        //NIX
                                    }
                                }
                                else
                                {
                                    var medi_id = "";
                                    foreach (var medi in medis)
                                    {
                                        cmbMedis.Items.Add(medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%"));
                                        if (medi.Split('$')[1].Replace("!!!PROZENTZEICHEN!!!", "%") ==
                                            cmbMedis.SelectedValue.ToString())
                                        {
                                            medi_id = medi.Split('$')[0];
                                        }
                                    }
                                    if (medi_id != "")
                                    {
                                        // client_id, created, modified, creatuser_id, lastuser_id, medicament_id, from, to, morning, midday, evening, night
                                        string[] dataa =
                                        {
                                            current_client_medi, DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                                            DateTime.Now.ToString("yyyy-MM-dd HH:mm"), u.Id, u.Id, medi_id,
                                            dtpNewMediFrom.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                            dtpNewMediTo.SelectedDate.Value.ToString("yyyy-MM-dd"),
                                            txtNewMediMorning.Text, txtNewMediMidday.Text, txtNewMediEvening.Text,
                                            txtNewMediNight.Text
                                        };
                                        c.addMedicamentForClient(dataa);
                                        update_medications();
                                        MessageBox.Show("Die Medikation wurde angewandt!", "Erfolg!",
                                            MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Internal Error! (E1337)", "Fehler!", MessageBoxButton.OK,
                                            MessageBoxImage.Error);
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Es muss ein Medikament ausgewählt werden!", "Fehler!",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Es müssen sowohl Von-Datum als auch Bis-Datum ausgefüllt werden!",
                                "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es müssen alle Felder ausgefüllt werden!", "Fehler!", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
                else if (msgboxResult == MessageBoxResult.No)
                {
                    //NIX
                }
            }
        }

        private void btnDeleteSelectedMedi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvMedikamente.SelectedIndex != -1)
                {
                    var mmm = (Medicaments) dgvMedikamente.SelectedItem;
                    c.CancelMediForClient(mmm.cmId);
                    update_medications();
                    MessageBox.Show("Das Medikament wurde abgesetzt!", "Erfolg!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Keine Medikation ausgewählt!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Internal Error! (E1338)", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteSelectedMedi_Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvMedikamente.SelectedIndex != -1)
                {
                    var msgboxResult =
                        MessageBox.Show(
                            "ACHTUNG!\n\nDas löschen eines Medikamts bewirkt, dass es nicht mehr in den Dokumentationen aufscheint (auch nicht in vergangenen) und sollte AUSSCHLIESSLICH bei versehentlich angelegten oder falsch angelegten Medikationen verwendet werden!\n\nWenn das Medikament abgesetzt wurde verwenden Sie bitte die \"Medikament absetzten\" Funktion!\n\n\nSoll der Vorgang fortgesetzt werden?",
                            "Achtung!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (msgboxResult == MessageBoxResult.Yes)
                    {
                        var mmm = (Medicaments) dgvMedikamente.SelectedItem;
                        c.DeleteMediForClient(mmm.cmId);
                        update_medications();
                        MessageBox.Show("Das Medikament wurde gelöscht!", "Erfolg!", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else if (msgboxResult == MessageBoxResult.No)
                    {
                        //NIX
                    }
                }
                else
                {
                    MessageBox.Show("Keine Medikation ausgewählt!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Internal Error! (E1339)", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddNewMediNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtNewMediName.Text != "" && txtNewMediDescription.Text != "")
                {
                    c.AddNewMedi(txtNewMediName.Text, txtNewMediDescription.Text, u.Id);
                    update_medications();
                    txtNewMediName.Text = "";
                    txtNewMediDescription.Text = "";
                    MessageBox.Show("Das Medikament wurde hinzugefügt!", "Erfolg!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Es müssen alle Felder ausgefüllt werden!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Internal Error! (E1340)", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool isMediObsolete(string mid)
        {
            var ret = false;
            foreach (var s in obsoleteMedis)
            {
                if (s.Split('§')[0] == mid)
                {
                    if (s.Split('§')[1] == "true")
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        private void dgvMedikamente_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var mmm = (Medicaments) e.Row.Item;
            if (isMediObsolete(mmm.cmId))
            {
                e.Row.Background = Brushes.LightPink;
            }
            else
            {
                e.Row.Background = Brushes.LightGreen;
            }
        }

        private void btnEditSelectedMedi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvMedikamente.SelectedIndex != -1)
                {
                    edit_medi = true;
                    var mmm = (Medicaments) dgvMedikamente.SelectedItem;
                    btnAddNewMedi.Content = "Änderungen speichern";
                    txtNewMediMorning.Text = mmm.morning;
                    txtNewMediMidday.Text = mmm.midday;
                    txtNewMediEvening.Text = mmm.evening;
                    txtNewMediNight.Text = mmm.night;
                    cmbMedis.SelectedItem = mmm.name;
                    dtpNewMediFrom.SelectedDate = c.GetMediDateFrom(mmm.cmId);
                    dtpNewMediTo.SelectedDate = c.GetMediDateTo(mmm.cmId);
                    btnMediUndoChanges.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("Keine Medikation ausgewählt!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                MessageBox.Show("Internal Error! (E1341)", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnMediUndoChanges_Click(object sender, RoutedEventArgs e)
        {
            txtNewMediMorning.Text = "";
            txtNewMediMidday.Text = "";
            txtNewMediEvening.Text = "";
            txtNewMediNight.Text = "";
            cmbMedis.SelectedIndex = -1;
            dtpNewMediFrom.SelectedDate = null;
            dtpNewMediTo.SelectedDate = null;
            btnAddNewMedi.Content = "hinzufügen";
            edit_medi = false;
            update_medications();
            btnMediUndoChanges.Visibility = Visibility.Hidden;
        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void btnKMGActiveEx_Click(object sender, RoutedEventArgs e)
        {
            var len_von = "Von".Length;
            var len_bis = "Nach".Length;
            var len_km = "Kilometer".Length;
            var len_summe = "Summe (€)".Length;
            var len_kfz = "Kennzeichen".Length;
            var len_dvon = "Datum Von".Length;
            var len_dbis = "Datum Bis".Length;
            foreach (KmG kmmg in dgKmG.Items)
            {
                var len_new_von = kmmg.Ortvon.Length;
                if (len_new_von > len_von)
                {
                    len_von = len_new_von;
                }

                var len_new_bis = kmmg.Ortbis.Length;
                if (len_new_bis > len_bis)
                {
                    len_bis = len_new_bis;
                }

                var len_new_km = kmmg.km.Length;
                if (len_new_km > len_km)
                {
                    len_km = len_new_km;
                }

                var len_new_summe = kmmg.Summe.Length;
                if (len_new_summe > len_summe)
                {
                    len_summe = len_new_summe;
                }

                var len_new_kfz = kmmg.Kennzeichen.Replace(" ", "").Replace("-", "").Replace("+", "").Length;
                if (len_new_kfz > len_kfz)
                {
                    len_kfz = len_new_kfz;
                }

                var len_new_dvon = kmmg.Zeitvon.Length;
                if (len_new_dvon > len_dvon)
                {
                    len_dvon = len_new_dvon;
                }

                var len_new_dbis = kmmg.Zeitbis.Length;
                if (len_new_dbis > len_dbis)
                {
                    len_dbis = len_new_dbis;
                }
            }
            double summme = 0;
            var emm = (Employee) cmbUserKmG.SelectedItem;
            var fagit = "Monat: " + cmbKMGMonth.Text + " " + cmbKMGYear.Text + "\nName: " + emm.FullName + "\n\n";
            fagit += "Kennzeichen" + fillSpace(len_kfz - "Kennzeichen".Length, " ") + " | Von" +
                     fillSpace(len_von - "Von".Length, " ") + " | Nach" + fillSpace(len_bis - "Nach".Length, " ") +
                     " | Datum Von" + fillSpace(len_dvon - "Datum Von".Length, " ") + " | Datum Bis" +
                     fillSpace(len_dbis - "Datum Bis".Length, " ") + " | Kilometer" +
                     fillSpace(len_km - "Kilometer".Length, " ") + " | Summe (€)\n";
            fagit += fillSpace(len_von + len_bis + len_km + len_dvon + len_dbis + len_summe + len_kfz + 18, "-") +
                     "\n";
            foreach (KmG kmmg in dgKmG.Items)
            {
                fagit += kmmg.Kennzeichen.ToUpper().Replace(" ", "").Replace("-", "").Replace("+", "") +
                         fillSpace(
                             len_kfz - kmmg.Kennzeichen.Replace(" ", "").Replace("-", "").Replace("+", "")
                                 .Length, " ") + " | " + kmmg.Ortvon + fillSpace(len_von - kmmg.Ortvon.Length, " ") +
                         " | " +
                         kmmg.Ortbis + fillSpace(len_bis - kmmg.Ortbis.Length, " ") + " | " + kmmg.Zeitvon +
                         fillSpace(len_dvon - kmmg.Zeitvon.Length, " ") + " | " + kmmg.Zeitbis +
                         fillSpace(len_dbis - kmmg.Zeitbis.Length, " ") + " | " + kmmg.km +
                         fillSpace(len_km - kmmg.km.Length, " ") + " | " +
                         ConvertToOrgenString(Convert.ToDecimal(kmmg.Summe).ToString("0000.00")) + "\n";
                summme += Convert.ToDouble(kmmg.Summe);
            }
            fagit += fillSpace(len_von + len_bis + len_km + len_dvon + len_dbis + len_summe + len_kfz + 18, "-") +
                     "\n";
            fagit += fillSpace(len_von + len_bis + len_km + len_dvon + len_dbis + len_kfz + 10, " ") + "Gesamt: " +
                     ConvertToOrgenString(Convert.ToDecimal(summme).ToString("0000.00")) + "€\n";
            var sfd = new SaveFileDialog();
            sfd.FileName = "KMG_" + emm.FullName.Replace(' ', '_') + "_" + cmbKMGMonth.Text + "_" + cmbKMGYear.Text;
            sfd.Filter = "PDF-Dokument|*.pdf";
            sfd.Title = "PDF exportieren";
            var kostolpdf = new CreateAFuckingPDF();
            kostolpdf.font = new XFont("Courier New", 10, XFontStyle.Regular);
            kostolpdf.pdfTitle = "Kilometergeld (" + emm.FullName + ")";
            kostolpdf.pageO = PageOrientation.Landscape;
            kostolpdf.text = fagit;
            if ((bool) sfd.ShowDialog() && sfd.FileName != "")
            {
                kostolpdf.truenamebro = sfd.FileName;
                kostolpdf.createThisShit2();
                if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(sfd.FileName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string ConvertToOrgenString(string orgerString)
        {
            var orgeChars = orgerString.ToCharArray();
            if (orgeChars[0] == '0' && orgeChars[1] == '0' && orgeChars[2] == '0')
            {
                orgeChars[0] = ' ';
                orgeChars[1] = ' ';
                orgeChars[2] = ' ';
            }
            else if (orgeChars[0] == '0' && orgeChars[1] == '0')
            {
                orgeChars[0] = ' ';
                orgeChars[1] = ' ';
            }
            else if (orgeChars[0] == '0')
            {
                orgeChars[0] = ' ';
            }
            return new string(orgeChars);
        }

        private void cmbUserKmG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnKMGActiveEx.IsEnabled = false;
        }

        private void cmbKMGMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKMGset)
            {
                btnKMGActiveEx.IsEnabled = false;
                if (cmbKMGYear.SelectedIndex != -1)
                {
                    var month = (int.Parse(cmbKMGMonth.SelectedIndex.ToString()) + 1).ToString();
                    if (month == "1")
                    {
                        month = "Jänner";
                    }
                    else if (month == "2")
                    {
                        month = "Februar";
                    }
                    else if (month == "3")
                    {
                        month = "März";
                    }
                    else if (month == "4")
                    {
                        month = "April";
                    }
                    else if (month == "5")
                    {
                        month = "Mai";
                    }
                    else if (month == "6")
                    {
                        month = "Juni";
                    }
                    else if (month == "7")
                    {
                        month = "Juli";
                    }
                    else if (month == "8")
                    {
                        month = "August";
                    }
                    else if (month == "9")
                    {
                        month = "September";
                    }
                    else if (month == "10")
                    {
                        month = "Oktober";
                    }
                    else if (month == "11")
                    {
                        month = "November";
                    }
                    else if (month == "12")
                    {
                        month = "Dezember";
                    }
                    //MessageBox.Show(cmbKMGYear.Text + " " + (cmbKMGMonth.SelectedIndex + 1).ToString());
                    updatecmbUserKmG(cmbKMGYear.Text, (cmbKMGMonth.SelectedIndex + 1).ToString());
                }
            }
        }

        private void cmbKMGYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKMGset)
            {
                btnKMGActiveEx.IsEnabled = false;
                if (cmbKMGMonth.SelectedIndex != -1)
                {
                    var month = (int.Parse(cmbKMGMonth.SelectedIndex.ToString()) + 1).ToString();
                    if (month == "1")
                    {
                        month = "Jänner";
                    }
                    else if (month == "2")
                    {
                        month = "Februar";
                    }
                    else if (month == "3")
                    {
                        month = "März";
                    }
                    else if (month == "4")
                    {
                        month = "April";
                    }
                    else if (month == "5")
                    {
                        month = "Mai";
                    }
                    else if (month == "6")
                    {
                        month = "Juni";
                    }
                    else if (month == "7")
                    {
                        month = "Juli";
                    }
                    else if (month == "8")
                    {
                        month = "August";
                    }
                    else if (month == "9")
                    {
                        month = "September";
                    }
                    else if (month == "10")
                    {
                        month = "Oktober";
                    }
                    else if (month == "11")
                    {
                        month = "November";
                    }
                    else if (month == "12")
                    {
                        month = "Dezember";
                    }
                    //MessageBox.Show(cmbKMGYear.Text + " " + (cmbKMGMonth.SelectedIndex + 1).ToString());
                    updatecmbUserKmG(cmbKMGYear.Text, (cmbKMGMonth.SelectedIndex + 1).ToString());
                }
            }
        }

        public bool isNonWorkingDay(DateTime day)
        {
            var returner = false;
            foreach (var nonDay in nonWorkingDays)
            {
                if (day.DayOfWeek == DayOfWeek.Sunday || nonDay == day.ToString("dd.MM.yyyy") + " 00:00:00")
                {
                    returner = true;
                    break;
                }
            }
            return returner;
        }

        private void dgvZeiterfassung_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var mmm = (WorkingTime) e.Row.Item;

            if (mmm.art == "Nachtdienst")
            {
                e.Row.Background = Brushes.LightBlue;
            }
            else if (mmm.art == "Tagdienst")
            {
                e.Row.Background = Brushes.LightGoldenrodYellow;
            }
            else
            {
                e.Row.Background = Brushes.White;
            }

            if (isNonWorkingDay(mmm.datetimefrom))
            {
                e.Row.Background = Brushes.LightPink;
            }
        }

        private void btnAktualisierenNewestDokus_Click(object sender, RoutedEventArgs e)
        {
            refreshNewestDokus();
        }

        private void btnRecoverDoku_Click(object sender, RoutedEventArgs e)
        {
            txtDokuAußenkontakt.Text = txtDokuTempAussenkontakt.Text;
            txtDokuKoerperlich.Text = txtDokuTempKoerperlich.Text;
            txtDokuPflichte.Text = txtDokuTempPflichten.Text;
            txtDokuPsychisch.Text = txtDokuTempPsychisch.Text;
            txtDokuSchulisch.Text = txtDokuTempSchulisch.Text;
            txtDokuAußenkontakt.Background = Brushes.LightCyan;
            txtDokuKoerperlich.Background = Brushes.LightCyan;
            txtDokuPflichte.Background = Brushes.LightCyan;
            txtDokuPsychisch.Background = Brushes.LightCyan;
            txtDokuSchulisch.Background = Brushes.LightCyan;
            btnRecoverDoku.Visibility = Visibility.Hidden;
        }

        private void txtDokuKoerperlich_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back && e.Key != Key.LeftCtrl && e.Key != Key.RightCtrl &&
                e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Up && e.Key != Key.Down &&
                e.Key != Key.LeftAlt && e.Key != Key.RightAlt && e.Key != Key.Return && e.Key != Key.LeftShift &&
                e.Key != Key.RightShift && e.Key != Key.Tab && e.Key != Key.LWin && e.Key != Key.RWin &&
                e.Key != Key.Escape && e.Key != Key.Print)
            {
                saveTempDoku();
            }
            txtDokuAußenkontakt.Background = Brushes.White;
            txtDokuKoerperlich.Background = Brushes.White;
            txtDokuPflichte.Background = Brushes.White;
            txtDokuPsychisch.Background = Brushes.White;
            txtDokuSchulisch.Background = Brushes.White;
            btnRecoverDoku.Visibility = Visibility.Hidden;
        }

        public void getSQLitedata()
        {
/*
            db_clients = db.GetTable<clients>();
            db_clientsmedications = db.GetTable<clientsmedications>();
            db_clientstoservices = db.GetTable<clientstoservices>();
            db_medicamtens = db.GetTable<medicaments>();
            db_services = db.GetTable<services>();
            db_users = db.GetTable<users>();
            db_userstoservices = db.GetTable<userstoservices>();
            db_clientsdailydocs = db.GetTable<clientsdailydocs>();*/
        }

        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
/*
            getSQLitedata();

            var query = from p in db_clientsmedications where p.neu == 1 select p;
            foreach (clientsmedications tmp in query)
            {
                c.syncClientsmedications(tmp);
            }

            var query2 = from p in db_clientsdailydocs where p.neu == 1 select p;
            foreach (clientsdailydocs tmp in query2)
            {
                c.syncClientsdailydocs(tmp);
            }
            */
        }

        private void btnSyncDown_Click(object sender, RoutedEventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();

            var comLite = new SQLiteCommand("select * from clientsmedications;", con);
            comLite.ExecuteNonQuery();
        }

        private void saveTempDoku()
        {
            txtDokuTempAussenkontakt.Text = txtDokuAußenkontakt.Text;
            txtDokuTempKoerperlich.Text = txtDokuKoerperlich.Text;
            txtDokuTempPflichten.Text = txtDokuPflichte.Text;
            txtDokuTempPsychisch.Text = txtDokuPsychisch.Text;
            txtDokuTempSchulisch.Text = txtDokuSchulisch.Text;
        }

        private void txtDokuKoerperlichAnzeige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuKoerperlichAnzeige.Text, true);
            rb.ShowDialog();
        }

        private void txtDokuSchulischAnzeige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuSchulischAnzeige.Text, true);
            rb.ShowDialog();
        }

        private void txtDokuPsychischAnzeige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuPsychischAnzeige.Text, true);
            rb.ShowDialog();
        }

        private void txtDokuPflichteAnzeige_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void txtDokuPflichteAnzeige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuPflichteAnzeige.Text, true);
            rb.ShowDialog();
        }

        private void txtDokuAußenkontaktAnzeige_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuAußenkontaktAnzeige.Text, true);
            rb.ShowDialog();
        }

        private void txtDokuKoerperlich_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuKoerperlich.Text, false);
            rb.ShowDialog();
            txtDokuKoerperlich.Text = rb.getReadBig();
            saveTempDoku();
        }

        private void txtDokuSchulisch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuSchulisch.Text, false);
            rb.ShowDialog();
            txtDokuSchulisch.Text = rb.getReadBig();
            saveTempDoku();
        }

        private void txtDokuPsychisch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuPsychisch.Text, false);
            rb.ShowDialog();
            txtDokuPsychisch.Text = rb.getReadBig();
            saveTempDoku();
        }

        private void txtDokuPflichte_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuPflichte.Text, false);
            rb.ShowDialog();
            txtDokuPflichte.Text = rb.getReadBig();
            saveTempDoku();
        }

        private void txtDokuAußenkontakt_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var rb = new ReadBig();
            rb.setReadBig(txtDokuAußenkontakt.Text, false);
            rb.ShowDialog();
            txtDokuAußenkontakt.Text = rb.getReadBig();
            saveTempDoku();
        }

        private void dgvKB_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var mmm = (KassaBuchNode) e.Row.Item;

            if (mmm.netto > 0)
            {
                e.Row.Background = Brushes.LightGreen;
            }
            else if (mmm.netto < 0)
            {
                e.Row.Background = Brushes.LightPink;
            }
            else if (mmm.netto == 0)
            {
                e.Row.Background = Brushes.LightBlue;
            }
            else
            {
                e.Row.Background = Brushes.LightPink;
            }
            dgvKB.Focus();
        }

        private void dgNewestDokus_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (isloggedin)
            {
                var mmmm = (NewestDokus) e.Row.Item;

                switch (mmmm.art)
                {
                    case "Tagesdokumentation":
                        e.Row.Background = Brushes.LightBlue;
                        break;
                    case "Telefonat":
                        e.Row.Background = Brushes.LightGreen;
                        break;
                    case "Vorfallsprotokoll":
                        e.Row.Background = Brushes.LightPink;
                        break;
                    case "Gesprächsprotokoll":
                        e.Row.Background = Brushes.LightGoldenrodYellow;
                        break;
                    case "Fallverlaufsgespräch":
                        e.Row.Background = Brushes.LightCyan;
                        break;
                    case "Jahresbericht":
                        e.Row.Background = Brushes.LightSeaGreen;
                        break;
                    case "Zwischenbericht":
                        e.Row.Background = Brushes.LightSteelBlue;
                        break;
                    case "Bericht":
                        e.Row.Background = Brushes.LightGray;
                        break;
                    default:
                        e.Row.Background = Brushes.White;
                        break;
                }
            }
        }

        private void btnFunkZugeh_Click(object sender, RoutedEventArgs e)
        {
            var a = new User();
            User b;


            var temps = cmbAdminUsers.Text.Split(' ');
            for (var i = 0; i < userlist.Count; i++)
            {
                b = userlist[i];
                if (temps[0] == b.Firstname && temps[1] == b.Lastname)
                {
                    a = userlist[i];
                    break;
                }
            }

            //aw.ShowDialog();
            var tmp = new Funktionszugehoerigkeit(a.Kostalid, c);
            tmp.ShowDialog();
            funcz = tmp;
            //aw.Sh(true); 
            /*
            if (aw.aw1 != "" && aw.aw2 != "" && aw.aw3 != "" && aw.aw4 != "" && aw.aw5 != "")
            {
                if (aw.gewset)
                {
                    ChkVerifyAnswers.IsChecked = true;
                }
                else
                {
                    ChkVerifyAnswers.IsChecked = false;
                }
            }
            else
            {
                ChkVerifyAnswers.IsChecked = false;
            }
            */
        }

        #region Kassabuch

        private void tabKassabuch_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        public void refreshKBItems()
        {
            if (cmbBxKBHaus.SelectedIndex == -1)
            {
                lblKBErr.Content = "Bitte Wählen Sie das Haus, dessen Elemente aktualisiert werden soll!";
                return;
            }
            var hid = cmbBxKBHaus.SelectedValue.ToString();
            //DGV und CMB leeren und befüllen
            if (cmbBxKBHaus.SelectedValue != null)
            {
                var knrs = c.getKBKontoNr(cmbBxKBHaus.SelectedValue.ToString());
                cmbBxKBKnR.ItemsSource = knrs;
                cmbBxKBKnR.DisplayMemberPath = "ges";
                cmbBxKBKnR.SelectedValuePath = "id";
                txtBxBelNr.Text = (c.getHighestKBBelNr(hid) + 1).ToString();
                lblKBMaxBelNr.Content = "(" + c.getHighestKBBelNr(hid) + " ist die Höchste)";
                lblKBKassStand.Content = "Kassastand: " + c.getKBBallance(hid);
                dgvKB.ItemsSource = c.getKBEintr(hid, dtPckrKBFiltVon.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    dtPckrKBFiltBis.SelectedDate.Value.ToString("yyyy-MM-dd"));
                lblKBKst1.Content = "Kassastand am 1." + DateTime.Now.Month + "." +
                                    DateTime.Now.Year + ": " +
                                    c.getKBBal1(cmbBxKBHaus.SelectedValue.ToString(),
                                        new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                                            .ToString("yyyy-MM-dd"));
            }
            lblKBErr.Content = "";
            lblKBErr.Foreground = Brushes.Red;
        }

        public void KBEingetr()
        {
            var hid = cmbBxKBHaus.SelectedValue.ToString();
            lblKBMaxBelNr.Content = "(" + c.getHighestKBBelNr(hid) + " ist die Höchste)";
            lblKBKassStand.Content = "Kassastand: " + c.getKBBallance(hid);
            txtBxKBBeschr.Text = "";
            txtBxKBBrutto.Text = "";
            txtBxBelNr.Text = (c.getHighestKBBelNr(hid) + 1).ToString();
            txtBxBelNr.Focus();
            txtBxBelNr.SelectAll();
        }

        private void bttnKBAkt_Click(object sender, RoutedEventArgs e)
        {
            //c.KBInsertMonthTransaction();
            if (cmbBxKBHaus.SelectedIndex != -1)
            {
                var hid = cmbBxKBHaus.SelectedValue.ToString();
                txtBxBelNr.Text = (c.getHighestKBBelNr(hid) + 1).ToString();
            }
            refreshKBItems();
        }

        private void cmbBxKBHaus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            refreshKBItems();
            //lblKBKst1.Content = "Kassastand am 1." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + ": " + c.getKBBal1(cmbBxKBHaus.SelectedValue.ToString(), (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString("yyyy-MM-dd"));
            //lblKBKstL.Content = "Kassastand am " + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month).ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Year.ToString() + ": " + "?";
        }

        private void cmbBxKBKnR_DropDownClosed(object sender, EventArgs e)
        {
            cmbBxKBKnR.DisplayMemberPath = "knr";
            cmbBxKBKnR.HorizontalContentAlignment = HorizontalAlignment.Right;
        }

        private void cmbBxKBKnR_DropDownOpened(object sender, EventArgs e)
        {
            cmbBxKBKnR.DisplayMemberPath = "ges";
            cmbBxKBKnR.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void bttnKBKnrEintr_Click(object sender, RoutedEventArgs e)
        {
            cmbBxKBHaus.Background = color1;
            txtBxKBKnrKnr.Background = color1;
            if (cmbBxKBHaus.SelectedValue != null)
            {
                if (txtBxKBKnrKnr.Text != "")
                {
                    int val;
                    if (int.TryParse(txtBxKBKnrKnr.Text, out val))
                    {
                        if (val >= 0 || val <= 99999)
                        {
                            c.addKBKnr(txtBxKBKnrKnr.Text, txtBxKBKnrBeschr.Text, cmbBxKBHaus.SelectedValue.ToString());

                            lblKBErr.Content = "Kontonummer eingetragen: " + txtBxKBKnrKnr.Text +
                                               " mit Beschreibung \"" + txtBxKBKnrBeschr.Text + "\"";
                            lblKBErr.Foreground = Brushes.Green;

                            var ktonr = (KontoNr) cmbBxKBKnR.SelectedItem;
                            var knrs = c.getKBKontoNr(cmbBxKBHaus.SelectedValue.ToString());
                            cmbBxKBKnR.ItemsSource = knrs;
                            cmbBxKBKnR.DisplayMemberPath = "ges";
                            cmbBxKBKnR.SelectedValuePath = "id";
                            try
                            {
                                for (var i = 0; i < cmbBxKBKnR.Items.Count; i++)
                                {
                                    if (((KontoNr) cmbBxKBKnR.Items[i]).Equals(ktonr))
                                    {
                                        cmbBxKBKnR.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            txtBxKBKnrKnr.Background = color1;
                            lblKBErr.Content = "Die eingegebene Kontonummer ist ungültig!";
                        }
                    }
                    else
                    {
                        txtBxKBKnrKnr.Background = color1;
                        lblKBErr.Content = "Die eingegebene Kontonummer ist ungültig!";
                    }
                }
                else
                {
                    txtBxKBKnrKnr.Background = color1;
                    lblKBErr.Content = "Geben Sie bitte eine Kontonummer ein!";
                }
            }
            else
            {
                cmbBxKBHaus.Background = color1;
                lblKBErr.Content = "Bitte wählen Sie das Haus aus, für das die Kontonummer eingetragen werden soll!";
            }
        }

        private void bttnKBEintr_Click(object sender, RoutedEventArgs e)
        {
            txtBxBelNr.Background = color1;
            lblKBErr.Background = color1;
            string belnr, knr, beschr, brutto, steuers = "0", netto, mwst, datum, uid, hid;

            try
            {
                int bel;
                if (int.TryParse(txtBxBelNr.Text, out bel))
                {
                    belnr = txtBxBelNr.Text;
                }
                else
                {
                    txtBxBelNr.Background = color2;
                    lblKBErr.Content = "Diese eingegebene Belegnummer ist ungültig!";
                    return;
                }
                if (cmbBxKBKnR.SelectedIndex != -1)
                {
                    knr = cmbBxKBKnR.SelectedValue.ToString();
                }
                else
                {
                    lblKBErr.Background = color2;
                    lblKBErr.Content = "Bitte wählen Sie eine Kontonummer aus!";
                    return;
                }
                beschr = txtBxKBBeschr.Text;
                float br;
                if (float.TryParse(brutto = txtBxKBBrutto.Text, out br))
                {
                    if (br < 0)
                    {
                        br *= -1;
                    }
                }
                else
                {
                    lblKBErr.Background = color2;
                    lblKBErr.Content = "Dieser Betrag ist ungültig!";
                    return;
                }
                if ((bool) rdBttnKBAbh.IsChecked)
                {
                    br *= -1;
                    brutto = "-" + brutto;
                }
                if ((bool) rdBttnKBSteuers0.IsChecked)
                {
                    steuers = "0";
                }
                else if ((bool) rdBttnKBSteuers10.IsChecked)
                {
                    steuers = "10";
                }
                else if ((bool) rdBttnKBSteuers20.IsChecked)
                {
                    steuers = "20";
                }
                netto = (br / (100 + int.Parse(steuers)) * 100).ToString();
                beschr = txtBxKBBeschr.Text;
                mwst = (br - double.Parse(netto)).ToString();
                datum = dtPckrKBDat.SelectedDate.Value.ToString("yyyy-MM-dd");
                uid = u.Id;
                hid = cmbBxKBHaus.SelectedValue.ToString();
                c.addKBEintr(belnr, knr, beschr, brutto, steuers, netto, mwst, datum, uid, hid);
                lblKBErr.Content = "Datensatz eingetragen";
                lblKBErr.Foreground = Brushes.Green;
                KBEingetr();
                dgvKB.ItemsSource = c.getKBEintr(hid, dtPckrKBFiltVon.SelectedDate.Value.ToString("yyyy-MM-dd"),
                    dtPckrKBFiltBis.SelectedDate.Value.ToString("yyyy-MM-dd"));
            }
            catch
            {
                /**/
                /**/
            }
        }

        private void bttnKBExport_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            var kbns = (List<KassaBuchNode>) dgvKB.ItemsSource;
            var skbc = new SelectKBColumns(false);
            var delimiter = ";";
            var file = string.Empty;
            sfd.FileName = "Kassabuch-" + DateTime.Now.Date.ToString().Split(' ')[0];
            sfd.Filter = "CSV-Datei|*.csv";
            sfd.Title = "CSV Exportieren";
            skbc.ShowDialog();
            if (skbc.ok && (bool) sfd.ShowDialog() && sfd.FileName != "")
            {
                try
                {
                    for (var i = 0; i < skbc.dgvKBOrder.Columns.Count; i++)
                    {
                        switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                        {
                            case "Belegnummer":
                                file += "Belegnummer";
                                break;

                            case "Datum":
                                file += "Datum";
                                break;

                            case "Kommentar":
                                file += "Kommentar";
                                break;

                            case "Bruttobetrag":
                                file += "Bruttobetrag";
                                break;

                            case "Einnahmen":
                                file += "Einnahmen";
                                break;

                            case "Ausgaben":
                                file += "Ausgaben";
                                break;

                            case "Steuersatz":
                                file += "Steuersatz";
                                break;

                            case "Nettobetrag":
                                file += "Nettobetrag";
                                break;

                            case "Mehrwertsteuer":
                                file += "Mehrwertsteuer";
                                break;

                            case "Kassastand":
                                file += "Kassastand";
                                break;

                            case "Kontonummer":
                                file += "Kontonummer";
                                break;

                            case "Eingetragen von":
                                file += "Eingetragen von";
                                break;
                        }
                        if (i != skbc.dgvKBOrder.Columns.Count - 1)
                        {
                            file += delimiter;
                        }
                    }
                    file += Environment.NewLine;
                    for (var i = 0; i < kbns.Count; i++)
                    {
                        for (var j = 0; j < skbc.dgvKBOrder.Columns.Count; j++)
                        {
                            switch ((string) skbc.dgvKBOrder.Columns[j].Header)
                            {
                                case "Belegnummer":
                                    file += kbns[i].belnr.ToString();
                                    break;

                                case "Datum":
                                    file += kbns[i].dat;
                                    break;

                                case "Kommentar":
                                    file += kbns[i].bez;
                                    break;

                                case "Bruttobetrag":
                                    file += kbns[i].eing.Length > kbns[i].ausg.Length
                                        ? kbns[i].eing
                                        : "-" + kbns[i].ausg;
                                    break;

                                case "Einnahmen":
                                    file += kbns[i].eing;
                                    break;

                                case "Ausgaben":
                                    file += kbns[i].ausg;
                                    break;

                                case "Steuersatz":
                                    file += kbns[i].steuers.ToString();
                                    break;

                                case "Nettobetrag":
                                    file += kbns[i].netto.ToString();
                                    break;

                                case "Mehrwertsteuer":
                                    file += kbns[i].mwst.ToString();
                                    break;

                                case "Kassastand":
                                    file += kbns[i].kassst.ToString();
                                    break;

                                case "Kontonummer":
                                    file += kbns[i].knr.ToString();
                                    break;

                                case "Eingetragen von":
                                    file += kbns[i].name;
                                    break;
                            }
                            if (j != skbc.dgvKBOrder.Columns.Count - 1)
                            {
                                file += delimiter;
                            }
                        }
                        file += Environment.NewLine;
                    }
                    using (var sw = new StreamWriter(sfd.FileName))
                    {
                        sw.Write(file);
                    }
                    if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo,
                            MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            Process.Start(sfd.FileName);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblKBErr.Content = "Beim Öffnen der Datei ist ein Fehler aufgetreten (" + ex.Message + ")";
                }
            }
            else
            {
                lblKBErr.Content = "Vom Nutzer abgebrochen!";
            }
        }

        private void bttnKBPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgvKB.ItemsSource == null || ((List<KassaBuchNode>) dgvKB.ItemsSource).Count < 1)
            {
                lblKBErr.Content = "Die aktuelle Liste ist leer";
                return;
            }
            var pädehef = "";
            int lbel = -1,
                ldat = -1,
                lcom = -1,
                lbrutt = -1,
                lein = -1,
                laus = -1,
                lsteu = -1,
                lnett = -1,
                lmwst = -1,
                lkass = -1,
                lknr = -1,
                leingetr = -1;
            var skbc = new SelectKBColumns(true);
            skbc.ShowDialog();
            if (skbc.canceled || !skbc.closed)
            {
                lblKBErr.Content = "Vom Nutzer abgebrochen";
                return;
            }
            var sfd = new SaveFileDialog();
            sfd.FileName = "Kassabuch-" + DateTime.Now.Date.ToString().Split(' ')[0];
            sfd.Filter = "PDF-Dokument|*.pdf";
            sfd.Title = "PDF generieren";

            var kstlpls = new CreateAFuckingPDF();
            var len = 1 + skbc.dgvKBOrder.Columns.Count * 3;

            //90sp
            //71z

            //┌─┬─┬─┐
            //│ │ │ │
            //├─┼─┼─┤
            //│ │ │ │
            //├─┼─┼─┤
            //│ │ │ │
            //└─┴─┴─┘

            //längste Zeichen
            for (var i = 0;
                i < skbc.dgvKBOrder.Columns.Count;
                i++) ////////////////////////////////////////////////////////////////////////////////////////////////////
            {
                switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                {
                    case "Belegnummer":
                        lbel = 0;
                        for (var bel = 0; bel < dgvKB.Items.Count; bel++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[bel]).belnr.ToString().Length > lbel)
                            {
                                lbel = ((KassaBuchNode) dgvKB.Items[bel]).belnr.ToString().Length;
                            }
                        }
                        if (lbel < "BelegNr.".Length)
                        {
                            lbel = "BelegNr.".Length;
                        }
                        len += lbel;
                        break;

                    case "Datum":
                        ldat = 10;
                        len += ldat;
                        break;

                    case "Kommentar":
                        lcom = 0;
                        for (var com = 0; com < dgvKB.Items.Count; com++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[com]).bez.Length > lcom)
                            {
                                lcom = ((KassaBuchNode) dgvKB.Items[com]).bez.Length;
                            }
                        }
                        if (lcom < "Kommentar".Length)
                        {
                            lcom = "Kommentar".Length;
                        }
                        len += lcom;
                        break;

                    case "Bruttobetrag":
                        lbrutt = 0;
                        for (var bru = 0; bru < dgvKB.Items.Count; bru++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[bru]).eing.Length > lbrutt)
                            {
                                lbrutt = ((KassaBuchNode) dgvKB.Items[bru]).eing.Length;
                            }
                            if (("-" + ((KassaBuchNode) dgvKB.Items[bru]).ausg).Length > lbrutt)
                            {
                                lbrutt = ((KassaBuchNode) dgvKB.Items[bru]).ausg.Length;
                            }
                        }
                        if (lbrutt < "Brutto".Length)
                        {
                            lbrutt = "Brutto".Length;
                        }
                        len += lbrutt;
                        break;

                    case "Einnahmen":
                        lein = 0;
                        for (var ein = 0; ein < dgvKB.Items.Count; ein++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[ein]).eing.Length > lein)
                            {
                                lein = ((KassaBuchNode) dgvKB.Items[ein]).eing.Length;
                            }
                        }
                        if (lein < "Einnahmen".Length)
                        {
                            lein = "Einnahmen".Length;
                        }
                        len += lein;
                        break;

                    case "Ausgaben":
                        laus = 0;
                        for (var aus = 0; aus < dgvKB.Items.Count; aus++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[aus]).ausg.Length > laus)
                            {
                                laus = ((KassaBuchNode) dgvKB.Items[aus]).ausg.Length;
                            }
                        }
                        if (laus < "Ausgaben".Length)
                        {
                            laus = "Ausgaben".Length;
                        }
                        len += laus;
                        break;

                    case "Steuersatz":
                        lsteu = 0;
                        for (var steu = 0; steu < dgvKB.Items.Count; steu++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[steu]).steuers.ToString().Length > lsteu)
                            {
                                lsteu = ((KassaBuchNode) dgvKB.Items[steu]).steuers.ToString().Length;
                            }
                        }
                        if (lsteu < "Steuersatz".Length)
                        {
                            lsteu = "Steuersatz".Length;
                        }
                        len += lsteu;
                        break;

                    case "Nettobetrag":
                        lnett = 0;
                        for (var nett = 0; nett < dgvKB.Items.Count; nett++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[nett]).netto.ToString().Length > lnett)
                            {
                                lnett = ((KassaBuchNode) dgvKB.Items[nett]).netto.ToString().Length;
                            }
                        }
                        if (lnett < "Netto".Length)
                        {
                            lnett = "Netto".Length;
                        }
                        len += lnett;
                        break;

                    case "Mehrwertsteuer":
                        lmwst = 0;
                        for (var mwst = 0; mwst < dgvKB.Items.Count; mwst++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[mwst]).mwst.ToString().Length > lmwst)
                            {
                                lmwst = ((KassaBuchNode) dgvKB.Items[mwst]).mwst.ToString().Length;
                            }
                        }
                        if (lmwst < "MWST".Length)
                        {
                            lmwst = "MWST".Length;
                        }
                        len += lmwst;
                        break;

                    case "Kassastand":
                        lkass = 0;
                        for (var kass = 0; kass < dgvKB.Items.Count; kass++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[kass]).kassst.ToString().Length > lkass)
                            {
                                lkass = ((KassaBuchNode) dgvKB.Items[kass]).kassst.ToString().Length;
                            }
                        }
                        if (lkass < "Kassastand".Length)
                        {
                            lkass = "Kassastand".Length;
                        }
                        len += lkass;
                        break;

                    case "Kontonummer":
                        lknr = 0;
                        for (var knr = 0; knr < dgvKB.Items.Count; knr++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[knr]).knr.ToString().Length > lknr)
                            {
                                lknr = ((KassaBuchNode) dgvKB.Items[knr]).knr.ToString().Length;
                            }
                        }
                        if (lknr < "KontoNr.".Length)
                        {
                            lknr = "KontoNr.".Length;
                        }
                        len += lknr;
                        break;

                    case "Eingetragen von":
                        leingetr = 0;
                        for (var eingetr = 0; eingetr < dgvKB.Items.Count; eingetr++)
                        {
                            if (((KassaBuchNode) dgvKB.Items[eingetr]).name.Length > leingetr)
                            {
                                leingetr = ((KassaBuchNode) dgvKB.Items[eingetr]).name.Length;
                            }
                        }
                        if (leingetr < "Einträger".Length)
                        {
                            leingetr = "Einträger".Length;
                        }
                        len += leingetr;
                        break;
                } //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            }

            if (len > 90)
            {
                if ((lbel == -1) ^ (len - lcom > 90))
                {
                    MessageBox.Show(
                        "Es ist nicht genug Platz für die gewählten Spalten verfügbar, der PDF-Export kann nicht erfolgen!");
                    return;
                }
                var lcomsp = 0;
                string[] comsp;
                len -= lcom;
                for (var j = 0; j < dgvKB.Items.Count; j++)
                {
                    comsp = ((KassaBuchNode) dgvKB.Items[j]).bez.Split(' ');
                    for (var k = 0; k < comsp.Length; k++)
                    {
                        if (comsp[k].Length > lcomsp)
                        {
                            lcomsp = comsp[k].Length;
                        }
                    }
                }
                if (lcomsp < "Kommentar".Length)
                {
                    lcomsp = "Kommentar".Length;
                }
                if (len + lcomsp > 90)
                {
                    MessageBox.Show(
                        "Es ist nicht genug Platz für die gewählten Spalten verfügbar, der PDF-Export kann nicht erfolgen!");
                    return;
                }
                lcom = 89 - len;
            }

            //┌─┬─┬─┐
            //│ │ │ │
            //├─┼─┼─┤
            //│ │ │ │
            //├─┼─┼─┤
            //│ │ │ │
            //└─┴─┴─┘

            //Header
            pädehef += "┌─";
            for (var i = 0; i < skbc.dgvKBOrder.Columns.Count; i++)
            {
                switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                {
                    case "Belegnummer":
                        pädehef += fillSpace(lbel, "─");
                        break;

                    case "Datum":
                        pädehef += fillSpace(ldat, "─");
                        break;

                    case "Kommentar":
                        pädehef += fillSpace(lcom, "─");
                        break;

                    case "Bruttobetrag":
                        pädehef += fillSpace(lbrutt, "─");
                        break;

                    case "Einnahmen":
                        pädehef += fillSpace(lein, "─");
                        break;

                    case "Ausgaben":
                        pädehef += fillSpace(laus, "─");
                        break;

                    case "Steuersatz":
                        pädehef += fillSpace(lsteu, "─");
                        break;

                    case "Nettobetrag":
                        pädehef += fillSpace(lnett, "─");
                        break;

                    case "Mehrwertsteuer":
                        pädehef += fillSpace(lmwst, "─");
                        break;

                    case "Kassastand":
                        pädehef += fillSpace(lkass, "─");
                        break;

                    case "Kontonummer":
                        pädehef += fillSpace(lknr, "─");
                        break;

                    case "Eingetragen von":
                        pädehef += fillSpace(leingetr, "─");
                        break;
                }
                if (i == skbc.dgvKBOrder.Columns.Count - 1)
                {
                    pädehef += "─┐";
                }
                else
                {
                    pädehef += "─┬─";
                }
            }
            pädehef += Environment.NewLine;
            pädehef += "│ ";
            for (var i = 0; i < skbc.dgvKBOrder.Columns.Count; i++)
            {
                switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                {
                    case "Belegnummer":
                        pädehef += "BelegNr.";
                        pädehef += fillSpace(lbel - "BelegNr.".Length, " ");
                        break;

                    case "Datum":
                        pädehef += "Datum";
                        pädehef += fillSpace(ldat - "Datum".Length, " ");
                        break;

                    case "Kommentar":
                        pädehef += "Kommentar";
                        pädehef += fillSpace(lcom - "Kommentar".Length, " ");
                        break;

                    case "Bruttobetrag":
                        pädehef += "Brutto";
                        pädehef += fillSpace(lbrutt - "Brutto".Length, " ");
                        break;

                    case "Einnahmen":
                        pädehef += "Einnahmen";
                        pädehef += fillSpace(lein - "Einnahmen".Length, " ");
                        break;

                    case "Ausgaben":
                        pädehef += "Ausgaben";
                        pädehef += fillSpace(laus - "Ausgaben".Length, " ");
                        break;

                    case "Steuersatz":
                        pädehef += "Steuersatz";
                        pädehef += fillSpace(lsteu - "Steuersatz".Length, " ");
                        break;

                    case "Nettobetrag":
                        pädehef += "Netto";
                        pädehef += fillSpace(lnett - "Netto".Length, " ");
                        break;

                    case "Mehrwertsteuer":
                        pädehef += "MWST";
                        pädehef += fillSpace(lmwst - "MWST".Length, " ");
                        break;

                    case "Kassastand":
                        pädehef += "Kassastand";
                        pädehef += fillSpace(lkass - "Kassastand".Length, " ");
                        break;

                    case "Kontonummer":
                        pädehef += "KontoNr.";
                        pädehef += fillSpace(lknr - "KontoNr.".Length, " ");
                        break;

                    case "Eingetragen von":
                        pädehef += "Einträger";
                        pädehef += fillSpace(leingetr - "Einträger".Length, " ");
                        break;
                }
                if (i == skbc.dgvKBOrder.Columns.Count - 1)
                {
                    pädehef += " │";
                }
                else
                {
                    pädehef += " │ ";
                }
            }
            pädehef += Environment.NewLine;
            pädehef += "├─";
            for (var i = 0; i < skbc.dgvKBOrder.Columns.Count; i++)
            {
                switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                {
                    case "Belegnummer":
                        pädehef += fillSpace(lbel, "─");
                        break;

                    case "Datum":
                        pädehef += fillSpace(ldat, "─");
                        break;

                    case "Kommentar":
                        pädehef += fillSpace(lcom, "─");
                        break;

                    case "Bruttobetrag":
                        pädehef += fillSpace(lbrutt, "─");
                        break;

                    case "Einnahmen":
                        pädehef += fillSpace(lein, "─");
                        break;

                    case "Ausgaben":
                        pädehef += fillSpace(laus, "─");
                        break;

                    case "Steuersatz":
                        pädehef += fillSpace(lsteu, "─");
                        break;

                    case "Nettobetrag":
                        pädehef += fillSpace(lnett, "─");
                        break;

                    case "Mehrwertsteuer":
                        pädehef += fillSpace(lmwst, "─");
                        break;

                    case "Kassastand":
                        pädehef += fillSpace(lkass, "─");
                        break;

                    case "Kontonummer":
                        pädehef += fillSpace(lknr, "─");
                        break;

                    case "Eingetragen von":
                        pädehef += fillSpace(leingetr, "─");
                        break;
                }
                if (i == skbc.dgvKBOrder.Columns.Count - 1)
                {
                    pädehef += "─┤";
                }
                else
                {
                    pädehef += "─┼─";
                }
            }


            var nextLine = string.Empty;
            for (var i = 0; i < dgvKB.Items.Count; i++)
            {
                pädehef += Environment.NewLine;
                pädehef += "│ ";
                for (var j = 0; j < skbc.dgvKBOrder.Columns.Count; j++)
                {
                    switch ((string) skbc.dgvKBOrder.Columns[j].Header)
                    {
                        case "Belegnummer":
                            pädehef += fillSpace(lbel - ((KassaBuchNode) dgvKB.Items[i]).belnr.ToString().Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).belnr.ToString();
                            break;

                        case "Datum":
                            pädehef += fillSpace(ldat - ((KassaBuchNode) dgvKB.Items[i]).dat.Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).dat;
                            break;

                        case "Kommentar":
                            if (((KassaBuchNode) dgvKB.Items[i]).bez.Length <= lcom)
                            {
                                pädehef += ((KassaBuchNode) dgvKB.Items[i]).bez;
                                pädehef += fillSpace(lcom - ((KassaBuchNode) dgvKB.Items[i]).bez.Length, " ");
                            }
                            else
                            {
                                var words = ((KassaBuchNode) dgvKB.Items[i]).bez.Split(' ');
                                var usedlength = 0;
                                for (var l = 0; l < words.Length; l++)
                                {
                                    if (usedlength + words[l].Length <= lcom)
                                    {
                                        pädehef += words[l];
                                        usedlength += words[l].Length;
                                        if (l < words.Length - 1 && usedlength + 1 <= lcom)
                                        {
                                            pädehef += " ";
                                            usedlength++;
                                        }
                                    }
                                    else
                                    {
                                        pädehef += fillSpace(lcom - usedlength, " ");
                                        for (var m = l; m < words.Length; m++)
                                        {
                                            nextLine += words[m] + " ";
                                        }
                                        nextLine = nextLine.Substring(0, nextLine.Length - 1);
                                        break;
                                    }
                                }
                            }
                            break;

                        case "Bruttobetrag":
                            if (lein > laus)
                            {
                                pädehef += fillSpace(lbrutt - ((KassaBuchNode) dgvKB.Items[i]).eing.Length, " ");
                                pädehef += ((KassaBuchNode) dgvKB.Items[i]).eing;
                            }
                            else
                            {
                                pädehef += fillSpace(lbrutt - ("-" + ((KassaBuchNode) dgvKB.Items[i]).ausg).Length,
                                    " ");
                                pädehef += "-" + ((KassaBuchNode) dgvKB.Items[i]).ausg;
                            }
                            break;

                        case "Einnahmen":
                            pädehef += fillSpace(lein - ((KassaBuchNode) dgvKB.Items[i]).eing.Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).eing;
                            break;

                        case "Ausgaben":
                            pädehef += fillSpace(laus - ((KassaBuchNode) dgvKB.Items[i]).ausg.Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).ausg;
                            break;

                        case "Steuersatz":
                            pädehef += fillSpace(lsteu - ((KassaBuchNode) dgvKB.Items[i]).steuers.ToString().Length,
                                " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).steuers.ToString();
                            break;

                        case "Nettobetrag":
                            pädehef += fillSpace(lnett - ((KassaBuchNode) dgvKB.Items[i]).netto.ToString().Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).netto.ToString();
                            break;

                        case "Mehrwertsteuer":
                            pädehef += fillSpace(lmwst - ((KassaBuchNode) dgvKB.Items[i]).mwst.ToString().Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).mwst.ToString();
                            break;

                        case "Kassastand":
                            pädehef += fillSpace(lkass + -((KassaBuchNode) dgvKB.Items[i]).kassst.ToString().Length,
                                " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).kassst.ToString();
                            break;

                        case "Kontonummer":
                            pädehef += fillSpace(lknr - ((KassaBuchNode) dgvKB.Items[i]).knr.ToString().Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).knr.ToString();
                            break;

                        case "Eingetragen von":
                            pädehef += fillSpace(leingetr - ((KassaBuchNode) dgvKB.Items[i]).name.Length, " ");
                            pädehef += ((KassaBuchNode) dgvKB.Items[i]).name;
                            break;
                    }
                    if (j == skbc.dgvKBOrder.Columns.Count - 1)
                    {
                        pädehef += " │";
                    }
                    else
                    {
                        pädehef += " │ ";
                    }
                }
                while (nextLine != "")
                {
                    pädehef += Environment.NewLine;
                    pädehef += "│ ";
                    for (var j = 0; j < skbc.dgvKBOrder.Columns.Count; j++)
                    {
                        switch ((string) skbc.dgvKBOrder.Columns[j].Header)
                        {
                            case "Belegnummer":
                                pädehef += fillSpace(lbel, " ");
                                break;

                            case "Datum":
                                pädehef += fillSpace(ldat, " ");
                                break;

                            case "Kommentar":
                                var words = nextLine.Split(' ');
                                var usedlength = 0;
                                for (var l = 0; l < words.Length; l++)
                                {
                                    if (usedlength + words[l].Length <= lcom)
                                    {
                                        usedlength += words[l].Length;
                                        pädehef += words[l];
                                        if (l < words.Length - 1 && usedlength + 1 <= lcom)
                                        {
                                            pädehef += " ";
                                            usedlength++;
                                        }
                                        if (l == words.Length - 1)
                                        {
                                            nextLine = "";
                                            pädehef += fillSpace(lcom - usedlength, " ");
                                        }
                                    }
                                    else
                                    {
                                        nextLine = "";
                                        pädehef += fillSpace(lcom - usedlength, " ");
                                        for (var m = l; m < words.Length; m++)
                                        {
                                            nextLine += words[m] + " ";
                                        }
                                        nextLine = nextLine.Substring(0, nextLine.Length - 1);
                                        break;
                                    }
                                }
                                break;

                            case "Bruttobetrag":
                                pädehef += fillSpace(lbrutt, " ");
                                break;

                            case "Einnahmen":
                                pädehef += fillSpace(lein, " ");
                                break;

                            case "Ausgaben":
                                pädehef += fillSpace(laus, " ");
                                break;

                            case "Steuersatz":
                                pädehef += fillSpace(lsteu, " ");
                                break;

                            case "Nettobetrag":
                                pädehef += fillSpace(lnett, " ");
                                break;

                            case "Mehrwertsteuer":
                                pädehef += fillSpace(lmwst, " ");
                                break;

                            case "Kassastand":
                                pädehef += fillSpace(lkass, " ");
                                break;

                            case "Kontonummer":
                                pädehef += fillSpace(lknr, " ");
                                break;

                            case "Eingetragen von":
                                pädehef += fillSpace(leingetr, " ");
                                break;
                        }
                        if (j == skbc.dgvKBOrder.Columns.Count - 1)
                        {
                            pädehef += " │";
                        }
                        else
                        {
                            pädehef += " │ ";
                        }
                    }
                }
            }
            pädehef += Environment.NewLine;
            pädehef += "└─";
            for (var i = 0; i < skbc.dgvKBOrder.Columns.Count; i++)
            {
                switch ((string) skbc.dgvKBOrder.Columns[i].Header)
                {
                    case "Belegnummer":
                        pädehef += fillSpace(lbel, "─");
                        break;

                    case "Datum":
                        pädehef += fillSpace(ldat, "─");
                        break;

                    case "Kommentar":
                        pädehef += fillSpace(lcom, "─");
                        break;

                    case "Bruttobetrag":
                        pädehef += fillSpace(lbrutt, "─");
                        break;

                    case "Einnahmen":
                        pädehef += fillSpace(lein, "─");
                        break;

                    case "Ausgaben":
                        pädehef += fillSpace(laus, "─");
                        break;

                    case "Steuersatz":
                        pädehef += fillSpace(lsteu, "─");
                        break;

                    case "Nettobetrag":
                        pädehef += fillSpace(lnett, "─");
                        break;

                    case "Mehrwertsteuer":
                        pädehef += fillSpace(lmwst, "─");
                        break;

                    case "Kassastand":
                        pädehef += fillSpace(lkass, "─");
                        break;

                    case "Kontonummer":
                        pädehef += fillSpace(lknr, "─");
                        break;

                    case "Eingetragen von":
                        pädehef += fillSpace(leingetr, "─");
                        break;
                }
                if (i == skbc.dgvKBOrder.Columns.Count - 1)
                {
                    pädehef += "─┘";
                }
                else
                {
                    pädehef += "─┴─";
                }
            }

            kstlpls.text = pädehef;
            if ((bool) sfd.ShowDialog() && sfd.FileName != "")
            {
                kstlpls.truenamebro = sfd.FileName;
                kstlpls.createThisShit2();
                if (MessageBox.Show("Dokument öffnen?", "Öffnen", MessageBoxButton.YesNo, MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)
                {
                    try
                    {
                        Process.Start(sfd.FileName);
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                lblKBErr.Content = "Vom Nutzer abgebrochen!";
            }
        }

        private string fillSpace(int len, string s)
        {
            var ret = "";
            for (var i = 0; i < len; i++)
            {
                ret += s;
            }
            return ret;
        }

        private void dgvKB_CurrentCellChanged(object sender, EventArgs e)
        {
            if (rowBeingEdited != null)
            {
                if (kbrekstopp)
                {
                    kbrekstopp = false;
                    return;
                }
                var liste = (List<KassaBuchNode>) dgvKB.ItemsSource;
                var node = (KassaBuchNode) rowBeingEdited.Item;
                DateTime dt;
                float f;

                switch (colhd)
                {
                    case "Belegnummer":
                        c.updateKB("BelNr", node.belnr.ToString(), node.id.ToString());
                        lblKBErr.Content = "";
                        break;
                    case "Datum":
                        if (DateTime.TryParse(node.dat, out dt))
                        {
                            c.updateKB("Datum", "\'" + dt.ToString("yyyy-MM-dd") + "\'",
                                node.id.ToString()); //Potentieller Bug ?
                            lblKBErr.Content = "";
                        }
                        else
                        {
                            lblKBErr.Content =
                                "Kann Datum nicht verarbeiten! Verwenden Sie \"dd.MM.yyyy\" (z.B. 1.1.2014)";
                        }
                        break;
                    case "Kommentar":
                        c.updateKB("Bezeichn", "\'" + node.bez + "\'", node.id.ToString());
                        lblKBErr.Content = "";
                        break;
                    case "Einnahmen":
                        if (float.TryParse(node.eing, out f))
                        {
                            node.ausg = "";
                            node.netto = f / (100 + node.steuers) * 100;
                            node.mwst = f - node.netto;
                            c.updateKB("Brutto", node.eing, node.id.ToString());
                            c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                            c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                            refreshKBItems();
                            kbrekstopp = true;
                            rowBeingEdited = null;
                        }
                        else
                        {
                            lblKBErr.Content = "Kann den Betrag nicht verarbeiten!";
                        }
                        break;
                    case "Ausgaben":
                        if (float.TryParse("-" + node.ausg, out f))
                        {
                            node.eing = "";
                            node.netto = f / (100 + node.steuers) * 100;
                            node.mwst = f - node.netto;
                            c.updateKB("Brutto", "-" + node.ausg, node.id.ToString());
                            c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                            c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                            refreshKBItems();
                            kbrekstopp = true;
                            rowBeingEdited = null;
                        }
                        else
                        {
                            lblKBErr.Content = "Kann den Betrag nicht verarbeiten!";
                        }
                        break;
                    case "Steuersatz %":
                        if (node.steuers == 0 || node.steuers == 10 || node.steuers == 20)
                        {
                            if (node.eing == "")
                            {
                                f = float.Parse(node.ausg);
                                node.netto = f / (100 + node.steuers) * 100;
                                node.mwst = f - node.netto;
                                c.updateKB("Steuers", node.steuers.ToString(), node.id.ToString());
                                c.updateKB("Brutto", "-" + node.ausg, node.id.ToString());
                                c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                                c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                            }
                            else
                            {
                                f = float.Parse(node.eing);
                                node.netto = f / (100 + node.steuers) * 100;
                                node.mwst = f - node.netto;
                                c.updateKB("Steuers", node.steuers.ToString(), node.id.ToString());
                                c.updateKB("Brutto", node.eing, node.id.ToString());
                                c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                                c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                            }
                            refreshKBItems();
                            kbrekstopp = true;
                            rowBeingEdited = null;
                        }
                        else
                        {
                            lblKBErr.Content = "Ungültiger Steuersatz!";
                        }
                        break;
                    case "Nettobetrag":
                        f = node.netto / 100 * (100 + node.steuers);
                        if (node.netto > 0)
                        {
                            node.eing = f.ToString();
                            node.ausg = "";
                            c.updateKB("Brutto", node.eing, node.id.ToString());
                        }
                        else
                        {
                            node.ausg = f.ToString();
                            node.eing = "";
                            c.updateKB("Brutto", "-" + node.ausg, node.id.ToString());
                        }
                        node.mwst = f - node.netto;
                        c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                        c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                        refreshKBItems();
                        kbrekstopp = true;
                        rowBeingEdited = null;
                        break;
                    case "Mehrwertsteuer":
                        if (node.steuers == 0)
                        {
                            lblKBErr.Content = "Kann Brutto und Netto nicht aus MWST errechnen, wenn Steuersatz 0 ist!";
                            break;
                        }
                        f = node.mwst / node.steuers * (100 + node.steuers);
                        if (f > 0)
                        {
                            node.eing = f.ToString();
                            node.ausg = "";
                            c.updateKB("Brutto", node.eing, node.id.ToString());
                        }
                        else
                        {
                            node.ausg = f.ToString();
                            node.eing = "";
                            c.updateKB("Brutto", "-" + node.ausg, node.id.ToString());
                        }
                        node.netto = f / (100 + node.steuers) * 100;
                        c.updateKB("Netto", node.netto.ToString(), node.id.ToString());
                        c.updateKB("MWST", node.mwst.ToString(), node.id.ToString());
                        refreshKBItems();
                        kbrekstopp = true;
                        rowBeingEdited = null;
                        break;
                    case "Kontonummer":
                        //Existiert diese Kontonummer?
                        int knrid;
                        if ((knrid = c.getKBKnrIDByKnr(node.knr.ToString(), cmbBxKBHaus.SelectedValue.ToString())) !=
                            -1)
                        {
                            c.updateKB("KontoNr", knrid.ToString(), node.id.ToString());
                            lblKBErr.Content = "";
                        }
                        else
                        {
                            lblKBErr.Content = "Kontonummer existiert nicht!";
                        }
                        break;
                    case "Eingetragen von":
                        //Existiert name?
                        int uid;
                        if ((uid = c.getKBUserIDByName(node.name, cmbBxKBHaus.SelectedValue.ToString())) !=
                            -1)
                        {
                            c.updateKB("EintrUserID", uid.ToString(), node.id.ToString());
                            lblKBErr.Content = "";
                        }
                        else
                        {
                            lblKBErr.Content = "Dieser Nutzer existiert nicht! (Achten Sie auf Groß/Kleinschreibung)";
                        }
                        break;
                    case "Kassastand":
                        refreshKBItems();
                        kbrekstopp = true;
                        rowBeingEdited = null;
                        lblKBErr.Content = "Dieses Feld kann nicht verändert werden";
                        break;
                }
            }
        }

        private void dgvKB_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var rowView = e.Row;
            colhd = (string) e.Column.Header;
            rowBeingEdited = rowView;
        }

        private void dtPckrKBFiltVon_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBxKBHaus.SelectedIndex != -1)
            {
                refreshKBItems();
            }
        }

        private void dtPckrKBFiltBis_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBxKBHaus.SelectedIndex != -1)
            {
                refreshKBItems();
            }
        }


        private void bttnKBKasstDat_Click(object sender, RoutedEventArgs e)
        {
            if (cmbBxKBHaus.SelectedIndex != -1)
            {
                var kbkad = new KBKassstAtDate(c, cmbBxKBHaus.SelectedValue.ToString());
                kbkad.Show();
            }
            else
            {
                lblKBErr.Content = "Ein Haus muss ausgewählt sein!";
            }
        }

        #endregion


        #region Contacts

        private void tabKontakte_Loaded(object sender, RoutedEventArgs e)
        {
            if (isOnline)
            {
            }
            else
            {
                MessageBox.Show(
                    "Sie sind OFFLINE!\nBitte stellen Sie eine Netzwerkverbindung her und starten TheraS4 erneut!",
                    "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPW.IsEnabled = false;
                txtUser.IsEnabled = false;
                btnLogin.IsEnabled = false;
            }
        }

        private void fillContactCmb()
        {
            // WARUM BEIM PROGRAMMSTART?!?!?!?
            /*
            try
            {
                List<string> fillTitle;
                List<string> fillGroups;
                fillTitle = c.fillTitleIntoContacts();
                fillGroups = c.fillGroupsIntoContacts();

                for (int i = 0; i < fillTitle.Count; i++)
                {
                    cmbContTitle.Items.Add(fillTitle[i].ToString());
                }

                for (int i = 0; i < fillGroups.Count; i++)
                {
                    cmbContGroup.Items.Add(fillGroups[i].ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            */
        }

        private void refreshAllContacts()
        {
            contactSearch = false;

            dgvContact.ItemsSource = "";
            dgvContact.ItemsSource = allContacts;
        }

        private bool refreshContacts()
        {
            contactSearch = true;

            var cmb = new List<int>();
            var keys = new List<string>();

            if (contSearch1)
            {
                if (cmbContSearch1.SelectedIndex != -1 && txtContSearch1.Text != "")
                {
                    if (contSearch2)
                    {
                        if (cmbContSearch2.SelectedIndex != -1 && txtContSearch2.Text != "")
                        {
                            if (contSearch3)
                            {
                                if (cmbContSearch3.SelectedIndex != -1 && txtContSearch3.Text != "")
                                {
                                    cmb.Add(cmbContSearch1.SelectedIndex + 1);
                                    cmb.Add(cmbContSearch2.SelectedIndex + 1);
                                    cmb.Add(cmbContSearch3.SelectedIndex + 1);

                                    keys.Add(txtContSearch1.Text);
                                    keys.Add(txtContSearch2.Text);
                                    keys.Add(txtContSearch3.Text);
                                }
                                else
                                {
                                    if (cmbContSearch3.SelectedIndex == -1 && txtContSearch3.Text == "")
                                    {
                                        MessageBox.Show(
                                            "Bitte wählen Sie ein Kriterium und einen Suchbegriff im 3. Suchfeld!",
                                            "Achtung !", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (cmbContSearch3.SelectedIndex == -1)
                                    {
                                        MessageBox.Show("Bitte wählen Sie ein Kriterium im 3. Suchfeld!", "Achtung !",
                                            MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (txtContSearch3.Text == "")
                                    {
                                        MessageBox.Show("Bitte wählen Sie einen Suchbegriff im 3. Suchfeld!",
                                            "Achtung !", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }

                                    return false;
                                }
                            }
                            else
                            {
                                cmb.Add(cmbContSearch1.SelectedIndex + 1);
                                cmb.Add(cmbContSearch2.SelectedIndex + 1);

                                keys.Add(txtContSearch1.Text);
                                keys.Add(txtContSearch2.Text);
                            }
                        }
                        else
                        {
                            if (cmbContSearch2.SelectedIndex == -1 && txtContSearch2.Text == "")
                            {
                                MessageBox.Show("Bitte wählen Sie ein Kriterium und einen Suchbegriff im 2. Suchfeld!",
                                    "Achtung !", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (cmbContSearch2.SelectedIndex == -1)
                            {
                                MessageBox.Show("Bitte wählen Sie ein Kriterium im 2. Suchfeld!", "Achtung !",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else if (txtContSearch2.Text == "")
                            {
                                MessageBox.Show("Bitte wählen Sie einen Suchbegriff im 2. Suchfeld!", "Achtung !",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            return false;
                        }
                    }
                    else
                    {
                        cmb.Add(cmbContSearch1.SelectedIndex + 1);

                        keys.Add(txtContSearch1.Text);
                    }
                }
                else
                {
                    if (cmbContSearch1.SelectedIndex == -1 && txtContSearch1.Text == "")
                    {
                        MessageBox.Show("Bitte wählen Sie ein Kriterium und einen Suchbegriff im 1. Suchfeld!",
                            "Achtung !", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (cmbContSearch1.SelectedIndex == -1)
                    {
                        MessageBox.Show("Bitte wählen Sie ein Kriterium im 1. Suchfeld!", "Achtung !",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (txtContSearch1.Text == "")
                    {
                        MessageBox.Show("Bitte wählen Sie einen Suchbegriff im 1. Suchfeld!", "Achtung !",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    return false;
                }


                if (keys.Count > 0)
                {
                    var run = allContacts;
                    var found = new List<Contacts>();
                    bool cont;

                    for (var i = 0; i < cmb.Count; i++)
                    {
                        for (var j = 0; j < run.Count; j++)
                        {
                            cont = true;

                            switch (cmb[i])
                            {
                                case 1:
                                    if (!run[j].salutation.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 2:
                                    if (!run[j].title.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 3:
                                    if (!run[j].firstname.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 4:
                                    if (!run[j].lastname.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 5:
                                    if (!run[j].institution.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 6:
                                    if (!run[j].company.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 7:
                                    if (!run[j].department.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 8:
                                    if (!run[j].country.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 9:
                                    if (!run[j].city.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 10:
                                    if (!run[j].street.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 11:
                                    if (!run[j].zip.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 12:
                                    if (!run[j].groups.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 13:
                                    if (!run[j].phone1.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 14:
                                    if (!run[j].phone2.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 15:
                                    if (!run[j].fax.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 16:
                                    if (!run[j].email.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 17:
                                    if (!run[j].www.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 18:
                                    if (!run[j].skype.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 19:
                                    if (!run[j].function.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                                case 20:
                                    if (!run[j].comment.ToLower().Contains(keys[i].ToLower()))
                                    {
                                        cont = false;
                                    }
                                    break;
                            }

                            if (cont)
                            {
                                found.Add(run[j]);
                            }
                        }

                        run = found;
                        found = new List<Contacts>();
                    }

                    dgvContact.ItemsSource = "";
                    dgvContact.ItemsSource = run;
                }
            }
            else
            {
                refreshAllContacts();
            }

            deleteContactText();

            return true;
        }

        private void deleteContactText()
        {
            /*
            cmbContSalutation.SelectedValue = "";
            cmbContTitle.SelectedValue = "";
            txtContFN.Text = "";
            txtContLN.Text = "";
            txtContInst.Text = "";
            txtContComp.Text = "";
            txtContDep.Text = "";
            txtContCountry.Text = "";
            txtContCity.Text = "";
            txtContStreet.Text = "";
            txtContZip.Text = "";
            cmbContGroup.SelectedValue = "";
            txtContTel1.Text = "";
            txtContTel2.Text = "";
            txtContFax.Text = "";
            txtContEmail.Text = "";
            txtContWeb.Text = "";
            txtContSkype.Text = "";
            txtContFunction.Text = "";
            txtContCom.Text = "";
            */
        }

        private void deleteContact(Contacts cont)
        {
            var newList = new List<Contacts>();

            for (var i = 0; i < allContacts.Count; i++)
            {
                if (allContacts[i].id == cont.id)
                {
                    for (var j = i; j < allContacts.Count; j++)
                    {
                        if (j != allContacts.Count - 1)
                        {
                            newList.Add(allContacts[j + 1]);
                        }
                        else
                        {
                            allContacts[j] = null;
                        }
                    }
                    break;
                }
                newList.Add(allContacts[i]);
            }

            allContacts = newList;
            refreshContacts();

            c.deleteContacts(cont.id);
        }

        private void showContact()
        {
            /*
            cmbContSalutation.IsEnabled = true;
            cmbContTitle.IsEnabled = true;
            txtContFN.IsEnabled = true;
            txtContLN.IsEnabled = true;
            txtContInst.IsEnabled = true;
            txtContComp.IsEnabled = true;
            txtContDep.IsEnabled = true;
            txtContCountry.IsEnabled = true;
            txtContCity.IsEnabled = true;
            txtContStreet.IsEnabled = true;
            txtContZip.IsEnabled = true;
            cmbContGroup.IsEnabled = true;
            txtContTel1.IsEnabled = true;
            txtContTel2.IsEnabled = true;
            txtContFax.IsEnabled = true;
            txtContEmail.IsEnabled = true;
            txtContWeb.IsEnabled = true;
            txtContSkype.IsEnabled = true;
            txtContFunction.IsEnabled = true;
            txtContCom.IsEnabled = true;

            btnContSave.Visibility = Visibility.Visible;
            btnContSave.IsEnabled = true;

            btnContCancel.Visibility = Visibility.Visible;
            btnContCancel.IsEnabled = true;

            btnContNew.Visibility = Visibility.Hidden;
            btnContNew.IsEnabled = false;

            btnContChange.Visibility = Visibility.Hidden;
            btnContChange.IsEnabled = false;

            btnContDelete.Visibility = Visibility.Hidden;
            btnContDelete.IsEnabled = false;
            */
        }

        private void hideContact()
        {
            /*
            cmbContSalutation.IsEnabled = false;
            cmbContTitle.IsEnabled = false;
            txtContFN.IsEnabled = false;
            txtContLN.IsEnabled = false;
            txtContInst.IsEnabled = false;
            txtContComp.IsEnabled = false;
            txtContDep.IsEnabled = false;
            txtContCountry.IsEnabled = false;
            txtContCity.IsEnabled = false;
            txtContStreet.IsEnabled = false;
            txtContZip.IsEnabled = false;
            cmbContGroup.IsEnabled = false;
            txtContTel1.IsEnabled = false;
            txtContTel2.IsEnabled = false;
            txtContFax.IsEnabled = false;
            txtContEmail.IsEnabled = false;
            txtContWeb.IsEnabled = false;
            txtContSkype.IsEnabled = false;
            txtContFunction.IsEnabled = false;
            txtContCom.IsEnabled = false;

            btnContSave.Visibility = Visibility.Hidden;
            btnContSave.IsEnabled = false;

            btnContCancel.Visibility = Visibility.Hidden;
            btnContCancel.IsEnabled = false;

            btnContNew.Visibility = Visibility.Visible;
            btnContNew.IsEnabled = true;

            btnContChange.Visibility = Visibility.Visible;
            btnContChange.IsEnabled = true;

            btnContDelete.Visibility = Visibility.Visible;
            btnContDelete.IsEnabled = true;

            lblContAlertCity.Visibility = Visibility.Hidden;
            lblContAlertCountry.Visibility = Visibility.Hidden;
            lblContAlertDep.Visibility = Visibility.Hidden;
            lblContAlertEmail.Visibility = Visibility.Hidden;
            lblContAlertFax.Visibility = Visibility.Hidden;
            lblContAlertFN.Visibility = Visibility.Hidden;
            lblContAlertFunction.Visibility = Visibility.Hidden;
            lblContAlertLN.Visibility = Visibility.Hidden;
            lblContAlertSalutation.Visibility = Visibility.Hidden;
            lblContAlertStreet.Visibility = Visibility.Hidden;
            lblContAlertTel1.Visibility = Visibility.Hidden;
            lblContAlertTel2.Visibility = Visibility.Hidden;
            lblContAlertZip.Visibility = Visibility.Hidden;
            */
        }

        private bool? checkContact()
        {
            return true;
            /*
            bool? ok = true;
            char[] zeichen = { '!', '"', '§', '$', '%', '&', '(', ')', '{', '}', '[', ']', '=', '?', '\\', '+', '*', '~', '#', '_', ':', ',', ';', '<', '>', '|', '^', '°', '²', '³', '@', '€', 'µ' };
            char[] abc = new char[52];

            // Hide Alerts
            lblContAlertCity.Visibility = Visibility.Hidden;
            lblContAlertCountry.Visibility = Visibility.Hidden;
            lblContAlertDep.Visibility = Visibility.Hidden;
            lblContAlertEmail.Visibility = Visibility.Hidden;
            lblContAlertFax.Visibility = Visibility.Hidden;
            lblContAlertFN.Visibility = Visibility.Hidden;
            lblContAlertLN.Visibility = Visibility.Hidden;
            lblContAlertFunction.Visibility = Visibility.Hidden;
            lblContAlertSalutation.Visibility = Visibility.Hidden;
            lblContAlertStreet.Visibility = Visibility.Hidden;
            lblContAlertTel1.Visibility = Visibility.Hidden;
            lblContAlertTel2.Visibility = Visibility.Hidden;
            lblContAlertZip.Visibility = Visibility.Hidden;

            // fill with ABC
            int a = 0;
            for (int i = 65; i <= 122; i++)
            {
                if (i <= 90 || i >= 97)
                {
                    abc[a] = (char)i;
                    a++;
                }
            }

            // check ABC
            for (int i = 0; i < abc.Length; i++)
            {
                if (txtContTel1.Text.Contains(abc[i]))
                {
                    ok = false;
                    lblContAlertTel1.Visibility = Visibility.Visible;
                }

                if (txtContTel2.Text.Contains(abc[i]))
                {
                    ok = false;
                    lblContAlertTel2.Visibility = Visibility.Visible;
                }

                if (txtContZip.Text.Contains(abc[i]))
                {
                    ok = false;
                    lblContAlertZip.Visibility = Visibility.Visible;
                }

                if (txtContFax.Text.Contains(abc[i]))
                {
                    ok = false;
                    lblContAlertFax.Visibility = Visibility.Visible;
                }
            }

            // check Number

            for (int i = 0; i <= 9; i++)
            {
                if (txtContFN.Text.Contains(i.ToString()))
                {
                    ok = false;
                    lblContAlertFN.Visibility = Visibility.Visible;
                }

                if (txtContLN.Text.Contains(i.ToString()))
                {
                    ok = false;
                    lblContAlertLN.Visibility = Visibility.Visible;
                }

                if (txtContCountry.Text.Contains(i.ToString()))
                {
                    ok = false;
                    lblContAlertCountry.Visibility = Visibility.Visible;
                }

                if (txtContCity.Text.Contains(i.ToString()))
                {
                    ok = false;
                    lblContAlertCity.Visibility = Visibility.Visible;
                }
            }

            // check Sonder
            for (int i = 0; i < zeichen.Length; i++)
            {
                if (txtContFN.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertFN.Visibility = Visibility.Visible;
                }

                if (txtContLN.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertLN.Visibility = Visibility.Visible;
                }

                if (txtContCountry.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertCountry.Visibility = Visibility.Visible;
                }

                if (txtContCity.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertCity.Visibility = Visibility.Visible;
                }

                if (txtContDep.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertDep.Visibility = Visibility.Visible;
                }

                if (txtContZip.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertZip.Visibility = Visibility.Visible;
                }

                if (txtContFunction.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertFunction.Visibility = Visibility.Visible;
                }

                if (txtContStreet.Text.Contains(zeichen[i]))
                {
                    ok = false;
                    lblContAlertStreet.Visibility = Visibility.Visible;
                }
            }

            // check Email
            if (txtContEmail.Text != "")
            {
                if (!txtContEmail.Text.Contains('@'))
                {
                    ok = false;
                    lblContAlertEmail.Visibility = Visibility.Visible;
                }

                if (!txtContEmail.Text.Contains('.'))
                {
                    ok = false;
                    lblContAlertEmail.Visibility = Visibility.Visible;
                }
            }

            // check Pflichtfelder
            if (txtContLN.Text == "" && txtContInst.Text == "")
            {
                ok = null;
                MessageBox.Show("Es muss entweder ein Nachname oder eine Institution angegeben sein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (txtContCountry.Text == "" && txtContCity.Text == "" && txtContStreet.Text == "" && txtContZip.Text == "" && txtContTel1.Text == "" && txtContFax.Text == "" && txtContEmail.Text == "" && txtContWeb.Text == "" && txtContCom.Text == "" && txtContSkype.Text == "")
                {
                    ok = null;
                    MessageBox.Show("Es muss mindestens eines der folgenden Felder zusätzlich angegeben sein:\n\tAdresse (Land, Stadt, Straße, PLZ)\n\tTelefon\n\tFax\n\tEmail\n\tSkype\n\tWebseite\n\tKommentar", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if ((txtContCity.Text != "" && (txtContStreet.Text == "" || txtContZip.Text == "" || txtContCountry.Text == "")) ||
                        (txtContStreet.Text != "" && (txtContCity.Text == "" || txtContZip.Text == "" || txtContCountry.Text == "")) ||
                        (txtContCountry.Text != "" && (txtContCity.Text == "" || txtContZip.Text == "" || txtContStreet.Text == "")) ||
                        (txtContZip.Text != "" && (txtContStreet.Text == "" || txtContCity.Text == "" || txtContCountry.Text == "")))
                    {
                        ok = null;
                        MessageBox.Show("Es muss die ganze Adresse angegeben werden!\n\tLand, Stadt, Straße und PLZ", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            return ok;
        }

        private bool checkNewContact(Contacts cont)
        {
            for (int i = 0; i < allContacts.Count; i++)
            {
                if (allContacts[i].city == cont.city &&
                    allContacts[i].comment == cont.comment &&
                    allContacts[i].company == cont.company &&
                    allContacts[i].country == cont.country &&
                    allContacts[i].department == cont.department &&
                    allContacts[i].email == cont.email &&
                    allContacts[i].fax == cont.fax &&
                    allContacts[i].firstname == cont.firstname &&
                    allContacts[i].function == cont.function &&
                    allContacts[i].groups == cont.groups &&
                    allContacts[i].institution == cont.institution &&
                    allContacts[i].lastname == cont.lastname &&
                    allContacts[i].phone1 == cont.phone1 &&
                    allContacts[i].phone2 == cont.phone2 &&
                    allContacts[i].salutation == cont.salutation &&
                    allContacts[i].skype == cont.skype &&
                    allContacts[i].street == cont.street &&
                    allContacts[i].title == cont.title &&
                    allContacts[i].www == cont.www &&
                    allContacts[i].zip == cont.zip)
                {
                    return false;
                }
            }

            return true;
            */
        }

        private void btnContNew_Click(object sender, RoutedEventArgs e)
        {
            showContact();
            deleteContactText();
            contactMode = true;
        }

        private void btnContChange_Click(object sender, RoutedEventArgs e)
        {
            if (dgvContact.SelectedIndex != -1)
            {
                //showContact();
                var tmp = allContacts.ElementAt(dgvContact.SelectedIndex);
                var ec = new EditContacts(u.Id, (Contacts) dgvContact.SelectedItem, c);
                ec.ShowDialog();
                contactMode = false;
                if (ec.saved)
                {
                    tmp = ec.selectedItem;
                    dgvContact.ItemsSource = null;
                    dgvContact.ItemsSource = allContacts;
                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie den Datensatz, welchen Sie bearbeiten wollen!", "Achtung!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnContDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvContact.SelectedIndex != -1)
            {
                if (MessageBox.Show("Sind Sie sicher, dass sie diesen Datensatz löschen wollen?", "Achtung!",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    var cont = (Contacts) dgvContact.SelectedItem;
                    deleteContact(cont);
                }
            }
            else
            {
                MessageBox.Show("Bitte wählen Sie den Datensatz, welchen Sie löschen wollen!", "Achtung!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnContSave_Click(object sender, RoutedEventArgs e)
        {
            var ec = new EditContacts(u.Id, c);
            ec.ShowDialog();
            /*
            bool? ok = checkContact();
            Contacts cont = new Contacts(allContacts.Last<Contacts>().id + 1, cmbContSalutation.Text, cmbContTitle.Text, txtContFN.Text, txtContLN.Text, txtContInst.Text, txtContComp.Text, txtContDep.Text, txtContCountry.Text, txtContZip.Text, txtContCity.Text, txtContStreet.Text, txtContTel1.Text, txtContTel2.Text, txtContFax.Text, txtContEmail.Text, txtContWeb.Text, txtContSkype.Text, txtContCom.Text, cmbContGroup.Text, txtContFunction.Text);

            if (checkNewContact(cont))
            {
                if (ok == true)
                {
                    if (contactMode)
                    {
                        allContacts.Add(cont);
                        //c.setContacts(u.Id, txtContInst.Text, cmbContSalutation.Text, cmbContTitle.Text, txtContFN.Text, txtContLN.Text, txtContComp.Text, txtContDep.Text, txtContStreet.Text, txtContZip.Text, txtContCountry.Text, txtContCity.Text, txtContTel1.Text, txtContTel2.Text, txtContFax.Text, txtContEmail.Text, txtContWeb.Text, txtContSkype.Text, txtContCom.Text, cmbContGroup.Text, txtContFunction.Text);
                    }
                    else
                    {
                        Contacts selected = (Contacts)dgvContact.SelectedItem;
                        dgvContact.SelectedIndex = -1;
                        int selectedID = -1;
                        int i = 0;

                        while (i < allContacts.Count && selectedID == -1)
                        {
                            if (allContacts[i].id == selected.id)
                            {
                                selectedID = i;
                            }
                            else
                            {
                                i++;
                            }
                        }

                        allContacts[selectedID].id = selected.id;
                        allContacts[selectedID].salutation = cmbContSalutation.Text;
                        allContacts[selectedID].title = cmbContTitle.Text;
                        allContacts[selectedID].firstname = txtContFN.Text;
                        allContacts[selectedID].lastname = txtContLN.Text;
                        allContacts[selectedID].institution = txtContInst.Text;
                        allContacts[selectedID].company = txtContComp.Text;
                        allContacts[selectedID].department = txtContDep.Text;
                        allContacts[selectedID].country = txtContCountry.Text;
                        allContacts[selectedID].zip = txtContZip.Text;
                        allContacts[selectedID].city = txtContCity.Text;
                        allContacts[selectedID].street = txtContStreet.Text;
                        allContacts[selectedID].phone1 = txtContTel1.Text;
                        allContacts[selectedID].phone2 = txtContTel2.Text;
                        allContacts[selectedID].fax = txtContFax.Text;
                        allContacts[selectedID].email = txtContEmail.Text;
                        allContacts[selectedID].www = txtContWeb.Text;
                        allContacts[selectedID].skype = txtContSkype.Text;
                        allContacts[selectedID].comment = txtContCom.Text;
                        allContacts[selectedID].groups = cmbContGroup.Text;
                        allContacts[selectedID].function = txtContFunction.Text;

                        contID = selected.id;
                        c.changeContacts(contID, u.Id, txtContInst.Text, cmbContSalutation.Text, cmbContTitle.Text, txtContFN.Text, txtContLN.Text, txtContComp.Text, txtContDep.Text, txtContStreet.Text, txtContZip.Text, txtContCountry.Text, txtContCity.Text, txtContTel1.Text, txtContTel2.Text, txtContFax.Text, txtContEmail.Text, txtContWeb.Text, txtContSkype.Text, txtContCom.Text, cmbContGroup.Text, txtContFunction.Text);
                    }

                    hideContact();

                    if (contactSearch)
                    {
                        refreshContacts();
                    }
                    else
                    {
                        refreshAllContacts();
                    }
                }
                else if (ok == false)
                {
                    MessageBox.Show("Es ist ein Fehler aufgetreten!\nBitte überprüfen Sie die markierten Eingabefelder nach evt. Zahlen, Sonderzeichen oder leeren Feldern!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Dieser Kontakt existiert bereits!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            */
        }


        private void btnContSearch1Plus_Click(object sender, RoutedEventArgs e)
        {
            if (refreshContacts())
            {
                cmbContSearch1.Visibility = Visibility.Visible;
                txtContSearch1.Visibility = Visibility.Visible;
                btnContSearch1Minus.Visibility = Visibility.Visible;
                btnContSearch2Plus.Visibility = Visibility.Visible;
                lblContSearch.Visibility = Visibility.Visible;

                btnContSearch1Plus.Visibility = Visibility.Hidden;

                contSearch1 = true;
            }
        }

        private void btnContSearch2Plus_Click(object sender, RoutedEventArgs e)
        {
            if (refreshContacts())
            {
                cmbContSearch2.Visibility = Visibility.Visible;
                txtContSearch2.Visibility = Visibility.Visible;
                btnContSearch2Minus.Visibility = Visibility.Visible;
                btnContSearch3Plus.Visibility = Visibility.Visible;

                btnContSearch1Minus.Visibility = Visibility.Hidden;
                btnContSearch2Plus.Visibility = Visibility.Hidden;

                contSearch2 = true;
            }
        }

        private void btnContSearch3Plus_Click(object sender, RoutedEventArgs e)
        {
            if (refreshContacts())
            {
                cmbContSearch3.Visibility = Visibility.Visible;
                txtContSearch3.Visibility = Visibility.Visible;
                btnContSearch3Minus.Visibility = Visibility.Visible;

                btnContSearch3Plus.Visibility = Visibility.Hidden;
                btnContSearch2Minus.Visibility = Visibility.Hidden;

                contSearch3 = true;
            }
        }

        private void btnContSearch1Minus_Click(object sender, RoutedEventArgs e)
        {
            btnContSearch1Plus.Visibility = Visibility.Visible;

            cmbContSearch1.Visibility = Visibility.Hidden;
            txtContSearch1.Visibility = Visibility.Hidden;
            btnContSearch1Minus.Visibility = Visibility.Hidden;
            btnContSearch2Plus.Visibility = Visibility.Hidden;
            lblContSearch.Visibility = Visibility.Hidden;

            cmbContSearch1.SelectedIndex = -1;
            txtContSearch1.Text = "";

            contSearch1 = false;

            refreshContacts();
        }

        private void btnContSearch2Minus_Click(object sender, RoutedEventArgs e)
        {
            btnContSearch2Plus.Visibility = Visibility.Visible;
            btnContSearch1Minus.Visibility = Visibility.Visible;

            cmbContSearch2.Visibility = Visibility.Hidden;
            txtContSearch2.Visibility = Visibility.Hidden;
            btnContSearch2Minus.Visibility = Visibility.Hidden;
            btnContSearch3Plus.Visibility = Visibility.Hidden;

            cmbContSearch2.SelectedIndex = -1;
            txtContSearch2.Text = "";

            contSearch2 = false;

            refreshContacts();
        }

        private void btnContSearch3Minus_Click(object sender, RoutedEventArgs e)
        {
            btnContSearch3Plus.Visibility = Visibility.Visible;
            btnContSearch2Minus.Visibility = Visibility.Visible;

            cmbContSearch3.Visibility = Visibility.Hidden;
            txtContSearch3.Visibility = Visibility.Hidden;
            btnContSearch3Minus.Visibility = Visibility.Hidden;

            cmbContSearch3.SelectedIndex = -1;
            txtContSearch3.Text = "";

            contSearch3 = false;

            refreshContacts();
        }

        private void btnContSearch_Click(object sender, RoutedEventArgs e)
        {
            refreshContacts();
        }

        private void txtContSearch1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                refreshContacts();
            }
        }

        private void txtContSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                refreshContacts();
            }
        }

        private void txtContSearch3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                refreshContacts();
            }
        }

        private void dgvContact_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            if (dgvContact.SelectedIndex != -1)
            {
                Contacts cont = (Contacts)dgvContact.SelectedItem;

                contID = cont.id;
                cmbContSalutation.Text = cont.salutation;
                cmbContTitle.Text = cont.title;
                txtContFN.Text = cont.firstname;
                txtContLN.Text = cont.lastname;
                txtContInst.Text = cont.institution;
                txtContComp.Text = cont.company;
                txtContDep.Text = cont.department;
                txtContCountry.Text = cont.country;
                txtContCity.Text = cont.city;
                txtContStreet.Text = cont.street;
                txtContZip.Text = cont.zip;
                cmbContGroup.Text = cont.groups;
                txtContTel1.Text = cont.phone1;
                txtContTel2.Text = cont.phone2;
                txtContFax.Text = cont.fax;
                txtContEmail.Text = cont.email;
                txtContWeb.Text = cont.www;
                txtContSkype.Text = cont.skype;
                txtContFunction.Text = cont.function;
                txtContCom.Text = cont.comment;

                hideContact();
            }

            btnContSave.Visibility = Visibility.Hidden;
            btnContSave.IsEnabled = false;

            btnContCancel.Visibility = Visibility.Hidden;
            btnContCancel.IsEnabled = false;

            btnContNew.Visibility = Visibility.Visible;
            btnContNew.IsEnabled = true;

            btnContChange.Visibility = Visibility.Visible;
            btnContChange.IsEnabled = true;
            */
        }

        private void dgvContact_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnContChange_Click(sender, e);
        }

        #endregion
    }
}