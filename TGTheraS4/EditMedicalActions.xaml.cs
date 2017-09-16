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

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für EditMedicalActions.xaml
    /// </summary>
    public partial class EditMedicalActions : Window
    {
        SQLCommands c;
        String id;
        public bool del = false;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditMedicalActions(String id,String[] services, String client, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            this.id = id;
            cmbArt.ItemsSource = c.geileMethode();
            cmbArt.SelectedValuePath = "aid";
            cmbArt.DisplayMemberPath = "name";
            lbllala.Visibility = Visibility.Hidden;
            lblArt.Visibility = Visibility.Hidden;
            foreach (string service in services)
            {
                string[] temp = c.WgToClients(service).Split('%');
                foreach (string name in temp)
                {
                    string[] namen = name.Split('$');
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
                        cmbgetKlient.Items.Add(namen[0] + " " + namen[1]);
                    }
                }

                cmbgetKlient.IsEnabled = false;
                cmbgetKlient.SelectedValue = client;
            }


        }

        public EditMedicalActions(String client, String date, String art, String desc, bool showdel)
        {
            InitializeComponent();
            cmbArt.ItemsSource = c.geileMethode();
            cmbArt.SelectedValuePath = "aid";
            cmbArt.DisplayMemberPath = "name";
            btnNewArt.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Hidden;
            cmbArt.Visibility = Visibility.Hidden;
            cmbgetKlient.Visibility = Visibility.Hidden;
            dpBemerkt.IsEnabled = false;
            txtdesc.IsEnabled = false;
            if (showdel)
            {
                btnDel.Visibility = Visibility.Visible;
            }

            lbllala.Content = client;
            lblArt.Content = art;
            dpBemerkt.SelectedDate = DateTime.Parse(date).Date;
            txtdesc.Text = desc;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Sind Sie sicher, dass Sie abbrechen wollen?", "Frage", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                this.Close();
        }

        private void btnNewArt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditArt ea = new EditArt(c);
                ea.ShowDialog();
                cmbArt.ItemsSource = c.geileMethode();
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e) //Dieser huans rudi
        {
            dpBemerkt.Background = color1;
            cmbArt.Background = color1;
            cmbgetKlient.Background = color1;

            if (dpBemerkt.SelectedDate.Value.ToString() != "" && cmbArt.SelectedIndex != -1)
            {
                try
                {
                    String hoergeraetstrahlenangriff12345 = c.getIdbyNameClients(cmbgetKlient.SelectedValue.ToString());
                    String date = dpBemerkt.SelectedDate.Value.ToString("yyyy-MM-dd");
                    c.WasSollDieseMethodeKoennen(id, hoergeraetstrahlenangriff12345, date, txtdesc.Text, cmbArt.SelectedValue.ToString());
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    dpBemerkt.Background = color2;
                    cmbArt.Background = color2;
                    cmbgetKlient.Background = color2;
                }
            }
            else
            {
                dpBemerkt.Background = color2;
                cmbArt.Background = color2;
                MessageBox.Show("Bitte füllen Sie alle Felder aus!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            del = true;
            this.Close();
        }
    }
}
