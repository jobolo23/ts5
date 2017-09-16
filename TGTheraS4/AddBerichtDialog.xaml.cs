using System.Windows;
using System.Windows.Media;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für AddBerichtDialog.xaml
    /// </summary>
    public partial class AddBerichtDialog : Window
    {
        public int art = -1;

        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public string Name;
        public bool set;


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
                Close();
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
            Close();
        }
    }
}