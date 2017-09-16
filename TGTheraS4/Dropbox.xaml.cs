using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using mshtml;

namespace TheraS5
{
    /// <summary>
    ///     Interaktionslogik für Dropbox.xaml
    /// </summary>
    public partial class Dropbox : Window
    {
        public string email;

        public string firstname;
        public string lastname;
        public string pass = "TherapeutischeGemeinschaften";

        public Dropbox(string firstname, string lastname, string email)
        {
            InitializeComponent();
            this.firstname = firstname;
            this.lastname = lastname;
            this.email = email;

            var uri = new Uri("https://www.dropbox.com/");

            WebBrowserCreateDropbox.Navigate(uri, null, null,
                "User-Agent: Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/7.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; eSobiSubscriber 1.0.0.40; .NET4.0E; .NET4.0C)");
        }

        private void WebBrowserCreateDropbox_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                if (WebBrowserCreateDropbox.Source.ToString() == "https://www.dropbox.com/")
                {
                    var doc = (HTMLDocument) WebBrowserCreateDropbox.Document;
                    doc.getElementsByName("fname").item(1).SetAttribute("value", firstname);
                    doc.getElementsByName("lname").item(1).SetAttribute("value", lastname);
                    doc.getElementsByName("email").item(1).SetAttribute("value", email);
                    doc.getElementsByName("password").item(1).SetAttribute("value", pass);
                    var count = 0;
                    foreach (HTMLFormElement form in doc.forms)
                    {
                        if (count == 4)
                        {
                            var children = form as IEnumerable;
                            var inputs = children.OfType<HTMLInputElement>();
                            var submitButton = inputs.First(i => i.type == "checkbox");
                            foreach (var submitButton2 in inputs)
                            {
                                if (submitButton2.type == "checkbox")
                                {
                                    submitButton = submitButton2;
                                    submitButton.click();
                                    MessageBox.Show(
                                        "Bitte überprüfen Sie, ob der Dropbox-Account erstellt wurde!\nSollte der Account nicht automatisch erstellt worden sein, erstellen Sie ihn bitte Automatisch.",
                                        "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            break;
                        }
                        count++;
                    }
                    count = 0;
                    foreach (HTMLFormElement form in doc.forms)
                    {
                        if (count == 4)
                        {
                            var children = form as IEnumerable;
                            var inputs = children.OfType<HTMLButtonElement>();
                            var submitButton = inputs.First(i => i.type == "submit");
                            foreach (var submitButton2 in inputs)
                            {
                                if (submitButton2.type == "submit")
                                {
                                    submitButton = submitButton2;
                                    submitButton.click();
                                }
                            }
                            break;
                        }
                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}