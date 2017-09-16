using System;
using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "medicaments")]
    internal class medicaments
    {
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
        public string name { get; set; }

        [Column]
        public string description { get; set; }

        [Column]
        public int neu { get; set; }
    }
}