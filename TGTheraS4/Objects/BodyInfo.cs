using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TheraS5.Objects
{
    public class BodyInfo
    {
        public string datum { get; set; }
        public string groeße { get; set; }
        public string gewicht { get; set; }

        public BodyInfo(String datum, String groeße, String gewicht)
        {
            this.datum = DateTime.Parse(datum).Date.ToString("yyyy-MM-dd");
            this.gewicht = gewicht;
            this.groeße = groeße;
        }
    }
}
