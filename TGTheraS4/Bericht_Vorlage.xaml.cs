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
using TGTheraS4.Objects;

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für Bericht_Vorlage.xaml
    /// </summary>
    public partial class Bericht_Vorlage : Window
    {
        public bool set = false;
        public int vorlage = -1;
        public string name = "";
        public string content = "";
        SQLCommands c = new SQLCommands();
        List<Klienten_Berichte> Liste = new List<Klienten_Berichte>();
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public Bericht_Vorlage()
        {
            InitializeComponent();

            Liste = c.getBericht_Vorlage();
            foreach (Klienten_Berichte tmp in Liste)
            {
                cmbvorlage.Items.Add(tmp.name);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            cmbvorlage.Background = color1;
            txtspeichern.Background = color1;
            
            if (cmbvorlage.Text != "" || txtspeichern.Text != "")
            {
                if (cmbvorlage.Text != "" && txtspeichern.Text != "")
                {
                    MessageBox.Show("Es kann nur gespeichert ODER geladen werden!");
                    txtspeichern.Background = color2;
                    cmbvorlage.Background = color2;
                }
                else
                {
                    set = true;
                    if (cmbvorlage.Text != "")
                    {
                        vorlage = Liste.ElementAt(cmbvorlage.SelectedIndex).id;
                        content = Liste.ElementAt(cmbvorlage.SelectedIndex).content;
                        name = "";
                    }
                    else
                    {
                        vorlage = -1;
                        name = txtspeichern.Text;
                    }
                    this.Close();
                }
            }
            
        }

        private void btnAb_Click(object sender, RoutedEventArgs e)
        {
            set = false;
            this.Close();
        }
    }
}
