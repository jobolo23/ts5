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

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für Klient_Berichte_Art_C.xaml
    /// </summary>
    public partial class Klient_Berichte_Art_C : Window
    {
        public bool set = false;
        public int art = 1;


        public Klient_Berichte_Art_C(int a)
        {
            InitializeComponent();

            art = a;

            cmbArt.Items.Add("Telefonat");
            cmbArt.Items.Add("Vorfallsprotokoll");
            cmbArt.Items.Add("Gesprächsprotokoll");
            cmbArt.Items.Add("Fallverlaufsgespräch");
            cmbArt.Items.Add("Jahresbericht");
            cmbArt.Items.Add("Zwischenbericht");
            cmbArt.SelectedIndex = art;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            art = cmbArt.SelectedIndex;
            set = true;
            this.Close();
        }

        private void btnAb_Click(object sender, RoutedEventArgs e)
        {
            set = false;
            this.Close();
        }
    }
}
