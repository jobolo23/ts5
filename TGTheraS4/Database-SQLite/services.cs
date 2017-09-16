using System;
using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "services")]
    internal class services
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
        public int provider_id { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public string street { get; set; }

        [Column]
        public string zip { get; set; }

        [Column]
        public string city { get; set; }

        [Column]
        public string phone_1 { get; set; }

        [Column]
        public string phone_2 { get; set; }

        [Column]
        public string fax { get; set; }

        [Column]
        public string email_address { get; set; }

        [Column]
        public string home_page { get; set; }

        [Column]
        public int active { get; set; }

        [Column]
        public DateTime start { get; set; }

        [Column]
        public DateTime end { get; set; }

        [Column]
        public string comment { get; set; }

        [Column]
        public int neu { get; set; }
    }
}