using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModels;
using IntranetTG;
using TGTheraS4.Database.Objects;

namespace TheraS5.Objects
{
    public class WikiDoc
    {
        public int client_id;
        public DateTime Erstellt ;
        public string Erstellung { get; set; }
        public DateTime Verändert;
        public string Veränderung { get; set; }
        public int createuser_id;
        public string Ersteller { get; set; }
        public int lastuser_id;
        public string Verändert_von { get; set; }
        public string Name { get; set; }
        public string Bewertung { get; set; }
        public string path;
        public int filesize;

        public WikiDoc(Wiki wiki)
        {
            var c = new SQLCommands(new MySqlConnectionInformation("0", "0", "0", "0", "0"));
            client_id = 0;
            Erstellt = (DateTime) wiki.Created;
            Verändert = ( DateTime ) wiki.Modified;
            createuser_id = ( int ) wiki.CreateuserId;
            Ersteller = c.getNameByID(createuser_id.ToString(), false);
            if (wiki.LastuserId == null)
            {
                lastuser_id = -1;
                Verändert_von = "keine Angabe";
            }
            else
            {
                lastuser_id = ( int ) wiki.LastuserId;
                Verändert_von = c.getNameByID(lastuser_id.ToString(), false);
            }
            Name = wiki.Title;
            path = wiki.Path;
            Bewertung = "Keine Bewertungen";
            filesize = 0;
        }
    }
}
