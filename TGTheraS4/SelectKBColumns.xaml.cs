using System.Windows;
using System.Windows.Controls;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für SelectKBColumns.xaml
    /// </summary>
    public partial class SelectKBColumns : Window
    {
        private readonly string belhead = "Belegnummer";
        private readonly string dathead = "Datum";
        private readonly string komhead = "Kommentar";
        private readonly string brhead = "Bruttobetrag";
        private readonly string einhead = "Einnahmen";
        private readonly string aushead = "Ausgaben";
        private readonly string steuhead = "Steuersatz";
        private readonly string netthead = "Nettobetrag";
        private readonly string mwsthead = "Mehrwertsteuer";
        private readonly string ksssthead = "Kassastand";
        private readonly string knrhead = "Kontonummer";
        private readonly string eintrhead = "Eingetragen von";
        public bool closed, canceled, ok;

        private readonly DataGridTextColumn dgtcbel;
        private readonly DataGridTextColumn dgtcdat;
        private readonly DataGridTextColumn dgtckomm;
        private readonly DataGridTextColumn dgtcbr;
        private readonly DataGridTextColumn dgtcein;
        private readonly DataGridTextColumn dgtcaus;
        private readonly DataGridTextColumn dgtcsteu;
        private readonly DataGridTextColumn dgtcnett;
        private readonly DataGridTextColumn dgtcmwst;
        private readonly DataGridTextColumn dgtckassst;
        private readonly DataGridTextColumn dgtcknr;
        private readonly DataGridTextColumn dgtceinga;
        private readonly bool PDF;
        private bool CSV;

        public SelectKBColumns(bool forPDF)
        {
            dgtcbel = new DataGridTextColumn {Header = belhead};
            dgtcdat = new DataGridTextColumn {Header = dathead};
            dgtckomm = new DataGridTextColumn {Header = komhead};
            dgtcbr = new DataGridTextColumn {Header = brhead};
            dgtcein = new DataGridTextColumn {Header = einhead};
            dgtcaus = new DataGridTextColumn {Header = aushead};
            dgtcsteu = new DataGridTextColumn {Header = steuhead};
            dgtcnett = new DataGridTextColumn {Header = netthead};
            dgtcmwst = new DataGridTextColumn {Header = mwsthead};
            dgtckassst = new DataGridTextColumn {Header = ksssthead};
            dgtcknr = new DataGridTextColumn {Header = knrhead};
            dgtceinga = new DataGridTextColumn {Header = eintrhead};

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
                PDF = true;
                CSV = false;
            }
            else
            {
                CSV = true;
                PDF = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void bttnKBSelColOK_Click(object sender, RoutedEventArgs e)
        {
            ok = true;
            closed = true;
            Close();
        }

        private void bttnKBSelColCan_Click(object sender, RoutedEventArgs e)
        {
            canceled = true;
            closed = true;
            Close();
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