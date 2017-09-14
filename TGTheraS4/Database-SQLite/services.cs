using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TGTheraS4.Database_SQLite
{
    [Table(Name = "services")]
    class services
    {

        public services()
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
        public int provider_id { get; set; }
        [Column]
        public String name { get; set; }
        [Column]
        public String street { get; set; }
        [Column]
        public String zip { get; set; }
        [Column]
        public String city { get; set; }
        [Column]
        public String phone_1 { get; set; }
        [Column]
        public String phone_2 { get; set; }
        [Column]
        public String fax { get; set; }
        [Column]
        public String email_address { get; set; }
        [Column]
        public String home_page { get; set; }
        [Column]
        public int active { get; set; }
        [Column]
        public DateTime start { get; set; }
        [Column]
        public DateTime end { get; set; }
        [Column]
        public String comment { get; set; }
        [Column]
        public int neu { get; set; }
    }
}
