using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Funny3.xaml
    /// </summary>
    public partial class Funny3 : Window
    {
        private readonly Random a = new Random();
        private int counter;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private bool first = true;

        public Funny3()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
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
                var tmp = "";
                for (var i = 0; i < 25; i++)
                {
                    tmp += a.Next(0, 2).ToString();
                }
                label1.Content += "\n" + tmp;
            }
            counter++;

            if (counter == 15)
            {
                Close();
            }
        }
    }
}