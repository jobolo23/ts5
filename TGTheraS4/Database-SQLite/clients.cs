using System;
using System.Data.Linq.Mapping;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "clients")]
    public class clients
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
        public int salutation_id { get; set; }

        [Column]
        public int contact_id { get; set; }

        [Column]
        public DateTime inclusion { get; set; }

        [Column]
        public DateTime leaving { get; set; }

        [Column]
        public string firstname { get; set; }

        [Column]
        public string lastname { get; set; }

        [Column]
        public int sex { get; set; }

        [Column]
        public int status { get; set; }

        [Column]
        public string icd { get; set; }

        [Column]
        public DateTime date_of_birth { get; set; }

        [Column]
        public string place_of_birth { get; set; }

        [Column]
        public string citizenship { get; set; }

        [Column]
        public string migration { get; set; }

        [Column]
        public string social_insurance_number { get; set; }

        [Column]
        public string insurance { get; set; }

        [Column]
        public string co_insured { get; set; }

        [Column]
        public string district_authority { get; set; }

        [Column]
        public int assignment { get; set; }

        [Column]
        public int residenceLast { get; set; }

        [Column]
        public int schoolVisited { get; set; }

        [Column]
        public string schoolOthers { get; set; }

        [Column]
        public string schoolGraduation { get; set; }

        [Column]
        public int jobTraining { get; set; }

        [Column]
        public int youthWelfare { get; set; }

        [Column]
        public int youthPsychiatry { get; set; }

        [Column]
        public string youthOthers { get; set; }

        [Column]
        public int intitiativeAdmission { get; set; }

        [Column]
        public string initiativeOthers { get; set; }

        [Column]
        public int reasonAdmission { get; set; }

        [Column]
        public string reasonOthers { get; set; }

        [Column]
        public string inclusion_reason { get; set; }

        [Column]
        public string inclusion_school { get; set; }

        [Column]
        public string inclusion_health { get; set; }

        [Column]
        public string inclusion_medication { get; set; }

        [Column]
        public string comment { get; set; }

        [Column]
        public double size { get; set; }

        [Column]
        public double weight { get; set; }

        [Column]
        public double pocket_money { get; set; }

        [Column]
        public string mc_relation { get; set; }

        [Column]
        public int mc_salutation_id { get; set; }

        [Column]
        public int mc_title_id { get; set; }

        [Column]
        public string mc_firstname { get; set; }

        [Column]
        public string mc_lastname { get; set; }

        [Column]
        public string mc_street { get; set; }

        [Column]
        public string mc_zip { get; set; }

        [Column]
        public string mc_city { get; set; }

        [Column]
        public string mc_phone_1 { get; set; }

        [Column]
        public string mc_phone_2 { get; set; }

        [Column]
        public string mc_fax { get; set; }

        [Column]
        public string mc_email { get; set; }

        [Column]
        public string mc_comment { get; set; }

        [Column]
        public int Uni_ID { get; set; }

        [Column]
        public int neu { get; set; }
    }
}