namespace TheraS5.Objects
{
    public class Service
    {
        public Service()
        {
        }

        public Service(string id, string name)
        {
            Id = id;
            this.name = name;
        }

        public string Id { get; set; }

        private string name { get; set; }

        public string Name
        {
            get => name;
            set => name = value;
        }
    }
}