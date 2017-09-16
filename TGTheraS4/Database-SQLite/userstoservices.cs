using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "userstoservices")]
    class userstoservices
    {

        public userstoservices()
        {

        }

        [Column(IsPrimaryKey = true)]
        public int id { get; set; }
        [Column]
        public int user_id { get; set; }
        [Column]
        public int service_id { get; set; }
        [Column]
        public int neu { get; set; }
    }
}
