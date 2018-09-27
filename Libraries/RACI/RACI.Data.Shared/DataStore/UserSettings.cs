using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RACI.Data
{
    public class UserSettings : ProfileNode<RaciSystem>
    {
        public UserSettings() : this("", "") { }
        public UserSettings(String name, String description = "") : base(name, description) { }
        public String UserId { get; set; }
        public String HomeDir { get; set; }
        public string IdentityId { get; set; }
        public RaciUser Identity { get; set; } //private set???
    }
}
