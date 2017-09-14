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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntranetTG;
using IntranetTG.Functions;

namespace TGTheraS4
{
    /// <summary>
    /// Interaction logic for EditShoutDialog.xaml
    /// </summary>
    public partial class EditShoutDialog : Window
    {
        private SQLCommands c = new SQLCommands();
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditShoutDialog()
        {
            InitializeComponent();
            txtShout.Focus();     
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtShout.Background = color1;
            if (txtShout.Text != "")
            {
                string s = "";

                int[] abstand = new int[100];
                int tmp = 0;
                int run = 1;

                for (int i = 0; i < txtShout.Text.Length; i++)
                {
                    if (i >= (150 * run))
                    {
                        if (tmp == 0)
                        {
                            tmp = 150 * run;
                        }
                        abstand[run - 1] = tmp;
                        run++;
                        tmp = 0;
                    }
                    else
                    {
                        if (txtShout.Text[i].ToString() == " ")
                        {
                            tmp = i;
                        }
                    }
                }

                run = 0;
                for (int i = 0; i < txtShout.Text.Length; i++)
                {
                    s += txtShout.Text[i].ToString();
                    if (i == abstand[run] && abstand[run] != 0)
                    {
                        s += "\n";
                        run++;
                    }
                }

                c.setShout(s, Functions.ActualUserFromList.Id);              
                this.Close();
                
            }
            else
            {
                txtShout.Background = color2;
                MessageBox.Show("Sie müssen etwas eintragen", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
     
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }
    }
}
