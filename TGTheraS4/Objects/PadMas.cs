namespace TheraS5.Objects
{
    internal class PadMas
    {
        public PadMas(string created, string from, string mas, string stat, string datVon, string datBis)
        {
            this.created = created;
            this.from = from;
            this.mas = mas;
            this.stat = stat;
            this.datVon = datVon;
            this.datBis = datBis;
        }

        public string created { get; set; }
        public string from { get; set; }
        public string mas { get; set; }
        public string stat { get; set; }
        public string datVon { get; set; }
        public string datBis { get; set; }
    }
}