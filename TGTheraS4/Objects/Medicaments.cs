using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace TGTheraS4.Objects
{
    class Medicaments
    {

        public Medicaments(string p, string p_2, string p_3, string p_4, string p_5, bool p_6, bool p_7, bool p_8, bool p_9, string id,string cmid, string created)
        {
            // TODO: Complete member initialization
            this.name = p;
            this.morning = p_2;
            this.midday = p_3;
            this.evening = p_4;
            this.night = p_5;
            this.morningConfirmed = p_6;
            this.middayConfirmed = p_7;
            this.eveningConfirmed = p_8;
            this.nightConfirmed = p_9;
            this.mediId = id;
            this.cmId = cmid;
            this.created = Convert.ToDateTime(created);
        }

        public Medicaments(string name, string morning, string midday, string evening, string night, string mediID, string cmID)
        {
            this.name = name;
            this.morning = morning;
            this.midday = midday;
            this.evening = evening;
            this.night = night;
            this.mediId = mediID;
            this.cmId = cmID; 
        }





        public string name { get; set; }
        public string morning { get; set; }
        public string midday { get; set; }
        public string evening { get; set; }
        public string night { get; set; }
        public string mediId { get; set; }
        public string cmId { get; set; }
        public bool morningConfirmed { get; set; }
        public bool middayConfirmed { get; set; }
        public bool eveningConfirmed { get; set; }
        public bool nightConfirmed { get; set; }
        public DateTime created { get; set; }

    }
}
