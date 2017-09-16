namespace TheraS5.Database.Objects
{
    public class UserInformation
    {
        public UserInformation(string username, string password, string applicationId, string userId, string privileges)
        {
            Username = username;
            Password = password;
            ApplicationId = applicationId;
            UserId = userId;
            Privileges = privileges;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
        public string UserId { get; set; }
        public string Privileges { get; set; }
    }
}