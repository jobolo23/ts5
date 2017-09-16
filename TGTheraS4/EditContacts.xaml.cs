using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using IntranetTG;
using TheraS5.Objects;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für EditContacts.xaml
    /// </summary>
    public partial class EditContacts : Window
    {
        public SQLCommands c;
        private readonly SolidColorBrush color1 = Brushes.White;
        private readonly SolidColorBrush color2 = new SolidColorBrush(Color.FromArgb(0xAA, 0xFF, 0x17, 0x0E));
        public bool neu = true;
        public bool newone;
        private readonly List<Salutations> sal = new List<Salutations>();
        public bool saved;
        public Contacts selectedItem;
        private readonly List<IntranetTG.Objects.Title> tit = new List<IntranetTG.Objects.Title>();
        public string userid = "";

        public EditContacts(string userid, SQLCommands sql)
        {
            InitializeComponent();
            c = sql;
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

        public EditContacts(string userid, Contacts selectedItem, SQLCommands sql) : this(userid, sql)
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
            foreach (var t in tit)
            {
                if (t.Name == selectedItem.title)
                {
                    cmbtitel.SelectedItem = t;
                    break;
                }
            }

            foreach (var t in sal)
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
            Close();
        }

        public bool check()
        {
            var ret = true;

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
                    c.setContacts(userid, txtinstitution.Text, ((Salutations) cmbsalu.SelectedItem).id,
                        ((IntranetTG.Objects.Title) cmbtitel.SelectedItem).id, txtfirstname.Text, txtlastname.Text,
                        txtcompany.Text, txtdepartment.Text, txtstreet.Text, txtzip.Text, txtcountry.Text, txtcity.Text,
                        txtphone.Text, txtemail.Text, txtcomment.Text, txtfunc.Text);
                    newone = true;
                    Close();
                }
                else
                {
                    c.changeContacts2(userid, txtinstitution.Text, ((Salutations) cmbsalu.SelectedItem).id,
                        ((IntranetTG.Objects.Title) cmbtitel.SelectedItem).id, txtfirstname.Text, txtlastname.Text,
                        txtcompany.Text, txtdepartment.Text, txtstreet.Text, txtzip.Text, txtcountry.Text, txtcity.Text,
                        txtphone.Text, txtemail.Text, txtcomment.Text, txtfunc.Text, selectedItem.id);
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
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Bitte geben Sie alle Werte ein!", "Fehler", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void txtfunc_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}