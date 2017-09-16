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

namespace TheraS5
{
    /// <summary>
    /// Interaction logic for EditUserControl.xaml
    /// </summary>
    public partial class GridUserControl : UserControl
    {
        public int editNumber;
        public DataGridRow row;
        public SQLCommands commands;

        public GridUserControl(SQLCommands sql)
        {
            InitializeComponent();
            commands = sql;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //do save
            Save();

            //pnlEdit.Visibility = Visibility.Hidden;

        }

        public void Save()
        {

            switch (Functions.EditNumberEditDialog)
            {
                case Enumerations.EditDialog.HourType:

                    ////HourType hourType = new HourType();
                    ////hourType.ShortName = row.Cells[0].Value.ToString();
                    ////hourType.Name = row.Cells[1].Value.ToString();
                    ////if (row.Cells[2].Value.ToString() != "")
                    ////                 {
                    ////                     hourType.Percent = Convert.ToInt32(row.Cells[2].Value);
                    ////                 }
                    //List<string> data = new List<string> { row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString() };



                    //if (row.Cells[0].RowIndex == 0)
                    //{
                    //    //insert
                    //    commands.setHourTypes(data);
                    //}
                    //else
                    //{
                    //    //update
                    //    //commands.updateHourTypes(data, Convert.ToInt32(row.Cells[0].Value));
                    //}

                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //do cancel - refresh 
            string[] names = new string[] { "Id", "Name", "Bezeichnung", "Prozent" };

           // FillMoreDataWithColumnDefinitions(dgvTable, commands.getHourTypes(), names);

            //pnlEdit.Visibility = Visibility.Visible;

        }

    }
}
