using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IntranetTG;

using IntranetTG.Objects;

namespace TGTheraS4
{
    /// <summary>
    /// Interaction logic for EditWorkingTimeDialog.xaml
    /// </summary>
    public partial class EditWorkingTimeDialog : Window
    {
        private SQLCommands commands;
        public List<Project> projects = new List<Project>();
        public string projectString = "";
        private bool isNew = false;
        object[] editData = null;
        int userid;
        private int month;
        private int year;
        private bool check;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditWorkingTimeDialog(SQLCommands sql)
        {
            InitializeComponent();
            commands = sql;
            dpFrom.Text = DateTime.Now.ToShortDateString();
            //tpFrom.Value = DateTime.Now;
            isNew = true;
            this.month = -1;
            this.check = false;
            this.year = -1;
            userid = -1;
        }

        public EditWorkingTimeDialog(int month, int year, bool check, int userid)
        {
            InitializeComponent();
            dpFrom.Text = DateTime.Now.ToShortDateString();
            //tpFrom.Value = DateTime.Now;
            isNew = true;
            this.month = month;
            this.check = check;
            this.year = year;
            this.userid = userid;
        }

        public EditWorkingTimeDialog(object[] data)
        {
            InitializeComponent();

            editData = data;

            //projectString = commands.getProjects();

            dpFrom.Text = data[1].ToString();
            //tpFrom.Value = Convert.ToDateTime(data[1]);
            dpTo.Text = data[2].ToString();
            //tpTo.Value = Convert.ToDateTime(data[2]);
            txtComment.Text = data[3].ToString();
            cbxType.Text = data[4].ToString();
        }

