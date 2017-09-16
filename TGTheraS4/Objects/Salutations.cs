namespace TheraS5.Objects
{
    public class Salutations
    {
        public string anrede;
        public string id;

        public Salutations(string id, string name, string anrede)
        {
            this.id = id;
            Name = name;
            this.anrede = anrede;
        }

        public string Name { get; set; }
    }
}