using System;

namespace TheraS5.Objects
{
    public class Task
    {
        public Task(string von, string zu, string startdate, string enddate, string desc)
        {
            //SQLCommands c = sql;
            this.von = von;
            this.zu = zu;
            this.startdate = DateTime.Parse(startdate).Date.ToString("yyyy-MM-dd");
            this.enddate = DateTime.Parse(enddate).Date.ToString("yyyy-MM-dd");
            this.desc = desc;
        }


        public string von { get; set; }
        public string zu { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string desc { get; set; }
    }
}