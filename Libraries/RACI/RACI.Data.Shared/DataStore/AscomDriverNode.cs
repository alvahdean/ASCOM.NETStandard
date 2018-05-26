using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ASCOM.Utilities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RACI.Data
{

    public class AscomDriverNode : ProfileNode<AscomPlatformNode, AscomDeviceNode>
    {
        public AscomDriverNode() : this("", "") { }
        public AscomDriverNode(String name, String description = "") : base(name, description) { }
    }

}
