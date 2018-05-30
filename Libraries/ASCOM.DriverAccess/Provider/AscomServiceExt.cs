using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using ASCOM.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RACI.Data;
using RACI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.DriverAccess
{
    public static class AscomServiceExt
    {
        public static void AddAscom(this IServiceCollection services)
        {
            services.AddScoped<TraceLogger>();
            services.AddDbContext<RaciModel>(ServiceLifetime.Scoped);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<RaciModel>()
                .AddDefaultTokenProviders();
            services.AddSingleton<DriverApiService>();
            services.AddSingleton<DriverImplementationService>();
        }
    }
}
