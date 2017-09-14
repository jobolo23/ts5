using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace IntranetTG.Objects
{
    public class WorkingTime
    {
        public string art { get; set; }
        public string comment { get; set; }
        public DateTime datetimefrom { get; set; }
        public DateTime datetimeto { get; set; }
        public string datetimefrom2 { get; set; }
        public string datetimeto2 { get; set; }
        public string username { get; set; }
        public bool isverifed = false;
        public TimeSpan time;

        public WorkingTime(string art, string datetimefrom, string datetimeto, string comment)
        {
            this.art = art;
            this.datetimefrom = DateTime.Parse(datetimefrom);
            this.datetimeto = DateTime.Parse(datetimeto);
            this.comment = comment;

            time = this.datetimeto - this.datetimefrom;
        }

        public WorkingTime(string art, string datetimefrom, string datetimeto, string comment, bool isverifed)
        {
            this.art = art;
            this.datetimefrom = DateTime.Parse(datetimefrom);
            this.datetimeto = DateTime.Parse(datetimeto);
            this.comment = comment;
            this.isverifed = isverifed;

            time = this.datetimeto - this.datetimefrom;
        }

        public WorkingTime(string username, string art, string datetimefrom, string datetimeto, string comment, bool isverifed)
        {
            this.username = username;
            this.art = art;
            this.datetimefrom = DateTime.Parse(datetimefrom);
            this.datetimeto = DateTime.Parse(datetimeto);
            this.comment = comment;
            this.isverifed = isverifed;

            time = this.datetimeto - this.datetimefrom;
        }

        public WorkingTime(string username, string art, string datetimefrom, string datetimeto, string comment)
        {
            this.username = username;
            this.art = art;
            this.datetimefrom = DateTime.Parse(datetimefrom);
            this.datetimeto = DateTime.Parse(datetimeto);
            this.comment = comment;

            time = this.datetimeto - this.datetimefrom;
        }
    }
}
