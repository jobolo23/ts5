using System.Net;
using Newtonsoft.Json;
using TGTheraS4.Database.Objects;

namespace TGTheraS4.Services {
    public class GeneralService
    {
        public string ApplicationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        private const string GeneralServiceUri = "https://thera-s5.appspot.com/GeneralService";

        public GeneralService(string applicationId, string userName, string password)
        {
            this.ApplicationId = applicationId;
            this.UserName = userName;
            this.Password = password;
        }

        public bool NetworkCheck()
        {
            var wc = new WebClient();
            var json = wc.DownloadString($"{GeneralServiceUri}/NetworkCheck");
            dynamic jsonDe = JsonConvert.DeserializeObject(json);
            return jsonDe.NetworkCheck;
        }

        public MySqlConnectionInformation GetMySqlConnectionInformation()
        {
            var wc = new WebClient();
            var json = wc.DownloadString($"{GeneralServiceUri}/GetMySqlConnectionInformation?a={this.ApplicationId}&u={this.UserName}&t={this.Password}");
            return JsonConvert.DeserializeObject<MySqlConnectionInformation>(json);
        }

        public UserInformation GetUserInformation ()
        {
            var wc = new WebClient();
            var json = wc.DownloadString($"{GeneralServiceUri}/GetUserInformation?a={this.ApplicationId}&u={this.UserName}&t={this.Password}");
            return JsonConvert.DeserializeObject<UserInformation>(json);
        }
    }
}
