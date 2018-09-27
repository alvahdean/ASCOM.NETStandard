using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{

    public interface IRaciUser : IIdentityUser<string>
    {
        int? UserSettingsId { get; set; }
        UserSettings UserSettings { get; set; }
    }
}
