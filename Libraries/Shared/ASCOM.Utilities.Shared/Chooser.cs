// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Chooser
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Interfaces;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
    //[ClassInterface(ClassInterfaceType.None)]
    //[ComVisible(true)]
    //[Guid("B7A1F5A0-71B4-44f9-91E9-468697957D6B")]
    public class Chooser : IChooser, IChooserExtra, IDisposable
    {
        private ChooserForm m_frmChooser;

        private string m_sDeviceType;
        private bool disposedValue;

        public string DeviceType
        {
            get
            {
                return this.m_sDeviceType;
            }
            set
            {
                if (value=="")
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException("Chooser:DeviceType - Illegal DeviceType value \"\" (empty string)");
                this.m_sDeviceType = value;
            }
        }

        public Chooser()
        {
            this.m_sDeviceType = "";
            this.disposedValue = false;
            try
            {
                //this.m_frmChooser = new ChooserForm();
                //throw new NotImplementedException("Chooser UI not implemented");
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox("Chooser.New " + ex.ToString(),MsgBoxStyle.OkOnly, (object)null);
                //ProjectData.ClearProjectError();
            }
            this.m_sDeviceType = "Telescope";
        }

        ~Chooser()
        {
            this.Dispose(false);
            // ISSUE: explicit finalizer call
            //base.Finalize();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                int num = disposing ? 1 : 0;
                if (this.m_frmChooser != null)
                {
                    this.m_frmChooser.Dispose();
                    this.m_frmChooser = null;
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        public string Choose(string DriverProgID)
        {
            string str;
            try
            {
                if (string.IsNullOrEmpty(this.m_sDeviceType))
                    throw new ASCOM.Utilities.Exceptions.InvalidValueException("Unknown device type, DeviceType property has not been set");
                this.m_frmChooser.DeviceType = this.m_sDeviceType;
                this.m_frmChooser.StartSel = DriverProgID;
                int num = (int)this.m_frmChooser.ShowDialog();
                str = this.m_frmChooser.Result;
                this.m_frmChooser.Dispose();
                this.m_frmChooser = null;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                int num = (int)Interaction.MsgBox((object)("Chooser Exception: " + exception.ToString()), MsgBoxStyle.OkOnly, (object)null);
                EventLogCode.LogEvent("Chooser", "Exception", EventLogEntryType.Error, GlobalConstants.EventLogErrors.ChooserException, exception.ToString());
                str = "";
                //ProjectData.ClearProjectError();
            }
            return str;
        }

        //[ComVisible(false)]
        public string Choose()
        {
            return this.Choose("");
        }
    }
}
