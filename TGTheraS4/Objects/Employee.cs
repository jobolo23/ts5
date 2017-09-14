using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetTG.Objects
{
   public class Employee
    {
       //####################################Anfang AL#####################
       //Stammdaten objekt

        public Employee() { }


        public Employee(string firstname, string lastname)
        {
            firstName = firstname;
            lastName = lastname;        
        }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }


        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }


        private string fullName;

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        private DateTime start;

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        private DateTime lastLogin;

        public DateTime LastLogin
        {
            get { return lastLogin; }
            set { lastLogin = value; }
        }


       //#####################################Ende AL#######################

    }
}
