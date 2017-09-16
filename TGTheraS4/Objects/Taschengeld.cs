using System;

namespace TheraS5.Objects
{
    internal class Taschengeld
    {
        public Taschengeld(string name, string date, string in_, string out_, string state, string comment)
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

        public string name { get; set; }
        public string date { get; set; }
        public string in_ { get; set; }
        public string out_ { get; set; }
        public string state { get; set; }
        public string comment { get; set; }
    }
}