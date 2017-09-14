using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntranetTG;

namespace TGTheraS4.Objects
{
    public class NewestDokus
    {
        

        public string name { get; set; }
        public string tag { get; set; }
        public string wg { get; set; }
        public string ersteller { get; set; }
        public string art { get; set; }
        public string created { get; set; }
        public string id { get; set; }


        public NewestDokus(String name, String tag, String wg, String ersteller, String art, String created, String id)
        {

            this.name = name;
            this.tag = tag;
            this.wg = wg;
            this.ersteller = ersteller;
            this.art = art;
            this.created = created;
            this.id = id;
        }
    }
}
