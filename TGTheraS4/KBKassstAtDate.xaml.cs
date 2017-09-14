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
    /// Interaktionslogik für KBKassstAtDate.xaml
    /// </summary>
    public partial class KBKassstAtDate : Window
    {
        IntranetTG.SQLCommands c;
        string hid;

        public KBKassstAtDate(IntranetTG.SQLCommands c, string hid)
        {
            this.c = c;
            this.hid = hid;
            InitializeComponent();
        }

        private void calMS_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            calMS.DisplayMode = CalendarMode.Year;
        }

        private void bttnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void calMS_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            lblKassst.Content = "Kassastand am 1." + calMS.DisplayDate.Month.ToString() + "." + calMS.DisplayDate.Year.ToString() + ": " + c.getKassstAtDate(hid, (new DateTime(calMS.DisplayDate.Year, calMS.DisplayDate.Month, 1)).ToString("yyyy-MM-dd").ToString());
        }
    }
}