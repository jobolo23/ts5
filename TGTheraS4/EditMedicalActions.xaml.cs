using System;
using System.Windows;
using System.Windows.Media;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für EditMedicalActions.xaml
    /// </summary>
    public partial class EditMedicalActions : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public bool del;
        private readonly string id;

        public EditMedicalActions(string id, string[] services, string client, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            this.id = id;
            cmbArt.ItemsSource = c.geileMethode();
            cmbArt.SelectedValuePath = "aid";
            cmbArt.DisplayMemberPath = "name";
            lbllala.Visibility = Visibility.Hidden;
            lblArt.Visibility = Visibility.Hidden;
            foreach (var service in services)
            {
                foreach (var cl in c.WgToClients(service, SQLCommands.ClientFilter.NotLeft))
                {
                    cmbgetKlient.Items.Add(cl.Key + " " + cl.Value);
                }

                cmbgetKlient.IsEnabled = false;
                cmbgetKlient.SelectedValue = client;
            }
        }

        public EditMedicalActions(string client, string date, string art, string desc, bool showdel, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
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
            if (MessageBox.Show("Sind Sie sicher, dass Sie abbrechen wollen?", "Frage", MessageBoxButton.YesNo,
                    MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                Close();
            }
        }

        private void btnNewArt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ea = new EditArt(c);
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
                    var hoergeraetstrahlenangriff12345 = c.getIdbyNameClients(cmbgetKlient.SelectedValue.ToString());
                    var date = dpBemerkt.SelectedDate.Value.ToString("yyyy-MM-dd");
                    c.WasSollDieseMethodeKoennen(id, hoergeraetstrahlenangriff12345, date, txtdesc.Text,
                        cmbArt.SelectedValue.ToString());
                    Close();
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
                MessageBox.Show("Bitte füllen Sie alle Felder aus!", "Fehler!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            del = true;
            Close();
        }
    }
}