using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using IntranetTG;
using TheraS5.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Bericht_Vorlage.xaml
    /// </summary>
    public partial class Bericht_Vorlage : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public string content = "";
        private readonly List<Klienten_Berichte> Liste = new List<Klienten_Berichte>();
        public string name = "";
        public bool set;
        public int vorlage = -1;

        public Bericht_Vorlage(SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            Liste = c.getBericht_Vorlage();
            foreach (var tmp in Liste)
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
                    Close();
                }
            }
        }

        private void btnAb_Click(object sender, RoutedEventArgs e)
        {
            set = false;
            Close();
        }
    }
}