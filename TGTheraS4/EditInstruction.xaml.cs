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
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class EditInstruction : Window
    {
        private SQLCommands c;
        private String uid;
        private String name;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditInstruction(String name, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            btnGelesen.Visibility = Visibility.Hidden;
            btnGelesenvon.Visibility = Visibility.Hidden;
            lblAuthoren.Visibility = Visibility.Hidden;
            this.name = name;
        }

        public EditInstruction(bool isAdmin,String uid, String date, String title, String desc , String name, SQLCommands sql)
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

            String title = txtTitel.Text;
            String desc = txtBeschreibung.Text;
            if (title.Length == 0 || desc.Length == 0)
            {
                MessageBox.Show("Die Textfelder dürfen nicht leer sein", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("Das ausgewählte Datum darf nicht kleiner sein als das Aktuelle", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                dateInstructionStart.Background = color2;
            }
            else
            {
                String date = dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd");
                String id = c.getIdbyName(name);
                c.setInstruction(date, title, desc,id);
                this.Close();
            }

        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGelesen_Click(object sender, RoutedEventArgs e)
        {
            String date = dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            c.setInstructionRead(uid, txtTitel.Text , dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd"), txtBeschreibung.Text);
            this.Close();
        }

        private void btnGelesenvon_Click(object sender, RoutedEventArgs e)
        {
            ShowReadInstruction sri = new ShowReadInstruction(dateInstructionStart.SelectedDate.Value.ToString("yyyy-MM-dd"), txtTitel.Text, txtBeschreibung.Text, c);
            sri.Show();
        }

    }
}
