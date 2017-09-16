using System.Data;
using System.Windows;
using System.Windows.Controls;
using IntranetTG;
using IntranetTG.Functions;
using IntranetTG.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaction logic for EditUserControl.xaml
    /// </summary>
    public partial class EditUserControl : UserControl
    {
        public SQLCommands commands;
        public int editNumber;
        public DataGridRow row;


        public EditUserControl(SQLCommands sql)
        {
            InitializeComponent();
            commands = sql;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (Functions.EditNumberEditDialog == Enumerations.EditDialog.WorkingTime)
            {
                var editdialog = new EditWorkingTimeDialog(commands);
                editdialog.Show();
            }
        }

        private void dgEdit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEdit.SelectedItem != null)
            {
                var row = (DataRowView) dgEdit.SelectedItem;
                if (Functions.EditNumberEditDialog == Enumerations.EditDialog.WorkingTime)
                {
                    //string dataDb = commands.getWorkingTimeById(Functions.actualUserFromList.Id, row.Row[0].ToString());
                    //derzeit noch % am schluss vom string, was aber nicht stört. 
                    var data = row.Row.ItemArray; //dataDb.Split('$');
                    var editdialog = new EditWorkingTimeDialog(data);
                    editdialog.Show();
                }
            }
        }
    }
}