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
    //[Authorize]
    //[Produces("application/json")]
    //[Route("rascom/[controller]", Name = "Devices")]
    public abstract class DevicesController: RascomController
    {
        private static Util util = new Util();
        protected TraceLogger tlog { get; }
        protected IAscomDriverFactory driverProvider;

        protected bool isValid { get; set; }

        protected virtual bool validate()
        {
            return isValid;
        }

        protected void Log(String message,String logId="")
        {
            logId = this.GetType().Name;
            tlog?.LogMessageCrLf(logId, message);
        }


        public DevicesController(
            UserManager<ApplicationUser> userManager
            , ILogger logger
            , IAscomDriverFactory drvProvider) : base(userManager,logger)
        {
            driverProvider = drvProvider;

            tlog = new TraceLogger(GetType().Name,"Controller");
            tlog.Enabled=true;

            Log($"Constructor: {GetType().Name}> starting...");
            validate();
            Log($"\t{GetType().Name}: complete (Success={isValid})...");
        }

        [HttpGet(Name = "GetDeviceIndex")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            return new JsonResult(new RascomResult(isValid ? hlp.RegisteredDeviceTypes : null));
        }

        [HttpGet("{devName}/Connected", Name = "GetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(string devName)
        {
            IAscomDriver device = createDevice(devName);
            return new JsonResult(new RascomResult(device.Connected));
        }

        private IAscomDriver createDevice(string devName)
        {
            return driverProvider.GetDriver<IAscomDriver>(devName);
        }

        [HttpPost("{devName}/Connected/{state}", Name = "SetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(String devName, bool state)
        {
            IAscomDriver device = createDevice(devName);
            device.Connected = state;
            return new JsonResult(new RascomResult(device.Connected));
        }

        [HttpGet("{devName}/Description", Name = "GetDescription")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Description(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Description));
        }

        [HttpGet("{devName}/DriverInfo", Name = "GeIAscomDriverInfo")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverInfo(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).DriverInfo));
        }

        [HttpGet("{devName}/DriverVersion", Name = "GeIAscomDriverVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverVersion(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).DriverVersion));
        }

        [HttpGet("{devName}/InterfaceVersion", Name = "GetInterfaceVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult InterfaceVersion(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Description));
        }

        [HttpGet("{devName}/Name", Name = "GetName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Name(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Name));
        }

        [HttpGet("{devName}/SupportedActions", Name = "GetSupportedActions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SupportedActions(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).SupportedActions.ToStrings()));
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
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
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
            catch(Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult());
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
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
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
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
        }
    }

    /*
    public abstract class TypedDevicesController<IAscomDriver> : RascomController
        where IAscomDriver :class, IAscomDriver
    {
        private DeviceViewModel<IAscomDriver> Model { get; set; }
        private static Util util = new Util();
        protected TraceLogger tlog { get; }
        IAscomDriver drvInst = default(IAscomDriver);
        protected IAscomDriverProvider driverProvider;
        protected Type driverType { get => typeof(IAscomDriver); }

        protected String driverName { get => driverType.Name; }

        protected bool isValid { get; set; }

        protected virtual bool validate()
        {
            return isValid;
        }

        protected void Log(String message, String logId = "")
        {
            logId = logId ?? driverName;
            tlog?.LogMessageCrLf(logId, message);
        }


        public TypedDevicesController(
            UserManager<ApplicationUser> userManager
            , ILogger<AccountController> logger
            , IAscomDriverProvider drvProvider) : base(userManager, logger)
        {
            driverProvider = drvProvider;

            tlog = new TraceLogger(GetType().Name, "Controller");
            tlog.Enabled = true;

            Log($"Constructor: {GetType().Name}<{driverName}> starting...");
            if (!hlp.IsSupportedType(driverName))
            {
                Log($"\tUnsupported DeviceType '{driverName}'");
                Log($"\tConstructor: {GetType().Name}<{driverName}> failed...");
                return;
            }
            validate();
            Log($"\t{GetType().Name}<{driverName}> complete (Success={isValid})...");
        }

        [HttpGet(Name = "GetDeviceIndex")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            return new JsonResult(new RascomResult(isValid ? hlp.RegisteredDeviceTypes : null));
        }

        [HttpGet("{devName}/Connected", Name = "GetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(string devName)
        {
            IAscomDriver device = createDevice(devName);
            return new JsonResult(new RascomResult(device.Connected));
        }

        protected IAscomDriver createDevice(string devName)
        {
            driverProvider.GeIAscomDriver<IAscomDriver>()
        }

        [HttpPost("{devName}/Connected/{state}", Name = "SetConnected")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Connected(String devName, bool state)
        {
            IAscomDriver device = createDevice(devName);
            device.Connected = state;
            return new JsonResult(new RascomResult(device.Connected));
        }

        [HttpGet("{devName}/Description", Name = "GetDescription")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Description(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Description));
        }

        [HttpGet("{devName}/DriverInfo", Name = "GeIAscomDriverInfo")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverInfo(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).DriverInfo));
        }

        [HttpGet("{devName}/DriverVersion", Name = "GeIAscomDriverVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult DriverVersion(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).DriverVersion));
        }

        [HttpGet("{devName}/InterfaceVersion", Name = "GetInterfaceVersion")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult InterfaceVersion(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Description));
        }

        [HttpGet("{devName}/Name", Name = "GetName")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Name(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).Name));
        }

        [HttpGet("{devName}/SupportedActions", Name = "GetSupportedActions")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SupportedActions(string devName)
        {
            return new JsonResult(new RascomResult(createDevice(devName).SupportedActions.ToStrings()));
        }

        [HttpPut("{devName}/Action/{actionName}/{actionParameters}", Name = "ExecAction")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Action(string devName, string actionName, string actionParameters)
        {
            String result = null;
            IAscomDriver device = default(IAscomDriver);
            try
            {
                device = createDevice(devName);
                result = device.Action(actionName, actionParameters);
            }
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
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
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult());
        }

        [HttpGet("{devName}/CommandBool/{cmd}/{raw}", Name = "ExecCommandBool")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CommandBool(string devName, string cmd, bool raw)
        {
            bool result = false;
            IAscomDriver device = null;
            try
            {
                device = createDevice(devName);
                result = device.CommandBool(cmd, raw);
            }
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
        }

        [HttpGet("{devName}/CommandString/{cmd}/{raw}", Name = "ExecCommandString")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CommandString(string devName, string cmd, bool raw)
        {
            String result = "";
            IAscomDriver device = null;
            try
            {
                device = createDevice(devName);
                result = device.CommandString(cmd, raw);
            }
            catch (Exception ex) { new JsonResult(new RascomResult(ex)); }
            return new JsonResult(new RascomResult(result));
        }
    }
    */
}
