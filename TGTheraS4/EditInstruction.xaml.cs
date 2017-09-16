using System;
using System.Windows;
using System.Windows.Media;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class EditInstruction : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        private readonly string name;
        private readonly string uid;

        public EditInstruction(string name, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            btnGelesen.Visibility = Visibility.Hidden;
            btnGelesenvon.Visibility = Visibility.Hidden;
            lblAuthoren.Visibility = Visibility.Hidden;
            this.name = name;
        }

        public EditInstruction(bool isAdmin, string uid, string date, string title, string desc, string name,
            SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            btnSpeichern.Visibility = Visibility.Hidden;
            if (!isAdmin)
            {
                btnGelesenvon.Visibility = Visibility.Hidden;
            }
            dateInstructionStart.SelectedDate = DateTime.Parse(date).Date;
            txtTitel.Text = title;
            txtBeschreibung.Text = desc;
            dateInstructionStart.IsEnabled = false;
            txtTitel.IsEnabled = false;
            txtBeschreibung.IsEnabled = false;
            this.uid = uid;
            this.name = name;
            lblAuthoren.Content = name;
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            txtTitel.Background = color1;
            txtBeschreibung.Background = color1;
            dateInstructionStart.Background = color1;

            var title = txtTitel.Text;
            var desc = txtBeschreibung.Text;
            if (title.Length == 0 || desc.Length == 0)
            {
                MessageBox.Show("Die Textfelder dürfen nicht leer sein", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtTitel.Background = color2;
                txtBeschreibung.Background = color2;
            }
            else if (dateInstructionStart.SelectedDate == null)
            {
                MessageBox.Show("Kein Datum ausgewählt", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                dateInstructionStart.Background = color2;
            }

            else if (dateInstructionStart.SelectedDate < DateTime.Now.Date)
            {
                MessageBox.Show("Das ausgewählte Datum darf nicht kleiner sein als das Aktuelle", "Achtung",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                dateInstructionStart.Background = color2;
            }
            else
            {
                var date = dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd");
                var id = c.getIdbyName(name);
                c.setInstruction(date, title, desc, id);
                Close();
            }
        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnGelesen_Click(object sender, RoutedEventArgs e)
        {
            var date = dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            c.setInstructionRead(uid, txtTitel.Text, dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd"),
                txtBeschreibung.Text);
            Close();
        }

        private void btnGelesenvon_Click(object sender, RoutedEventArgs e)
        {
            var sri = new ShowReadInstruction(dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd"),
                txtTitel.Text, txtBeschreibung.Text, c);
            sri.Show();
        }
    }
}