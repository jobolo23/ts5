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
using TGTheraS4.Objects;

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für ShowReadInstruction.xaml
    /// </summary>
    public partial class ShowReadInstruction : Window
    {
        private SQLCommands c;
        private List<ReadInstruction> l = new List<ReadInstruction>();

        public ShowReadInstruction(String date, String title, String desc, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            l = c.getInstructionRead(title, date, desc);
            dgReadInstruction.ItemsSource = l;
        }

        private void btnabbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
