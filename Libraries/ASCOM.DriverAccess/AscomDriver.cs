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
using ASCOM.Utilities.Exceptions;

namespace ASCOM.DriverAccess
{
    public class AscomDriver : IDisposable, IAscomDriver
    {
        internal TraceLogger TL;
        private int interfaceVersion;
        private bool disposedValue;
        private string deviceType;

        internal MemberFactory MemberFactory { get; private set; }

        public bool Connected
        {
            get
            {
                if (this.deviceType == "FOCUSER" & this.interfaceVersion == 1)
                {
                    this.TL.LogMessage("Connected Get", "Device is Focuser and Interfaceverison is 1 so issuing Link command");
                    return (bool)this.MemberFactory.CallMember(1, "Link", new Type[0]);
                }
                this.TL.LogMessage("Connected Get", "Issuing Connected command");
                return (bool)this.MemberFactory.CallMember(1, "Connected", new Type[0]);
            }
            set
            {
                if (this.deviceType == "FOCUSER" & this.interfaceVersion == 1)
                {
                    this.TL.LogMessage("Connected Set", "Device is Focuser and Interfaceverison is 1 so issuing Link command: " + (object)value);
                    this.MemberFactory.CallMember(2, "Link", new Type[0], (object)value);
                }
                else
                {
                    this.TL.LogMessage("Connected Set", "Issuing Connected command: " + (object)value);
                    this.MemberFactory.CallMember(2, "Connected", new Type[0], (object)value);
                }
            }
        }

        public string Description
        {
            get
            {
                try
                {
                    return (string)this.MemberFactory.CallMember(1, "Description", new Type[1]
                    {
            typeof (string)
                    });
                }
                catch (Exception ex)
                {
                    if (this.interfaceVersion == 1)
                    {
                        switch (this.deviceType)
                        {
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                this.TL.LogMessage("Description Get", "This is " + this.deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                this.TL.LogMessage("Description Get", "Received exception. Device type is " + this.deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        this.TL.LogMessage("Description Get", "Received exception. Device type is " + this.deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        public string DriverInfo
        {
            get
            {
                try
                {
                    return (string)this.MemberFactory.CallMember(1, "DriverInfo", new Type[1]
                    {
            typeof (string)
                    });
                }
                catch (Exception ex)
                {
                    if (this.interfaceVersion == 1)
                    {
                        switch (this.deviceType)
                        {
                            case "CAMERA":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                this.TL.LogMessage("DriverInfo Get", "This is " + this.deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                this.TL.LogMessage("DriverInfo Get", "Received exception. Device type is " + this.deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        this.TL.LogMessage("DriverInfo Get", "Received exception. Device type is " + this.deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        public string DriverVersion
        {
            get
            {
                try
                {
                    return (string)this.MemberFactory.CallMember(1, "DriverVersion", new Type[1]
                    {
            typeof (string)
                    });
                }
                catch (Exception ex)
                {
                    if (this.interfaceVersion == 1)
                    {
                        switch (this.deviceType)
                        {
                            case "CAMERA":
                            case "DOME":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                this.TL.LogMessage("DriverVersion Get", "This is " + this.deviceType + " interface version 1, so returning empty string");
                                return "0.0";
                            default:
                                this.TL.LogMessage("DriverVersion Get", "Received exception. Device type is " + this.deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        this.TL.LogMessage("DriverVersion Get", "Received exception. Device type is " + this.deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        public short InterfaceVersion
        {
            get
            {
                try
                {
                    return Convert.ToInt16(this.MemberFactory.CallMember(1, "InterfaceVersion", new Type[0]));
                }
                catch (PropertyNotImplementedException ex)
                {
                    this.TL.LogMessage("InterfaceVersion Get", "Received PropertyNotImplementedException so returning interface version = 1");
                    return 1;
                }
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    return (string)this.MemberFactory.CallMember(1, "Name", new Type[1]
                    {
            typeof (string)
                    });
                }
                catch (Exception ex)
                {
                    if (this.interfaceVersion == 1)
                    {
                        switch (this.deviceType)
                        {
                            case "CAMERA":
                            case "FILTERWHEEL":
                            case "FOCUSER":
                            case "ROTATOR":
                                this.TL.LogMessage("Name Get", "This is " + this.deviceType + " interface version 1, so returning empty string");
                                return "";
                            default:
                                this.TL.LogMessage("Name Get", "Received exception. Device type is " + this.deviceType + " and interface version is 1 so throwing received exception: " + ex.Message);
                                throw;
                        }
                    }
                    else
                    {
                        this.TL.LogMessage("Name Get", "Received exception. Device type is " + this.deviceType + " and interface version is >1 so throwing received exception: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                try
                {
                    return (ArrayList)this.MemberFactory.CallMember(1, "SupportedActions", new Type[0]);
                }
                catch (Exception ex)
                {
                    if (this.interfaceVersion == 1 | this.deviceType == "TELESCOPE" & this.interfaceVersion == 2)
                    {
                        this.TL.LogMessage("SupportedActions Get", "SupportedActions is not implmented in " + this.deviceType + " version " + (object)this.interfaceVersion + " returning an empty ArrayList");
                        return new ArrayList();
                    }
                    this.TL.LogMessage("SupportedActions Get", "Received exception: " + ex.Message);
                    throw;
                }
            }
        }

        public AscomDriver()
        {

        }

        public AscomDriver(string deviceProgId) : this()
        {
            this.TL = new TraceLogger("", "DriverAccess");
            this.TL.Enabled = false;
            //this.TL.Enabled = RegistryCommonCode.GetBool("Trace DriverAccess", false);
            this.TL.LogMessage("AscomDriver", "Successfully created TraceLogger");
            this.deviceType = this.GetType().Name.ToUpper();
            this.TL.LogMessage("AscomDriver", "Device type: " + this.GetType().Name);

            this.TL.LogMessage("AscomDriver", "Device ProgID: " + deviceProgId);
            this.MemberFactory = new MemberFactory(deviceProgId, this.TL);
            try
            {
                this.interfaceVersion = (int)this.InterfaceVersion;
            }
            catch
            {
                this.interfaceVersion = 1;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue && disposing)
            {
                if (this.MemberFactory != null)
                {
                    this.MemberFactory.Dispose();
                    this.MemberFactory = (MemberFactory)null;
                }
                if (this.TL != null)
                    this.TL.Dispose();
            }
            this.disposedValue = true;
        }

        public void SetupDialog()
        {
            this.MemberFactory.CallMember(3, "SetupDialog", new Type[0]);
        }

        public string Action(string ActionName, string ActionParameters)
        {
            return (string)this.MemberFactory.CallMember(3, "Action", new Type[2]
            {
        typeof (string),
        typeof (string)
            }, (object)ActionName, (object)ActionParameters);
        }

        public void CommandBlind(string Command, bool Raw)
        {
            this.MemberFactory.CallMember(3, "CommandBlind", new Type[2]
            {
        typeof (string),
        typeof (bool)
            }, (object)Command, (object)Raw);
        }

        public bool CommandBool(string Command, bool Raw)
        {
            return (bool)this.MemberFactory.CallMember(3, "CommandBool", new Type[2]
            {
        typeof (string),
        typeof (bool)
            }, (object)Command, (object)Raw);
        }

        public string CommandString(string Command, bool Raw)
        {
            return (string)this.MemberFactory.CallMember(3, "CommandString", new Type[2]
            {
        typeof (string),
        typeof (bool)
            }, (object)Command, (object)Raw);
        }
    }
}
