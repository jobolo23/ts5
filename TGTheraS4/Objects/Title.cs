using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetTG.Objects
{
    public class Title 
    {
        public string id;
        public string Name { get; set; }
        
        public Title (string id, string name)
        {
            this.id = id;
            this.Name = name;
        }
    }
}

