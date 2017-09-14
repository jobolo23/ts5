using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    public class MediAkt
    {
        public string date { get; set; }
        public string art { get; set; }
        public string desc { get; set; }

        public MediAkt(String date,String art, String desc)
        {
            SQLCommands c = new SQLCommands();
            this.date = DateTime.Parse(date).Date.ToString("yyyy-MM-dd"); ;
            this.art = art;
            this.desc = desc;
        }
    }
}
