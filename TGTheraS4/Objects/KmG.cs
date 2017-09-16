using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheraS5.Objects
{
    public class KmG
    {
        //Kennzeichen,Ortvon,Ortbis,Zeitvon,Zeitbis,Summe,km
        public string Verfasser { get; set; }
        public string Kennzeichen { get; set; }
        public string km { get; set; }
        public string Ortvon { get; set; }
        public string Ortbis { get; set; }
        public string Zeitvon { get; set; }
        public string Zeitbis { get; set; }
        public string Summe { get; set; }

        public KmG(string Verfasser, string Kennzeichen, string km, string Ortvon, string Ortbis, string Zeitvon, string Zeitbis, string Summe)
        {
            this.Verfasser = Verfasser;
            this.Kennzeichen = Kennzeichen;
            this.km = km;
            this.Ortvon = Ortvon;
            this.Ortbis = Ortbis;
            this.Zeitvon = Zeitvon;
            this.Zeitbis = Zeitbis;
            this.Summe = Summe;
        }
    }
}
