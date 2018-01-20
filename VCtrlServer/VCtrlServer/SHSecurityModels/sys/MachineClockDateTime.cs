using System;
using System.Collections.Generic;
using System.Text;

namespace SHSecurityModels
{
    public class MachineClockDateTime : IDateTime
    {
        public DateTime Now => System.DateTime.Now;
    }
}
