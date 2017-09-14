using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntranetTG.Objects
{
    public class Enumerations
    {

        //public enum Status
        //{
        //    Intern = 0,
        //    Extern = 1

        //}

        public enum EditDialog
        {
            HourType = 0,
            WorkingTime = 1
        }

        public enum InstructionTask
        {
            Instruction = 0,
            Task = 1
        }

        public enum Status
        {
            Zugewiesen = 0,
            Gelesen = 1,
            Abgeschlossen = 2,
            Archivieren = 3
        }

        public enum UserGroup
        {
            User = 0,
            Admin = 1,
            Leitender = 2
        }
    }
}
