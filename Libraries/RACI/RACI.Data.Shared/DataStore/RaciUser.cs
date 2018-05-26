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
    public class RaciUser : ProfileNode<RaciSystem>
    {
        public RaciUser() : this("", "") { }
        public RaciUser(String name, String description = "") : base(name, description) { }
        public String UserId { get; set; }
        public String HomeDir { get; set; }
    }
}
