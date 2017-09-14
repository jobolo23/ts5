using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    class Taschengeld
    {
        public string name { get; set; }
        public string date { get; set; }
        public string in_ { get; set; }
        public string out_ { get; set; }
        public string state { get; set; }
        public string comment { get; set; }

        public Taschengeld(String name, String date, String in_, String out_, String state, String comment)
        {
            this.name = name;

            if (date == null || date == "")
            {
                this.date = "0000-00-00 00:00";
            }
            else
            {
                this.date = DateTime.Parse(date).ToString("yyyy-MM-dd HH:mm");
            }

            this.state = state;
            this.out_ = out_;
            this.in_ = in_;
            this.comment = comment;
        }
    }
}
