using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TheraS5.Database_SQLite
{
    public class clientsdailydocs
    {
        public clientsdailydocs()
        {

        }


        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        [Column]
        public DateTime created { get; set; }
        [Column]
        public DateTime modified { get; set; }
        [Column]
        public int createuser_id { get; set; }
        [Column]
        public int lastuser_id { get; set; }
        [Column]
        public int client_id { get; set; }
        [Column]
        public DateTime for_day { get; set; }
        [Column]
        public string content_bodily { get; set; }
        [Column]
        public string content_psychic { get; set; }
        [Column]
        public string content_external_contact { get; set; }
        [Column]
        public string content_responsibilities { get; set; }
        [Column]
        public int draft { get; set; }
        [Column]
        public int insert_key { get; set; }
        [Column]
        public string content_school { get; set; }
        [Column]
        public int neu { get; set; }


    }
}
