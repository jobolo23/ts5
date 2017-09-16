using System;
using System.Windows;
using System.Windows.Threading;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Funny2.xaml
    /// </summary>
    public partial class Funny2 : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public int selected;
        private int value;

        public Funny2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            progressBar1.Maximum = 10;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (value == 10)
            {
                dispatcherTimer.Stop();
                choose();
                Close();
            }
            progressBar1.Value = value;
            label3.Content = 10 - value;
            button1.Content = "OK ( " + (10 - value) + " )";
            value++;
        }

        private void choose()
        {
            if (radioButton1.IsChecked == true)
            {
                selected = 1;
            }
            else if (radioButton2.IsChecked == true)
            {
                selected = 2;
            }
            else if (radioButton3.IsChecked == true)
            {
                selected = 3;
            }
            else if (radioButton4.IsChecked == true)
            {
                selected = 4;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            choose();
            Close();
        }
    }
}