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
    /// Interaktionslogik für PDFDatePicker.xaml
    /// </summary>
    public partial class PDFDatePicker : Window
    {
        public DateTime datum = new DateTime(1900, 1, 1);
        public bool abb = false;
        int clicked = 0;

        public PDFDatePicker()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DateTime tmp = new DateTime(Convert.ToInt32(comboBox2.Text), Convert.ToInt32(comboBox1.Text), 1);
            datum = tmp;
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            clicked++;
            if (clicked == 3)
                button2.Opacity = 1;
            if (clicked == 4)
            {
                Funny a = new Funny();
                a.ShowDialog();
            }
            if (clicked == 5)
                button2.Opacity = 0;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            abb = true;
            this.Close();
        }

        
    }
}


