using System.Windows;
using System.Windows.Controls;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für MonthSelector.xaml
    /// </summary>
    public partial class MonthSelector : Window
    {
        public bool closed, canceled, ok;

        public MonthSelector()
        {
            InitializeComponent();
        }

        private void bttnMSOK_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            ok = true;
            Close();
        }

        private void bttnMSCan_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            canceled = true;
            Close();
        }

        private void calMS_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            calMS.DisplayMode = CalendarMode.Year;
        }
    }
}