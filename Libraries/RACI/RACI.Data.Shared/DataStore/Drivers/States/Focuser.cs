using System;
using System.Collections.Generic;

namespace RACI.Data
{
    public class FocuserState : DriverState
    {
        public bool IsMoving { get; }
        public bool Link { get; set; }
        public int Position { get; }
        public bool TempComp { get; set; }
        public double Temperature { get; }
    }
}
