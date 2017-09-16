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
using IntranetTG.Functions;

namespace TheraS5
{

    /// <summary>
    /// Interaktionslogik für EditTask.xaml
    /// </summary>
    public partial class EditTask : Window
    {
        // status 0 user 2 hat die aufgabe bekommen
        // status 1 user 1 hat die aufgabe zu überprüfen
        // status 2 aufgabe ist fertig
        private SQLCommands c;
        private String uid2;
        private String uid1;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        private bool changedate = false;

        public EditTask(String uid1, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            cmbTaskforUser.ItemsSource = Functions.EmployeeList;
            cmbTaskforUser.DisplayMemberPath = "FullName";
            cmbTaskforUser.SelectedValuePath = "Id";
            btnerledigt.Visibility = Visibility.Hidden;
            btnerneut.Visibility = Visibility.Hidden;
            btnfertig.Visibility = Visibility.Hidden;
            lblan.Visibility = Visibility.Hidden;
            this.uid1 = uid1;
        }

        //mode true auftraggeber
        //mode fase auftragnehmer
        public EditTask(bool mode, bool erledigt, String uid1, String uid2, String stringstartdate, String stringenddate, String desc, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            this.uid1 = uid1;
            this.uid2 = uid2;
            lblan.Content = c.getNameByID(uid2, false);

            btnSave.Visibility = Visibility.Hidden;
            cmbTaskforUser.Visibility = Visibility.Hidden;

            startdate.IsEnabled = false;
            enddate.IsEnabled = false;
            txtdesc.IsEnabled = false;

            startdate.SelectedDate = DateTime.Parse(stringstartdate).Date;
            enddate.SelectedDate = DateTime.Parse(stringenddate).Date;
            txtdesc.Text = desc;

            if (mode)
            {
                btnerledigt.Visibility = Visibility.Hidden;
                if (!erledigt)
                {
                    btnfertig.Content = "Verwerfen";
                    btnerneut.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                btnfertig.Visibility = Visibility.Hidden;
                btnerneut.Visibility = Visibility.Hidden;
            }
        }

        private void cmbTaskforUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            uid2 = cmbTaskforUser.SelectedValue.ToString();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtdesc.Background = color1;
            startdate.Background = color1;
            enddate.Background = color1;


            String desc = txtdesc.Text;
            if (desc.Length == 0)
            {
                MessageBox.Show("Das Textfeld darf nicht leer sein", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtdesc.Background = color2;
            }
            else if (startdate.SelectedDate == null || enddate.SelectedDate == null)
            {
                MessageBox.Show("Kein Start oder End Datum angegeben", "Achtung",MessageBoxButton.OK,MessageBoxImage.Warning);
                startdate.Background = color2;
                enddate.Background = color2;
            }
            else if (startdate.SelectedDate > enddate.SelectedDate)
            {
                MessageBox.Show("Das anfangs Datum darf nicht kleiner sein als das end Datum ", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                startdate.Background = color2;
            }
            else if (startdate.SelectedDate < DateTime.Now.Date)
            {
                MessageBox.Show("Das anfangs Datum muss größer oder gleich dem heutigen Datum sein", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                startdate.Background = color2;
            }
            else
            {
                String stringstartdate = startdate.SelectedDate.Value.ToString("yyyy-MM-dd");
                String stringenddate = enddate.SelectedDate.Value.ToString("yyyy-MM-dd");
                c.setTasks(uid1, uid2, stringstartdate, stringenddate, desc);
                this.Close();
            }
        }

        private void btnerledigt_Click(object sender, RoutedEventArgs e)
        {
            c.changeModefrom0to1(uid1, uid2, startdate.SelectedDate.Value.ToString("yyyy-MM-dd"), enddate.SelectedDate.Value.ToString("yyyy-MM-dd"), txtdesc.Text);
            this.Close();
        }

        private void btnerneut_Click(object sender, RoutedEventArgs e)
        {
            if (changedate)
            {
                c.changeEnddateTasks(uid1, uid2, startdate.SelectedDate.Value.ToString("yyyy-MM-dd"), enddate.SelectedDate.Value.ToString("yyyy-MM-dd"), txtdesc.Text);
                c.changeModefrom1to0(uid1, uid2, startdate.SelectedDate.Value.ToString("yyyy-MM-dd"), enddate.SelectedDate.Value.ToString("yyyy-MM-dd"), txtdesc.Text);
                changedate = false;
                this.Close();
            }
            else
            {
                enddate.IsEnabled = true;
                changedate = true;
            }
        }

        private void btnfertig_Click(object sender, RoutedEventArgs e)
        {
            c.changeModefrom1to2(uid1, uid2, startdate.SelectedDate.Value.ToString("yyyy-MM-dd"), enddate.SelectedDate.Value.ToString("yyyy-MM-dd"), txtdesc.Text);
            this.Close();
        }
    }
}
