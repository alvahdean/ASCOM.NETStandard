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
using RACI.ASCOM.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace RACI.ASCOM.Service.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("rascom/devices/[controller]", Name = "Focuser")]
    public class FocuserController : DevicesController<Focuser>
    {
        private FocuserViewModel Model { get; set; }
        public FocuserController(
            UserManager<ApplicationUser> userManager
            , ILogger<AccountController> logger)
            :base(userManager,logger)
        {
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
        [HttpGet("{devName}/TempComp", Name = "GetFocuserTempComp")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempComp(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).TempComp); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/TempComp/{state}", Name = "SetFocuserTempComp")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempComp(string devName,bool state)
        {
            RascomResult result = new RascomResult();
            try
            {
                createDevice(devName).TempComp = state;
            }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/StepSize", Name = "GetFocuserStepSize")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult StepSize(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).StepSize); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Position", Name = "GetFocuserPosition")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Position(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).Position); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/MaxStep", Name = "GetFocuserMaxStep")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult MaxStep(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).MaxStep); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/MaxIncrement", Name = "GetFocuserMaxIncrement")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult MaxIncrement(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).MaxIncrement); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Link", Name = "GetFocuserLink")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Link(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).Link); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Link/{state}", Name = "SetFocuserLink")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Link(string devName, bool state)
        {
            RascomResult result = new RascomResult();
            try { createDevice(devName).Link = state; }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/IsMoving", Name = "GetFocuserIsMoving")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult IsMoving(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).IsMoving); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Absolute", Name = "GetFocuserAbsolute")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Absolute(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).Absolute); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/TempCompAvailable", Name = "GetFocuserTempCompAvailable")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult TempCompAvailable(string devName)
        {
            RascomResult result = new RascomResult();
            try { result = new RascomResult(createDevice(devName).TempCompAvailable); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Temperature", Name = "GetFocuserTemperature")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Temperature(string devName)
        {
            RascomResult result = new RascomResult();
            try { result=new RascomResult(createDevice(devName).Temperature); }
            catch (Exception ex) { result = new RascomResult(ex); }
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
            RascomResult result = new RascomResult();
            try { createDevice(devName).Halt(); }
            catch (Exception ex) { result = new RascomResult(ex); }
            return new JsonResult(result);
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
            RascomResult result = null;
            try { result= new RascomResult(createDevice(devName).Position); }
            catch(Exception ex) { result = new RascomResult(ex); }
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
        [HttpGet("{devName}/Setup", Name = "FocuserSetup")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public IActionResult Setup(string devName)
        {
            RascomResult result = new RascomResult(true);
            try { createDevice(devName).SetupDialog(); }
            catch (Exception ex) { result = new RascomResult(ex); }
            return new JsonResult(result);
        }

    }
}
