using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "userstoservices")]
    internal class userstoservices
    {
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