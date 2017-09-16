using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TGTheraS4.Objects
{
    public class Art
    {
        public string aid { get; set; }
        public string name { get; set; }

        public Art(string aid, string name)
        {
            this.aid = aid;
            this.name = name;
        }
    }
}
