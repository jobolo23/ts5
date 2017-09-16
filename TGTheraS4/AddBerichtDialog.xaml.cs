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

namespace TheraS5
{
    /// <summary>
    /// Interaktionslogik für AddBerichtDialog.xaml
    /// </summary>
    public partial class AddBerichtDialog : Window
    {
        public int art = -1;
        public string Name = null;
        public bool set = false;

        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));



        public AddBerichtDialog()
        {
            InitializeComponent();

            cmbArt.Items.Add("Telefonat");
            cmbArt.Items.Add("Vorfallsprotokoll");
            cmbArt.Items.Add("Gesprächsprotokoll");
            cmbArt.Items.Add("Fallverlaufsgespräch");
            cmbArt.Items.Add("Jahresbericht");
            cmbArt.Items.Add("Zwischenbericht");
            cmbArt.SelectedIndex = 3;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            txtName.Background = color1;
            if (txtName.Text != "")
            {
                Name = txtName.Text;
                art = cmbArt.SelectedIndex;
                set = true;
                this.Close();
            }
            else
            {
                txtName.Background = color2;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            art = -1;
            Name = null;
            set = false;
            this.Close();
        }
    }
}
