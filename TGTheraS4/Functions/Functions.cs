using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG.Objects;
using System.Security.Cryptography;
using System.Windows;
using TGTheraS4.Objects;
using System.Threading;

namespace IntranetTG.Functions
{

    public static class Functions
    {

        //#############################Anfang AL ################

        public static bool checkUser(User user)
        {
            bool nameOk = false;

            //userList = FileHandler.readUserFromFile(); <---- (users.tg) nur wenn keine DB

            //user aus liste suchen nach name
            /*foreach (User userlocal in userList)
            {
                if (user.Username == userlocal.Username)
                {*/
            //name ist richtig
            nameOk = true;

            // wenn existiert dann passwörter vergleichen
            actualUser = user;
            actualUserFromList = user;

            if (!checkPassword())
            {
                Exception ex = new Exception("Passwort falsch.");
                MessageBox.Show("Passwort falsch");

                nameOk = false;


            }
            else
            {

            }

            /* }

             i++;
         }*/
            return nameOk;

        }

        public static bool checkPassword()
        {
            bool isCorrect = false;
            string source = actualUser.Password;
            using (MD5 md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);

                if (/*VerifyMd5Hash(md5Hash, source, actualUserFromList.DbPwdHash)*/true)
                {
                    isCorrect = true;
                }
                else
                {
                    isCorrect = false;
                }
            }
            return isCorrect;
        }
        public static string DateConverter(string date)
        {
            if (date.Trim() != "")
            {
                string[] old = date.Split('.');
                string converted = old[2] + "-" + old[1] + "-" + old[0];
                return converted;
            }
            return "";
        }

        private static byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private static string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }


        internal static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        internal static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Properties


        private static User actualUserFromList;

        public static User ActualUserFromList
        {
            get { return actualUserFromList; }
            set { actualUserFromList = value; }
        }

        private static IList<User> userList;

        public static IList<User> UserList
        {
            get { return userList; }
            set { userList = value; }
        }

        private static List<Employee> employeeList;

        public static List<Employee> EmployeeList
        {
            get { return employeeList; }
            set { employeeList = value; }
        }

        private static List<Project> projectList;

        public static List<Project> ProjectList
        {
            get { return projectList; }
            set { projectList = value; }
        }

        private static List<Service> serviceList;

        public static List<Service> ServiceList
        {
            get { return serviceList; }
            set { serviceList = value; }
        }

        private static Enumerations.EditDialog editNumberEditDialog;

        public static Enumerations.EditDialog EditNumberEditDialog
        {
            get { return editNumberEditDialog; }
            set { editNumberEditDialog = value; }
        }


        private static Enumerations.InstructionTask appointmentTask;

        public static Enumerations.InstructionTask AppointmentTask
        {
            get { return appointmentTask; }
            set { appointmentTask = value; }
        }

        private static SQLCommands connection;

        public static SQLCommands Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        private static string[] databaseString;

        public static string[] DatabaseString
        {
            get { return databaseString; }
            set { databaseString = value; }
        }


        private static User actualUser;

        public static User ActualUser
        {
            get { return actualUser; }
            set { actualUser = value; }
        }

        public static void fun()
        {
            Console.Beep(587, 250);//D
            Console.Beep(587, 250);
            Console.Beep(440, 250);//A
            Console.Beep(523, 250);//C
            Console.Beep(587, 500);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(659, 250);//E
            Console.Beep(698, 500);//F
            Console.Beep(698, 250);
            Thread.Sleep(250);
            Console.Beep(698, 250);
            Console.Beep(783, 250);//G
            Console.Beep(659, 500);
            Console.Beep(659, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(523, 250);
            Console.Beep(523, 250);
            Console.Beep(587, 250);
            Thread.Sleep(500);
            Console.Beep(440, 250);
            Console.Beep(523, 250);
            Console.Beep(587, 500);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(659, 250);
            Console.Beep(698, 250);
            Console.Beep(698, 250);
            Thread.Sleep(250);
            Console.Beep(698, 250);
            Console.Beep(783, 250);
            Console.Beep(659, 250);
            Thread.Sleep(250);
            Console.Beep(659, 250);
            Console.Beep(587, 250);
            Console.Beep(523, 250);
            Console.Beep(587, 250);
            Thread.Sleep(500);
            Console.Beep(440, 250);//A
            Console.Beep(523, 250);//C
            Console.Beep(587, 500);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(698, 500);
            Console.Beep(783, 250);
            Console.Beep(783, 250);
            Thread.Sleep(250);
            Console.Beep(783, 250);
            Console.Beep(880, 250);//A hoch
            Console.Beep(932, 250);
            Console.Beep(932, 500);
            Thread.Sleep(250);
            Console.Beep(880, 250);
            Console.Beep(783, 250);
            Console.Beep(880, 250);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(659, 250);//E
            Console.Beep(698, 500);//F
            Console.Beep(698, 250);
            Console.Beep(783, 250);//G
            Console.Beep(880, 250);//A hoch
            Console.Beep(587, 500);//D
            Thread.Sleep(250);
            Console.Beep(587, 250);//D
            Console.Beep(698, 250);//F
            Console.Beep(659, 250);//E
            Thread.Sleep(250);
            Console.Beep(659, 500);//E
            Console.Beep(698, 250);//F
            Console.Beep(587, 250);//D
            Console.Beep(659, 250);//E

        }

        #endregion
        //##############################Ende AL######################################


    }
}
