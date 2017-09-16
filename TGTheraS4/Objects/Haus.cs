namespace TheraS5.Objects
{
    public class Haus
    {
        public Haus(string id, string name)
        {
            try
            {
                this.id = int.Parse(id);
                this.name = name;
            }
            catch
            {
                /**/
                /**/
            }
        }

        public int id { get; set; }
        public string name { get; set; }
    }
}