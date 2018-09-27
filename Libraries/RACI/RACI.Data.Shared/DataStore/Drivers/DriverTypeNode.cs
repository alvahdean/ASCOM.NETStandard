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
    public class DriverTypeNode: ProfileNode<AscomPlatformNode, AscomDeviceNode,ProfileValue>
    {
        public DriverTypeNode() : this("", "") { }
        public DriverTypeNode(String name, String description = "") : base(name, description) { }
    }

}
