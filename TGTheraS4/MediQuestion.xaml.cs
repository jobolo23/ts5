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
    /// Interaktionslogik für MediQuestion.xaml
    /// </summary>
    public partial class MediQuestion : Window
    {
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public MediQuestion(string medi)
        {
            InitializeComponent();
            lblMedi.Content = medi;
        }

        private string reason = "";

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Background = color1;
            if (textBox1.Text != "")
            {
                reason = textBox1.Text;
                DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Sie müssen einen Grund angeben!");
                textBox1.Background = color2;
            }
        }
        public string getIt()
        {
            //this.ShowDialog();
            return reason;
        }

        private void chk_no_permission_Checked(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = false;
        }

        private void btn_cancel_medi_Click(object sender, RoutedEventArgs e)
        {
            reason = textBox1.Text;
            DialogResult = false;
            this.Close();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            reason = textBox1.Text;
            DialogResult = false;
            //this.Close();
        }
    }
}
