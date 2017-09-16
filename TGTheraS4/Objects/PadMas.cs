using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TheraS5.Objects
{
    class PadMas
    {
        public string created { get; set; }
        public string from { get; set; }
        public string mas { get; set; }
        public string stat { get; set; }
        public string datVon { get; set; }
        public string datBis { get; set; }

        public PadMas(String created, String from, String mas, String stat, String datVon, String datBis)
        {
            this.created = created;
            this.from = from;
            this.mas = mas;
            this.stat = stat;
            this.datVon = datVon;
            this.datBis = datBis;
        }
    }
}
