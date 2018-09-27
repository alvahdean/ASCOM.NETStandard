// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.GlobalConstants
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;

namespace ASCOM.Utilities
{
#warning Internal class exposed as public during porting: GlobalConstants
    //[StandardModule]
    //public sealed class GlobalConstants
    //internal sealed class GlobalConstants
    public static class GlobalConstants
    {
    public const string SERIAL_FILE_NAME_VARNAME = "SerTraceFile";
    public const string SERIAL_AUTO_FILENAME = "C:\\SerialTraceAuto.txt";
    public const string SERIAL_DEFAULT_FILENAME = "C:\\SerialTrace.txt";
    public const string SERIAL_DEBUG_TRACE_VARNAME = "SerDebugTrace";
    public const string SERIALPORT_COM_PORT_SETTINGS = "COMPortSettings";
    public const string SERIAL_FORCED_COMPORTS_VARNAME = "COMPortSettings\\ForceCOMPorts";
    public const string SERIAL_IGNORE_COMPORTS_VARNAME = "COMPortSettings\\IgnoreCOMPorts";
    public const string TRACE_XMLACCESS = "Trace XMLAccess";
    public const bool TRACE_XMLACCESS_DEFAULT = false;
    public const string TRACE_PROFILE = "Trace Profile";
    public const bool TRACE_PROFILE_DEFAULT = false;
    public const string TRACE_UTIL = "Trace Util";
    public const bool TRACE_UTIL_DEFAULT = false;
    public const string TRACE_TIMER = "Trace Timer";
    public const bool TRACE_TIMER_DEFAULT = false;
    public const string SERIAL_TRACE_DEBUG = "Serial Trace Debug";
    public const bool SERIAL_TRACE_DEBUG_DEFAULT = false;
    public const string SIMULATOR_TRACE = "Trace Simulators";
    public const bool SIMULATOR_TRACE_DEFAULT = false;
    public const string DRIVERACCESS_TRACE = "Trace DriverAccess";
    public const bool DRIVERACCESS_TRACE_DEFAULT = false;
    public const string CHOOSER_USE_CREATEOBJECT = "Chooser Use CreateObject";
    public const bool CHOOSER_USE_CREATEOBJECT_DEFAULT = false;
    public const string ABANDONED_MUTEXT_TRACE = "Trace Abandoned Mutexes";
    public const bool ABANDONED_MUTEX_TRACE_DEFAULT = false;
    public const string ASTROUTILS_TRACE = "Trace Astro Utils";
    public const bool ASTROUTILS_TRACE_DEFAULT = false;
    public const string NOVAS_TRACE = "Trace NOVAS";
    public const bool NOVAS_TRACE_DEFAULT = false;
    public const string SERIAL_WAIT_TYPE = "Serial Wait Type";
    public const Serial.WaitType SERIAL_WAIT_TYPE_DEFAULT = Serial.WaitType.WaitForSingleObject;
    public const string PROFILE_MUTEX_NAME = "ASCOMProfileMutex";
    public const int PROFILE_MUTEX_TIMEOUT = 5000;
    public const string TRACE_TRANSFORM = "Trace Transform";
    public const bool TRACE_TRANSFORM_DEFAULT = false;
    public const string REGISTRY_UTILITIES_FOLDER = "Software\\ASCOM\\Utilities";
    public const string EVENT_SOURCE = "ASCOM Platform";
    public const string EVENTLOG_NAME = "ASCOM";
    public const string EVENTLOG_MESSAGES = "ASCOM\\EventLogMessages.txt";
    public const string EVENTLOG_ERRORS = "ASCOM\\EventLogErrors.txt";
    public const string REGISTRY_ROOT_KEY_NAME = "SOFTWARE\\ASCOM";
    public const string REGISTRY_5_BACKUP_SUBKEY = "Platform5Original";
    public const string REGISTRY_55_BACKUP_SUBKEY = "Platform55Original";
    public const string PLATFORM_VERSION_NAME = "PlatformVersion";
    public const string COLLECTION_DEFAULT_VALUE_NAME = "***** DefaultValueName *****";
    public const string COLLECTION_DEFAULT_UNSET_VALUE = "===== ***** UnsetValue ***** =====";
    public const string VALUES_FILENAME = "Profile.xml";
    public const string VALUES_FILENAME_ORIGINAL = "ProfileOriginal.xml";
    public const string VALUES_FILENAME_NEW = "ProfileNew.xml";
    public const string PROFILE_NAME = "Profile";
    public const string SUBKEY_NAME = "SubKey";
    public const string DEFAULT_ELEMENT_NAME = "DefaultElement";
    public const string VALUE_ELEMENT_NAME = "Element";
    public const string NAME_ATTRIBUTE_NAME = "Name";
    public const string VALUE_ATTRIBUTE_NAME = "Value";
    public const string XML_SUBKEYNAME_ELEMENTNAME = "SubKeyName";
    public const string XML_DEFAULTVALUE_ELEMENTNAME = "DefaultValue";
    public const string XML_NAME_ELEMENTNAME = "Name";
    public const string XML_DATA_ELEMENTNAME = "Data";
    public const string XML_SUBKEY_ELEMENTNAME = "SubKey";
    public const string XML_VALUE_ELEMENTNAME = "Value";
    public const string XML_VALUES_ELEMENTNAME = "Values";
    public const string DRIVERS_32BIT = "Drivers Not Compatible With 64bit Applications";
    public const string DRIVERS_64BIT = "Drivers Not Compatible With 32bit Applications";
    public const string PLATFORM_VERSION_EXCEPTIONS = "ForcePlatformVersion";
    public const string PLATFORM_VERSION_SEPARATOR_EXCEPTIONS = "ForcePlatformVersionSeparator";
    public const string FORCE_SYSTEM_TIMER = "ForceSystemTimer";
    public const string PLATFORM_INSTALLER_PROPDUCT_CODE = "{8961E141-B307-4882-ABAD-77A3E76A40C1}";
    public const string DEVELOPER_INSTALLER_PROPDUCT_CODE = "{4A195DC6-7DF9-459E-8F93-60B61EB45288}";
    public const string DRIVER_AUTHOR_MESSAGE_DRIVER = "Please contact the driver author and request an updated driver.";
    public const string DRIVER_AUTHOR_MESSAGE_INSTALLER = "Please contact the driver author and request an updated installer.";
    public const string PLATFORM_INFORMATION_SUBKEY = "Platform";
    public const string PLATFORM_VERSION = "Platform Version";
    public const string PLATFORM_VERSION_DEFAULT_BAD_VALUE = "0.0.0.0";
    public const double ABSOLUTE_ZERO_CELSIUS = -273.15;

