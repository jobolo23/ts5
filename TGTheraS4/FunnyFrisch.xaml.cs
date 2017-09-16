using System;
using System.Windows;
using System.Windows.Threading;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für FunnyFrisch.xaml
    /// </summary>
    public partial class FunnyFrisch : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public FunnyFrisch()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
        }
    }
}