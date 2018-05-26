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

    public class AscomSettingsNode : ProfileNode<AscomPlatformNode>
    {
        public AscomSettingsNode() : this("", "") { }
        public AscomSettingsNode(String name, String description = "") : base(name, description) { }
    }

}
