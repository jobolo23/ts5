using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetTG.Objects
{
   public class User
    {

        public User() { }

        public User(string userName, string userPassword,string dbPwdHash) {

            username = userName;
            password = userPassword;
            this.dbPwdHash = dbPwdHash;
        
        }

        public User(string userName, string firstname, string lastname, int id)
        {
            this.kostalid = id;
            this.username = userName;
            this.firstname = firstname;
            this.lastname = lastname;

        }

        private int kostalid;
        public int Kostalid
        {
            get { return kostalid; }
            set { kostalid = value; }
        }

        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        private string lastname;
        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

       private string dbPwdHash;

        public string DbPwdHash
        {
            get { return dbPwdHash; }
            set { dbPwdHash = value; }
        }

        
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }



        private string[] services;

        public string [] Services
        {
            get { return services; }
            set { services = value; }
        }

        private bool isAdmin;
       
        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

        private int wasArbeitetDiesesHoffentlichGeistigNochFitteMenschlicheIndividuum; //1 = Verwaltung; 2 = Soz. Päd. 3 = beides

        public int WasArbeitetDiesesHoffentlichGeistigNochFitteMenschlicheIndividuum
        {
            get { return wasArbeitetDiesesHoffentlichGeistigNochFitteMenschlicheIndividuum; }
            set { wasArbeitetDiesesHoffentlichGeistigNochFitteMenschlicheIndividuum = value; }
        }
    }
}