    //[DebuggerNonUserCode]
    //static GlobalConstants()
    //{
    //}
#warning Internal enum exposed as public during porting: ASCOM.Utilities.GlobalConstants.EventLogErrors
        public enum EventLogErrors
        //internal enum EventLogErrors
        {
      EventLogCreated,
      ChooserFormLoad,
      MigrateProfileVersions,
      MigrateProfileRegistryKey,
      RegistryProfileMutexTimeout,
      XMLProfileMutexTimeout,
      XMLAccessReadError,
      XMLAccessRecoveryPreviousVersion,
      XMLAccessRecoveredOK,
      ChooserSetupFailed,
      ChooserDriverFailed,
      ChooserException,
      Chooser32BitOnlyException,
      Chooser64BitOnlyException,
      FocusSimulatorNew,
      FocusSimulatorSetup,
      TelescopeSimulatorNew,
      TelescopeSimulatorSetup,
      VB6HelperProfileException,
      DiagnosticsLoadException,
      DriverCompatibilityException,
      TimerSetupException,
      DiagnosticsHijackedCOMRegistration,
      UninstallASCOMInfo,
      UninstallASCOMError,
      ProfileExplorerException,
      InstallTemplatesInfo,
      InstallTemplatesError,
      TraceLoggerException,
      TraceLoggerMutexTimeOut,
      TraceLoggerMutexAbandoned,
      RegistryProfileMutexAbandoned,
    }
  }
}
