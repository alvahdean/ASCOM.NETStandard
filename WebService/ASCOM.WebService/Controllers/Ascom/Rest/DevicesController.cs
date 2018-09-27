using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using ASCOM;
using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using ASCOM.WebService.Models;
using ASCOM.DriverAccess;
using ASCOM.Internal;
using ASCOM.DeviceInterface;
using RACI.Data;
using Microsoft.AspNetCore.Routing;

namespace ASCOM.WebService.Controllers
{

    [Authorize]
    [Produces("application/json")]
    [Route("rascom/[controller]")]
    public class DevicesController<TDriver>: DeviceTemplateController<TDriver, DriverMeta, DriverProperties, DriverState>
                where TDriver : class, IAscomDriver
    {
        public DevicesController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger logger)
            : base(sysHelper, userManager, signInManager, logger) { }

        #region Group Get/Set Properties
        override protected T SetPropsData<T>(string devName, T data)
        {
            SetProperty(devName, "SupportedActions", new ArrayList(data.SupportedActions));
            return GetPropsData<T>(devName);
        }
        override protected T SetStateData<T>(string devName, T data)
        {
            SetProperty(devName, "Connected", data.Connected);
            return GetStateData<T>(devName);
        }
        override protected T SetMetaData<T>(string devName, T data)
        {
            base.SetMetaData(devName,data);
            SetProperty(devName, "Description", data.Description);
            SetProperty(devName, "DeviceRoot", data.DeviceRoot.AbsoluteUri);
            SetProperty(devName, "DeviceType", data.DeviceType);
            SetProperty(devName, "Name", data.Name);
            return GetMetaData<T>(devName);
        }

