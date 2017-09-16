using System;

namespace IntranetTG.Objects
{
    public class Employee
    {
        //####################################Anfang AL#####################
        //Stammdaten objekt

        public Employee()
        {
        }


        public Employee(string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public User User { get; set; }

        public DateTime Start { get; set; }

        public DateTime LastLogin { get; set; }


        //#####################################Ende AL#######################
    }
}