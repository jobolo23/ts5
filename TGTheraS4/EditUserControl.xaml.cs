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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IntranetTG;
using IntranetTG.Functions;
using IntranetTG.Objects;
using System.Data;


namespace TheraS5
{
    /// <summary>
    /// Interaction logic for EditUserControl.xaml
    /// </summary>
    public partial class EditUserControl : UserControl
    {
        public int editNumber;
        public DataGridRow row;
        public SQLCommands commands;


        public EditUserControl(SQLCommands sql)
        {
            InitializeComponent();
            commands = sql;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (Functions.EditNumberEditDialog == Enumerations.EditDialog.WorkingTime)
            {
                EditWorkingTimeDialog editdialog = new EditWorkingTimeDialog(commands);
                editdialog.Show();
            }    
        }

        private void dgEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEdit.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgEdit.SelectedItem;
                if (Functions.EditNumberEditDialog == Enumerations.EditDialog.WorkingTime)
                {
                    //string dataDb = commands.getWorkingTimeById(Functions.actualUserFromList.Id, row.Row[0].ToString());
                    //derzeit noch % am schluss vom string, was aber nicht stört. 
                    object[] data = row.Row.ItemArray;//dataDb.Split('$');
                    EditWorkingTimeDialog editdialog = new EditWorkingTimeDialog(data);
                    editdialog.Show();
                }                
            }
        }
        
    }
}
