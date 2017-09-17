using System;
using DataModels;

namespace TheraS5.Objects
{
    internal class Medicaments
    {
        public Medicaments(string p, string p_2, string p_3, string p_4, string p_5, bool p_6, bool p_7, bool p_8,
            bool p_9, string id, string cmid, string created)
        {
            // TODO: Complete member initialization
            name = p;
            morning = p_2;
            midday = p_3;
            evening = p_4;
            night = p_5;
            morningConfirmed = p_6;
            middayConfirmed = p_7;
            eveningConfirmed = p_8;
            nightConfirmed = p_9;
            mediId = id;
            cmId = cmid;
            this.created = Convert.ToDateTime(created);
        }

        public Medicaments(string name, string morning, string midday, string evening, string night, string mediID,
            string cmID)
        {
            this.name = name;
            this.morning = morning;
            this.midday = midday;
            this.evening = evening;
            this.night = night;
            mediId = mediID;
            cmId = cmID;
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