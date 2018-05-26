using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCOM.DriverAccess.Plugins
{
    public class AscomDriverService : IServiceProvider
    {
        IServiceProvider serviceProvider;
        public AscomDriverService()
        {
            serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddSingleton<IAscomDriverProvider, AscomDriverProvider>()
            .BuildServiceProvider();
        }

        public object GetService(Type serviceType)
        {
           return serviceProvider.GetService(serviceType);
        }
    }
}
