using ASCOM.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RACI.Data;
using ASCOM.Utilities.Exceptions;
using Microsoft.Win32;

namespace ASCOM.Utilities
{
    public class RegistryAccess : EntityStore
    {
        public RegistryAccess() : this(false) { }
        public RegistryAccess(string p_CallingComponent) : base(p_CallingComponent) { }

        public RegistryAccess(bool p_IgnoreChecks) : base(p_IgnoreChecks) { }

        public enum RegWow64Options
        {
            KEY_WOW64_32KEY,
            KEY_WOW64_64KEY
        }
        internal RegistryKey OpenSubKey(RegistryKey classesRoot, string v1, bool v2, object kEY_WOW64_64KEY)
        {
            throw new System.NotImplementedException("OpenSubKey not implemented in EntityStore");
        }
    }
}
