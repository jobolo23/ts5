namespace TheraS5.Objects
{
    public class Contacts
    {
        public string id;


        public Contacts(string id, string salutation, string title, string firstname, string lastname,
            string institution, string company, string department, string country, string zip, string city,
            string street, string phone1, string phone2, string fax, string email, string www, string skype,
            string comment, string groups, string function)
        {
            this.id = id;
            this.salutation = salutation;
            this.title = title;
            this.firstname = firstname;
            this.lastname = lastname;
            this.institution = institution;
            this.company = company;
            this.department = department;
            this.country = country;
            this.zip = zip;
            this.city = city;
            this.street = street;
            this.phone1 = phone1;
            this.phone2 = phone2;
            this.fax = fax;
            this.email = email;
            this.www = www;
            this.skype = skype;
            this.comment = comment;
            this.groups = groups;
            this.function = function;

            Fullname = firstname + " " + lastname;
        }

        public string salutation { get; set; }
        public string title { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string institution { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string country { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string www { get; set; }
        public string skype { get; set; }
        public string comment { get; set; }
        public string groups { get; set; }
        public string function { get; set; }

        public string Fullname { get; set; }
    }
}