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
using IntranetTG.Objects;
using IntranetTG;

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für Bericht.xaml
    /// </summary>
    public partial class Bericht : Window
    {
        User u;
        SQLCommands c;
        string[] drucken = new string[6]; 
        public Bericht()
        {
            InitializeComponent();

        }
        public Bericht(User u, SQLCommands sql)
        {
            InitializeComponent();
            this.c = sql;
            this.u = u;
            FillKidsIntoBerichtKombo();
            
        }

        private void FillKidsIntoBerichtKombo(){
            foreach (var service in u.Services)
            {
                foreach (var client in c.WgToClients(service))
                {
                    cmbKlient.Items.Add(client);
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
                        drucken = c.getDokuOverTime(cmbKlient.Text, (DateTime)dateVon.SelectedDate, (DateTime)dateBis.SelectedDate);
                        if ((DateTime)dateVon.SelectedDate < (DateTime)dateBis.SelectedDate)
                        {


                            foreach (string feld in drucken)
                            {
                                int i = 0;

                                PrintDialog printDialog = new PrintDialog();


                                if ((bool)printDialog.ShowDialog().GetValueOrDefault())
                                {
                                    if (i != 4)
                                    {
                                        FlowDocument flowDocument = new FlowDocument();
                                        foreach (string line in feld.Split('$'))
                                        {
                                            Paragraph myParagraph = new Paragraph();

                                            myParagraph.Margin = new Thickness(10);
                                            myParagraph.Padding = new Thickness(20);
                                            myParagraph.Inlines.Add(new Run(line));
                                            flowDocument.Blocks.Add(myParagraph);
                                        }
                                        DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                                        printDialog.PrintDocument(paginator, getTitle(i));
                                        i++;
                                    }
                                    else
                                        return;
                                }
                            }

                            MessageBoxResult ergebnis = MessageBox.Show("Möchten Sie einen weiteren Bericht Drucken?", "Weiter machen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (ergebnis == MessageBoxResult.No)
                                this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Von-Datum muss kleiner als Bis-Datum sein!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es muss ein Bis-Datum ausgewählt sein!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Es muss ein Von-Datum ausgewählt sein!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Es muss ein Klient ausgewählt sein!", "Achtung", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private string getTitle(int i)
        {
            if (i == 0)
                return "Körperlich";
            else if (i == 1)
                return "Schulisch"; 
            else if (i == 2)
                return "Psychisch";
            else if (i == 3)
                return "Außenkontakt";
            else if (i == 4)
                return "Pflichten";
            else if (i == 5)
                return "";
            else
                return "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }

        private void cmbKlient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
