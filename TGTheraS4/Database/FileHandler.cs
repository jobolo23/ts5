using System;
using System.Collections.Generic;
using System.IO;
using IntranetTG.Objects;

namespace IntranetTG
{
    internal class FileHandler

        //##########################aal##################
    {
        //wohin damit?


        public static List<User> users;


        public static void writeUserInFile(User newUser)
        {
            try
            {
                users.Add(newUser);


                var usersInStrings = new string[users.Count];

                var i = 0;
                foreach (var user in users)
                {
                    usersInStrings[i] = user.Username + "," + user.Password;
                    i++;
                }

                WriteFile("users.tg", usersInStrings);
            }
            catch (Exception)
            {
                throw new Exception("No userdata in file");
            }
        }

        public static void WriteFile(string filename, string[] stringlines)
        {
            var file = new StreamWriter(filename);
            file.Write(stringlines);
            file.Close();
        }

        public static List<User> readUserFromFile()
        {
            users = new List<User>();
            try
            {
                var lines = File.ReadAllLines("users.tg");

                foreach (var line in lines)
                {
                    var subStrings = line.Split(',');
                    var userFromFile = new User();

                    userFromFile.Username = subStrings[0];
                    userFromFile.Password = subStrings[1];


                    var houses = subStrings[2].Split('$');

                    userFromFile.Services = houses;

                    if (subStrings[3] == "1")
                    {
                        userFromFile.IsAdmin = true;
                    }
                    else
                    {
                        userFromFile.IsAdmin = false;
                    }
                    users.Add(userFromFile);
                }
            }

            catch (Exception)
            {
                throw new Exception("No users read from file");
            }

            return users;
        }
    }

    //#########################eal#######################
}