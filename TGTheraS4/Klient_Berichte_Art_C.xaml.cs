using System.Windows;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Klient_Berichte_Art_C.xaml
    /// </summary>
    public partial class Klient_Berichte_Art_C : Window
    {
        public int art = 1;
        public bool set;


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
            Close();
        }

        private void btnAb_Click(object sender, RoutedEventArgs e)
        {
            set = false;
            Close();
        }
    }
}