using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using IntranetTG;
using IntranetTG.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Bericht.xaml
    /// </summary>
    public partial class Bericht : Window
    {
        private readonly SQLCommands c;
        private string[] drucken = new string[6];
        private readonly User u;

        public Bericht()
        {
            InitializeComponent();
        }

        public Bericht(User u, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            this.u = u;
            FillKidsIntoBerichtKombo();
        }

        private void FillKidsIntoBerichtKombo()
        {
            foreach (var service in u.Services)
            {
                foreach (var client in c.WgToClients(service, SQLCommands.ClientFilter.NotLeft))
                {
                    cmbKlient.Items.Add(client.Key + " " + client.Value);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (cmbKlient.Text != "")
            {
                if (dateVon.Text != "")
                {
                    if (dateBis.Text != "")
                    {
                        drucken = c.getDokuOverTime(cmbKlient.Text, (DateTime) dateVon.SelectedDate,
                            (DateTime) dateBis.SelectedDate);
                        if ((DateTime) dateVon.SelectedDate < (DateTime) dateBis.SelectedDate)
                        {
                            foreach (var feld in drucken)
                            {
                                var i = 0;

                                var printDialog = new PrintDialog();


                                if (printDialog.ShowDialog().GetValueOrDefault())
                                {
                                    if (i != 4)
                                    {
                                        var flowDocument = new FlowDocument();
                                        foreach (var line in feld.Split('$'))
                                        {
                                            var myParagraph = new Paragraph();

                                            myParagraph.Margin = new Thickness(10);
                                            myParagraph.Padding = new Thickness(20);
                                            myParagraph.Inlines.Add(new Run(line));
                                            flowDocument.Blocks.Add(myParagraph);
                                        }
                                        var paginator = ((IDocumentPaginatorSource) flowDocument).DocumentPaginator;
                                        printDialog.PrintDocument(paginator, getTitle(i));
                                        i++;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                            }

                            var ergebnis = MessageBox.Show("Möchten Sie einen weiteren Bericht Drucken?",
                                "Weiter machen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (ergebnis == MessageBoxResult.No)
                            {
                                Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Von-Datum muss kleiner als Bis-Datum sein!", "Achtung",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es muss ein Bis-Datum ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Es muss ein Von-Datum ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Es muss ein Klient ausgewählt sein!", "Achtung", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private string getTitle(int i)
        {
            if (i == 0)
            {
                return "Körperlich";
            }
            if (i == 1)
            {
                return "Schulisch";
            }
            if (i == 2)
            {
                return "Psychisch";
            }
            if (i == 3)
            {
                return "Außenkontakt";
            }
            if (i == 4)
            {
                return "Pflichten";
            }
            if (i == 5)
            {
                return "";
            }
            return "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var curApp = Application.Current;
            var mainWindow = curApp.MainWindow;
            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
        }

        private void cmbKlient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}