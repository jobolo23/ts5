using System.Collections.Generic;
using System.Windows;
using IntranetTG;
using TheraS5.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für ShowReadInstruction.xaml
    /// </summary>
    public partial class ShowReadInstruction : Window
    {
        private readonly SQLCommands c;
        private readonly List<ReadInstruction> l = new List<ReadInstruction>();

        public ShowReadInstruction(string date, string title, string desc, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            l = c.getInstructionRead(title, date, desc);
            dgReadInstruction.ItemsSource = l;
        }

        private void btnabbrechen_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}