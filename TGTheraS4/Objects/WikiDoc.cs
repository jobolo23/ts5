using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheraS5.Objects
{
    public class WikiDoc
    {
        public int client_id;
        public DateTime Erstellt ;
        public string Erstellung { get; set; }
        public DateTime Verändert;
        public string Veränderung { get; set; }
        public int createuser_id;
        public string Ersteller { get; set; }
        public int lastuser_id;
        public string Verändert_von { get; set; }
        public string Name { get; set; }
        public string Bewertung { get; set; }
        public string path;
        public int filesize;
    }
}
