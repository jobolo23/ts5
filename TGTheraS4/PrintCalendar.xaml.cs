﻿using System.Windows;

namespace TheraS5.Objects
{
    /// <summary>
    ///     Interaktionslogik für PrintCalendar.xaml
    /// </summary>
    public partial class PrintCalendar : Window
    {
        private string ret = "NULL";

        public PrintCalendar()
        {
            InitializeComponent();
        }

        private void btnSavePDFCal_Click(object sender, RoutedEventArgs e)
        {
            switch (slctServiceForPrint.SelectedItem.ToString())
            {
                case "TG Neufeld":
                    ret = "a2dzaWc1ZW1hdjhnN2ZxNDgxcnQ5OHM1MG9AZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                case "TG Ebenfurth":
                    ret = "Z3ZpY21ldWRhMzdiMTY4OHM3b2x0c2lmdDRAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                case "TG Grinzing":
                    ret = "aHNoa2hkczNsc2h2cWs3ZDY2Mm43MzdhdmdAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                case "TG Sitzendorf":
                    ret = "ZTVmbjVvbWNnOGtoZ3N0azJwMDJ1MDRqcTRAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                case "TG UMF":
                    ret = "dWVtYW9jb2NmNG50cDJkMWtmZWhtMjRjanNAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                case "TG Privatschule":
                    ret = "MjdrMWY4OXZmNGpvbjcydGkxbTEwMW12NWNAZ3JvdXAuY2FsZW5kYXIuZ29vZ2xlLmNvbQ";
                    DialogResult = true;
                    break;

                default:
                    break;
            }

            Close();
        }

        private void dlgPrintCal_Loaded(object sender, RoutedEventArgs e)
        {
            slctServiceForPrint.Items.Add("TG Neufeld");
            slctServiceForPrint.Items.Add("TG Ebenfurth");
            slctServiceForPrint.Items.Add("TG Grinzing");
            slctServiceForPrint.Items.Add("TG Sitzendorf");
            slctServiceForPrint.Items.Add("TG UMF");
            slctServiceForPrint.Items.Add("TG Privatschule");
        }

        public string getIt()
        {
            Close();
            return ret;
        }
    }
}