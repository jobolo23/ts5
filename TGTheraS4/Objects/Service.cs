using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGTheraS4.Objects
{
   public class Service
    {
       private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Service()
        {

        }

        public Service(String id, String name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
