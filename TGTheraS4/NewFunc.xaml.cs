using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaktionslogik für NewFunc.xaml
    /// </summary>
    public partial class NewFunc : Window
    {
        public bool finished = false;
        public string txt = "";
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

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
            if (String.IsNullOrEmpty(txt.Trim()))
            {
                textBox.Background = color2;
                MessageBox.Show("Sie müssen einen Namen eingeben!");
            }
            else
            {
                finished = true;
                this.Close();
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            finished = false;
            textBox.Background = color1;
            this.Close();
        }
    }
}
