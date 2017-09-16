namespace TheraS5.Objects
{
    public class KontoNr
    {
        public KontoNr(string id, string knr, string desc)
        {
            try
            {
                this.id = int.Parse(id);
                this.knr = int.Parse(knr);
                this.desc = desc;
                ges = knr + " " + desc;
            }
            catch
            {
                /**/
                /**/
            }
        }

        public int id { get; set; }
        public int knr { get; set; }
        public string desc { get; set; }
        public string ges { get; set; }


        public bool Equals(KontoNr other)
        {
            return other.knr == knr && other.desc == desc;
        }
    }
}