        override protected T GetPropsData<T>(string devName)
        {
            T result = base.GetPropsData<T>(devName);
            result.SupportedActions = GetProperty<ArrayList>(devName, "SupportedActions").ToStrings();
            return result;
        }
        override protected T GetStateData<T>(string devName)
        {
            T result = base.GetStateData<T>(devName);
            result.Connected = GetProperty<bool>(devName, "Connected");
            return result;
        }
        override protected T GetMetaData<T>(string devName)
        {
            T result = base.GetMetaData<T>(devName);
            result.Description = GetProperty<string>(devName, "Description");
            result.DeviceRoot = new Uri(GetProperty<string>(devName, "DeviceRoot"));
            result.DeviceType = GetProperty<string>(devName, "DeviceType");
            result.Name = GetProperty<string>(devName, "Name");
            return result;
        }
        #endregion
    }


    public abstract class DeviceTemplateController<TDriver,TMeta,TProps,TState> : RascomController
        where TDriver : class, IAscomDriver
        where TState: DriverState,new()
        where TProps : DriverProperties, new()
        where TMeta : DriverMeta, new()
    {
        #region Internal fields
        private static Dictionary<string, TDriver> Drivers { get; } = new Dictionary<string, TDriver>();
        private static object driverSync = new object();
        #endregion

        #region Helper utilities
        protected string DriverType { get; private set; }
        protected string ControllerId { get; private set; }
        protected TDriver createDevice(string devName)
        {
            TDriver result = null;
            lock (driverSync)
            {
                if (Drivers.ContainsKey(devName))
                    result = Drivers[devName];
                else
                {
                    result = DriverLoader.GetImplementation<TDriver>(devName);
                    if (result == null)
                        throw new DriverException($"Unable to create a driver instance for {devName} (DeviceType={DriverType})");
                    Drivers.Add(devName, result);
                }
            }
            return result;
        }

        protected TValue GetProperty<TValue>(TDriver driver, string propName)
        {
            return (TValue)driver.GetType().GetProperty(propName).GetValue(driver, null);
        }
        protected TValue GetProperty<TValue>(TDriver driver, string propName, TValue defaultValue)
        {
            TValue result;
            try { result = (TValue)driver.GetType().GetProperty(propName).GetValue(driver, null); }
            catch { result = defaultValue; }
            return result;
        }
        protected TValue GetProperty<TValue>(string devName, string propName)
        {
            TDriver driver = createDevice(devName);
            return GetProperty<TValue>(driver,propName);
        }
        protected TValue GetProperty<TValue>(string devName, string propName, TValue defaultValue)
        {
            TDriver driver = createDevice(devName);
            return GetProperty(driver, propName, defaultValue);
        }
        protected TValue SetProperty<TValue>(TDriver driver, string propName, TValue value)
        {
            driver.GetType().GetProperty(propName).SetValue(driver, value);
            return GetProperty<TValue>(driver, propName);
        }
        protected TValue SetProperty<TValue>(string devName, string propName, TValue value)
        {
            TDriver driver = createDevice(devName);
            return SetProperty(driver, propName, value);
        }

        protected IActionResult GetPropertyResult(TDriver driver, string propName)
        {
            JsonResult result;
            try
            {
                var value = driver.GetType().GetProperty(propName).GetValue(driver, null);
                result = new JsonResult(value);
            }
            catch (Exception ex)
            {
                result = new JsonResult(ex);
            }
            return result;
        }
        protected IActionResult GetPropertyResult(string devName, string propName)
        {
            IActionResult result;
            try
            {
                TDriver driver = createDevice(devName);
                result= GetPropertyResult(driver,propName);
            }
            catch (Exception ex)
            {
                result = new JsonResult(ex);
            }
            return result;
        }
        protected IActionResult SetPropertyResult<TValue>(TDriver driver, string propName, TValue value)
            where TValue : struct
        {
            IActionResult result;
            try
            {
                driver.GetType().GetProperty(propName).SetValue(driver, value);
                result = GetPropertyResult(driver, propName);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult SetPropertyResult<TValue>(string devName, string propName, TValue value)
            where TValue : struct
        {
            IActionResult result;
            try
            {
                TDriver driver = createDevice(devName);
                result= SetPropertyResult(driver, propName, value);
            }
            catch (Exception ex) { result= new JsonResult(ex); }
            return result;
        }

        protected IActionResult GetPropertiesResult(string devName)
        {
            JsonResult result;
            try
            {
                TProps props = GetPropsData<TProps>(devName);
                result = new JsonResult(props);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult SetPropertiesResult(string devName, TProps props)
        {
            JsonResult result;
            try
            {
                props=SetPropsData(devName, props);
                result = new JsonResult(props);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult GetMetaResult(string devName)
        {
            JsonResult result;
            try
            {
                TMeta data = GetMetaData<TMeta>(devName);
                result = new JsonResult(data);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult SetMetaResult(string devName, TMeta data)
        {
            JsonResult result;
            try
            {
                data = SetMetaData(devName, data);
                result = new JsonResult(data);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult GetStateResult(string devName)
        {
            JsonResult result;
            try
            {
                TState data = GetStateData<TState>(devName);
                result = new JsonResult(data);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }
        protected IActionResult SetStateResult(string devName, TState data)
        {

            JsonResult result;
            try
            {
                data = SetStateData(devName, data);
                result = new JsonResult(data);
            }
            catch (Exception ex) { result = new JsonResult(ex); }
            return result;
        }

        virtual protected T SetPropsData<T>(string devName, T props) where T: DriverProperties,new() => 
            GetPropsData<T>(devName);
        virtual protected T SetStateData<T>(string devName, T props) where T : DriverState, new() => 
            GetStateData<T>(devName);
        virtual protected T SetMetaData<T>(string devName, T props) where T : DriverMeta, new() => 
            GetMetaData<T>(devName);
        virtual protected T GetPropsData<T>(string devName) where T: DriverProperties,new() => 
            new T();
        virtual protected T GetStateData<T>(string devName) where T : DriverState,new() => 
            new T();
        virtual protected T GetMetaData<T>(string devName) where T : DriverMeta,new() => 
            new T();

        #endregion

        #region Instance management
        public DeviceTemplateController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger logger)
            : base(sysHelper, userManager, signInManager, logger)
        {
            ControllerId = Guid.NewGuid().ToString();
            DriverType = DriverLoader.ApiTypeFor(typeof(TDriver)).Name;
        }
        #endregion

        #region Group Get/Set operations
        [HttpGet("{devName}/ext/State")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetState(string devName)
        {
            return GetStateResult(devName);
        }

        [HttpPost("{devName}/ext/State")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SetState(string devName, [FromBody] TState data)
        {
            SetState(devName, data);
            return GetState(devName);
        }

        [HttpGet("{devName}/ext/Meta")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetMeta(string devName)
        {
            return GetMetaResult(devName);
        }

        [HttpPost("{devName}/ext/Meta")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SetMeta(string devName, [FromBody] TMeta data)
        {
            SetMeta(devName, data);
            return GetMeta(devName);
        }

        [HttpGet("{devName}/ext/Properties")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult GetProperties(string devName)
        {
            return GetPropertiesResult(devName);
        }

        [HttpPost("{devName}/ext/Properties")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SetProperties(string devName, [FromBody] TProps data)
        {
            SetProperties(devName, data);
            return GetProperties(devName);
        }
        #endregion

        #region Root requests
        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            Profile p = new Profile();
            return new JsonResult(p.RegisteredDevices(DriverType));
        }

        [HttpGet("{devName}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[Route("RegisteredDevices/{driverType}/{devName}")]
        public IActionResult GetProfile(string devName)
        {
            Profile p = new Profile() { DeviceType = DriverType };
            if (!p.IsRegistered(devName))
                return new NotFoundObjectResult($"{DriverType}:{devName}");
            IASCOMProfile prf = p.GetProfile(devName);
            return new JsonResult(prf.ToDictionary());
        }
        #endregion

        #region ASCOM base driver API
        [HttpGet("{devName}/Connected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(string devName) => GetPropertyResult(devName, "Connected");

        [HttpPost("{devName}/Connected/{state}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(String devName, bool state)=> SetPropertyResult(devName, "Connected", state);

        [HttpGet("{devName}/Description")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Description(string devName)=> GetPropertyResult(devName, "Description");

        [HttpGet("{devName}/DriverInfo")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverInfo(string devName)=> GetPropertyResult(devName, "DriverInfo");

        [HttpGet("{devName}/DriverVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverVersion(string devName)=> GetPropertyResult(devName, "DriverVersion");

        [HttpGet("{devName}/InterfaceVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult InterfaceVersion(string devName)=> GetPropertyResult(devName, "InterfaceVersion");

        [HttpGet("{devName}/Name")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Name(string devName) { return GetPropertyResult(devName, "Name"); }

        [HttpGet("{devName}/SupportedActions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SupportedActions(string devName)=> GetPropertyResult(devName, "SupportedActions");

        [HttpPut("{devName}/Action/{actionName}/{actionParameters}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Action(string devName,string actionName,[FromBody] string actionParameters)
        {
            try
            {
                TDriver driver = createDevice(devName);
                return driver==null
                    ? new JsonResult(driver.Action(actionName, actionParameters))
                    : throw new ApplicationException($"Driver not found: {devName}");
            }
            catch (Exception ex) { return new JsonResult(ex); }
        }

        [HttpGet("{devName}/CommandBlind/{command}/{raw}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CommandBlind(string devName, string command, bool raw)
        {
            IAscomDriver device = null;
            try
            {
                device = createDevice(devName);
                device.CommandBlind(command, raw);
            }
            catch(Exception ex) { return new JsonResult(ex); }
            return new JsonResult(true);
        }

        [HttpGet("{devName}/CommandBool/{cmd}/{raw}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CommandBool(string devName,string cmd, bool raw)
        {
            bool result = false;
            IAscomDriver device = null;
            try
            {
                device = createDevice(devName);
                result = device.CommandBool(cmd, raw);
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpGet("{devName}/CommandString/{cmd}/{raw}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CommandString(string devName,string cmd, bool raw)
        {
            String result = "";
            IAscomDriver device = null;
            try
            {
                device = createDevice(devName);
                result = device.CommandString(cmd, raw);
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Launches a configuration dialog box for the driver. The call will not return
        //     until the user clicks OK or cancel manually.
        //
        // Exceptions:
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented
        [HttpGet("{devName}/SetupDialog")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SetupDialog(string devName)
        {
            return new JsonResult(new MethodNotImplementedException($"SetupDialog"));
        }
        #endregion

    }
}
