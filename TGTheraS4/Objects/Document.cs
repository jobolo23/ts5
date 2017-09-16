using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModels;
using IntranetTG;
using TGTheraS4.Database.Objects;

namespace TGTheraS4.Objects
{
    public class Document
    {
        public int client_id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public int createuser_id { get; set; }
        public string createuser { get; set; }
        public int lastuser_id { get; set; }
        public string lastuser { get; set; }
        public string title { get; set; }
        public string path { get; set; }
        public int filesize { get; set; }

        public Document(Clientsdocument doc)
        {
            var c = new SQLCommands(new MySqlConnectionInformation("0", "0", "0", "0", "0"));
            client_id = (int) doc.ClientId;
            created = (DateTime)doc.Created;
            modified = ( DateTime ) doc.Modified;
            createuser_id = (int)doc.CreateuserId;
            createuser = c.getNameByID(createuser_id.ToString(), false);
            if (doc.LastuserId == null)
            {
                lastuser_id = -1;
                lastuser = "keine Angabe";
            }
            else
            {
                lastuser_id = (int) doc.LastuserId;
                lastuser = c.getNameByID(lastuser_id.ToString(), false);
            }
            title = doc.Title;
            path = doc.Path;
            filesize = (int)doc.Filesize;
        }

        public Document (Clientsphoto doc)
        {
            var c = new SQLCommands(new MySqlConnectionInformation("0", "0", "0", "0", "0"));
            client_id = ( int ) doc.ClientId;
            created = ( DateTime ) doc.Created;
            modified = ( DateTime ) doc.Modified;
            createuser_id = ( int ) doc.CreateuserId;
            createuser = c.getNameByID(createuser_id.ToString(), false);
            if (doc.LastuserId == null)
            {
                lastuser_id = -1;
                lastuser = "keine Angabe";
            }
            else
            {
                lastuser_id = ( int ) doc.LastuserId;
                lastuser = c.getNameByID(lastuser_id.ToString(), false);
            }
            title = doc.Title;
            path = doc.Path;
            filesize = ( int ) doc.Filesize;
        }
    }
}
