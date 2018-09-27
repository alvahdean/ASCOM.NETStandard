using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{

    public class AscomDeviceNode : ProfileNode<DriverTypeNode>
    {
        public AscomDeviceNode() : this("", "") { }
        public AscomDeviceNode(String name, String description = "") : base(name, description) { }
    }

    public class DomeNode : AscomDeviceNode
    {
        public DomeNode() : this("", "") { }
        public DomeNode(String name, String description = "") : base(name, description) { }
        public DomeState State { get; set; }
    }

    public class FilterWheelNode : AscomDeviceNode
    {
        public FilterWheelNode() : this("", "") { }
        public FilterWheelNode(String name, String description = "") : base(name, description) { }
        public FilterWheelState State { get; set; }
    }
    public class FocuserNode : AscomDeviceNode
    {
        public FocuserNode() : this("", "") { }
        public FocuserNode(String name, String description = "") : base(name, description) { }
        public FocuserState State { get; set; }
    }

}
