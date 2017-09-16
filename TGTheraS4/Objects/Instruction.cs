using System;

namespace TheraS5.Objects
{
    public class Instruction
    {
        public Instruction(string title, string desc, string date, string uid)
        {
            //SQLCommands c = sql;
            this.date = DateTime.Parse(date).Date.ToString("yyyy-MM-dd");
            this.title = title;
            this.desc = desc;
            name = uid;
        }

        public string date { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string name { get; set; }
    }
}