using System.Linq;
using System.Windows;
using System.Windows.Media;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Title.xaml
    /// </summary>
    public partial class Title : Window
    {
        public SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public string titel = "-1";

        public Title(SQLCommands sql)
        {
            c = sql;
            InitializeComponent();
        }

        private bool checktxt(string text)
        {
            if (text.Contains('\''))
            {
                return false;
            }
            if (text.Contains('\"'))
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Background = color1;
            if (textBox1.Text.Trim() != "")
            {
                if (checktxt(textBox1.Text.Trim()))
                {
                    if (c.checkWikiName(textBox1.Text.Trim()))
                    {
                        titel = textBox1.Text.Trim();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Verbotenes Zeichen verwenden! ( \' oder \" )", "SQL Injection",
                        MessageBoxButton.OK, MessageBoxImage.Stop);
                    textBox1.Background = color2;
                }
            }
            else
            {
                MessageBox.Show("Geben Sie einen Namen ein!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Stop);
                textBox1.Background = color2;
            }
        }
    }
}