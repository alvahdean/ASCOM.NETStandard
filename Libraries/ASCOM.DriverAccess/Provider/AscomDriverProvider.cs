using ASCOM.DeviceInterface;
using ASCOM.DriverAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ASCOM.DriverAccess
{



    public class AscomDriverProvider : IAscomDriverProvider
    {
        private IServiceProvider _serviceProvider;

        public AscomDriverProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IDictionary<string, IAscomDriver> GetDrivers()
        {
            var result = new Dictionary<string, IAscomDriver>();

            foreach (var type in AscomDriverLoader.DriverTypes)
            {
                var driver = (IAscomDriver)_serviceProvider.GetService(type);
                var name = type.FullName;

                result.Add(name, driver);
            }

            return result;
        }
        public TDriver GetDriver<TDriver>() where TDriver: class,IAscomDriver
        {
            return GetDriver<TDriver>(typeof(TDriver));
        }
        public TDriver GetDriver<TDriver>(Type driver) where TDriver : class, IAscomDriver
        {
            return GetDriver<TDriver>(driver.FullName);
        }
        public TDriver GetDriver<TDriver>(string driver) where TDriver : IAscomDriver
        {
            TDriver result = default(TDriver);
            Type dType = GetTypes().FirstOrDefault(t => t.FullName == driver);
            if (dType == null)
                throw new AscomException($"Driver type '{driver}' is not registered");
            if (!typeof(TDriver).IsAssignableFrom(dType))
                throw new AscomException($"Driver '{dType.FullName}' does not implement '{typeof(TDriver).FullName}'");
            result = (TDriver)_serviceProvider.GetService(dType);
            return result;
        }

        public IFocuserV2 GetFocuserV3(string driver) => GetDriver<IFocuserV2>(driver);
        public IFocuserV2 GetFocuserV2(string driver) => GetDriver<IFocuserV2>(driver);
        public IRotatorV2 GetRotator(string driver) => GetDriver<IRotatorV2>(driver);
        public ISwitchV2 GetSwitch(string driver) => GetDriver<ISwitchV2>(driver);
        public ISafetyMonitor GetSafetyMonitor(string driver) => GetDriver<ISafetyMonitor>(driver);
        public IDomeV2 GetDome(string driver) => GetDriver<IDomeV2>(driver);
        public ITelescopeV3 GetTelescope(string driver) => GetDriver<ITelescopeV3>(driver);
        public IVideo GetVideo(string driver) => GetDriver<IVideo>(driver);
        public ICameraV2 GetCamera(string driver) => GetDriver<ICameraV2>(driver);
        public IFilterWheelV2 GetFilterWheel(string driver) => GetDriver<IFilterWheelV2>(driver);
        public IObservingConditions GetObservingConditions(string driver) => GetDriver<IObservingConditions>(driver);

        public IEnumerable<string> GetNames()
        {
            return AscomDriverLoader.DriverTypes
                    .Select(t => t.FullName)
                    .Where(t => !string.IsNullOrWhiteSpace(t));
        }
        public IEnumerable<Type> GetTypes()
        {
            return AscomDriverLoader.DriverTypes;
        }
    }
}
