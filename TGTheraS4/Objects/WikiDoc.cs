using System;
using DataModels;
using IntranetTG;
using TheraS5.Database.Objects;

namespace TheraS5.Objects
{
    public class WikiDoc
    {
        public int client_id;
        public int createuser_id;
        public DateTime Erstellt;
        public int filesize;
        public int lastuser_id;
        public string path;
        public DateTime Verändert;

        public WikiDoc(Wiki wiki)
        {
            var c = new SQLCommands(new MySqlConnectionInformation("0", "0", "0", "0", "0"));
            client_id = 0;
            Erstellt = (DateTime) wiki.Created;
            Verändert = (DateTime) wiki.Modified;
            createuser_id = (int) wiki.CreateuserId;
            Ersteller = c.getNameByID(createuser_id.ToString(), false);
            if (wiki.LastuserId == null)
            {
                lastuser_id = -1;
                Verändert_von = "keine Angabe";
            }
            else
            {
                lastuser_id = (int) wiki.LastuserId;
                Verändert_von = c.getNameByID(lastuser_id.ToString(), false);
            }
            Name = wiki.Title;
            path = wiki.Path;
            Bewertung = "Keine Bewertungen";
            filesize = 0;
        }

        public string Erstellung { get; set; }
        public string Veränderung { get; set; }
        public string Ersteller { get; set; }
        public string Verändert_von { get; set; }
        public string Name { get; set; }
        public string Bewertung { get; set; }
    }
}