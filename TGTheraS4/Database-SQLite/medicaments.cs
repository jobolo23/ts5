using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "medicaments")]
    class medicaments
    {
        public medicaments()
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
        public String name { get; set; }
        [Column]
        public String description { get; set; }
        [Column]
        public int neu { get; set; }
    }
}
