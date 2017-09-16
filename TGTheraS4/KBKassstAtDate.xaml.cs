using System;
using System.Windows;
using System.Windows.Controls;
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für KBKassstAtDate.xaml
    /// </summary>
    public partial class KBKassstAtDate : Window
    {
        private readonly SQLCommands c;
        private readonly string hid;

        public KBKassstAtDate(SQLCommands c, string hid)
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
            Close();
        }

        private void calMS_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            lblKassst.Content = "Kassastand am 1." + calMS.DisplayDate.Month + "." + calMS.DisplayDate.Year + ": " +
                                c.getKassstAtDate(hid,
                                    new DateTime(calMS.DisplayDate.Year, calMS.DisplayDate.Month, 1)
                                        .ToString("yyyy-MM-dd"));
        }
    }
}