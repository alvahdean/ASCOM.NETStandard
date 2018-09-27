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
    [Route("rascom/devices/[controller]", Name = "Focuser")]
    public class FocuserController : DevicesController<IFocuserV2>
    {

        public FocuserController(
            SystemHelper sysHelper,
            UserManager<RaciUser> userManager,
            SignInManager<RaciUser> signInManager,
            ILogger<FocuserController> logger)
            : base(sysHelper, userManager, signInManager, logger) { }

        // Summary: The state of temperature compensation mode (if available), else always False.
        //
        // Exceptions:
        //   T:ASCOM.PropertyNotImplementedException:
        //     If ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable is False and an attempt
        //     is made to set ASCOM.DeviceInterface.IFocuserV2.TempComp to true.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     TempComp Read must be implemented and must not throw a PropertyNotImplementedException.
        //     TempComp Write can throw a PropertyNotImplementedException. If the ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable
        //     property is True, then setting ASCOM.DeviceInterface.IFocuserV2.TempComp to True
        //     puts the focuser into temperature tracking mode. While in temperature tracking
        //     mode, ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32) commands will be rejected
        //     by the focuser. Set to False to turn off temperature tracking.
        //     If temperature compensation is not available, this property must always return
        //     False.
        //     A ASCOM.PropertyNotImplementedException exception must be thrown if ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable
        //     is False and an attempt is made to set ASCOM.DeviceInterface.IFocuserV2.TempComp
        //     to true.
        [HttpGet("{devName}/TempComp", Name = "Focuser.Get.TempComp")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempComp(string devName)
        {
            bool result;
            try { result = createDevice(devName).TempComp; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: The state of temperature compensation mode (if available), else always False.
        //
        // Exceptions:
        //   T:ASCOM.PropertyNotImplementedException:
        //     If ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable is False and an attempt
        //     is made to set ASCOM.DeviceInterface.IFocuserV2.TempComp to true.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     TempComp Read must be implemented and must not throw a PropertyNotImplementedException.
        //     TempComp Write can throw a PropertyNotImplementedException. If the ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable
        //     property is True, then setting ASCOM.DeviceInterface.IFocuserV2.TempComp to True
        //     puts the focuser into temperature tracking mode. While in temperature tracking
        //     mode, ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32) commands will be rejected
        //     by the focuser. Set to False to turn off temperature tracking.
        //     If temperature compensation is not available, this property must always return
        //     False.
        //     A ASCOM.PropertyNotImplementedException exception must be thrown if ASCOM.DeviceInterface.IFocuserV2.TempCompAvailable
        //     is False and an attempt is made to set ASCOM.DeviceInterface.IFocuserV2.TempComp
        //     to true.
        [HttpGet("{devName}/TempComp/{state}", Name = "Focuser.Set.TempComp")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempComp(string devName,bool state)
        {
            bool result;
            try { result = createDevice(devName).TempComp=state; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Step size (microns) for the focuser.
        //
        // Exceptions:
        //   T:ASCOM.PropertyNotImplementedException:
        //     If the focuser does not intrinsically know what the step size is.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Can throw a not implemented exception Must throw an exception if the focuser
        //     does not intrinsically know what the step size is.
        [HttpGet("{devName}/StepSize", Name = "Focuser.Get.StepSize")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult StepSize(string devName)
        {
            double result;
            try { result = createDevice(devName).StepSize; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Current focuser position, in steps.
        //
        // Exceptions:
        //   T:ASCOM.PropertyNotImplementedException:
        //     If the property is not available for this device.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Can throw a not implemented exception Valid only for absolute positioning focusers
        //     (see the ASCOM.DeviceInterface.IFocuserV2.Absolute property). A ASCOM.PropertyNotImplementedException
        //     exception must be thrown if this device is a relative positioning focuser rather
        //     than an absolute position focuser.
        [HttpGet("{devName}/Position", Name = "Focuser.Get.Position")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Position(string devName)
        {
            double result;
            try { result = createDevice(devName).Position; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Maximum step position permitted.
        //
        // Exceptions:
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented The focuser can step between 0 and ASCOM.DeviceInterface.IFocuserV2.MaxStep.
        //     If an attempt is made to move the focuser beyond these limits, it will automatically
        //     stop at the limit.
        [HttpGet("{devName}/MaxStep", Name = "Focuser.Get.MaxStep")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult MaxStep(string devName)
        {
            double result;
            try { result = createDevice(devName).MaxStep; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Maximum increment size allowed by the focuser; i.e. the maximum number of steps
        //     allowed in one move operation.
        //
        // Exceptions:
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented For most focusers this is the same as the ASCOM.DeviceInterface.IFocuserV2.MaxStep
        //     property. This is normally used to limit the Increment display in the host software.
        [HttpGet("{devName}/MaxIncrement", Name = "Focuser.Get.MaxIncrement")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult MaxIncrement(string devName)
        {
            double result;
            try { result = createDevice(devName).MaxIncrement; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: State of the connection to the focuser.
        //
        // Remarks:
        //     Must throw an exception if the call was not successful Must be implemented Set
        //     True to start the connection to the focuser; set False to terminate the connection.
        //     The current connection status can also be read back through this property. An
        //     exception will be raised if the link fails to change state for any reason.
        //     Note
        //     The FocuserV1 interface was the only interface to name its "Connect" method "Link"
        //     all others named their "Connect" method as "Connected". All interfaces including
        //     Focuser now have a ASCOM.DeviceInterface.IFocuserV2.Connected method and this
        //     is the recommended method to use to "Connect" to Focusers exposing the V2 and
        //     later interfaces.
        //     Do not use a NotConnectedException here, that exception is for use in other methods
        //     that require a connection in order to succeed.
        [HttpGet("{devName}/Link", Name = "Focuser.Get.Link")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Link(string devName)
        {
            JsonResult result;
            try { result = new JsonResult(createDevice(devName).Link); }
            catch (Exception ex) { result = new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: State of the connection to the focuser.
        //
        // Remarks:
        //     Must throw an exception if the call was not successful Must be implemented Set
        //     True to start the connection to the focuser; set False to terminate the connection.
        //     The current connection status can also be read back through this property. An
        //     exception will be raised if the link fails to change state for any reason.
        //     Note
        //     The FocuserV1 interface was the only interface to name its "Connect" method "Link"
        //     all others named their "Connect" method as "Connected". All interfaces including
        //     Focuser now have a ASCOM.DeviceInterface.IFocuserV2.Connected method and this
        //     is the recommended method to use to "Connect" to Focusers exposing the V2 and
        //     later interfaces.
        //     Do not use a NotConnectedException here, that exception is for use in other methods
        //     that require a connection in order to succeed.
        [HttpPost("{devName}/Link/{state}", Name = "Focuser.Set.Link")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Link(string devName, bool state)
        {
            bool result;
            try { result = createDevice(devName).Link=state; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: True if the focuser is currently moving to a new position. False if the focuser
        //     is stationary.
        //
        // Exceptions:
        //   T:ASCOM.NotConnectedException:
        //     If the driver is not connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented
        [HttpGet("{devName}/IsMoving", Name = "Focuser.Get.IsMoving")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult IsMoving(string devName)
        {
            bool result;
            try { result = createDevice(devName).IsMoving; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: True if the focuser is capable of absolute position; that is, being commanded
        //     to a specific step location.
        //
        // Exceptions:
        //   T:ASCOM.NotConnectedException:
        //     If the driver must be connected in order to determine the property value.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented
        [HttpGet("{devName}/Absolute", Name = "Focuser.Get.Absolute")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Absolute(string devName)
        {
            bool result;
            try { result = createDevice(devName).Absolute; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: True if focuser has temperature compensation available.
        //
        // Exceptions:
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented Will be True only if the focuser's temperature compensation
        //     can be turned on and off via the ASCOM.DeviceInterface.IFocuserV2.TempComp property.
        [HttpGet("{devName}/TempCompAvailable", Name = "Focuser.Get.TempCompAvailable")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempCompAvailable(string devName)
        {
            bool result;
            try { result = createDevice(devName).TempCompAvailable; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Current ambient temperature as measured by the focuser.
        //
        // Exceptions:
        //   T:ASCOM.PropertyNotImplementedException:
        //     If the property is not available for this device.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected and this information is only available when connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Can throw a not implemented exception Raises an exception if ambient temperature
        //     is not available. Commonly available on focusers with a built-in temperature
        //     compensation mode.
        [HttpGet("{devName}/Temperature", Name = "Focuser.Get.Temperature")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Temperature(string devName)
        {
            double result;
            try { result = createDevice(devName).Temperature; }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(result);
        }

        // Summary: Immediately stop any focuser motion due to a previous ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32)
        //     method call.
        //
        // Exceptions:
        //   T:ASCOM.MethodNotImplementedException:
        //     Focuser does not support this method.
        //
        //   T:ASCOM.NotConnectedException:
        //     If the driver is not connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Can throw a not implemented exceptionSome focusers may not support this function,
        //     in which case an exception will be raised.
        //     Recommendation: Host software should call this method upon initialization and,
        //     if it fails, disable the Halt button in the user interface.
        [HttpGet("{devName}/Halt", Name = "FocuserHalt")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Halt(string devName)
        {
            IFocuserV2 dev = createDevice(devName);
            if (dev.IsMoving)
            {
                try { dev.Halt(); }
                catch (Exception ex) { return new JsonResult(ex); }
            }
            return new JsonResult(!dev.IsMoving);
        }

        // Summary: Moves the focuser by the specified amount or to the specified position depending
        //     on the value of the ASCOM.DeviceInterface.IFocuserV2.Absolute property.
        //
        // Parameters:
        //   Position:
        //     Step distance or absolute position, depending on the value of the ASCOM.DeviceInterface.IFocuserV2.Absolute
        //     property.
        //
        // Exceptions:
        //   T:ASCOM.InvalidOperationException:
        //     If a Move operation is requested when ASCOM.DeviceInterface.IFocuserV2.TempComp
        //     is True
        //
        //   T:ASCOM.NotConnectedException:
        //     If the device is not connected.
        //
        //   T:ASCOM.DriverException:
        //     Must throw an exception if the call was not successful
        //
        // Remarks:
        //     Must be implemented If the ASCOM.DeviceInterface.IFocuserV2.Absolute property
        //     is True, then this is an absolute positioning focuser. The ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32)
        //     command tells the focuser to move to an exact step position, and the Position
        //     parameter of the ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32) method is
        //     an integer between 0 and ASCOM.DeviceInterface.IFocuserV2.MaxStep.
        //     If the ASCOM.DeviceInterface.IFocuserV2.Absolute property is False, then this
        //     is a relative positioning focuser. The ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32)
        //     command tells the focuser to move in a relative direction, and the Position parameter
        //     of the ASCOM.DeviceInterface.IFocuserV2.Move(System.Int32) method (in this case,
        //     step distance) is an integer between minus ASCOM.DeviceInterface.IFocuserV2.MaxIncrement
        //     and plus ASCOM.DeviceInterface.IFocuserV2.MaxIncrement.
        [HttpGet("{devName}/Move", Name = "FocuserMove")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Move(string devName, int Position)
        {
            IFocuserV2 dev = createDevice(devName);
            if (dev.IsMoving)
                return new JsonResult(new Exception($"Device Busy"));

            try { dev.Move(Position); }
            catch (Exception ex) { return new JsonResult(ex); }
            return new JsonResult(dev.IsMoving);
        }

    }
}
