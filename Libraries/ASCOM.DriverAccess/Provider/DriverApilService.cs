using ASCOM.DeviceInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.DriverAccess
{
    public class DriverApiService : DriverServiceBase
    {

        public DriverApiService(IServiceProvider service) : base(service) 
        {
            Selector = (Type dType) => { return DriverLoader.GetApiDriver(dType); };
        }

    }
}
