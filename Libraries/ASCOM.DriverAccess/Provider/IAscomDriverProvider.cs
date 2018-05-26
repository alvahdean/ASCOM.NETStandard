using ASCOM.DeviceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASCOM.DriverAccess
{
    public interface IAscomDriverProvider
    {
        IEnumerable<string> GetNames();
        TDriver GetDriver<TDriver>() where TDriver : class, IAscomDriver;
        TDriver GetDriver<TDriver>(Type driver) where TDriver : class, IAscomDriver;
        TDriver GetDriver<TDriver>(string driver) where TDriver : IAscomDriver;
        IFocuserV2 GetFocuserV3(string driver);
        IFocuserV2 GetFocuserV2(string driver);
        IRotatorV2 GetRotator(string driver);
        ISwitchV2 GetSwitch(string driver);
        ISafetyMonitor GetSafetyMonitor(string driver);
        IDomeV2 GetDome(string driver);
        ITelescopeV3 GetTelescope(string driver);
        IVideo GetVideo(string driver);
        ICameraV2 GetCamera(string driver);
        IFilterWheelV2 GetFilterWheel(string driver);
        IObservingConditions GetObservingConditions(string driver);

    }
}
