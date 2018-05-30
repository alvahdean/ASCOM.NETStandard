using ASCOM.DeviceInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.DriverAccess
{
    public class DriverImplementationService : DriverServiceBase
    {
        public DriverImplementationService(IServiceProvider service) : base(service)
        {
            Selector = (Type dType) => { return DriverLoader.GetImplementation(dType); };
        }
    }
}
