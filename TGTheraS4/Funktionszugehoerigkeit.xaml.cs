using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using IntranetTG;
using TheraS5.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Funktionszugehoerigkeit.xaml
    /// </summary>
    public partial class Funktionszugehoerigkeit : Window
    {
        private readonly SQLCommands c;
        private bool close;
        public List<UserFunctions> func = new List<UserFunctions>();
        public int id;
        public List<UserFunctions> selected = new List<UserFunctions>();
        public bool set;

        public Funktionszugehoerigkeit(int id, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
            func = c.getUserFunctions();
            this.id = id;
            selected = c.getUserFunctions(id);
            foreach (var f in selected)
            {
                UserFunctions found = null;
                foreach (var f2 in func)
                {
                    if (f2.id == f.id)
                    {
                        found = f2;
                    }
                }
                if (found != null)
                {
                    func.Remove(found);
                }
            }
            dgFuncAll.ItemsSource = func;
            dgHouseSelected.ItemsSource = selected;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgFuncAll.SelectedIndex < 0)
            {
                MessageBox.Show("Sie müssen eine Funktion auswählen!");
            }
            else
            {
                var tmp = new UserFunctions();
                tmp = (UserFunctions) dgFuncAll.SelectedItem;
                selected.Add(tmp);
                func.Remove(tmp);
                dgHouseSelected.ItemsSource = null;
                dgHouseSelected.ItemsSource = selected;

                dgFuncAll.ItemsSource = null;
                dgFuncAll.ItemsSource = func;
            }
        }

        private void btnNewFunc_Click(object sender, RoutedEventArgs e)
        {
            var newf = new NewFunc(true);
            newf.ShowDialog();
            if (newf.finished)
            {
                c.addFunctions(newf.txt);
                func = new List<UserFunctions>();
                func = c.getUserFunctions();
                foreach (var f in selected)
                {
                    UserFunctions found = null;
                    foreach (var f2 in func)
                    {
                        if (f2.id == f.id)
                        {
                            found = f2;
                        }
                    }
                    if (found != null)
                    {
                        func.Remove(found);
                    }
                }
                dgFuncAll.ItemsSource = func;
            }
        }

        private void btnNewEditFunc_Click(object sender, RoutedEventArgs e)
        {
            if (dgFuncAll.SelectedIndex < 0)
            {
                MessageBox.Show("Sie müssen eine Funktion auswählen!");
            }
            else
            {
                var newf = new NewFunc(false);
                newf.ShowDialog();
                if (newf.finished)
                {
                    c.editFunctions(newf.txt, ((UserFunctions) dgFuncAll.SelectedItem).id);
                    func = new List<UserFunctions>();
                    func = c.getUserFunctions();
                    foreach (var f in selected)
                    {
                        UserFunctions found = null;
                        foreach (var f2 in func)
                        {
                            if (f2.id == f.id)
                            {
                                found = f2;
                            }
                        }
                        if (found != null)
                        {
                            func.Remove(found);
                        }
                    }
                    dgFuncAll.ItemsSource = func;
                }
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dgHouseSelected.SelectedIndex < 0)
            {
                MessageBox.Show("Sie müssen eine Funktion auswählen!");
            }
            else
            {
                var tmp = new UserFunctions();
                tmp = (UserFunctions) dgHouseSelected.SelectedItem;
                func.Add(tmp);
                selected.Remove(tmp);
                dgFuncAll.ItemsSource = null;
                dgFuncAll.ItemsSource = func;

                dgHouseSelected.ItemsSource = null;
                dgHouseSelected.ItemsSource = selected;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                c.saveUserfunc(id, selected);
                set = true;

                close = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Fehler beim Speichern der Daten! \nBitte versuchen Sie es in einigen Augenblicken erneut!");
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
        }
    }
}