using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    public class BodyInfo
    {
        public string datum { get; set; }
        public string groeße { get; set; }
        public string gewicht { get; set; }

        public BodyInfo(String datum, String groeße, String gewicht)
        {
            SQLCommands c = new SQLCommands();
            this.datum = DateTime.Parse(datum).Date.ToString("yyyy-MM-dd");
            this.gewicht = gewicht;
            this.groeße = groeße;
        }
    }
}
