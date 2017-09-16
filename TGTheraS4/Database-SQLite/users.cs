using System;
using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "users")]
    internal class users
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
        public int group_id { get; set; }

        [Column]
        public int salutation_id { get; set; }

        [Column]
        public int employment_id { get; set; }

        [Column]
        public int weeklyDays { get; set; }

        [Column]
        public double weeklyHours { get; set; }

        [Column]
        public int lunchTime { get; set; }

        [Column]
        public int wageAdmin { get; set; }

        [Column]
        public int title_id { get; set; }

        [Column]
        public int clientsview_id { get; set; }

        [Column]
        public int regulations { get; set; }

        [Column]
        public int language { get; set; }

        [Column]
        public string firstname { get; set; }

        [Column]
        public string lastname { get; set; }

        [Column]
        public string username { get; set; }

        [Column]
        public string password { get; set; }

        [Column]
        public DateTime date_of_birth { get; set; }

        [Column]
        public DateTime inclusion { get; set; }

        [Column]
        public DateTime leaving { get; set; }

        [Column]
        public string social_insurance_number { get; set; }

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
        public string bank { get; set; }

        [Column]
        public string bank_code { get; set; }

        [Column]
        public string bank_account_number { get; set; }

        [Column]
        public double overtime { get; set; }

        [Column]
        public double holiday_entitlement { get; set; }

        [Column]
        public double holidays_open { get; set; }

        [Column]
        public string email_address { get; set; }

        [Column]
        public string email_user { get; set; }

        [Column]
        public string email_password { get; set; }

        [Column]
        public string email_pop_server { get; set; }

        [Column]
        public int email_check_home { get; set; }

        [Column]
        public string email_signature { get; set; }

        [Column]
        public int allproviders { get; set; }

        [Column]
        public double nursing_leave_entitlement { get; set; }

        [Column]
        public double nursing_leave_open { get; set; }

        [Column]
        public double extra_hour { get; set; }

        [Column]
        public string home_email_address { get; set; }

        [Column]
        public int lone_parent { get; set; }

        [Column]
        public string isAdmin { get; set; }

        [Column]
        public string pwThera { get; set; }

        [Column]
        public string gmail_address { get; set; }

        [Column]
        public string gmail_password { get; set; }

        [Column]
        public int neu { get; set; }
    }
}