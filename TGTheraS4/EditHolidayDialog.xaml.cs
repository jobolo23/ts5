using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntranetTG;
using IntranetTG.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für EditHolidayDialog.xaml
    /// </summary>
    public partial class EditHolidayDialog : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        private readonly SQLCommands commands;
        private object[] editData;
        private readonly string[] holidays;
        private bool isNew;

        private bool isSaved = true;
        private readonly int month;
        public List<Project> projects = new List<Project>();
        public string projectString = "";
        private User u;
        private readonly int userid;
        private readonly int year;


        public EditHolidayDialog(SQLCommands sql)
        {
            InitializeComponent();
            commands = sql;
            c = sql;
            dpFrom.Text = DateTime.Now.ToShortDateString();
            //tpFrom.Value = DateTime.Now;
            isNew = true;
            month = -1;
            year = -1;
            var holidaysstring = c.getNonWorkingDays();
            if (holidaysstring == null)
            {
                return;
            }
            holidays = holidaysstring.Split('%');
        }

        public EditHolidayDialog(int month, int year, int userid)
        {
            InitializeComponent();
            dpFrom.Text = DateTime.Now.ToShortDateString();
            //tpFrom.Value = DateTime.Now;
            isNew = true;
            this.month = month;
            this.year = year;
            this.userid = userid;
            var holidaysstring = c.getNonWorkingDays();
            if (holidaysstring == null)
            {
                return;
            }
            holidays = holidaysstring.Split('%');
        }

        public EditHolidayDialog(object[] data)
        {
            InitializeComponent();

            editData = data;

            //projectString = commands.getProjects();

            dpFrom.Text = data[1].ToString();
            //tpFrom.Value = Convert.ToDateTime(data[1]);
            dpTo.Text = data[2].ToString();
            //tpTo.Value = Convert.ToDateTime(data[2]);
            txtComment.Text = data[3].ToString();
            cbxType.Text = data[4].ToString();
            var holidaysstring = c.getNonWorkingDays();
            if (holidaysstring == null)
            {
                return;
            }
            holidays = holidaysstring.Split('%');
        }

        public void setuser(User u)
        {
            this.u = u;
        }

        public bool checkIT(string data)
        {
            var trueStory = true;

            var tmp = new string[2];
            tmp = data.Split(':');
            if ((tmp[0].Length > 2) | (tmp[1].Length > 2))
            {
                trueStory = false;
            }
            if ((Convert.ToInt32(tmp[0]) < 0) | (Convert.ToInt32(tmp[0]) > 23))
            {
                trueStory = false;
            }
            if ((Convert.ToInt32(tmp[1]) < 0) | (Convert.ToInt32(tmp[1]) > 59))
            {
                trueStory = false;
            }

            return trueStory;
        }

        public void dobtnSave()
        {
            var dateFrom = new DateTime(dpFrom.SelectedDate.Value.Year, dpFrom.SelectedDate.Value.Month,
                dpFrom.SelectedDate.Value.Day, 0, 0, 0);

            var dateTo = new DateTime(dpTo.SelectedDate.Value.Year, dpTo.SelectedDate.Value.Month,
                dpTo.SelectedDate.Value.Day, 0, 0, 0);
            DateTime sampleday;
            IFormatProvider culture = new CultureInfo("de-DE", true);
            var isholday = false;
            var days = new LinkedList<DateTime>();
            for (var tmp = dateFrom; (dateTo - tmp).TotalDays >= 0; tmp = tmp.Add(new TimeSpan(1, 0, 0, 0, 0)))
            {
                if (u.WorkingTimeType == 1)
                {
                    if (tmp.ToString("ddd") != "Sa" && tmp.ToString("ddd") != "So")
                    {
                        for (var i = 0; i < holidays.Length; i++)
                        {
                            if (holidays[i] != "")
                            {
                                sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                if (sampleday.Day == tmp.Day && sampleday.Month == tmp.Month &&
                                    sampleday.Year == tmp.Year)
                                {
                                    //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                    isholday = true;
                                }
                            }
                        }
                        if (isholday == false)
                        {
                            days.AddLast(tmp);
                        }
                    }
                }
                else if (u.WorkingTimeType == 2)
                {
                    if (tmp.ToString("ddd") != "So")
                    {
                        for (var i = 0; i < holidays.Length; i++)
                        {
                            if (holidays[i] != "")
                            {
                                sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                if (sampleday.Day == tmp.Day && sampleday.Month == tmp.Month &&
                                    sampleday.Year == tmp.Year)
                                {
                                    //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                    isholday = true;
                                }
                            }
                        }
                        if (isholday == false)
                        {
                            days.AddLast(tmp);
                        }
                    }
                }
                isholday = false;
            }
            if (days.Count > 0)
            {
                var a = days.ElementAt(0);
                var von = days.ElementAt(0);
                var send = false;
                for (var i = 0; i < days.Count; i++)
                {
                    if (!((days.ElementAt(i) - a).TotalDays <= 1))
                    {
                        if (!setwt(userid, cbxType.SelectionBoxItem.ToString(), von, a, txtComment.Text))
                        {
                            MessageBox.Show(
                                "Fehler beim Eintragen. Möglicherweise wurden fehlerhafte Daten eingetragen!", "Fehler",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            isSaved = false;
                        }
                        send = true;
                        von = days.ElementAt(i);
                    }
                    else
                    {
                        send = false;
                    }
                    a = days.ElementAt(i);
                }
                if (send == false)
                {
                    if (!setwt(userid, cbxType.SelectionBoxItem.ToString(), von, a, txtComment.Text))
                    {
                        MessageBox.Show("Fehler beim Eintragen. Möglicherweise wurden fehlerhafte Daten eingetragen!",
                            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        isSaved = false;
                    }
                }
            }

            Close();
        }

        public void dobtnSave_each(DateTime dateFrom, DateTime dateTo)
        {
            DateTime sampleday;
            IFormatProvider culture = new CultureInfo("de-DE", true);
            var isholday = false;
            var days = new LinkedList<DateTime>();
            for (var tmp = dateFrom; (dateTo - tmp).TotalDays >= 0; tmp = tmp.Add(new TimeSpan(1, 0, 0, 0, 0)))
            {
                if (u.WorkingTimeType == 1)
                {
                    if (tmp.ToString("ddd") != "Sa" && tmp.ToString("ddd") != "So")
                    {
                        for (var i = 0; i < holidays.Length; i++)
                        {
                            if (holidays[i] != "")
                            {
                                sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                if (sampleday.Day == tmp.Day && sampleday.Month == tmp.Month &&
                                    sampleday.Year == tmp.Year)
                                {
                                    //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                    isholday = true;
                                }
                            }
                        }
                        if (isholday == false)
                        {
                            days.AddLast(tmp);
                        }
                    }
                }
                else if (u.WorkingTimeType == 2)
                {
                    if (tmp.ToString("ddd") != "So")
                    {
                        for (var i = 0; i < holidays.Length; i++)
                        {
                            if (holidays[i] != "")
                            {
                                sampleday = DateTime.ParseExact(holidays[i], "dd.MM.yyyy HH:mm:ss", culture);
                                if (sampleday.Day == tmp.Day && sampleday.Month == tmp.Month &&
                                    sampleday.Year == tmp.Year)
                                {
                                    //Wenn der Tag ein Feiertag ist, dann wird die Variable auf wahr geschalten
                                    isholday = true;
                                }
                            }
                        }
                        if (isholday == false)
                        {
                            days.AddLast(tmp);
                        }
                    }
                }
                isholday = false;
            }
            if (days.Count > 0)
            {
                var a = days.ElementAt(0);
                var von = days.ElementAt(0);
                var send = false;
                for (var i = 0; i < days.Count; i++)
                {
                    if (!((days.ElementAt(i) - a).TotalDays <= 1))
                    {
                        if (!setwt(userid, cbxType.SelectionBoxItem.ToString(), von, a, txtComment.Text))
                        {
                            MessageBox.Show(
                                "Fehler beim Eintragen. Möglicherweiße wurden Fehlerhafte Daten eingetragen!", "Fehler",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            isSaved = false;
                        }
                        send = true;
                        von = days.ElementAt(i);
                    }
                    else
                    {
                        send = false;
                    }
                    a = days.ElementAt(i);
                }
                if (send == false)
                {
                    if (!setwt(userid, cbxType.SelectionBoxItem.ToString(), von, a, txtComment.Text))
                    {
                        MessageBox.Show("Fehler beim Eintragen. Möglicherweiße wurden Fehlerhafte Daten eingetragen!",
                            "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        isSaved = false;
                    }
                }
            }

            Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dpFrom.SelectedDate <= dpTo.SelectedDate)
            {
                cbxType.Background = color1;
                if (cbxType.SelectedIndex == -1)
                {
                    cbxType.Background = color2;
                    return;
                }


                var von = dpFrom.SelectedDate.Value;
                var bis = dpTo.SelectedDate.Value;

                if (von.Month == bis.Month)
                {
                    dobtnSave();
                }
                else
                {
                    var von1 = new DateTime();
                    var bis1 = new DateTime();

                    von1 = von;
                    var Days = DateTime.DaysInMonth(von1.Year, von1.Month);
                    bis1 = new DateTime(von1.Year, von1.Month, Days);
                    for (var i = 0; i <= bis.Month - von.Month; i++)
                    {
                        dobtnSave_each(von1, bis1);
                        bis1 = bis1 + new TimeSpan(1, 0, 0, 0);
                        if (bis1.Month < bis.Month)
                        {
                            von1 = bis1;
                            bis1 = new DateTime(von1.Year, von1.Month, DateTime.DaysInMonth(von1.Year, von1.Month));
                        }
                        else if (bis1.Month == bis.Month)
                        {
                            von1 = new DateTime(bis1.Year, bis1.Month, 1);
                            bis1 = bis;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                dpTo.SelectedDate = dpFrom.SelectedDate;
            }
        }

        public bool setwt(int uid, string art, DateTime datetimefrom, DateTime datetimeto, string comment)
        {
            if (isSaved)
            {
                return commands.setHolidaytime(uid, art, datetimefrom, datetimeto, comment);
            }
            return false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var curApp = Application.Current;
            var mainWindow = curApp.MainWindow;
            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
            dpFrom.SelectedDate = new DateTime(year, month, 1);
            dpTo.SelectedDate = new DateTime(year, month, 1);

            cbxType.SelectedIndex = 0;
        }

        private void dpFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var tmp = dpTo.SelectedDate.Value - dpFrom.SelectedDate.Value;
                if (tmp.Days < 0)
                {
                    dpTo.SelectedDate = dpFrom.SelectedDate;
                }
            }
            catch
            {
                /**/
                /**/
            }
        }

        private void dpTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var temp = dpTo.SelectedDate.Value - dpFrom.SelectedDate.Value;
                if (temp.Days < 0)
                {
                    MessageBox.Show("Von-Datum muss kleiner als Bis-Datum sein!", "Fehler", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    dpTo.SelectedDate = dpFrom.SelectedDate;
                }
            }
            catch
            {
                /**/
                /**/
            }
        }

        public bool getSaved()
        {
            return isSaved;
        }
    }
}