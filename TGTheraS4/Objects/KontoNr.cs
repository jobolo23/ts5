using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TheraS5.Objects
{
    public class KontoNr
    {
        public int id { get; set; }
        public int knr { get; set; }
        public string desc { get; set; }
        public string ges { get; set; }

        public KontoNr(string id, string knr, string desc)
        {
            try
            {
                this.id = Int32.Parse(id);
                this.knr = Int32.Parse(knr);
                this.desc = desc;
                this.ges = knr + " " + desc;
            }
            catch 
            {
                /**/
                    /**/
            }
        }


        public bool Equals(KontoNr other)
        {
            return (other.knr == this.knr && other.desc == this.desc);
        }
    }
}
