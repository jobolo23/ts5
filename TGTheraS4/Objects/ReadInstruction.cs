namespace TheraS5.Objects
{
    public class ReadInstruction
    {
        public ReadInstruction(string firstname, string lastname, string read)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.read = read;
        }

        public string lastname { get; set; }
        public string firstname { get; set; }
        public string read { get; set; }
    }
}