using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Funny.xaml
    /// </summary>
    public partial class Funny : Window
    {
        private bool check;
        private bool close;
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private bool proxy;
        private int value;

        public Funny()
        {
            InitializeComponent();
        }

        private void myGif_MediaOpened(object sender, RoutedEventArgs e)
        {
        }

        private void myGif_MediaEnded(object sender, RoutedEventArgs e)
        {
            myGif.Position = new TimeSpan(0, 0, 1);
            myGif.Play();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            progressBar1.Maximum = 100;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Updating the Label which displays the current second
            if (value < 100)
            {
                progressBar1.Value = value;
                value++;
                value++;
                if (value == 50 && check == false)
                {
                    label2.Content = "NSA verhindert das löschen!";
                    value = value - 10;
                    progressBar1.Value = value;
                    check = true;
                }
                else if (value < 10 && check == false)
                {
                    label2.Content = "IP-Adresse wird verschlüsselt";
                }
                else if (value < 45 && check == false)
                {
                    label2.Content = "Lösche das Internet";
                }
                else if (value > 45 && value < 55 && check)
                {
                    label2.Content = "Hacke die NSA";
                }
                else if (value == 76 && check)
                {
                    dispatcherTimer.Stop();
                    label2.Content = "Die NSA ist Ihnen auf der Spur!";
                    var fun = new Funny2();
                    fun.ShowDialog();

                    if (fun.selected == 2)
                    {
                        proxy = true;
                    }

                    var fun2 = new Funny3();
                    fun2.ShowDialog();

                    dispatcherTimer.Start();
                }
                else if (value > 55 && check)
                {
                    label2.Content = "Lösche das restliche Internet";
                }
            }
            else
            {
                if (proxy)
                {
                    close = true;
                    label2.Content = "Internet gelöscht";
                    Close();
                }
                else
                {
                    close = true;
                    label2.Content = "Internet konnte nicht gelöscht werden";
                    Close();
                }
            }

            // Forcing the CommandManager to raise the RequerySuggested event
            //CommandManager.InvalidateRequerySuggested();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (close == false)
            {
                e.Cancel = true;
                MessageBox.Show("Sie können uns nicht aufhalten.", "Zugriff verweigert", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                dispatcherTimer.Stop();
                if (proxy)
                {
                    MessageBox.Show(
                        "Das Internet wurde gelöscht!\nDiese Aktion unterliegt der höchsten Geheimhaltung! Reden Sie mit niemanden!",
                        "Löschen war erfolgreich!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MessageBox.Show("Arbeiten Sie bitte normal weiter.", "Info", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        "Das Internet konnte nicht gelöscht werden!\nDie NSA hat Sie lokalisiert und die Polizei benachrichtigt! Reden Sie mit niemanden!",
                        "Löschen war nicht erfolgreich!", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Arbeiten Sie bitte normal weiter.", "Info", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}