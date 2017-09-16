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
using IntranetTG;

namespace TheraS5
{
    /// <summary>
    /// Interaktionslogik für EditArt.xaml
    /// </summary>
    public partial class EditArt : Window
    {

        SQLCommands c;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditArt(SQLCommands sql)
        {
            c = sql;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtArt.Background = color1;
            if (!String.IsNullOrWhiteSpace(txtArt.Text))
            {
                c.setnewMedicalActionType(txtArt.Text);
                this.Close();
            }else
            {
                txtArt.Background = color2;
            }
        }
    }
}
