using System;
using System.Collections.Generic;

namespace RACI.Data
{
    public class DriverProperties
    {
        public List<string> SupportedActions { get; set; }
    }

    public class DomeProperties : DriverProperties
    {
        public bool CanFindHome { get; set; }
        public bool CanPark { get; set; }
        public bool CanSetAltitude { get; set; }
        public bool CanSetAzimuth { get; set; }
        public bool CanSetPark { get; set; }
        public bool CanSetShutter { get; set; }
        public bool CanSlave { get; set; }
        public bool CanSyncAzimuth { get; set; }
    }

    public class FocuserProperties : DriverProperties
    {
        public bool Absolute { get; set; }
        public bool TempCompAvailable { get; set; }
        public int MaxStep { get; set; }
        public int MaxIncrement { get; set; }
        public int StepSize { get; set; }
    }

    public class FilterWheelProperties : DriverProperties
    {
        public List<int> FocusOffsets { get; set; }
        public List<string> Names { get; set; }
    }
}
