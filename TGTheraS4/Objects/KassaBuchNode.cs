using System;

namespace TheraS5.Objects
{
    public class KassaBuchNode
    {
        public DateTime date;
        public int id;

        public KassaBuchNode(string id, string belnr, DateTime dat, string bez, string brutto, string steuers,
            string netto, string mwst, string knr, string name, string haus)
        {
            try
            {
                DateTime datum;
                float fbrutto;
                this.id = int.Parse(id);
                this.belnr = int.Parse(belnr);
                datum = dat;
                this.bez = bez;
                fbrutto = float.Parse(brutto);
                if (fbrutto > 0)
                {
                    eing = fbrutto.ToString();
                    ausg = "";
                }
                else
                {
                    eing = "";
                    ausg = (fbrutto * -1).ToString();
                }
                this.steuers = int.Parse(steuers);
                this.netto = float.Parse(netto);
                this.mwst = float.Parse(mwst);
                this.knr = int.Parse(knr);
                this.name = name;
                this.haus = haus;
                this.dat = datum.Day + "." + datum.Month + "." + datum.Year;
                date = datum;
            }
            catch
            {
                /**/
                /**/
            }
        }

        public int belnr { get; set; }
        public string dat { get; set; }
        public string bez { get; set; }
        public string eing { get; set; }
        public string ausg { get; set; }
        public int steuers { get; set; }
        public float netto { get; set; }
        public float mwst { get; set; }
        public float kassst { get; set; }
        public int knr { get; set; }
        public string name { get; set; }
        public string haus { get; set; }

        public void addKassst(string kassst)
        {
            try
            {
                this.kassst = float.Parse(kassst);
            }
            catch
            {
            }
        }
    }
}