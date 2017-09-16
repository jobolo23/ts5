namespace IntranetTG.Objects
{
    public class User
    {
        public User()
        {
        }

        public User(string userName, string userPassword, string dbPwdHash)
        {
            Username = userName;
            Password = userPassword;
            DbPwdHash = dbPwdHash;
        }

        public User(string userName, string firstname, string lastname, int id)
        {
            Kostalid = id;
            Username = userName;
            Firstname = firstname;
            Lastname = lastname;
        }

        public int Kostalid { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Id { get; set; }

        public string DbPwdHash { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string[] Services { get; set; }

        public bool IsAdmin { get; set; }

        public int WorkingTimeType { get; set; }
    }
}