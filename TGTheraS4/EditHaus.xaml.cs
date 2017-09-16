using System.Windows;
using System.Windows.Media;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für EditHaus.xaml
    /// </summary>
    public partial class EditHaus : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        private readonly string id;

        public EditHaus(string id, SQLCommands sql)
        {
            this.id = id;
            c = sql;
            InitializeComponent();
        }

        private void btnSaveHouse_Click(object sender, RoutedEventArgs e)
        {
            txtHouseName.Background = color1;
            txtHouseZIP.Background = color1;
            txtHouseCity.Background = color1;
            dpHouseStart.Background = color1;
            txtHouseStreet.Background = color1;

            if (txtHouseName.Text != "")
            {
                if (txtHouseStreet.Text != "")
                {
                    if (txtHouseZIP.Text != "")
                    {
                        if (txtHouseCity.Text != "")
                        {
                            if (dpHouseStart.Text != "")
                            {
                                c.setHouse(id, txtHouseName.Text, txtHouseStreet.Text, txtHouseZIP.Text,
                                    txtHouseCity.Text, txtHouseTel.Text, txtHouseEMail.Text, txtHouseHomepage.Text,
                                    dpHouseStart.Text);
                                Close();
                            }
                            else
                            {
                                dpHouseStart.Background = color2;
                                MessageBox.Show("Bitte tragen Sie ein Start-Datum ein!", "Fehler!", MessageBoxButton.OK,
                                    MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            txtHouseCity.Background = color2;
                            MessageBox.Show("Bitte tragen Sie eine Stadt ein!", "Fehler!", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        txtHouseZIP.Background = color2;
                        MessageBox.Show("Bitte tragen Sie eine PLZ ein!", "Fehler!", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    txtHouseStreet.Background = color2;
                    MessageBox.Show("Bitte tragen Sie eine Straße ein!", "Fehler!", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                txtHouseName.Background = color2;
                MessageBox.Show("Bitte tragen Sie einen Namen ein!", "Fehler!", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
    }
}