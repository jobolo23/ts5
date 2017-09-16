using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "clientstoservices")]
    internal class clientstoservices
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }

        [Column]
        public int client_id { get; set; }

        [Column]
        public int service_id { get; set; }

        [Column]
        public int neu { get; set; }
    }
}