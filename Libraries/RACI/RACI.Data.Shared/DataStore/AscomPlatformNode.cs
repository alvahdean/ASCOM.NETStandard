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
    public class AscomPlatformNode : ProfileNode<RaciSystem>
    {
        public AscomPlatformNode() : this("", "") { }
        public AscomPlatformNode(String name, String description = "") : base(name, description) { }
    }  
}
