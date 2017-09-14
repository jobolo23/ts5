﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    public class Task
    {
        

        public string von { get; set; }
        public string zu { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string desc { get; set; }


        public Task(String von, String zu, String startdate, String enddate, String desc)
        {
            SQLCommands c = new SQLCommands();
            this.von = c.getNameByID(von);
            this.zu = c.getNameByID(zu);
            this.startdate = DateTime.Parse(startdate).Date.ToString("yyyy-MM-dd");
            this.enddate = DateTime.Parse(enddate).Date.ToString("yyyy-MM-dd");
            this.desc = desc;
        }
    }
}
