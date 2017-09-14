using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TGTheraS4.Database_SQLite
{
    [Table(Name = "users")]
    class users
    {

        public users()
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
        public String firstname { get; set; }
        [Column]
        public String lastname { get; set; }
        [Column]
        public String username { get; set; }
        [Column]
        public String password { get; set; }
        [Column]
        public DateTime date_of_birth { get; set; }
        [Column]
        public DateTime inclusion { get; set; }
        [Column]
        public DateTime leaving { get; set; }
        [Column]
        public String social_insurance_number { get; set; }
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
        public String bank { get; set; }
        [Column]
        public String bank_code { get; set; }
        [Column]
        public String bank_account_number { get; set; }
        [Column]
        public double overtime { get; set; }
        [Column]
        public double holiday_entitlement { get; set; }
        [Column]
        public double holidays_open { get; set; }
        [Column]
        public String email_address { get; set; }
        [Column]
        public String email_user { get; set; }
        [Column]
        public String email_password { get; set; }
        [Column]
        public String email_pop_server { get; set; }
        [Column]
        public int email_check_home { get; set; }
        [Column]
        public String email_signature { get; set; }
        [Column]
        public int allproviders { get; set; }
        [Column]
        public double nursing_leave_entitlement { get; set; }
        [Column]
        public double nursing_leave_open { get; set; }
        [Column]
        public double extra_hour { get; set; }
        [Column]
        public String home_email_address { get; set; }
        [Column]
        public int lone_parent { get; set; }
        [Column]
        public String isAdmin { get; set; }
        [Column]
        public String pwThera { get; set; }
        [Column]
        public String gmail_address { get; set; }
        [Column]
        public String gmail_password { get; set; }
        [Column]
        public int neu { get; set; }

    }
}
