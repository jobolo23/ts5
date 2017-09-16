namespace IntranetTG.Objects
{
    public class Title
    {
        public string id;

        public Title(string id, string name)
        {
            this.id = id;
            Name = name;
        }

        public string Name { get; set; }
    }
}