using System.Net;
using Newtonsoft.Json;
using TheraS5.Database.Objects;

namespace TheraS5.Services
{
    public class GeneralService
    {
        private const string GeneralServiceUri = "https://thera-s5.appspot.com/GeneralService";

        public GeneralService(string applicationId, string userName, string password)
        {
            ApplicationId = applicationId;
            UserName = userName;
            Password = password;
        }

        public string ApplicationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

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
            var json = wc.DownloadString(
                $"{GeneralServiceUri}/GetMySqlConnectionInformation?a={ApplicationId}&u={UserName}&t={Password}");
            return JsonConvert.DeserializeObject<MySqlConnectionInformation>(json);
        }

        public UserInformation GetUserInformation()
        {
            var wc = new WebClient();
            var json = wc.DownloadString(
                $"{GeneralServiceUri}/GetUserInformation?a={ApplicationId}&u={UserName}&t={Password}");
            return JsonConvert.DeserializeObject<UserInformation>(json);
        }
    }
}