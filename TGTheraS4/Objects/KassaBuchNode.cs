using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TGTheraS4.Objects
{
    public class KassaBuchNode
    {
        public int id;
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
        public DateTime date;

        public KassaBuchNode(string id, string belnr, DateTime dat, string bez, string brutto, string steuers, string netto, string mwst, string knr, string name, string haus)
        {
            try
            {
                DateTime datum;
                float fbrutto;
                this.id = Int32.Parse(id);
                this.belnr = Int32.Parse(belnr);
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
                    this.eing = "";
                    this.ausg = (fbrutto * -1).ToString();
                }
                this.steuers = Int32.Parse(steuers);
                this.netto = float.Parse(netto);
                this.mwst = float.Parse(mwst);
                this.knr = Int32.Parse(knr);
                this.name = name;
                this.haus = haus;
                this.dat = datum.Day.ToString() + "." + datum.Month.ToString() + "." + datum.Year.ToString();
                this.date = datum;
            }
            catch 
            {
                /**/
                    /**/
            }
        }

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
