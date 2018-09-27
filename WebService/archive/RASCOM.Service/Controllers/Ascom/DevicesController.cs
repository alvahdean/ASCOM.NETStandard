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
using RACI.ASCOM.Service.Models;
using ASCOM.DriverAccess;
using ASCOM.Internal;
using ASCOM.DeviceInterface;
using RACI.Data;

namespace RACI.ASCOM.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/[controller]", Name = "Devices")]
    public abstract class DevicesController<TDriver>: RascomController
        where TDriver: class, IAscomDriver
    {
        protected string DriverType => GetType().Name.Replace("Controller", "");
        protected TDriver createDevice(string devName) => DriverLoader.GetImplementation<TDriver>(devName);

        public DevicesController(
            UserManager<RaciUser> userManager,
            SystemHelper sysHelper,
            ILogger logger) : base(userManager, sysHelper, logger) { }

        [HttpGet(Name = "DeviceControllerIndex")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            Profile p = new Profile();
            return new RascomResult(p.RegisteredDevices(DriverType)).JsonResult;
        }

        [HttpGet("{devName}/Connected", Name = "GetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(string devName)
        {
            IAscomDriver device = createDevice(devName);
            return new RascomResult(device.Connected).JsonResult;
        }

        [HttpPost("{devName}/Connected/{state}", Name = "SetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(String devName, bool state)
        {
            IAscomDriver device = createDevice(devName);
            device.Connected = state;
            return new RascomResult(device.Connected).JsonResult;
        }

        [HttpGet("{devName}/Description", Name = "GetDescription")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Description(string devName)
        {
            return new RascomResult(createDevice(devName).Description).JsonResult;
        }

        [HttpGet("{devName}/DriverInfo", Name = "GeIAscomDriverInfo")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverInfo(string devName)
        {
            return new RascomResult(createDevice(devName).DriverInfo).JsonResult;
        }

        [HttpGet("{devName}/DriverVersion", Name = "GeIAscomDriverVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverVersion(string devName)
        {
            return new RascomResult(createDevice(devName).DriverVersion).JsonResult;
        }

        [HttpGet("{devName}/InterfaceVersion", Name = "GetInterfaceVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult InterfaceVersion(string devName)
        {
            return new RascomResult(createDevice(devName).Description).JsonResult;
        }

        [HttpGet("{devName}/Name", Name = "GetName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Name(string devName)
        {
            return new RascomResult(createDevice(devName).Name).JsonResult;
        }

        [HttpGet("{devName}/SupportedActions", Name = "GetSupportedActions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SupportedActions(string devName)
        {
            return new RascomResult(createDevice(devName).SupportedActions.ToStrings()).JsonResult;
        }

        [HttpPut("{devName}/Action/{actionName}/{actionParameters}", Name = "ExecAction")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Action(string devName,string actionName, string actionParameters)
        {
            String result = null;
            IAscomDriver device = default(IAscomDriver);
            try
            {
                device = createDevice(devName);
                result = device.Action(actionName, actionParameters);
            }
            catch (Exception ex) { return new RascomResult(ex).JsonResult; }
            return new RascomResult(result).JsonResult;
        }

        [HttpGet("{devName}/CommandBlind/{command}/{raw}", Name = "ExecCommandBlind")]
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
            catch(Exception ex) { return new RascomResult(ex).JsonResult; }
            return new RascomResult().JsonResult;
        }

        [HttpGet("{devName}/CommandBool/{cmd}/{raw}", Name = "ExecCommandBool")]
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
            catch (Exception ex) { return new RascomResult(ex).JsonResult; }
            return new RascomResult(result).JsonResult;
        }

        [HttpGet("{devName}/CommandString/{cmd}/{raw}", Name = "ExecCommandString")]
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
            catch (Exception ex) { return new RascomResult(ex).JsonResult; }
            return new RascomResult(result).JsonResult;
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
        [HttpGet("{devName}/SetupDialog", Name = "DeviceSetup")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Setup(string devName)
        {
            return new RascomResult(new MethodNotImplementedException("SetupDialog")).JsonResult;
        }

    }
}
