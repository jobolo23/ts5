using System.Windows;
using System.Windows.Media;
using IntranetTG;
using IntranetTG.Functions;

namespace TheraS5
{
    /// <summary>
    ///     Interaction logic for EditShoutDialog.xaml
    /// </summary>
    public partial class EditShoutDialog : Window
    {
        private readonly SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditShoutDialog(SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            txtShout.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtShout.Background = color1;
            if (txtShout.Text != "")
            {
                var s = "";

                var abstand = new int[100];
                var tmp = 0;
                var run = 1;

                for (var i = 0; i < txtShout.Text.Length; i++)
                {
                    if (i >= 150 * run)
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
                for (var i = 0; i < txtShout.Text.Length; i++)
                {
                    s += txtShout.Text[i].ToString();
                    if (i == abstand[run] && abstand[run] != 0)
                    {
                        s += "\n";
                        run++;
                    }
                }

                c.SetShout(s, Functions.ActualUserFromList.Id);
                Close();
            }
            else
            {
                txtShout.Background = color2;
                MessageBox.Show("Sie müssen etwas eintragen", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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
        }
    }
}