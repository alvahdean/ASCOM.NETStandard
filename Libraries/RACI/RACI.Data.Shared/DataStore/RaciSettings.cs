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

    public class RaciSettings : ProfileNode<RaciSystem>
    {
        public RaciSettings() : base("RACI", "RACI configuration settings") { }
    }
}
