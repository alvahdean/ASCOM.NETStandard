// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.VersionCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
    //[StandardModule]
    internal sealed class VersionCode
    {
        internal static void RunningVersions(TraceLogger TL)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            OperatingSystem osVersion = Environment.OSVersion;
            try
            {
                TL.LogMessage("Versions", "Main Process: " + Process.GetCurrentProcess().MainModule.FileName);
                FileVersionInfo fileVersionInfo = Process.GetCurrentProcess().MainModule.FileVersionInfo;
                TL.LogMessageCrLf("Versions", "  Product:  " + fileVersionInfo.ProductName + " " + fileVersionInfo.ProductVersion);
                TL.LogMessageCrLf("Versions", "  File:     " + fileVersionInfo.FileDescription + " " + fileVersionInfo.FileVersion);
                TL.LogMessageCrLf("Versions", "  Language: " + fileVersionInfo.Language);
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                TL.LogMessage("Versions", "Exception EX0: " + exception.ToString());
                //ProjectData.ClearProjectError();
            }
            try
            {
                TL.LogMessage("Versions", "OS Version: " + Conversions.ToString((int)osVersion.Platform) + ", Service Pack: " + osVersion.ServicePack + ", Full: " + osVersion.VersionString);
                switch (VersionCode.OSBits())
                {
                    case VersionCode.Bitness.Bits32:
                        TL.LogMessage("Versions", "Operating system is 32bit");
                        break;
                    case VersionCode.Bitness.Bits64:
                        TL.LogMessage("Versions", "Operating system is 64bit");
                        break;
                    default:
                        TL.LogMessage("Versions", "Operating system is unknown bits, PTR length is: " + Conversions.ToString(IntPtr.Size));
                        break;
                }
                switch (VersionCode.ApplicationBits())
                {
                    case VersionCode.Bitness.Bits32:
                        TL.LogMessage("Versions", "Application is 32bit");
                        break;
                    case VersionCode.Bitness.Bits64:
                        TL.LogMessage("Versions", "Application is 64bit");
                        break;
                    default:
                        TL.LogMessage("Versions", "Application is unknown bits, PTR length is: " + Conversions.ToString(IntPtr.Size));
                        break;
                }
                TL.LogMessage("Versions", "");
                TL.LogMessage("Versions", "CLR version: " + Environment.Version.ToString());
                string userDomainName = Environment.UserDomainName;
                string userName = Environment.UserName;
                string machineName = Environment.MachineName;
                int processorCount = Environment.ProcessorCount;
                string systemDirectory = Environment.SystemDirectory;
                long workingSet = Environment.WorkingSet;
                TL.LogMessage("Versions", "Machine name: " + machineName + " UserName: " + userName + " DomainName: " + userDomainName);
                TL.LogMessage("Versions", "Number of processors: " + Conversions.ToString(processorCount) + " System directory: " + systemDirectory + " Working set size: " + Conversions.ToString(workingSet) + " bytes");
                TL.LogMessage("Versions", "");
                TL.LogMessage("Versions", "My Documents:            " + Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                TL.LogMessage("Versions", "Application Data:        " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                TL.LogMessage("Versions", "Common Application Data: " + Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                TL.LogMessage("Versions", "Program Files:           " + Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                TL.LogMessage("Versions", "Common Files:            " + Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
                TL.LogMessage("Versions", "System:                  " + Environment.GetFolderPath(Environment.SpecialFolder.System));
                TL.LogMessage("Versions", "Current:                 " + Environment.CurrentDirectory);
                TL.LogMessage("Versions", "");
                Assembly[] assemblies = currentDomain.GetAssemblies();
                int index = 0;
                while (index < assemblies.Length)
                {
                    Assembly assembly = assemblies[index];
                    TL.LogMessage("Versions", "Loaded Assemblies: " + assembly.GetName().Name + " " + assembly.GetName().Version.ToString());
                    checked { ++index; }
                }
                TL.LogMessage("Versions", "");
                VersionCode.AssemblyInfo(TL, "Entry Assembly", Assembly.GetEntryAssembly());
                VersionCode.AssemblyInfo(TL, "Executing Assembly", Assembly.GetExecutingAssembly());
                TL.BlankLine();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                TL.LogMessageCrLf("Versions", "Unexpected exception: " + exception.ToString());
                //ProjectData.ClearProjectError();
            }
        }

        internal static Bitness OSBits()
        {
            if (VersionCode.IsWow64())
                return VersionCode.Bitness.Bits64;
            switch (IntPtr.Size)
            {
                case 4:
                    return VersionCode.Bitness.Bits32;
                case 8:
                    return VersionCode.Bitness.Bits64;
                default:
                    return VersionCode.Bitness.BitsUnknown;
            }
        }

        internal static Bitness ApplicationBits()
        {
            switch (IntPtr.Size)
            {
                case 4:
                    return VersionCode.Bitness.Bits32;
                case 8:
                    return VersionCode.Bitness.Bits64;
                default:
                    return VersionCode.Bitness.BitsUnknown;
            }
        }

        internal static void AssemblyInfo(TraceLogger TL, string AssName, Assembly Ass)
        {
            string fileName = (string)null;
            AssName = Strings.Left(AssName + ":" + Strings.Space(20), 19);
            if (Ass != null)
            {
                try
                {
                    AssemblyName name = Ass.GetName();
                    if (name == null)
                    {
                        TL.LogMessage("Versions", AssName + " Assembly name is missing, cannot determine version");
                    }
                    else
                    {
                        Version version = name.Version;
                        if ((object)version == null)
                        {
                            TL.LogMessage("Versions", AssName + " Assembly version is missing, cannot determine version");
                        }
                        else
                        {
                            string str = version.ToString();
                            if (!string.IsNullOrEmpty(str))
                                TL.LogMessage("Versions", AssName + " AssemblyVersion: " + str);
                            else
                                TL.LogMessage("Versions", AssName + " Assembly version string is null or empty, cannot determine assembly version");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX1: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    fileName = Ass.Location;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        TL.LogMessage("Versions", AssName + "Assembly location is missing, cannot determine file version");
                    }
                    else
                    {
                        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
                        if (versionInfo == null)
                        {
                            TL.LogMessage("Versions", AssName + " File version object is null, cannot determine file version number");
                        }
                        else
                        {
                            string fileVersion = versionInfo.FileVersion;
                            if (!string.IsNullOrEmpty(fileVersion))
                                TL.LogMessage("Versions", AssName + " FileVersion: " + fileVersion);
                            else
                                TL.LogMessage("Versions", AssName + " File version string is null or empty, cannot determine file version");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX2: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    AssemblyName name = Ass.GetName();
                    if (name == null)
                    {
                        TL.LogMessage("Versions", AssName + " Assembly name is missing, cannot determine full name");
                    }
                    else
                    {
                        string fullName = name.FullName;
                        if (!string.IsNullOrEmpty(fullName))
                            TL.LogMessage("Versions", AssName + " Name: " + fullName);
                        else
                            TL.LogMessage("Versions", AssName + " Full name string is null or empty, cannot determine full name");
                    }
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX3: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    TL.LogMessage("Versions", AssName + " CodeBase: " + Ass.GetName().CodeBase);
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX4: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    if (!string.IsNullOrEmpty(fileName))
                        TL.LogMessage("Versions", AssName + " Location: " + fileName);
                    else
                        TL.LogMessage("Versions", AssName + " Location is null or empty, cannot display location");
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX5: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    TL.LogMessage("Versions", AssName + " From GAC: " + Ass.GlobalAssemblyCache.ToString());
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL.LogMessage("AssemblyInfo", "Exception EX6: " + exception.ToString());
                    //ProjectData.ClearProjectError();
                }
            }
            else
                TL.LogMessage("Versions", AssName + " No assembly found");
        }

        private static bool IsWow64()
        {
            bool wow64Process = false;
            if (VersionCode.IsWow64Process(Process.GetCurrentProcess().Handle, ref wow64Process))
                return wow64Process;
            return false;
        }

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(IntPtr hProcess, ref bool wow64Process);

        internal static string DriverCompatibilityMessage(string ProgID, VersionCode.Bitness RequiredBitness, TraceLogger TL)
        {
            string result = "";
            TL.LogMessage("DriverCompatibility", "     ProgID: " + ProgID + ", Bitness: " + RequiredBitness.ToString());
            Console.WriteLine($"DriverCompatibility: ProgID: '{ProgID}', Bitness: {RequiredBitness}");
            return result;
        }

        //internal static string DriverCompatibilityMessage(string ProgID, VersionCode.Bitness RequiredBitness, TraceLogger TL)
        //{
        //  //PEReader peReader1 = (PEReader) null;
        //  RegistryKey registryKey1 = (RegistryKey) null;
        //  RegistryKey registryKey2 = (RegistryKey) null;
        //  RegistryAccess registryAccess = new RegistryAccess("DriverCompatibilityMessage");
        //  string str1 = "";
        //  TL.LogMessage("DriverCompatibility", "     ProgID: " + ProgID + ", Bitness: " + RequiredBitness.ToString());
        //  //PEReader peReader2;
        //  if (RequiredBitness == VersionCode.Bitness.Bits64)
        //  {
        //    RegistryKey registryKey3 = Registry.ClassesRoot.OpenSubKey(ProgID + "\\CLSID", false);
        //    if (registryKey3 != null)
        //    {
        //      string str2 = registryKey3.GetValue("").ToString();
        //      registryKey3.Close();
        //      RegistryKey registryKey4 = Registry.ClassesRoot.OpenSubKey("CLSID\\" + str2);
        //      bool flag;
        //      if (registryKey4 == null)
        //      {
        //        TL.LogMessage("DriverCompatibility", "     No entry in the 64bit registry, checking the 32bit registry");
        //        registryKey4 = Registry.ClassesRoot.OpenSubKey("Wow6432Node\\CLSID\\" + str2);
        //        flag = false;
        //      }
        //      else
        //      {
        //        TL.LogMessage("DriverCompatibility", "     Found entry in the 64bit registry");
        //        flag = true;
        //      }
        //      if (registryKey4 != null)
        //      {
        //        RegistryKey registryKey5 = registryKey4.OpenSubKey("InprocServer32");
        //        registryKey4.Close();
        //        if (registryKey5 != null)
        //        {
        //          string str3 = registryKey5.GetValue("", (object) "").ToString();
        //          string Left = registryKey5.GetValue("CodeBase", (object) "").ToString();
        //          if (Operators.CompareString(Left, "", false) != 0)
        //            str3 = Left;
        //          if (Operators.CompareString(Strings.Trim(str3).ToUpper(), "MSCOREE.DLL", false) == 0)
        //          {
        //            TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL");
        //            string str4 = registryKey5.GetValue("Assembly", (object) "").ToString();
        //            TL.LogMessage("DriverCompatibility", "     Found full name: " + str4);
        //            PortableExecutableKinds peKind;
        //            ImageFileMachine machine;
        //            if (Operators.CompareString(str4, "", false) != 0)
        //            {
        //              try
        //              {
        //                Assembly assembly = Assembly.ReflectionOnlyLoad(str4);
        //                str3 = assembly.CodeBase;
        //                TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " + str3);
        //                TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " + assembly.FullName + " ");
        //                assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit");
        //                if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus");
        //                if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly");
        //                if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable");
        //              }
        //              catch (IOException ex1)
        //              {
        //                //ProjectData.SetProjectError((Exception) ex1);
        //                IOException ioException1 = ex1;
        //                TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " + ioException1.Message);
        //                try
        //                {
        //                  Assembly assembly = Assembly.ReflectionOnlyLoad(str4 + ", processorArchitecture=x86");
        //                  str3 = assembly.CodeBase;
        //                  TL.LogMessage("DriverCompatibilityX86", "     Found file path: " + str3);
        //                  assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                  if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit");
        //                  if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus");
        //                  if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly");
        //                  if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable");
        //                }
        //                catch (IOException ex2)
        //                {
        //                  //ProjectData.SetProjectError((Exception) ex2);
        //                  IOException ioException2 = ex2;
        //                  TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " + ioException1.Message);
        //                  try
        //                  {
        //                    Assembly assembly = Assembly.ReflectionOnlyLoad(str4 + ", processorArchitecture=x64");
        //                    str3 = assembly.CodeBase;
        //                    TL.LogMessage("DriverCompatibilityX64", "     Found file path: " + str3);
        //                    assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                    if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit");
        //                    if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus");
        //                    if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly");
        //                    if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable");
        //                  }
        //                  catch (Exception ex3)
        //                  {
        //                    //ProjectData.SetProjectError(ex3);
        //                    TL.LogMessageCrLf("DriverCompatibilityX64", ioException2.ToString());
        //                    //ProjectData.ClearProjectError();
        //                  }
        //                  //ProjectData.ClearProjectError();
        //                }
        //                catch (Exception ex2)
        //                {
        //                  //ProjectData.SetProjectError(ex2);
        //                  Exception exception = ex2;
        //                  TL.LogMessageCrLf("DriverCompatibilityX32", exception.ToString());
        //                  //ProjectData.ClearProjectError();
        //                }
        //                //ProjectData.ClearProjectError();
        //              }
        //              catch (Exception ex)
        //              {
        //                //ProjectData.SetProjectError(ex);
        //                Exception exception = ex;
        //                TL.LogMessageCrLf("DriverCompatibility", exception.ToString());
        //                //ProjectData.ClearProjectError();
        //              }
        //            }
        //            else
        //            {
        //              TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!");
        //              str3 = "";
        //              TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string");
        //            }
        //          }
        //          if (Operators.CompareString(Strings.Right(Strings.Trim(str3), 4).ToUpper(), ".DLL", false) == 0)
        //          {
        //            try
        //            {
        //              peReader1 = new PEReader(str3, TL);
        //              if (peReader1.BitNess == VersionCode.Bitness.Bits32)
        //                str1 = !flag ? "This 32bit only driver won't work in a 64bit application even though it is correctly registered as a 32bit COM driver.\r\nPlease contact the driver author and request an updated driver." : "This 32bit only driver won't work in a 64bit application even though it is registered as a 64bit COM driver.\r\nPlease contact the driver author and request an updated driver.";
        //              else if (!flag)
        //                str1 = "This 64bit capable driver is only registered as a 32bit COM driver.\r\nPlease contact the driver author and request an updated installer.";
        //            }
        //            catch (FileNotFoundException ex)
        //            {
        //              //ProjectData.SetProjectError((Exception) ex);
        //              str1 = "Cannot find the driver executable: \r\n\"" + str3 + "\"";
        //              //ProjectData.ClearProjectError();
        //            }
        //            catch (Exception ex)
        //            {
        //              //ProjectData.SetProjectError(ex);
        //              Exception exception = ex;
        //              EventLogCode.LogEvent("DriverCompatibilityMessage", "Exception parsing " + ProgID + ", \"" + str3 + "\"", EventLogEntryType.Error, GlobalConstants.EventLogErrors.DriverCompatibilityException, exception.ToString());
        //              str1 = "PEReader Exception, please check ASCOM application Event Log for details";
        //              //ProjectData.ClearProjectError();
        //            }
        //            if (peReader1 != null)
        //            {
        //              peReader1.Dispose();
        //              //peReader2 = (PEReader) null;
        //            }
        //          }
        //          else
        //            TL.LogMessage("DriverCompatibility", "No codebase so can't test this driver, don't give an error message, just have to take a chance!");
        //          registryKey5.Close();
        //        }
        //      }
        //      else
        //        str1 = "Unable to find a CLSID entry for this driver, please re-install.";
        //    }
        //    else
        //      str1 = "This driver is not registered for COM (can't find ProgID), please re-install.";
        //  }
        //  else
        //  {
        //    RegistryKey registryKey3 = Registry.ClassesRoot.OpenSubKey(ProgID + "\\CLSID", false);
        //    if (registryKey3 != null)
        //    {
        //      TL.LogMessage("DriverCompatibility", "     Found 32bit ProgID registration");
        //      string str2 = registryKey3.GetValue("").ToString();
        //      registryKey3.Close();
        //      RegistryKey registryKey4 = (RegistryKey) null;
        //      if (VersionCode.OSBits() == VersionCode.Bitness.Bits64)
        //      {
        //        try
        //        {
        //          registryKey1 = registryAccess.OpenSubKey(Registry.ClassesRoot, "CLSID\\" + str2, false, RegistryAccess.RegWow64Options.KEY_WOW64_32KEY);
        //        }
        //        catch (Exception ex)
        //        {
        //          //ProjectData.SetProjectError(ex);
        //          //ProjectData.ClearProjectError();
        //        }
        //        try
        //        {
        //          registryKey2 = registryAccess.OpenSubKey(Registry.ClassesRoot, "CLSID\\" + str2, false, RegistryAccess.RegWow64Options.KEY_WOW64_64KEY);
        //        }
        //        catch (Exception ex)
        //        {
        //          //ProjectData.SetProjectError(ex);
        //          //ProjectData.ClearProjectError();
        //        }
        //      }
        //      else
        //      {
        //        registryKey4 = Registry.ClassesRoot.OpenSubKey("CLSID\\" + str2);
        //        TL.LogMessage("DriverCompatibility", "     Running on a 32bit OS, 32Bit Registered: " + Conversions.ToString(registryKey4 != null));
        //      }
        //      if (VersionCode.OSBits() == VersionCode.Bitness.Bits64)
        //      {
        //        TL.LogMessage("DriverCompatibility", "     Running on a 64bit OS, 32bit Registered: " + Conversions.ToString(registryKey1 != null) + ", 64Bit Registered: " + Conversions.ToString(registryKey2 != null));
        //        registryKey4 = registryKey1 == null ? registryKey2 : registryKey1;
        //      }
        //      if (registryKey4 != null)
        //      {
        //        TL.LogMessage("DriverCompatibility", "     Found CLSID entry");
        //        RegistryKey registryKey5 = registryKey4.OpenSubKey("InprocServer32");
        //        registryKey4.Close();
        //        if (registryKey5 != null)
        //        {
        //          string str3 = registryKey5.GetValue("", (object) "").ToString();
        //          string Left = registryKey5.GetValue("CodeBase", (object) "").ToString();
        //          if (Operators.CompareString(Left, "", false) != 0)
        //            str3 = Left;
        //          if (Operators.CompareString(Strings.Trim(str3).ToUpper(), "MSCOREE.DLL", false) == 0)
        //          {
        //            TL.LogMessage("DriverCompatibility", "     Found MSCOREE.DLL");
        //            string str4 = registryKey5.GetValue("Assembly", (object) "").ToString();
        //            TL.LogMessage("DriverCompatibility", "     Found full name: " + str4);
        //            PortableExecutableKinds peKind;
        //            ImageFileMachine machine;
        //            if (Operators.CompareString(str4, "", false) != 0)
        //            {
        //              try
        //              {
        //                Assembly assembly = Assembly.ReflectionOnlyLoad(str4);
        //                str3 = assembly.CodeBase;
        //                TL.LogMessage("DriverCompatibilityMSIL", "     Found file path: " + str3);
        //                TL.LogMessage("DriverCompatibilityMSIL", "     Found full name: " + assembly.FullName + " ");
        //                assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind Required32bit");
        //                if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind PE32Plus");
        //                if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind ILOnly");
        //                if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                  TL.LogMessage("DriverCompatibilityMSIL", "     Kind Not PE Executable");
        //              }
        //              catch (IOException ex1)
        //              {
        //                //ProjectData.SetProjectError((Exception) ex1);
        //                IOException ioException1 = ex1;
        //                TL.LogMessageCrLf("DriverCompatibility", "Could not find file, trying x86 version - " + ioException1.Message);
        //                try
        //                {
        //                  Assembly assembly = Assembly.ReflectionOnlyLoad(str4 + ", processorArchitecture=x86");
        //                  str3 = assembly.CodeBase;
        //                  TL.LogMessage("DriverCompatibilityX86", "     Found file path: " + str3);
        //                  assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                  if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind Required32bit");
        //                  if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind PE32Plus");
        //                  if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind ILOnly");
        //                  if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                    TL.LogMessage("DriverCompatibilityX86", "     Kind Not PE Executable");
        //                }
        //                catch (IOException ex2)
        //                {
        //                  //ProjectData.SetProjectError((Exception) ex2);
        //                  IOException ioException2 = ex2;
        //                  TL.LogMessageCrLf("DriverCompatibilityX64", "Could not find file, trying x64 version - " + ioException1.Message);
        //                  try
        //                  {
        //                    Assembly assembly = Assembly.ReflectionOnlyLoad(str4 + ", processorArchitecture=x64");
        //                    str3 = assembly.CodeBase;
        //                    TL.LogMessage("DriverCompatibilityX64", "     Found file path: " + str3);
        //                    assembly.GetLoadedModules()[0].GetPEKind(out peKind, out machine);
        //                    if ((peKind & PortableExecutableKinds.Required32Bit) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind Required32bit");
        //                    if ((peKind & PortableExecutableKinds.PE32Plus) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind PE32Plus");
        //                    if ((peKind & PortableExecutableKinds.ILOnly) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind ILOnly");
        //                    if ((peKind & PortableExecutableKinds.NotAPortableExecutableImage) != PortableExecutableKinds.NotAPortableExecutableImage)
        //                      TL.LogMessage("DriverCompatibilityX64", "     Kind Not PE Executable");
        //                  }
        //                  catch (Exception ex3)
        //                  {
        //                    //ProjectData.SetProjectError(ex3);
        //                    TL.LogMessageCrLf("DriverCompatibilityX64", ioException2.ToString());
        //                    //ProjectData.ClearProjectError();
        //                  }
        //                  //ProjectData.ClearProjectError();
        //                }
        //                catch (Exception ex2)
        //                {
        //                  //ProjectData.SetProjectError(ex2);
        //                  Exception exception = ex2;
        //                  TL.LogMessageCrLf("DriverCompatibilityX32", exception.ToString());
        //                  //ProjectData.ClearProjectError();
        //                }
        //                //ProjectData.ClearProjectError();
        //              }
        //              catch (Exception ex)
        //              {
        //                //ProjectData.SetProjectError(ex);
        //                Exception exception = ex;
        //                TL.LogMessageCrLf("DriverCompatibility", exception.ToString());
        //                //ProjectData.ClearProjectError();
        //              }
        //            }
        //            else
        //            {
        //              TL.LogMessage("DriverCompatibility", "'AssemblyFullName is null so we can't load the assembly, we'll just have to take a chance!");
        //              str3 = "";
        //              TL.LogMessage("DriverCompatibility", "     Set InprocFilePath to null string");
        //            }
        //          }
        //          if (Operators.CompareString(Strings.Right(Strings.Trim(str3), 4).ToUpper(), ".DLL", false) == 0)
        //          {
        //            try
        //            {
        //              peReader1 = new PEReader(str3, TL);
        //              if (peReader1.BitNess == VersionCode.Bitness.Bits64)
        //                str1 = "This is a 64bit only driver and is not compatible with this 32bit application.\r\nPlease contact the driver author and request an updated driver.";
        //            }
        //            catch (FileNotFoundException ex)
        //            {
        //              //ProjectData.SetProjectError((Exception) ex);
        //              str1 = "Cannot find the driver executable: \r\n\"" + str3 + "\"";
        //              //ProjectData.ClearProjectError();
        //            }
        //            catch (Exception ex)
        //            {
        //              //ProjectData.SetProjectError(ex);
        //              Exception exception = ex;
        //              EventLogCode.LogEvent("DriverCompatibilityMessage", "Exception parsing " + ProgID + ", \"" + str3 + "\"", EventLogEntryType.Error, GlobalConstants.EventLogErrors.DriverCompatibilityException, exception.ToString());
        //              str1 = "PEReader Exception, please check ASCOM application Event Log for details";
        //              //ProjectData.ClearProjectError();
        //            }
        //            if (peReader1 != null)
        //            {
        //              peReader1.Dispose();
        //              //peReader2 = (PEReader) null;
        //            }
        //          }
        //          else
        //            TL.LogMessage("DriverCompatibility", "No codebase or not a DLL so can't test this driver, don't give an error message, just have to take a chance!");
        //          registryKey5.Close();
        //        }
        //        else
        //          TL.LogMessage("DriverCompatibility", "This is not an inprocess DLL so no need to test further and no error message to return");
        //      }
        //      else
        //      {
        //        str1 = "Unable to find a CLSID entry for this driver, please re-install.";
        //        TL.LogMessage("DriverCompatibility", "     Could not find CLSID entry!");
        //      }
        //    }
        //    else
        //      str1 = "This driver is not registered for COM (can't find ProgID), please re-install.";
        //  }
        //  TL.LogMessage("DriverCompatibility", "     Returning: \"" + str1 + "\"");
        //  return str1;
        //}

        internal enum Bitness
        {
            Bits32,
            Bits64,
            BitsMSIL,
            BitsUnknown,
        }
    }
}
