using System.Windows;
using System.Windows.Media;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für EditArt.xaml
    /// </summary>
    public partial class EditArt : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditArt(SQLCommands sql)
        {
            c = sql;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtArt.Background = color1;
            if (!string.IsNullOrWhiteSpace(txtArt.Text))
            {
                c.setnewMedicalActionType(txtArt.Text);
                Close();
            }
            else
            {
                txtArt.Background = color2;
            }
        }
    }
}