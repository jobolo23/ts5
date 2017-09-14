using IntranetTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TGTheraS4.Objects;

namespace TGTheraS4
{
    /// <summary>
    /// Interaktionslogik für Funktionszugehoerigkeit.xaml
    /// </summary>
    public partial class Funktionszugehoerigkeit : Window
    {
        public List<UserFunctions> func = new List<UserFunctions>();
        public List<UserFunctions> selected = new List<UserFunctions>();
        SQLCommands c = new SQLCommands();
        bool close = false;
        public int id;
        public bool set = false;

        public Funktionszugehoerigkeit(int id)
        {
            InitializeComponent();
            func = c.getUserFunctions();
            this.id = id;
            selected = c.getUserFunctions(id);
            foreach (UserFunctions f in selected)
            {
                UserFunctions found = null;
                foreach (UserFunctions f2 in func)
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
                UserFunctions tmp = new UserFunctions();
                tmp = (UserFunctions)dgFuncAll.SelectedItem;
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

            NewFunc newf = new NewFunc(true);
            newf.ShowDialog();
            if (newf.finished)
            {
                c.addFunctions(newf.txt);
                func = new List<UserFunctions>();
                func = c.getUserFunctions();
                foreach (UserFunctions f in selected)
                {
                    UserFunctions found = null;
                    foreach (UserFunctions f2 in func)
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
                NewFunc newf = new NewFunc(false);
                newf.ShowDialog();
                if (newf.finished)
                {
                    c.editFunctions(newf.txt, ((UserFunctions)dgFuncAll.SelectedItem).id);
                    func = new List<UserFunctions>();
                    func = c.getUserFunctions();
                    foreach (UserFunctions f in selected)
                    {
                        UserFunctions found = null;
                        foreach (UserFunctions f2 in func)
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
                UserFunctions tmp = new UserFunctions();
                tmp = (UserFunctions)dgHouseSelected.SelectedItem;
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
                this.set = true;

                this.close = true;
                this.Close();
            }catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern der Daten! \nBitte versuchen Sie es in einigen Augenblicken erneut!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
        }
    }
}
