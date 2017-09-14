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
    /// Interaktionslogik für EditHaus.xaml
    /// </summary>
    public partial class EditHaus : Window
    {
        String id;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditHaus(String id)
        {
            this.id = id;
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
                                SQLCommands c = new SQLCommands();
                                c.setHouse(id, txtHouseName.Text, txtHouseStreet.Text, txtHouseZIP.Text, txtHouseCity.Text, txtHouseTel.Text, txtHouseEMail.Text, txtHouseHomepage.Text, dpHouseStart.Text);
                                this.Close();
                            }
                            else
                            {
                                dpHouseStart.Background = color2;
                                MessageBox.Show("Bitte tragen Sie ein Start-Datum ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            txtHouseCity.Background = color2;
                            MessageBox.Show("Bitte tragen Sie eine Stadt ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        txtHouseZIP.Background = color2;
                        MessageBox.Show("Bitte tragen Sie eine PLZ ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    txtHouseStreet.Background = color2;
                    MessageBox.Show("Bitte tragen Sie eine Straße ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                txtHouseName.Background = color2;
                MessageBox.Show("Bitte tragen Sie einen Namen ein!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
