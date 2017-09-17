using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGTheraS4.Objects {
    public class Shout {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int CreatedUserId { get; set; }
        public string Message { get; set; }
        public string CreatedUserFirstname { get; set; }
        public string CreatedUserLastname { get; set; }

        public Shout(uint id, DateTime? created, uint? createdUserId, string message, string createdUserFirstname, string createdUserLastname)
        {
            Id = (int)id;
            Created = (DateTime)created;
            CreatedUserId = (int)createdUserId;
            Message = message;
            CreatedUserFirstname = createdUserFirstname;
            CreatedUserLastname = createdUserLastname;
        }
    }
}
