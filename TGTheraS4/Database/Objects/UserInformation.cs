using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGTheraS4.Database.Objects {
    public class UserInformation {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
        public string UserId { get; set; }
        public string Privileges { get; set; }

        public UserInformation (string username, string password, string applicationId, string userId, string privileges)
        {
            this.Username = username;
            this.Password = password;
            this.ApplicationId = applicationId;
            this.UserId = userId;
            this.Privileges = privileges;
        }
    }
}
