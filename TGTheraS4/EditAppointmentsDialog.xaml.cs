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
using IntranetTG.Objects;
using IntranetTG.Functions;

namespace TGTheraS4
{
    
    public partial class EditAppointmentsDialog : Window
    {
        private SQLCommands commands;
        private bool isNew = false;
        object[] editData = null;
        private DateTime readDateTime;                
        private Enumerations.Status statTask;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditAppointmentsDialog(SQLCommands sql)
        {
            InitializeComponent();

            commands = sql;

            switch (Functions.AppointmentTask)
            {

                case Enumerations.InstructionTask.Task: // Dialog fuer Aufgabenfenster

                    this.Title = "Aufgabe erstellen ...";
                    this.lblTitle.Content = "Aufgabe:";
                    this.rbRead.Visibility = Visibility.Hidden; // Gelesen Knopf

                    cbUser.ItemsSource = Functions.EmployeeList; // Zugewiesen an box
                    cbUser.DisplayMemberPath = "FullName";
                    cbUser.SelectedValuePath = "Id";

                    lbUser.Visibility = Visibility.Hidden; // Zugewiesen an box auswahl

                    rbArchiv.Visibility = Visibility.Hidden; // Archivieren Knopf

                    break;
                case Enumerations.InstructionTask.Instruction: // Dialog fuer Dienansweisungsfenster

                    this.Title = "Dienstanweisung erstellen ...";
                    this.lblTitle.Content = "Dienstanweisung:";
                    this.lblEnd.Visibility = Visibility.Hidden; 
                    this.dpTo.Visibility = Visibility.Hidden; // Datumsauswahl

                    cbUser.Visibility = Visibility.Hidden; // Zugewiesen an box

                    lbUser.ItemsSource = Functions.EmployeeList; // Zugewiesen an box auswahl
                    lbUser.DisplayMemberPath = "FullName";
                    lbUser.SelectedValuePath = "Id";

                    break;
                default:
                    break;
            }
            this.lblEmployee.Content = "Zuweisen an:";
            dpFrom.Text = DateTime.Now.ToShortDateString();
            isNew = true;

            cbType.ItemsSource = Functions.ProjectList;
            cbType.DisplayMemberPath = "Name";
            cbType.SelectedValuePath = "Id";

            
            gbStatus.IsEnabled = false;
        }

