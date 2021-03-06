﻿using System;
using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "clientsmedications")]
    public class clientsmedications
    {
        [Column(IsPrimaryKey = true)]
        public int id { get; set; }

        [Column]
        public int client_id { get; set; }

        [Column]
        public DateTime created { get; set; }

        [Column]
        public DateTime modified { get; set; }

        [Column]
        public int createuser_id { get; set; }

        [Column]
        public int lastuser_id { get; set; }

        [Column]
        public int medicament_id { get; set; }

        [Column]
        public DateTime from { get; set; }

        [Column]
        public DateTime to { get; set; } //Eig. Date

        [Column]
        public DateTime apply_date { get; set; } //Eig. Date

        [Column]
        public string apply_time { get; set; } //Eig. Time

        [Column]
        public int morning { get; set; }

        [Column]
        public int midday { get; set; }

        [Column]
        public int evening { get; set; }

        [Column]
        public int night { get; set; }

        [Column]
        public DateTime cancelled { get; set; } //Eig. Date

        [Column]
        public int neu { get; set; }
    }
}