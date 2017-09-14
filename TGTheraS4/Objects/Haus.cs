using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TGTheraS4.Objects
{
    public class Haus
    {
        public int id { get; set; }
        public string name { get; set; }

        public Haus(string id, string name)
        {
            try
            {
                this.id = Int32.Parse(id);
                this.name = name;
            }
            catch 
            {
                /**/
                    /**/
            }
        }
    }
}
