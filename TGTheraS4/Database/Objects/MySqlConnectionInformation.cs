namespace TGTheraS4.Database.Objects {
    public class MySqlConnectionInformation {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }

        public MySqlConnectionInformation (string host, string port, string username, string password, string database)
        {
            this.Host = host;
            this.Port = port;
            this.Username = username;
            this.Password = password;
            this.Database = database;
        }
    }
}
