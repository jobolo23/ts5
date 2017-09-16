using System.Windows;
using System.Windows.Media;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für NewFunc.xaml
    /// </summary>
    public partial class NewFunc : Window
    {
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public bool finished;
        public string txt = "";

        public NewFunc()
        {
            InitializeComponent();
        }

        public NewFunc(bool v)
        {
            InitializeComponent();
            if (v)
            {
                label.Content = "Geben Sie den Namen der Funktion ein:";
            }
            else
            {
                label.Content = "Geben Sie einen neuen Namen für diese Funktion ein:";
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            textBox.Background = color1;
            txt = textBox.Text;
            if (string.IsNullOrEmpty(txt.Trim()))
            {
                textBox.Background = color2;
                MessageBox.Show("Sie müssen einen Namen eingeben!");
            }
            else
            {
                finished = true;
                Close();
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            textBox.Background = color1;
            Close();
        }
    }
}