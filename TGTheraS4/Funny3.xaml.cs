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
using System.Windows.Threading;

namespace TheraS5
{
    /// <summary>
    /// Interaktionslogik für Funny3.xaml
    /// </summary>
    public partial class Funny3 : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Random a = new Random();
        bool first = true;
        int counter = 0;

        public Funny3()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            label1.Foreground = Brushes.White;
            label1.Content = @"C:\World\system32>";
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (first)
            {
                label1.Content += "H4CKTHEW0RLD.exe";
                first = false;
            }
            else
            {
                string tmp = "";
                for (int i = 0; i < 25; i++)
                {
                    tmp += (a.Next(0, 2)).ToString();
                }
                label1.Content += "\n" + tmp;
            }
            counter++;

            if (counter == 15)
            {
                this.Close();
            }
        }
    }
}
