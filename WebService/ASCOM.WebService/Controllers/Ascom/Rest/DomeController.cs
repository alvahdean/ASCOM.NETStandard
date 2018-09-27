using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using ASCOM;
using ASCOM.DriverAccess;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ASCOM.Utilities.Exceptions;
using ASCOM.WebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RACI.Data;

namespace ASCOM.WebService.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/devices/[controller]", Name = "Dome")]
    public class DomeController : DeviceTemplateController<IDomeV2,DomeMeta,DomeProperties,DomeState>
    {
        public DomeController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger<FocuserController> logger)
            : base(sysHelper, userManager, signInManager, logger) { }

        #region Group Get/Set Properties
        new protected T SetPropsData<T>(string devName, T data)
            where T: DomeProperties,new()
        {
            data=base.SetPropsData(devName, data);
            SetProperty(devName, "CanFindHome", data.CanFindHome);
            SetProperty(devName, "CanPark", data.CanPark);
            SetProperty(devName, "CanSetAltitude", data.CanSetAltitude);
            SetProperty(devName, "CanSetAzimuth", data.CanSetAzimuth);
            SetProperty(devName, "CanSetPark", data.CanSetPark);
            SetProperty(devName, "CanSetShutter", data.CanSetShutter);
            SetProperty(devName, "CanSlave", data.CanSlave);
            SetProperty(devName, "CanSyncAzimuth", data.CanSyncAzimuth);
            return GetPropsData<T>(devName);
        }
        new protected T SetStateData<T>(string devName, T data)
            where T : DomeState, new()
        {
            data=base.SetStateData(devName, data);
            SetProperty(devName, "Altitude", data.Altitude);
            SetProperty(devName, "Azimuth", data.Azimuth);
            SetProperty(devName, "AtHome", data.AtHome);
            SetProperty(devName, "AtPark", data.AtPark);
            SetProperty(devName, "ShutterStatus", data.ShutterStatus);
            SetProperty(devName, "Slaved", data.Slaved);
            SetProperty(devName, "Slewing", data.Slewing);
            return GetStateData<T>(devName);
        }
        new protected T SetMetaData<T>(string devName, T data)
            where T : DomeMeta, new()
        {
            data = base.SetMetaData(devName, data);
            return GetMetaData<T>(devName);
        }
        new protected T GetPropsData<T>(string devName)
            where T : DomeProperties, new()
        {
            T data = base.GetPropsData<T>(devName);
            data.CanFindHome = GetProperty<bool>(devName, "CanFindHome");
            data.CanPark = GetProperty<bool>(devName, "CanPark");
            data.CanSetAltitude = GetProperty<bool>(devName, "CanSetAltitude");
            data.CanSetAzimuth = GetProperty<bool>(devName, "CanSetAzimuth");
            data.CanSetPark = GetProperty<bool>(devName, "CanSetPark");
            data.CanSetShutter = GetProperty<bool>(devName, "CanSetShutter");
            data.CanSlave = GetProperty<bool>(devName, "CanSlave");
            data.CanSyncAzimuth = GetProperty<bool>(devName, "CanSyncAzimuth");
            return data;

        }
        new protected T GetStateData<T>(string devName)
            where T: DomeState,new()
        {
            T data = base.GetStateData<T>(devName);
            data.Altitude=GetProperty<double>(devName, "Altitude");
            data.Azimuth=GetProperty<double>(devName, "Azimuth");
            data.AtHome=GetProperty<bool>(devName, "AtHome");
            data.AtPark=GetProperty<bool>(devName, "AtPark");
            data.ShutterStatus=GetProperty<string>(devName, "ShutterStatus");
            data.Slaved=GetProperty<bool>(devName, "Slaved");
            data.Slewing=GetProperty<bool>(devName, "Slewing");
            return data;
        }
        new protected T GetMetaData<T>(string devName)
            where T : DomeMeta, new()
        {
            T result = base.GetMetaData<T>(devName);
            return result;
        }
        #endregion

        #region ASCOM Dome API
        [HttpGet("{devName}/Altitude", Name = "Dome.Get.Altitude")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Altitude(string devName)=> GetPropertyResult(devName, "Altitude");

        [HttpGet("{devName}/AtHome", Name = "Dome.Get.AtHome")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult AtHome(string devName) => GetPropertyResult(devName, "AtHome");

        [HttpGet("{devName}/AtPark", Name = "Dome.Get.AtPark")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult AtPark(string devName) => GetPropertyResult(devName, "AtPark");

        [HttpGet("{devName}/Azimuth", Name = "Dome.Get.Azimuth")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Azimuth(string devName) => GetPropertyResult(devName, "Azimuth");

        [HttpGet("{devName}/CanFindHome", Name = "Dome.Get.CanFindHome")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanFindHome(string devName) => GetPropertyResult(devName, "CanFindHome");

        [HttpGet("{devName}/CanPark", Name = "Dome.Get.CanPark")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanPark(string devName) => GetPropertyResult(devName, "CanPark");

        [HttpGet("{devName}/CanSetAltitude", Name = "Dome.Get.CanSetAltitude")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSetAltitude(string devName) => GetPropertyResult(devName, "CanSetAltitude");

        [HttpGet("{devName}/CanSetAzimuth", Name = "Dome.Get.CanSetAzimuth")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSetAzimuth(string devName) => GetPropertyResult(devName, "CanSetAzimuth");

        [HttpGet("{devName}/CanSetPark", Name = "Dome.Get.CanSetPark")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSetPark(string devName) => GetPropertyResult(devName, "CanSetPark");

        [HttpGet("{devName}/CanSetShutter", Name = "Dome.Get.CanSetShutter")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSetShutter(string devName) => GetPropertyResult(devName, "CanSetShutter");

        [HttpGet("{devName}/CanSlave", Name = "Dome.Get.CanSlave")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSlave(string devName) => GetPropertyResult(devName, "CanSlave");

        [HttpGet("{devName}/CanSyncAzimuth", Name = "Dome.Get.CanSyncAzimuth")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CanSyncAzimuth(string devName) => GetPropertyResult(devName, "CanSyncAzimuth");

        [HttpGet("{devName}/ShutterStatus", Name = "Dome.Get.ShutterStatus")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult ShutterStatus(string devName) => GetPropertyResult(devName, "ShutterStatus");

        [HttpGet("{devName}/Slaved", Name = "Dome.Get.Slaved")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Slaved(string devName) => GetPropertyResult(devName, "Slaved");

        [HttpGet("{devName}/Slewing", Name = "Dome.Get.Slewing")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Slewing(string devName) => GetPropertyResult(devName, "Slewing");

        [HttpPost("{devName}/AbortSlew", Name = "Dome.Exec.AbortSlew")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult AbortSlew(string devName) 
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.AbortSlew();
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/CloseShutter", Name = "Dome.Exec.CloseShutter")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult CloseShutter(string devName)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.CloseShutter();
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/FindHome", Name = "Dome.Exec.FindHome")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult FindHome(string devName)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.FindHome();
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/OpenShutter", Name = "Dome.Exec.OpenShutter")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult OpenShutter(string devName)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.OpenShutter();
                return ShutterStatus(devName);
            }
            catch (Exception ex) { return new JsonResult(ex); }
        }

        [HttpPost("{devName}/Park", Name = "Dome.Exec.Park")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Park(string devName)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.Park();
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/SetPark", Name = "Dome.Exec.SetPark")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SetPark(string devName)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.SetPark();
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/SlewToAltitude/{alt}", Name = "Dome.Exec.SlewToAltitude")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SlewToAltitude(string devName, double alt)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.SlewToAltitude(alt);
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/SlewToAzimuth/{az}", Name = "Dome.Exec.SlewToAzimuth")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SlewToAzimuth(string devName, double az)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.SlewToAzimuth(az);
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        [HttpPost("{devName}/SyncToAzimuth/{az}", Name = "Dome.Exec.SyncToAzimuth")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult SyncToAzimuth(string devName, double az)
        {
            String result = "";
            IDomeV2 device = null;
            try
            {
                device = createDevice(devName);
                device.SyncToAzimuth(az);
            }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }
        #endregion

    }
}
