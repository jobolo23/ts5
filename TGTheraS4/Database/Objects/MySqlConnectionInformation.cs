namespace TheraS5.Database.Objects
{
    public class MySqlConnectionInformation
    {
        public MySqlConnectionInformation(string host, string port, string username, string password, string database)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
            Database = database;
        }

        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }
}