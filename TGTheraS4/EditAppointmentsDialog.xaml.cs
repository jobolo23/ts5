using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntranetTG;
using IntranetTG.Functions;
using IntranetTG.Objects;

namespace TheraS5
{
    public partial class EditAppointmentsDialog : Window
    {
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        private readonly SQLCommands commands;
        private readonly object[] editData;
        private readonly bool isNew;
        private DateTime readDateTime;
        private Enumerations.Status statTask;

        public EditAppointmentsDialog(SQLCommands sql)
        {
            InitializeComponent();

            commands = sql;

            switch (Functions.AppointmentTask)
            {
                case Enumerations.InstructionTask.Task: // Dialog fuer Aufgabenfenster

                    Title = "Aufgabe erstellen ...";
                    lblTitle.Content = "Aufgabe:";
                    rbRead.Visibility = Visibility.Hidden; // Gelesen Knopf

                    cbUser.ItemsSource = Functions.EmployeeList; // Zugewiesen an box
                    cbUser.DisplayMemberPath = "FullName";
                    cbUser.SelectedValuePath = "Id";

                    lbUser.Visibility = Visibility.Hidden; // Zugewiesen an box auswahl

                    rbArchiv.Visibility = Visibility.Hidden; // Archivieren Knopf

                    break;
                case Enumerations.InstructionTask.Instruction: // Dialog fuer Dienansweisungsfenster

                    Title = "Dienstanweisung erstellen ...";
                    lblTitle.Content = "Dienstanweisung:";
                    lblEnd.Visibility = Visibility.Hidden;
                    dpTo.Visibility = Visibility.Hidden; // Datumsauswahl

                    cbUser.Visibility = Visibility.Hidden; // Zugewiesen an box

                    lbUser.ItemsSource = Functions.EmployeeList; // Zugewiesen an box auswahl
                    lbUser.DisplayMemberPath = "FullName";
                    lbUser.SelectedValuePath = "Id";

                    break;
                default:
                    break;
            }
            lblEmployee.Content = "Zuweisen an:";
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

                    Title = "Aufgabe bearbeiten ...";
                    lblTitle.Content = "Aufgabe:";
                    rbRead.Visibility = Visibility.Hidden;

                    cbUser.ItemsSource = Functions.EmployeeList;
                    cbUser.DisplayMemberPath = "FullName";
                    cbUser.SelectedValuePath = "Id";
                    cbUser.SelectedValue = data[7];

                    lbUser.Visibility = Visibility.Hidden;
                    rbArchiv.Visibility = Visibility.Hidden;

                    break;
                case Enumerations.InstructionTask.Instruction:

                    Title = "Dienstanweisung bearbeiten ...";
                    lblTitle.Content = "Dienstanweisung:";
                    lblEnd.Visibility = Visibility.Hidden;
                    dpTo.Visibility = Visibility.Hidden;

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
                    if (data[9] == Functions.ActualUserFromList.Id)
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

            dpFrom.Text = data[5];
            dpTo.Text = data[6];
            txtTitle.Text = data[2];

            txtDescription.Text = data[3];

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
                        if ((bool) rbToUser.IsChecked)
                        {
                            statTask = Enumerations.Status.Zugewiesen;
                        }
                        if ((bool) rbRead.IsChecked)
                        {
                            statTask = Enumerations.Status.Gelesen;
                            readDateTime = DateTime.Now;
                        }
                        if ((bool) rbClose.IsChecked)
                        {
                            statTask = Enumerations.Status.Abgeschlossen;
                        }
                        var dateFrom = dpFrom.SelectedDate.Value;
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
                        Close();
                    }
                    else
                    {
                        var message = "";

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

                    if (txtTitle.Text != "" && cbUser.SelectedValue != null && cbType.SelectedValue != null &&
                        dpTo.SelectedDate != null)
                    {
                        if ((bool) rbToUser.IsChecked)
                        {
                            statTask = Enumerations.Status.Zugewiesen;
                        }

                        if ((bool) rbClose.IsChecked)
                        {
                            statTask = Enumerations.Status.Abgeschlossen;
                        }
                        DateTime dateFrom;


                        dateFrom = dpFrom.SelectedDate.Value;

                        var dateTo = dpTo.SelectedDate.Value;

                        var data = new List<string>
                        {
                            cbUser.SelectedValue.ToString(),
                            dateFrom.ToString("yyyy-MM-dd HH:mm"),
                            dateTo.ToString("yyyy-MM-dd HH:mm"),
                            txtTitle.Text,
                            txtDescription.Text,
                            cbType.SelectedValue.ToString(),
                            Convert.ToInt32(statTask).ToString()
                        };

                        if (isNew)
                        {
                            commands.setTask(data);
                        }
                        else
                        {
                            commands.updateTask(data, Convert.ToInt32(editData[0]));
                        }
                        Close();
                    }
                    else
                    {
                        var message = "";

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
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var curApp = Application.Current;
            var mainWindow = curApp.MainWindow;
            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
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