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
    /// Interaktionslogik für SelectKBColumns.xaml
    /// </summary>
    public partial class SelectKBColumns : Window
    {
        public bool closed = false, canceled = false, ok = false;
        private bool PDF = false, CSV = false;

        private string belhead = "Belegnummer", dathead = "Datum", komhead = "Kommentar", brhead = "Bruttobetrag", einhead = "Einnahmen", aushead = "Ausgaben", steuhead = "Steuersatz", netthead = "Nettobetrag", mwsthead = "Mehrwertsteuer", ksssthead = "Kassastand", knrhead = "Kontonummer", eintrhead = "Eingetragen von";

        private DataGridTextColumn dgtcbel, dgtcdat, dgtckomm, dgtcbr, dgtcein, dgtcaus, dgtcsteu, dgtcnett, dgtcmwst, dgtckassst, dgtcknr, dgtceinga;

        public SelectKBColumns(bool forPDF)
        {
            dgtcbel = new DataGridTextColumn { Header = belhead };
            dgtcdat = new DataGridTextColumn { Header = dathead };
            dgtckomm = new DataGridTextColumn { Header = komhead };
            dgtcbr = new DataGridTextColumn { Header = brhead };
            dgtcein = new DataGridTextColumn { Header = einhead };
            dgtcaus = new DataGridTextColumn { Header = aushead };
            dgtcsteu = new DataGridTextColumn { Header = steuhead };
            dgtcnett = new DataGridTextColumn { Header = netthead };
            dgtcmwst = new DataGridTextColumn { Header = mwsthead };
            dgtckassst = new DataGridTextColumn { Header = ksssthead };
            dgtcknr = new DataGridTextColumn { Header = knrhead };
            dgtceinga = new DataGridTextColumn { Header = eintrhead };

            InitializeComponent();
            chkBxKBSelDat.IsChecked = true;
            chkBxKBSelBelNr.IsChecked = true;
            chkBxKBSelComm.IsChecked = true;
            chkBxKBSelEinn.IsChecked = true;
            chkBxKBSelAusg.IsChecked = true;
            chkBxKBSelMWST.IsChecked = true;
            chkBxKBSelKnr.IsChecked = true;
            if (forPDF)
            {
                this.PDF = true;
                this.CSV = false;
            }
            else
            {
                this.CSV = true;
                this.PDF = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void bttnKBSelColOK_Click(object sender, RoutedEventArgs e)
        {
            this.ok = true;
            this.closed = true;
            this.Close();
        }

        private void bttnKBSelColCan_Click(object sender, RoutedEventArgs e)
        {
            this.canceled = true;
            this.closed = true;
            this.Close();
        }

        //CHECKBOX EVENTS

        public void chkBxKBSelBelNr_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcbel);
            chkCols();
        }

        private void chkBxKBSelDat_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcdat);
            chkCols();
        }

        private void chkBxKBSelComm_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtckomm);
            chkCols();
        }

        private void chkBxKBSelBrutto_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcbr);
            chkCols();
        }

        private void chkBxKBSelEinn_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcein);
            chkCols();
        }

        private void chkBxKBSelAusg_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcaus);
            chkCols();
        }

        private void chkBxKBSelSteuers_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcsteu);
            chkCols();
        }

        private void chkBxKBSelNetto_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcnett);
            chkCols();
        }

        private void chkBxKBSelMWST_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcmwst);
            chkCols();
        }

        private void chkBxKBSelKnr_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtcknr);
            chkCols();
        }

        private void chkBxKBSelEintr_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtceinga);
            chkCols();
        }

        private void chkBxKBSelBelNr_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcbel);
            chkCols();
        }

        private void chkBxKBSelDat_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcdat);
            chkCols();
        }

        private void chkBxKBSelComm_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtckomm);
            chkCols();
        }

        private void chkBxKBSelBrutto_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcbr);
            chkCols();
        }

        private void chkBxKBSelEinn_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcein);
            chkCols();
        }

        private void chkBxKBSelAusg_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcaus);
            chkCols();
        }

        private void chkBxKBSelSteuers_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcsteu);
            chkCols();
        }

        private void chkBxKBSelNetto_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcnett);
            chkCols();
        }

        private void chkBxKBSelMWST_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcmwst);
            chkCols();
        }

        private void chkBxKBSelKnr_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtcknr);
            chkCols();
        }

        private void chkBxKBSelEintr_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtceinga);
            chkCols();
        }

        private void chkBxKBSelKassst_Checked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Add(dgtckassst);
            chkCols();
        }

        private void chkBxKBSelKassst_Unchecked(object sender, RoutedEventArgs e)
        {
            dgvKBOrder.Columns.Remove(dgtckassst);
            chkCols();
        }

        private void chkCols()
        {
            if (PDF && dgvKBOrder.Columns.Count > 7)
            {
                lblKBHinw.Content = "Hinweis: Durch zu viele Spalten wird das PDF unausdruckbar oder unleselich!";
            }
            else
            {
                lblKBHinw.Content = "";
            }
        }
    }
}
