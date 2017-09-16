using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace TheraS5.Database_SQLite
{
    [Table(Name = "clients")]
    public class clients
    {


        public clients()
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
        public int salutation_id { get; set; }
        [Column]
        public int contact_id { get; set; }
        [Column]
        public DateTime inclusion { get; set; }
        [Column]
        public DateTime leaving { get; set; }
        [Column]
        public String firstname { get; set; }
        [Column]
        public String lastname { get; set; }
        [Column]
        public int sex { get; set; }
        [Column]
        public int status { get; set; }
        [Column]
        public String icd { get; set; }
        [Column]
        public DateTime date_of_birth { get; set; }
        [Column]
        public String place_of_birth { get; set; }
        [Column]
        public String citizenship { get; set; }
        [Column]
        public String migration { get; set; }
        [Column]
        public String social_insurance_number { get; set; }
        [Column]
        public String insurance { get; set; }
        [Column]
        public String co_insured { get; set; }
        [Column]
        public String district_authority { get; set; }
        [Column]
        public int assignment { get; set; }
        [Column]
        public int residenceLast { get; set; }
        [Column]
        public int schoolVisited { get; set; }
        [Column]
        public String schoolOthers { get; set; }
        [Column]
        public String schoolGraduation { get; set; }
        [Column]
        public int jobTraining { get; set; }
        [Column]
        public int youthWelfare { get; set; }
        [Column]
        public int youthPsychiatry { get; set; }
        [Column]
        public String youthOthers { get; set; }
        [Column]
        public int intitiativeAdmission { get; set; }
        [Column]
        public String initiativeOthers { get; set; }   
        [Column]
        public int reasonAdmission { get; set; }
        [Column]
        public String reasonOthers { get; set; }
        [Column]
        public String inclusion_reason { get; set; }
        [Column]
        public String inclusion_school { get; set; }
        [Column]
        public String inclusion_health { get; set; }
        [Column]
        public String inclusion_medication { get; set; }
        [Column]
        public String comment { get; set; }
        [Column]
        public double size { get; set; }
        [Column]
        public double weight { get; set; }
        [Column]
        public double pocket_money { get; set; }
        [Column]
        public String mc_relation { get; set; }
        [Column]
        public int mc_salutation_id { get; set; }
        [Column]
        public int mc_title_id { get; set; }
        [Column]
        public String mc_firstname { get; set; }
        [Column]
        public String mc_lastname { get; set; }
        [Column]
        public String mc_street { get; set; }
        [Column]
        public String mc_zip { get; set; }
        [Column]
        public String mc_city { get; set; }
        [Column]
        public String mc_phone_1 { get; set; }
        [Column]
        public String mc_phone_2 { get; set; }
        [Column]
        public String mc_fax { get; set; }
        [Column]
        public String mc_email { get; set; }
        [Column]
        public String mc_comment { get; set; }
        [Column]
        public int Uni_ID { get; set; }
        [Column]
        public int neu { get; set; }

    }
}
