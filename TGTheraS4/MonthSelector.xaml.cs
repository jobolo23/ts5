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
    /// Interaktionslogik für MonthSelector.xaml
    /// </summary>
    public partial class MonthSelector : Window
    {
        public bool closed = false, canceled = false, ok = false;
        public MonthSelector()
        {
            InitializeComponent();
        }

        private void bttnMSOK_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            ok = true;
            this.Close();
        }

        private void bttnMSCan_Click(object sender, RoutedEventArgs e)
        {
            closed = true;
            canceled = true;
            this.Close();
        }

        private void calMS_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            calMS.DisplayMode = CalendarMode.Year;
        }
    }
}
