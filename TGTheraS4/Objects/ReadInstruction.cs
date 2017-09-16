using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheraS5.Objects
{
    public class ReadInstruction
    {
        public string lastname { get; set; }
        public string firstname { get; set; }
        public String read  { get; set; }

        public ReadInstruction(String firstname ,string lastname , String read)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.read = read;
        }

    }
}
