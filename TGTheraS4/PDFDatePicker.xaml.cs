using System;
using System.Windows;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für PDFDatePicker.xaml
    /// </summary>
    public partial class PDFDatePicker : Window
    {
        public bool abb;
        private int clicked;
        public DateTime datum = new DateTime(1900, 1, 1);

        public PDFDatePicker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var tmp = new DateTime(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox1.Text), 1);
            datum = tmp;
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            clicked++;
            if (clicked == 3)
            {
                button2.Opacity = 1;
            }
            if (clicked == 4)
            {
                var a = new Funny();
                a.ShowDialog();
            }
            if (clicked == 5)
            {
                button2.Opacity = 0;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            abb = true;
            Close();
        }
    }
}