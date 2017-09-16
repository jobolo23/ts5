namespace TheraS5.Objects
{
    public class Art
    {
        public Art(string aid, string name)
        {
            this.aid = aid;
            this.name = name;
        }

        public string aid { get; set; }
        public string name { get; set; }
    }
}