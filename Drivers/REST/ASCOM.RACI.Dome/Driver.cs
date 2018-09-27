
#define Dome

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Linq;
using RACI.Client;

namespace ASCOM.RACI
{



    /// <summary>
    /// ASCOM Dome Driver for RACI.
    /// </summary>
    [Guid("18c229a2-5a78-4fff-881e-e6067b6f1cde")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Dome : DomeDriver, IDomeV2
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.RACI.Dome";
        internal static string driverType = "Dome";

        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "ASCOM Dome Driver for RACI.";

        internal static string epUrlName = "Endpoint Url"; 
        internal static string epUrlDefault = "https://localhost:44378/rascom";
        internal static string epDriverName = "Endpoint Driver"; 
        internal static string epDriverDefault = "xASCOM.Simulator.Dome";
        internal static string traceStateProfileName = "Trace Level";
        internal static bool traceStateDefault = true;
        internal static string knownEndpointsName = "Known Endpoints";
        
        // Variables to hold the currrent device configuration
        internal static string epUrl; 
        internal static string epDriver;
        internal static string[] knownEndpoints = new string[] { };
        internal static string[] epDrivers = new string[] { };
        internal static bool traceState
        {
            get => tl.Enabled;
            set
            {
                if (tl.Enabled != value)
                {

                    if (tl.Enabled)
                    {
                        LogMessage("Internal", $"TraceState changed {tl.Enabled}=>{value}");
                        tl.Enabled = value;
                    }
                    else
                    {
                        tl.Enabled = value;
                        LogMessage("Internal", $"TraceState changed {!value}=>{value}");
                    }
                    traceStateDefault = value;
                }
            }
        }

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RACI"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public Dome()
        {
            tl = new TraceLogger("", "RACI");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("Dome", "Starting initialisation");
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object
            //TODO: Implement your additional construction here

            tl.LogMessage("Dome", "Completed initialisation");
        }

        //
        // PUBLIC COM INTERFACE IDomeV2 IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        override public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        //public ArrayList SupportedActions
        //{
        //    get
        //    {
        //        tl.LogMessage("SupportedActions Get", "Returning empty arraylist");
        //        return new ArrayList();
        //    }
        //}

        //public string Action(string actionName, string actionParameters)
        //{
        //    LogMessage("", "Action {0}, parameters {1} not implemented", actionName, actionParameters);
        //    throw new ASCOM.ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        //}

        //public void CommandBlind(string command, bool raw)
        //{
        //    CheckConnected("CommandBlind");
        //    // Call CommandString and return as soon as it finishes
        //    this.CommandString(command, raw);
        //    // or
        //    throw new ASCOM.MethodNotImplementedException("CommandBlind");
        //    // DO NOT have both these sections!  One or the other
        //}

        //public bool CommandBool(string command, bool raw)
        //{
        //    CheckConnected("CommandBool");
        //    string ret = CommandString(command, raw);
        //    // TODO decode the return string and return true or false
        //    // or
        //    throw new ASCOM.MethodNotImplementedException("CommandBool");
        //    // DO NOT have both these sections!  One or the other
        //}

        //public string CommandString(string command, bool raw)
        //{
        //    CheckConnected("CommandString");
        //    // it's a good idea to put all the low level communication with the device here,
        //    // then all communication calls this function
        //    // you need something to ensure that only one command is in progress at a time

        //    throw new ASCOM.MethodNotImplementedException("CommandString");
        //}

        //public bool Connected
        //{
        //    get => base.Connected;
        //    set
        //    {
        //        tl.LogMessage("Connected", "Set {0}", value);
        //        base.Connected = value;
        //    }
        //}

        //public string Description => base.Description;

        //public string DriverInfo => base.DriverInfo;

        //public string DriverVersion => base.DriverVersion;

        //public short InterfaceVersion => base.InterfaceVersion;

        //public string Name => base.Name;

        #endregion

        #region IDome Implementation

        private bool domeShutterState = false; // Variable to hold the open/closed status of the shutter, true = Open

        //public void AbortSlew() => base.AbortSlew();
        //public double Altitude => base.Altitude;
        //public bool AtHome => base.AtHome;
        //public bool AtPark => base.AtPark;
        //public double Azimuth => base.Azimuth;
        //public bool CanFindHome => base.CanFindHome;
        //public bool CanPark => base.CanPark;
        //public bool CanSetAltitude => base.CanSetAltitude;
        //public bool CanSetAzimuth => base.CanSetAzimuth;
        //public bool CanSetPark => base.CanSetPark;
        //public bool CanSetShutter => base.CanSetShutter;
        //public bool CanSlave => base.CanSlave;
        //public bool CanSyncAzimuth => base.CanSyncAzimuth;
        //public void CloseShutter() => base.CloseShutter();
        //public void FindHome() => base.FindHome();
        //public void OpenShutter() => base.OpenShutter();
        //public void Park() => base.Park();
        //public void SetPark() => base.SetPark();
        //public ShutterState ShutterStatus => base.ShutterStatus;
        //public bool Slaved
        //{
        //    get => base.Slaved;
        //    set { base.Slaved = value; }
        //}
        //public void SlewToAltitude(double alt) => base.SlewToAltitude(alt);
        //public void SlewToAzimuth(double az) => base.SlewToAzimuth(az);
        //public bool Slewing => base.Slewing;
        //public void SyncToAzimuth(double az) => base.SyncToAzimuth(az);
        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "Dome";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        override protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Clean up the tracelogger and util objects
                    tl.Enabled = false;
                    tl.Dispose();
                    tl = null;
                    utilities.Dispose();
                    utilities = null;
                    astroUtilities.Dispose();
                    astroUtilities = null;
                    base.Dispose(disposing);
                }
            }
        }

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return Connected;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = driverType;
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault.ToString()));
                epUrl = driverProfile.GetValue(driverID, epUrlName, string.Empty, epUrlDefault);
                epDriver = driverProfile.GetValue(driverID, epDriverName, string.Empty, epDriverDefault);
                string knownEps = driverProfile.GetValue(driverID, knownEndpointsName, string.Empty, epUrlDefault);
                knownEndpoints=knownEps.Split(',').Select(t=>t.Trim()).ToArray();
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = driverType;
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(driverID, epUrlName, epUrl.ToString());
                driverProfile.WriteValue(driverID, epDriverName, epDriver.ToString());
                string knownEps = string.Join(",", knownEndpoints);
                driverProfile.WriteValue(driverID, knownEndpointsName, knownEps);
            }
        }


        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
