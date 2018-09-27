using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RACI.Data
{
    public class RaciUser : IdentityUser, IRaciUser
    {
        [ForeignKey("UserSettings")]
        public int? UserSettingsId { get; set; }
        public UserSettings UserSettings { get; set; }
    }
}
