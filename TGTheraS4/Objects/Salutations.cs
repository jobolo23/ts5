using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheraS5.Objects
{
    public class Salutations
    {
        public string id;
        public string Name { get; set; }
        public string anrede;

        public Salutations (string id, string name, string anrede)
        {
            this.id = id;
            this.Name = name;
            this.anrede = anrede;
        }
    }
}
