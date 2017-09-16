namespace TheraS5.Objects
{
    public class NewestDokus
    {
        public NewestDokus(string name, string tag, string wg, string ersteller, string art, string created, string id)
        {
            this.name = name;
            this.tag = tag;
            this.wg = wg;
            this.ersteller = ersteller;
            this.art = art;
            this.created = created;
            this.id = id;
        }


        public string name { get; set; }
        public string tag { get; set; }
        public string wg { get; set; }
        public string ersteller { get; set; }
        public string art { get; set; }
        public string created { get; set; }
        public string id { get; set; }
    }
}