        public bool checkIT(String data)
        {
            bool trueStory = true;

            string[] tmp = new string[2];
            tmp = data.Split(':');
            if ((tmp[0].Length > 2) | (tmp[1].Length > 2))
            {
                trueStory = false;
            }
            if ((Convert.ToInt32(tmp[0]) < 0) | (Convert.ToInt32(tmp[0]) > 23))
            {
                trueStory = false;
            }
            if ((Convert.ToInt32(tmp[1]) < 0) | (Convert.ToInt32(tmp[1]) > 59))
            {
                trueStory = false;
            }

            return trueStory;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            dpFrom.Background = color1;
            txt_von.Background = color1;
            txt_bis.Background = color1;
            dpTo.Background = color1;
            txtComment.Background = color1;

            bool trueStory = true;
            try
            {
                if (dpFrom.SelectedDate.Value > DateTime.Now)
                {
                    MessageBox.Show("Von Datum liegt in der Zukunft", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dpFrom.Background = color2;
                    return;
                }
                if (!txt_von.Text.Contains(':'))
                {
                    MessageBox.Show("Falsche Zeit eingetragen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_von.Background = color2;
                    return;
                }
                trueStory = checkIT(txt_von.Text);
                if (!txt_bis.Text.Contains(':'))
                {
                    MessageBox.Show("Falsche Zeit eingetragen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_bis.Background = color2;
                    return;
                }
                trueStory = checkIT(txt_bis.Text);

                string[] a = txt_von.Text.Split(':');
                if (a[0] == "24")
                {
                    MessageBox.Show("Falsche Zeit eingetragen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_von.Background = color2;
                    return;
                }
                a = txt_bis.Text.Split(':');
                if (a[0] == "24")
                {
                    MessageBox.Show("Falsche Zeit eingetragen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_bis.Background = color2;
                    return;
                }

                TimeSpan temp = dpTo.SelectedDate.Value - dpFrom.SelectedDate.Value;
                if (temp.Days == 0)
                {
                    //Wenn am selben Tag ended wie anfängt, muss die Bis-Uhrzeit größer als die Von-Uhrzeit sein!
                    String[] tmp = txt_von.Text.Split(':');
                    int von_hour = Convert.ToInt32(tmp[0]);
                    int von_min = Convert.ToInt32(tmp[1]);
                    tmp = txt_bis.Text.Split(':');
                    int bis_hour = Convert.ToInt32(tmp[0]);
                    int bis_min = Convert.ToInt32(tmp[1]);

                    if (von_hour == bis_hour)
                    {
                        if (bis_min < von_min)
                        {
                            trueStory = false;
                        }
                    }

                    if (bis_hour < von_hour)
                    {
                        trueStory = false;
                    }

                }

            }
            catch 
            {
                
                trueStory = false;
            }
            if (!trueStory)
            {
                MessageBox.Show("Falsche Zeit eingetragen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpTo.Background = color2;
                dpFrom.Background = color2;
                return;
            }

            if (txtComment.Text.Contains('"') | txtComment.Text.Contains("'"))
            {
                MessageBox.Show("Illegales Zeichen '\"' oder \"'\" verwendet", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtComment.Background = color2;
                return;
            }
            String[] time = txt_von.Text.Split(':');
            int hour = Convert.ToInt32(time[0]);
            int minute = Convert.ToInt32(time[1]);

            DateTime dateFrom = new DateTime(dpFrom.SelectedDate.Value.Year, dpFrom.SelectedDate.Value.Month, dpFrom.SelectedDate.Value.Day, hour, minute, 0);

            time = txt_bis.Text.Split(':');
            hour = Convert.ToInt32(time[0]);
            minute = Convert.ToInt32(time[1]);

            DateTime dateTo = new DateTime(dpTo.SelectedDate.Value.Year, dpTo.SelectedDate.Value.Month, dpTo.SelectedDate.Value.Day, hour, minute, 0);

            //... Aufteilung in Tag und Nachtstunden

            if (!setwt(userid, cbxType.SelectionBoxItem.ToString(), dateFrom, dateTo, txtComment.Text))
            {
                MessageBox.Show("Fehler beim Eintragen. Möglicherweiße gibt es diesen Datensatz bereits oder es gibt einen Fehler bei der Internetverbindung!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
            }
        }

        public bool setwt(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            bool noprobs = true;
            TimeSpan tmp = new TimeSpan();
            TimeSpan minutes = new TimeSpan(0, 0, 0);
            bool sameday = false;

            tmp = datetimeto - datetimefrom;

            DateTime nextday = datetimefrom + new TimeSpan(24, 0, 0);
            if (datetimeto.Day == nextday.Day && datetimeto.Month == nextday.Month && datetimeto.Year == nextday.Year && datetimeto.Hour == 0 && datetimeto.Minute == 0)
            {
                sameday = true;
            }
            else if (datetimefrom.Day == datetimeto.Day && datetimefrom.Month == datetimeto.Month && datetimefrom.Year == datetimeto.Year)
            {
                sameday = true;
            }

            if (sameday && tmp.TotalDays <= 1)
            {
                bool check = false;
                //Nachtdienst
                //Nachtdienst von 0 - 6

                // if (((datetimefrom.Year == datetimeto.Year) && (datetimefrom.Month == datetimeto.Month) && (datetimefrom.Day == datetimeto.Day)) && (datetimefrom.Hour >= 0 && ((datetimefrom.Hour <= 5) | (datetimefrom.Hour == 6 && datetimefrom.Minute == 0))))
                if (sameday && (datetimefrom.Hour >= 0 && ((datetimefrom.Hour <= 5) | (datetimefrom.Hour == 6 && datetimefrom.Minute == 0))))
                {
                    if (((datetimefrom.Year == datetimeto.Year) && (datetimefrom.Month == datetimeto.Month) && (datetimefrom.Day == datetimeto.Day)) && (datetimeto.Hour >= 0 && ((datetimeto.Hour <= 5) | (datetimeto.Hour == 6 && datetimeto.Minute == 0))))
                    {
                        check = true;
                        if (art != "Dienst")
                        {
                            if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, datetimeto, art + " - " + comment))
                            {
                                noprobs = false;
                            }
                        }
                        else
                        {
                            if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, datetimeto, comment))
                            {
                                noprobs = false;
                            }
                        }
                    }
                }

                //Nachtdienst von 22 - 0
                if (datetimefrom.Hour >= 22 && datetimefrom.Hour <= 23)
                {
                    if ((datetimeto.Hour >= 22 && datetimeto.Hour <= 23) | (datetimeto.Hour == 0 && datetimeto.Minute == 0))
                    {
                        check = true;
                        if (art != "Dienst")
                        {
                            if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, datetimeto, art + " - " + comment))
                            {
                                noprobs = false;
                            }
                        }
                        else
                        {
                            if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, datetimeto, comment))
                            {
                                noprobs = false;
                            }
                        }
                    }
                }

                //Tagdienst
                //Tagdienst von 6 - 22
                if (datetimefrom.Hour >= 6 && ((datetimefrom.Hour <= 21) | (datetimefrom.Hour == 22 && datetimefrom.Minute == 0)))
                {
                    if (datetimeto.Hour >= 6 && ((datetimeto.Hour <= 21) | (datetimeto.Hour == 22 && datetimeto.Minute == 0)))
                    {
                        check = true;
                        if (art != "Dienst")
                        {
                            if (!commands.setWorkingtime(uid, "Tagdienst", datetimefrom, datetimeto, art + " - " + comment))
                            {
                                noprobs = false;
                            }
                        }
                        else
                        {
                            if (!commands.setWorkingtime(uid, "Tagdienst", datetimefrom, datetimeto, comment))
                            {
                                noprobs = false;
                            }
                        }
                    }
                }

                //gemischt


                if (check == false)
                {
                    //Dienst von 0 - ?
                    if (datetimefrom.Hour >= 0 && ((datetimefrom.Hour <= 5) | (datetimefrom.Hour == 6 && datetimefrom.Minute == 0)))
                    {
                        if ((datetimeto.Hour >= 6) | (tmp.TotalDays == 1) | (datetimeto.Hour == 0 && datetimeto.Minute == 0))
                        {
                            if (!(datetimefrom.Hour == 6 && datetimefrom.Minute == 0))
                            {
                                if (art != "Dienst")
                                {
                                    if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), art + " - " + comment))
                                    {
                                        noprobs = false;
                                    }
                                }
                                else
                                {
                                    if (!commands.setWorkingtime(uid, "Nachtdienst", datetimefrom, new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), comment))
                                    {
                                        noprobs = false;
                                    }
                                }
                            }
                        }
                        if ((datetimeto.Hour >= 22) | (tmp.TotalDays == 1) | (datetimeto.Hour == 0 && datetimeto.Minute == 0))
                        {
                            if (art != "Dienst")
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), art + " - " + comment))
                                {
                                    noprobs = false;
                                }
                            }
                            else
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), comment))
                                {
                                    noprobs = false;
                                }
                            }
                            if (!(datetimeto.Hour == 22 && datetimeto.Minute == 0))
                            {
                                if (art != "Dienst")
                                {
                                    if (!commands.setWorkingtime(uid, "Nachtdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), datetimeto, art + " - " + comment))
                                    {
                                        noprobs = false;
                                    }
                                }
                                else
                                {
                                    if (!commands.setWorkingtime(uid, "Nachtdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), datetimeto, comment))
                                    {
                                        noprobs = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (art != "Dienst")
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), datetimeto, art + " - " + comment))
                                {
                                    noprobs = false;
                                }
                            }
                            else
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), datetimeto, comment))
                                {
                                    noprobs = false;
                                }
                            }
                        }
                    }
                    //Dienst von 6 - ?
                    else if (datetimefrom.Hour >= 6 && ((datetimefrom.Hour <= 21) | (datetimefrom.Hour == 0 && datetimefrom.Minute == 0)))
                    {
                        if ((datetimeto.Hour >= 22) | (datetimeto.Hour == 0))
                        {
                            if (art != "Dienst")
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", datetimefrom, new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), art + " - " + comment))
                                {
                                    noprobs = false;
                                }
                            }
                            else
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", datetimefrom, new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), comment))
                                {
                                    noprobs = false;
                                }
                            }
                            if (art != "Dienst")
                            {
                                if (!commands.setWorkingtime(uid, "Nachtdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), datetimeto, art + " - " + comment))
                                {
                                    noprobs = false;
                                }
                            }
                            else
                            {
                                if (!commands.setWorkingtime(uid, "Nachtdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 22, 0, 0), datetimeto, comment))
                                {
                                    noprobs = false;
                                }
                            }
                        }
                        else
                        {
                            if (art != "Dienst")
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), datetimeto, art + " - " + comment))
                                {
                                    noprobs = false;
                                }
                            }
                            else
                            {
                                if (!commands.setWorkingtime(uid, "Tagdienst", new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 6, 0, 0), datetimeto, comment))
                                {
                                    noprobs = false;
                                }
                            }
                        }
                    }
                }
            }
            //mehrere Tage
            else
            {
                DateTime firstday = new DateTime(datetimefrom.Year, datetimefrom.Month, datetimefrom.Day, 0, 0, 0) + new TimeSpan(24, 0, 0);
                DateTime secondday = new DateTime();
                noprobs = setwt(uid, art, datetimefrom, firstday, comment);

                while (true)
                {
                    tmp = datetimeto - firstday;
                    if ((tmp.TotalDays <= 1) && ((firstday.Year == datetimeto.Year) && (firstday.Month == datetimeto.Month) && (firstday.Day == datetimeto.Day) | (datetimeto.Hour == 0 && datetimeto.Minute == 0)))
                    {
                        noprobs = setwt(uid, art, firstday, datetimeto, comment);
                        break;
                    }
                    else
                    {
                        secondday = firstday;
                        firstday = new DateTime(firstday.Year, firstday.Month, firstday.Day, 0, 0, 0) + new TimeSpan(24, 0, 0);
                        noprobs = setwt(uid, art, secondday, firstday, comment);
                    }
                }
            }

            return noprobs;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
            dpFrom.SelectedDate = new DateTime(this.year, this.month, 1);
            dpTo.SelectedDate = new DateTime(this.year, this.month, 1);

            cbxType.SelectedIndex = 0;
        }

        private void dpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (check)
                {
                    if (!(this.month.ToString() == dpFrom.SelectedDate.Value.Month.ToString()))
                    {
                        MessageBox.Show("Nicht im Ausgewählten Monat.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        dpFrom.SelectedDate = new DateTime(year, month, 1);
                    }
                    if (!(this.year.ToString() == dpFrom.SelectedDate.Value.Year.ToString()))
                    {
                        MessageBox.Show("Nicht im Ausgewählten Jahr.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        dpFrom.SelectedDate = new DateTime(year, month, 1);
                    }
                }

                TimeSpan tmp = dpTo.SelectedDate.Value - dpFrom.SelectedDate.Value;
                if (tmp.Days < 0)
                {
                    dpTo.SelectedDate = dpFrom.SelectedDate;
                }
            }
            catch 
            {
                /**/
                    /**/
            }
        }

        private void dpTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            try
            {
                TimeSpan temp = dpTo.SelectedDate.Value - dpFrom.SelectedDate.Value;
                if (temp.Days < 0)
                {
                    MessageBox.Show("Von-Datum muss kleiner als Bis-Datum sein!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    dpTo.SelectedDate = dpFrom.SelectedDate;
                }
            }
            catch 
            {
                /**/
                    /**/
            }
        }
    }
}
