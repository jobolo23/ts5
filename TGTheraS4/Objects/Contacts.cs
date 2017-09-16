using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TheraS5.Objects
{
    public class Contacts
    {
        public String id;
        public String salutation { get; set; }
        public String title { get; set; }
        public String firstname { get; set; }
        public String lastname { get; set; }
        public String institution { get; set; }
        public String company { get; set; }
        public String department { get; set; }
        public String country { get; set; }
        public String zip { get; set; }
        public String city { get; set; }
        public String street { get; set; }
        public String phone1 { get; set; }
        public String phone2 { get; set; }
        public String fax { get; set; }
        public String email { get; set; }
        public String www { get; set; }
        public String skype { get; set; }
        public String comment { get; set; }
        public String groups { get; set; }
        public String function { get; set; }

        public String Fullname { get; set; }



        public Contacts(String id, String salutation, String title, String firstname, String lastname, String institution, String company, String department, String country, String zip, String city, String street, String phone1, String phone2, String fax, String email, String www, String skype, String comment, String groups, String function)
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

            this.Fullname = firstname + " " + lastname;
        }
    }
}
