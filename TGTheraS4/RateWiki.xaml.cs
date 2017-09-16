using System;
using System.Windows;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für RateWiki.xaml
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
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            rate = -1;
            Close();
        }
    }
}