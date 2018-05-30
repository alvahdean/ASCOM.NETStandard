using ASCOM.DeviceInterface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.DriverAccess
{
    public class DriverServiceBase : IServiceProvider
    {
        protected IServiceProvider svcProvider;
        protected ILogger logger;

        public DriverServiceBase(IServiceProvider service)
        {
            ILoggerFactory loggerFactory = svcProvider.GetService(typeof(ILoggerFactory)) as ILoggerFactory;
            logger = loggerFactory.CreateLogger(GetType());
            Selector= (Type dType)=> { return null; };
        }
        protected Func<Type,object> Selector { get; set; }
        virtual protected bool valdateRequestType(Type serviceType)
        {
            bool result = false;
            string driverName = serviceType?.FullName ?? "{null}";
            logger?.LogInformation($"Type requested: {driverName}");
            if (!DriverLoader.IsDriver(serviceType))
                logger?.LogInformation($"Invalid type request: '{driverName ?? "null"}', not a driver type");
            else if (serviceType.IsInterface)
                logger?.LogInformation($"Invalid type request: '{driverName}', request typemust be a concrete implmentation class");
            else if (DriverLoader.IsApiType(serviceType))
                logger?.LogInformation($"Invalid type request: '{driverName}', request type cannot be a drvier API type ");
            else
                result = true;
            return result;
        }
        virtual protected bool validateResult(object result)
        {
            if (result != null && DriverLoader.IsDriver(result.GetType()))
            {
                logger?.LogInformation($"Service type request successful");
                return true;
            }
            logger?.LogInformation($"Service type request failed");
            return false;
        }
        public object GetService(Type serviceType)
        {
            object result = valdateRequestType(serviceType) ? Selector(serviceType) : null;
            return validateResult(result) ? result : null;
        }
    }
}
