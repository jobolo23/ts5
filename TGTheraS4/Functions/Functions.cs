using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using IntranetTG.Objects;
using TheraS5.Objects;

namespace IntranetTG.Functions
{
    public static class Functions
    {
        //#############################Anfang AL ################

        public static bool checkUser(User user)
        {
            var nameOk = false;

            //userList = FileHandler.readUserFromFile(); <---- (users.tg) nur wenn keine DB

            //user aus liste suchen nach name
            /*foreach (User userlocal in userList)
            {
                if (user.Username == userlocal.Username)
                {*/
            //name ist richtig
            nameOk = true;

            // wenn existiert dann passwörter vergleichen
            ActualUser = user;
            ActualUserFromList = user;

            if (!checkPassword())
            {
                var ex = new Exception("Passwort falsch.");
                MessageBox.Show("Passwort falsch");

                nameOk = false;
            }

            /* }

             i++;
         }*/
            return nameOk;
        }

        public static bool checkPassword()
        {
            var isCorrect = false;
            var source = ActualUser.Password;
            using (var md5Hash = MD5.Create())
            {
                var hash = GetMd5Hash(md5Hash, source);

                if ( /*VerifyMd5Hash(md5Hash, source, actualUserFromList.DbPwdHash)*/true)
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
                var old = date.Split('.');
                var converted = old[2] + "-" + old[1] + "-" + old[0];
                return converted;
            }
            return "";
        }

        private static byte[] StringToByteArray(string str)
        {
            var enc = new ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private static string ByteArrayToString(byte[] arr)
        {
            var enc = new ASCIIEncoding();
            return enc.GetString(arr);
        }


        internal static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        internal static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input. 
            var hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            return false;
        }
        //##############################Ende AL######################################

        #region Properties

        public static User ActualUserFromList { get; set; }

        public static IList<User> UserList { get; set; }

        public static List<Employee> EmployeeList { get; set; }

        public static List<Project> ProjectList { get; set; }

        public static List<Service> ServiceList { get; set; }

        public static Enumerations.EditDialog EditNumberEditDialog { get; set; }


        public static Enumerations.InstructionTask AppointmentTask { get; set; }

        public static SQLCommands Connection { get; set; }

        public static string[] DatabaseString { get; set; }


        public static User ActualUser { get; set; }

        public static void fun()
        {
            Console.Beep(587, 250); //D
            Console.Beep(587, 250);
            Console.Beep(440, 250); //A
            Console.Beep(523, 250); //C
            Console.Beep(587, 500);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(659, 250); //E
            Console.Beep(698, 500); //F
            Console.Beep(698, 250);
            Thread.Sleep(250);
            Console.Beep(698, 250);
            Console.Beep(783, 250); //G
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
            Console.Beep(440, 250); //A
            Console.Beep(523, 250); //C
            Console.Beep(587, 500);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(698, 500);
            Console.Beep(783, 250);
            Console.Beep(783, 250);
            Thread.Sleep(250);
            Console.Beep(783, 250);
            Console.Beep(880, 250); //A hoch
            Console.Beep(932, 250);
            Console.Beep(932, 500);
            Thread.Sleep(250);
            Console.Beep(880, 250);
            Console.Beep(783, 250);
            Console.Beep(880, 250);
            Console.Beep(587, 250);
            Thread.Sleep(250);
            Console.Beep(587, 250);
            Console.Beep(659, 250); //E
            Console.Beep(698, 500); //F
            Console.Beep(698, 250);
            Console.Beep(783, 250); //G
            Console.Beep(880, 250); //A hoch
            Console.Beep(587, 500); //D
            Thread.Sleep(250);
            Console.Beep(587, 250); //D
            Console.Beep(698, 250); //F
            Console.Beep(659, 250); //E
            Thread.Sleep(250);
            Console.Beep(659, 500); //E
            Console.Beep(698, 250); //F
            Console.Beep(587, 250); //D
            Console.Beep(659, 250); //E
        }

        #endregion
    }
}