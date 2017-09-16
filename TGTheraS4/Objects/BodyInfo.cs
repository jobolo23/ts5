using System;

namespace TheraS5.Objects
{
    public class BodyInfo
    {
        public BodyInfo(string datum, string groeße, string gewicht)
        {
            this.datum = DateTime.Parse(datum).Date.ToString("yyyy-MM-dd");
            this.gewicht = gewicht;
            this.groeße = groeße;
        }

        public string datum { get; set; }
        public string groeße { get; set; }
        public string gewicht { get; set; }
    }
}