        public EditAppointmentsDialog(string[] data)
        {
            InitializeComponent();

            switch (Functions.AppointmentTask)
            {
                case Enumerations.InstructionTask.Task:

                    this.Title = "Aufgabe bearbeiten ...";
                    this.lblTitle.Content = "Aufgabe:";
                    this.rbRead.Visibility = Visibility.Hidden;

                    cbUser.ItemsSource = Functions.EmployeeList;
                    cbUser.DisplayMemberPath = "FullName";
                    cbUser.SelectedValuePath = "Id";
                    cbUser.SelectedValue = data[7];

                    lbUser.Visibility = Visibility.Hidden;
                    rbArchiv.Visibility = Visibility.Hidden;

                    break;
                case Enumerations.InstructionTask.Instruction:

                    this.Title = "Dienstanweisung bearbeiten ...";
                    this.lblTitle.Content = "Dienstanweisung:";
                    this.lblEnd.Visibility = Visibility.Hidden;
                    this.dpTo.Visibility = Visibility.Hidden;

                    cbUser.ItemsSource = Functions.EmployeeList;
                    cbUser.DisplayMemberPath = "FullName";
                    cbUser.SelectedValuePath = "Id";
                    cbUser.SelectedValue = data[7];
                    cbUser.IsEnabled = false;                 
                    
                    lbUser.ItemsSource = Functions.EmployeeList;
                    lbUser.DisplayMemberPath = "FullName";
                    lbUser.SelectedValuePath = "Id";
                    lbUser.SelectedValue = data[7];
                    lbUser.Visibility = Visibility.Hidden;
                    
                    dpFrom.IsEnabled = false;
                    cbType.IsEnabled = false;
                    txtDescription.IsEnabled = false;
                    txtTitle.IsEnabled = false;

                    if (data[7] == Functions.ActualUserFromList.Id)
                    {
                        rbClose.IsEnabled = false;
                    }
                    else
                    {
                        rbRead.IsEnabled = false;
                        if (Convert.ToInt32(data[8]) == 1)
                        {
                            rbArchiv.IsEnabled = true;
                        }
                        else
                        {
                            rbArchiv.IsEnabled = false;
                        }
                    }
                    if (data[9].ToString() == Functions.ActualUserFromList.Id)
                    {
                        rbArchiv.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        rbArchiv.Visibility = Visibility.Hidden;
                    }

                    break;
                default:
                    break;
            }
            editData = data;

            dpFrom.Text = data[5].ToString();
            dpTo.Text = data[6].ToString();
            txtTitle.Text = data[2].ToString();

            txtDescription.Text = data[3].ToString();

            cbType.ItemsSource = Functions.ProjectList;
            cbType.DisplayMemberPath = "Name";
            cbType.SelectedValuePath = "Id";
            cbType.SelectedValue = data[4];

            gbStatus.IsEnabled = true;

            if (Convert.ToInt32(data[8]) == 0)
            {
                rbToUser.IsChecked = true;
            }
            else if (Convert.ToInt32(data[8]) == 1)
            {
                rbRead.IsChecked = true;
                rbToUser.IsEnabled = false;
            }
            else if (Convert.ToInt32(data[8]) == 2)
            {
                rbClose.IsChecked = true;
                rbToUser.IsEnabled = false;
            }
            else if (Convert.ToInt32(data[8]) == 3)
            {
                rbArchiv.IsChecked = true;
            }
           
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            txtTitle.Background = color1;
            lbUser.Background = color1;
            cbType.Background = color1;
            cbUser.Background = color1;
            dpTo.Background = color1;
            switch (Functions.AppointmentTask)
            {
                case Enumerations.InstructionTask.Instruction:

                    if (txtTitle.Text != "" && lbUser.SelectedItems != null && cbType.SelectedValue != null)
                    {
                        if ((bool)rbToUser.IsChecked)
                        {
                            statTask = Enumerations.Status.Zugewiesen;
                        }
                        if ((bool)rbRead.IsChecked)
                        {
                            statTask = Enumerations.Status.Gelesen;
                            readDateTime = DateTime.Now;
                        }
                        if ((bool)rbClose.IsChecked)
                        {
                            statTask = Enumerations.Status.Abgeschlossen;
                        }
                        DateTime dateFrom = dpFrom.SelectedDate.Value;                                                                        
                        DateTime dateTo;

                        if (dpTo.SelectedDate != null)
                        {
                            dateTo = dpTo.SelectedDate.Value;
                        }
                        else
                        {
                            dateTo = DateTime.MinValue.Date;
                        }

                        foreach (Employee emp in lbUser.SelectedItems)
                        {
                            
                        }
                        this.Close();
                    }
                    else
                    {
                        string message = "";

                        if (txtTitle.Text == "")
                        {
                            txtTitle.Background = color2;
                            message += "Bitte den Namen der Dienstanweisung eingeben! \r\n";
                        }
                        if (lbUser.SelectedItems == null)
                        {
                            lbUser.Background = color2;
                            message += "Bitte einem Mitarbeiter zuweisen! \r\n";
                        }
                        if (cbType.SelectedValue == null)
                        {
                            cbType.Background = color2;
                            message += "Bitte eine Art auswählen! \r\n";
                        }
                        MessageBox.Show(message);
                    }                    
                    break;
                case Enumerations.InstructionTask.Task:

                    if (txtTitle.Text != "" && cbUser.SelectedValue != null && cbType.SelectedValue != null && dpTo.SelectedDate != null)
                    {
                        if ((bool)rbToUser.IsChecked)
                        {
                            statTask = Enumerations.Status.Zugewiesen;
                        }
                       
                        if ((bool)rbClose.IsChecked)
                        {
                            statTask = Enumerations.Status.Abgeschlossen;
                        }
                        DateTime dateFrom;
                       

                        
                           dateFrom = dpFrom.SelectedDate.Value;
                    
                        DateTime dateTo = dpTo.SelectedDate.Value;

                        List<string> data = new List<string> { cbUser.SelectedValue.ToString(), dateFrom.ToString("yyyy-MM-dd HH:mm"), dateTo.ToString("yyyy-MM-dd HH:mm"), txtTitle.Text, txtDescription.Text, cbType.SelectedValue.ToString(), (Convert.ToInt32(statTask)).ToString() };

                        if (isNew)
                        {
                            commands.setTask(data);
                        }
                        else
                        {
                            commands.updateTask(data, Convert.ToInt32(editData[0]));
                        }
                        this.Close();
                    }
                    else
                    {
                        string message = "";

                        if (cbUser.SelectedValue == null)
                        {
                            cbUser.Background = color1;
                            message += "Bitte einem Mitarbeiter zuweisen! \r\n";
                        }

                        if (cbType.SelectedValue == null)
                        {
                            cbType.Background = color1;
                            message += "Bitte eine Art auswählen! r\n";
                        }

                        if (txtTitle.Text == "")
                        {
                            txtTitle.Background = color1;
                            message += "Bitte den Namen der Aufgabe eingeben! \r\n";
                        }

                        if (dpTo.SelectedDate == null)
                        {
                            dpTo.Background = color1;
                            message += "Bitte das Enddatum eingeben! \r\n";
                        }
                        MessageBox.Show(message);
                    }

                    break;
                default:
                    break;
            }
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application curApp = Application.Current;
            Window mainWindow = curApp.MainWindow;
            this.Left = mainWindow.Left + (mainWindow.Width - this.ActualWidth) / 2;
            this.Top = mainWindow.Top + (mainWindow.Height - this.ActualHeight) / 2;
        }

