using System;

namespace TheraS5.Objects
{
    public class MediAkt
    {
        public MediAkt(string date, string art, string desc)
        {
            this.date = DateTime.Parse(date).Date.ToString("yyyy-MM-dd");
            ;
            this.art = art;
            this.desc = desc;
        }

        public string date { get; set; }
        public string art { get; set; }
        public string desc { get; set; }
    }
}