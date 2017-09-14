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

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für RateWiki.xaml
    /// </summary>
    public partial class RateWiki : Window
    {
        public int rate = -1;
        public RateWiki()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rate = Convert.ToInt32(slChoose.Value);
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            rate = -1;
            this.Close();
        }
    }
}