        private void cbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            switch (Functions.AppointmentTask)
            {
                case Enumerations.InstructionTask.Instruction:

                    if (cbUser.SelectedValue == null)
                    {
                        gbStatus.IsEnabled = false;
                    }
                    else
                    {
                        gbStatus.IsEnabled = true;
                        rbToUser.IsChecked = true;
                    }

                    // Wenn Dienstanweisung dann ist es nicht möglich diese jemanden anderen zuzuweisen. 
                    rbClose.Visibility = Visibility.Hidden;
                    rbToUser.Visibility = Visibility.Hidden;
                    rbArchiv.Visibility = Visibility.Hidden;
                    break;
                case Enumerations.InstructionTask.Task:
                    
                    rbArchiv.Visibility = Visibility.Hidden;

                    if (isNew)
                    {
                        if (cbUser.SelectedValue.ToString() == Functions.ActualUserFromList.Id)
                        {
                            rbRead.IsEnabled = false;
                            rbRead.IsChecked = true;
                            rbToUser.Visibility = Visibility.Hidden;
                        }
                    }
                    
                    break;
                default:
                    break;
            }

            if (cbUser.SelectedValue != null)
            {
          
            if (cbUser.SelectedValue.ToString() == Functions.ActualUserFromList.Id)
            {
                //kein Status Gelesen wenn man sich selbst eine Aufgabe gibt. Ich habe es immer gelesen. 
                rbRead.IsChecked = true;
                readDateTime = DateTime.Now;

                rbClose.IsEnabled = false;
            }
            else
            {
                rbToUser.IsChecked = true;
                rbClose.IsEnabled = false;
                rbRead.IsChecked = false;
            }
            }
        }

        private void lbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isNew)
            {
                if (lbUser.SelectedItems != null)
                {
                    gbStatus.IsEnabled = true;
                    rbToUser.IsChecked = true;
                }
            }
        }
    }
    //FUCK
}