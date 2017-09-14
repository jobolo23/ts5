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
    /// Interaktionslogik für EditContacts.xaml
    /// </summary>
    public partial class EditContacts : Window
    {
        public bool newone = false;
        public string userid = "";
        public SQLCommands c = new SQLCommands();
        List<Salutations> sal = new List<Salutations>();
        List<IntranetTG.Objects.Title> tit = new List<IntranetTG.Objects.Title>();
        public Contacts selectedItem;
        public bool neu = true;
        public bool saved = false;
        SolidColorBrush color1 = Brushes.White;
        SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));

        public EditContacts(string userid)
        {
            InitializeComponent();
            this.userid = userid;
            sal = c.getsalutations();
            cmbsalu.ItemsSource = sal;
            cmbsalu.DisplayMemberPath = "Name";
            cmbsalu.SelectedIndex = 0;

            tit = c.gettitel();
            cmbtitel.ItemsSource = tit;
            cmbtitel.DisplayMemberPath = "Name";
            cmbtitel.SelectedIndex = 0;
        }

        public EditContacts(string userid, bool soz)
        {
            InitializeComponent();
            this.userid = userid;
            sal = c.getsalutations();
            cmbsalu.ItemsSource = sal;
            cmbsalu.DisplayMemberPath = "Name";
            cmbsalu.SelectedIndex = 0;

            tit = c.gettitel();
            cmbtitel.ItemsSource = tit;
            cmbtitel.DisplayMemberPath = "Name";
            cmbtitel.SelectedIndex = 0;

            if (soz)
            {
                txtfunc.Text = "SozialarbeiterIn";
            }
        }

        public EditContacts(string userid, Contacts selectedItem) : this(userid)
        {
            sal = c.getsalutations();
            cmbsalu.ItemsSource = sal;
            cmbsalu.DisplayMemberPath = "Name";


            tit = c.gettitel();
            cmbtitel.ItemsSource = tit;
            cmbtitel.DisplayMemberPath = "Name";

            neu = false;
            this.selectedItem = selectedItem;
            txtfirstname.Text = selectedItem.firstname;
            txtlastname.Text = selectedItem.lastname;
            txtstreet.Text = selectedItem.street;
            txtcity.Text = selectedItem.city;
            txtemail.Text = selectedItem.email;
            txtphone.Text = selectedItem.phone1;
            txtinstitution.Text = selectedItem.institution;
            txtcountry.Text = selectedItem.country;
            txtzip.Text = selectedItem.zip;
            txtdepartment.Text = selectedItem.department;
            txtfunc.Text = selectedItem.function;
            txtcomment.Text = selectedItem.comment;
            txtcompany.Text = selectedItem.company;
            foreach (IntranetTG.Objects.Title t in tit)
            {
                if (t.Name == selectedItem.title)
                {
                    cmbtitel.SelectedItem = t;
                    break;
                }
            }

            foreach (Salutations t in sal)
            {
                if (t.Name == selectedItem.salutation)
                {
                    cmbsalu.SelectedItem = t;
                    break;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool check()
        {
            bool ret = true;

            txtfirstname.Background = color1;
            if (txtfirstname.Text.Trim() == "")
            {
                ret = false;
                txtfirstname.Background = color2;
            }
            txtlastname.Background = color1;
            if (txtlastname.Text.Trim() == "")
            {
                ret = false;
                txtlastname.Background = color2;
            }
            txtstreet.Background = color1;
            if (txtstreet.Text.Trim() == "")
            {
                ret = false;
                txtstreet.Background = color2;
            }
            txtcity.Background = color1;
            if (txtcity.Text.Trim() == "")
            {
                ret = false;
                txtcity.Background = color2;
            }
            txtemail.Background = color1;
            if (txtemail.Text.Trim() == "")
            {
                ret = false;
                txtemail.Background = color2;
            }
            txtphone.Background = color1;
            if (txtphone.Text.Trim() == "")
            {
                ret = false;
                txtphone.Background = color2;
            }
            txtinstitution.Background = color1;
            if (txtinstitution.Text.Trim() == "")
            {
                ret = false;
                txtinstitution.Background = color2;
            }
            txtcountry.Background = color1;
            if (txtcountry.Text.Trim() == "")
            {
                ret = false;
                txtcountry.Background = color2;
            }
            txtzip.Background = color1;
            if (txtzip.Text.Trim() == "")
            {
                ret = false;
                txtzip.Background = color2;
            }
            txtdepartment.Background = color1;
            if (txtdepartment.Text.Trim() == "")
            {
                ret = false;
                txtdepartment.Background = color2;
            }
            txtfunc.Background = color1;
            if (txtfunc.Text.Trim() == "")
            {
                ret = false;
                txtfunc.Background = color2;
            }
            txtcomment.Background = color1;
            if (txtcomment.Text.Trim() == "")
            {
                ret = false;
                txtcomment.Background = color2;
            }
            txtcompany.Background = color1;
            if (txtcompany.Text.Trim() == "")
            {
                ret = false;
                txtcompany.Background = color2;
            }


            return ret;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (check())
            {
                if (neu)
                {
                    SQLCommands c = new SQLCommands();
                    c.setContacts(userid, txtinstitution.Text, ((Salutations)cmbsalu.SelectedItem).id, ((IntranetTG.Objects.Title)cmbtitel.SelectedItem).id, txtfirstname.Text, txtlastname.Text, txtcompany.Text, txtdepartment.Text, txtstreet.Text, txtzip.Text, txtcountry.Text, txtcity.Text, txtphone.Text, txtemail.Text, txtcomment.Text, txtfunc.Text);
                    newone = true;
                    this.Close();
                }else
                {
                    SQLCommands c = new SQLCommands();
                    c.changeContacts2(userid, txtinstitution.Text, ((Salutations)cmbsalu.SelectedItem).id, ((IntranetTG.Objects.Title)cmbtitel.SelectedItem).id, txtfirstname.Text, txtlastname.Text, txtcompany.Text, txtdepartment.Text, txtstreet.Text, txtzip.Text, txtcountry.Text, txtcity.Text, txtphone.Text, txtemail.Text, txtcomment.Text, txtfunc.Text, selectedItem.id);
                    saved = true;
                    selectedItem.firstname = txtfirstname.Text;
                    selectedItem.lastname = txtlastname.Text;
                    selectedItem.street = txtstreet.Text;
                    selectedItem.city = txtcity.Text;
                    selectedItem.email = txtemail.Text;
                    selectedItem.phone1 = txtphone.Text;
                    selectedItem.institution = txtinstitution.Text;
                    selectedItem.country = txtcountry.Text;
                    selectedItem.zip = txtzip.Text;
                    selectedItem.department = txtdepartment.Text;
                    selectedItem.function = txtfunc.Text;
                    selectedItem.comment = txtcomment.Text;
                    selectedItem.company = txtcompany.Text;
                    selectedItem.title = cmbtitel.Text;
                    selectedItem.salutation = cmbsalu.Text;
                    this.Close();
                }
            }else
            {
                MessageBox.Show("Bitte geben Sie alle Werte ein!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void txtfunc_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
