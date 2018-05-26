using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.DriverAccess
{
    public static class AscomServiceExt
    {
        public static void AddAscomDrivers(this IServiceCollection services)
        {
            foreach (var devType in AscomDriverLoader.DriverTypes)
            {
                services.AddSingleton(devType);
            }
        }
    }
}
