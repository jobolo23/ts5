﻿using System.Windows;
using System.Windows.Controls;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für ReadBig.xaml
    /// </summary>
    public partial class ReadBig : Window
    {
        public ReadBig()
        {
            InitializeComponent();
        }


        private void wndwReadBig_Loaded(object sender, RoutedEventArgs e)
        {
            txtReadBig.Focus();
            lbl_count_words.Content = "Zeichen: " + txtReadBig.Text.Length;
        }

        public void setReadBig(string theText, bool readOnly)
        {
            txtReadBig.Text = theText;
            if (readOnly)
            {
                txtReadBig.IsReadOnly = true;
            }
        }

        public string getReadBig()
        {
            return txtReadBig.Text;
        }

        private void btn_big_spellcheck_Click(object sender, RoutedEventArgs e)
        {
            if (txtReadBig.SpellCheck.IsEnabled)
            {
                txtReadBig.SpellCheck.IsEnabled = false;
                btn_big_spellcheck.Content = "Wörterbuch aktivieren";
            }
            else
            {
                txtReadBig.SpellCheck.IsEnabled = true;
                btn_big_spellcheck.Content = "Wörterbuch deaktivieren";
            }
        }

        private void btn_big_ok_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txtReadBig_TextChanged(object sender, TextChangedEventArgs e)
        {
            lbl_count_words.Content = "Zeichen: " + txtReadBig.Text.Length;
        }
    }
}