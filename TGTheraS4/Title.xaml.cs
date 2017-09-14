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
    /// Interaktionslogik für Title.xaml
    /// </summary>
    public partial class Title : Window
    {
        public string titel = "-1";
        public SQLCommands c = new SQLCommands();
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public Title()
        {
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
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Verbotenes Zeichen verwenden! ( \' oder \" )" , "SQL Injection", MessageBoxButton.OK, MessageBoxImage.Stop);
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
