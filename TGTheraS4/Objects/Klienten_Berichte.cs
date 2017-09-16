using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheraS5.Objects
{
    public class Klienten_Berichte
    {
        public int id;
        public int art = -1;
        /*
         * Telefonat = 0
         * Vorfallsprotokoll = 1
         * Gesprächsprotokoll = 2
         * Fallverlaufsgespräch = 3
         * Jahresbericht = 4
         * Zwischenbericht = 5 
         */
        public int Client_id;
        public string name { get; set; }
        public string content;
        public int table = 0;
        /*
         * 1 = clientsfvgs
         * 2 = clientsreports 
         */
    }
}
