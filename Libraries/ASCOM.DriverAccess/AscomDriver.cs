// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess.AscomDriver
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using ASCOM.Utilities;
using System;
using System.Linq;
using System.Collections;
using ASCOM.DeviceInterface;
using System.ComponentModel;
using ASCOM.Utilities.Exceptions;
using RACI.Data;

namespace ASCOM.DriverAccess
{
    public class AscomDriver : AscomDriver<IAscomDriver>
    {
        protected AscomDriver() : base() { }
        public AscomDriver(string deviceProgId) : base(deviceProgId) { }
    }

    public class AscomDriver<TDriver> : IAscomDriver, INotifyPropertyChanged
        where TDriver : class, IAscomDriver
    {
        protected TDriver Impl { get; private set; }
        protected static string deviceType { get; private set; }
        internal IASCOMProfile profile;
        private SystemHelper sys = new SystemHelper();
        private AscomDeviceNode _devNode;
//        internal MemberFactory MemberFactory { get; private set; }
        static AscomDriver()
        {
            deviceType = DriverLoader.ApiTypeFor(typeof(TDriver))?.Name.ToUpper() ?? "";
        }
        protected AscomDriver()
        {
            Impl = null;
            profile = null;
        }
        public AscomDriver(string deviceProgId) :this()
        {
            TL = new TraceLogger("", "DriverAccess");
            TL.Enabled = false;
            TL.Enabled = RegistryCommonCode.GetBool("Trace DriverAccess", false);
            TL.LogMessage("AscomDriver", "Successfully created TraceLogger");
            TL.LogMessage("AscomDriver", $"Device type: {deviceType}");
            TL.LogMessage("AscomDriver", $"Device ProgID: {deviceProgId}");
         
            Impl = DriverLoader.GetImplementation<TDriver>(deviceProgId) ?? throw new AscomException($"Unable to obtain driver instance for '{deviceProgId}'");
            string tName = GetType().Name;
            if (tName != "AscomDriver")
            {
                Profile pu = new Profile() { DeviceType = GetType().Name };
                profile = pu.GetProfile(deviceProgId) ?? throw new AscomException($"Unable to get profile for driver '{pu.DeviceType}:{deviceProgId}'");
            }
            //this._memberFactory = this.MemberFactory;

            //TODO: Remove MemberFactory once all api driver classes are convertered
            //MemberFactory = new MemberFactory(deviceProgId, TL);
        }
        
        public bool Connected
        {
            get => Impl.Connected;
            set
            {
                if (Connected != value)
                {
                    Impl.Connected = value;
                    profile.SetValue(nameof(Connected), Impl.Connected.ToString());
                    RaisePropertyChanged(nameof(Connected));
                }
            }
        }
        public string Description { get => Impl.Description; }
        public string DriverInfo { get => Impl.DriverInfo; }
        public string DriverVersion { get => Impl.DriverVersion; }
        public short InterfaceVersion
        {
            get
            {
                //TODO: Just return Impl.InterfaceVersion, check not needed for templated version
                try { return Impl.InterfaceVersion; } catch { }
                return 1;
            }
        }
        public string Name { get => Impl.Name; }
        public ArrayList SupportedActions { get => Impl.SupportedActions; }

        internal TraceLogger TL { get; set; }

        public string Action(string ActionName, string ActionParameters)
        {
            TL.LogMessage($"Begin Action", $"Action(ActionName='{ActionName}',ActionParameters={ActionParameters})");
            string result = Impl.Action(ActionName, ActionParameters);
            TL.LogMessage($"End Action", $"Result: {result}");
            return result;
        }
        public void CommandBlind(string Command, bool Raw)
        {
            TL.LogMessage($"Begin CommandBlind", $"CommandBlind(Command='{Command}',Raw={Raw})");
            Impl.CommandBlind(Command, Raw);
            TL.LogMessage($"End CommandBlind", $"Result: void");
        }
        public bool CommandBool(string Command, bool Raw)
        {
            TL.LogMessage($"Begin CommandBool", $"CommandBool(Command='{Command}',Raw={Raw})");
            bool result = Impl.CommandBool(Command, Raw);
            TL.LogMessage($"End CommandBool", $"Result: {result}");
            return result;
        }
        public string CommandString(string Command, bool Raw)
        {
            TL.LogMessage($"Begin CommandString", $"CommandString(Command='{Command}',Raw={Raw})");
            string result = Impl.CommandString(Command, Raw);
            TL.LogMessage($"End CommandString", $"Result: {result}");
            return result;
        }
        public void SetupDialog()
        {
            TL.LogMessage($"Begin SetupDialog", $"SetupDialog()");
            Impl.SetupDialog();
            TL.LogMessage($"End SetupDialog", $"Result: void");
        }
        public void Dispose()
        {
            TL.LogMessage($"Begin Dispose", $"{GetType().FullName}.Dispose()");
            Impl.Dispose();
            TL.LogMessage($"End Dispose", $"");
        }

        #region PropertyChangedEvent notification
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propName)
        {
            TL.LogMessage("PropertyChanged", $"Property value '{propName}' changed");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        #region Convenience Members
        /// <summary>
        /// Brings up the ASCOM Chooser Dialog to choose a Focuser
        /// </summary>
        /// <param name="focuserId">Focuser Prog ID for default or null for None</param>
        /// <returns>Prog ID for chosen focuser or null for none</returns>
        public static string Choose(string deviceId)
        {
            using (Chooser chooser = new Chooser())
            {
                
                chooser.DeviceType = DriverLoader.ApiTypeFor(typeof(TDriver))?.Name ?? "";
                return chooser.Choose(deviceId);
            }
        }

        #endregion

    }


}
