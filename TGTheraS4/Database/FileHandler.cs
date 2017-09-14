using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG.Objects;
using System.IO;
using System.Windows;

namespace IntranetTG
{
    class FileHandler

        //##########################aal##################
    {
        //wohin damit?

        
        

        public static List<User> users = null;


        public FileHandler()
        {
            
        }

        public static void writeUserInFile(User newUser) {
            try
            {
                users.Add(newUser);


                String[] usersInStrings = new String[users.Count];
                
                int i = 0;
                foreach (User user in users)
                {
                    usersInStrings[i] = (user.Username + "," + user.Password); 
                        i++;
                }

                WriteFile("users.tg", usersInStrings);

   }
            catch (Exception)
            {
                
                throw new Exception("No userdata in file");
            }
        }

        public static void WriteFile(String filename, String[] stringlines)
        {
            StreamWriter file = new StreamWriter(filename);
            file.Write(stringlines);
            file.Close();
        }

        public static List<User> readUserFromFile() {

            users = new List<User>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines("users.tg");

                foreach (string line in lines)
                {

                    String[] subStrings = line.Split(new char[] { ',' });
                    User userFromFile = new User();

                    userFromFile.Username = subStrings[0];                   
                    userFromFile.Password = subStrings[1];
                    

                    string[] houses = subStrings[2].Split('$');

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
