using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheraS5.Objects
{
    public class Document
    {
        public int client_id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public int createuser_id { get; set; }
        public string createuser { get; set; }
        public int lastuser_id { get; set; }
        public string lastuser { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public int filesize { get; set; }
    }
}
