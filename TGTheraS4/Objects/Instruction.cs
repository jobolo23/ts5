using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    public class Instruction
    {
        public string date { get; set; }
        public string title  { get; set; }
        public string desc { get; set; }
        public string name {get; set;}



        public Instruction(String title, String desc , String date, String uid, SQLCommands sql)
        {
            SQLCommands c = sql;
            this.date = DateTime.Parse(date).Date.ToString("yyyy-MM-dd");
            this.title = title;
            this.desc = desc;
            this.name = c.getNameByID(uid);
        }

    }
}
