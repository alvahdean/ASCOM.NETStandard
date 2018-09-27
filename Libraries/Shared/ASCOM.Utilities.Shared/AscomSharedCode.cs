// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.AscomSharedCode
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using ASCOM.Utilities.Interfaces;

namespace ASCOM.Utilities
{
    ////[StandardModule]
    internal sealed class AscomSharedCode
    {
        internal static string ConditionPlatformVersion(string PlatformVersion, IAscomDataStore Profile, TraceLogger TL)
        {
            string str = PlatformVersion;

            if (Profile != null)
            {
                try
                {
                    string withoutExtension = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
                    TL?.LogMessage("ConditionPlatformVersion", "  ModuleFileName: \"" + withoutExtension + "\" \"" + Process.GetCurrentProcess().MainModule.FileName + "\"");
                    //if (Operators.CompareString(Strings.Left(withoutExtension.ToUpper(), 3), "IS-", false) == 0)
                    if (withoutExtension.ToUpper().StartsWith("IS-"))
                    {
                        TL?.LogMessage("ConditionPlatformVersion", "    Inno installer temporary executable detected, searching for parent process!");
                        TL?.LogMessage("ConditionPlatformVersion", "    Old Module Filename: " + withoutExtension);
                        PerformanceCounter performanceCounter = new PerformanceCounter("Process", "Creating Process ID", Process.GetCurrentProcess().ProcessName);
                        withoutExtension = Path.GetFileNameWithoutExtension(Process.GetProcessById(checked((int)Math.Round((double)performanceCounter.NextValue()))).MainModule.FileName);
                        TL?.LogMessage("ConditionPlatformVersion", "    New Module Filename: " + withoutExtension);
                        performanceCounter.Close();
                        performanceCounter.Dispose();
                    }
                    SortedList<string, string> sortedList1 = Profile.EnumProfile("ForcePlatformVersion");
                    IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator1 = null;
                    try
                    {
                        enumerator1 = sortedList1.GetEnumerator();
                        while (enumerator1.MoveNext())
                        {
                            System.Collections.Generic.KeyValuePair<string, string> current = enumerator1.Current;
                            TL?.LogMessage("ConditionPlatformVersion", "  ForcedFileName: \"" + current.Key + "\" \"" + current.Value + "\" \"" + Strings.UCase(Path.GetFileNameWithoutExtension(current.Key)) + "\" \"" + Strings.UCase(Path.GetFileName(current.Key)) + "\" \"" + Strings.UCase(current.Key) + "\" \"" + current.Key + "\" \"" + Strings.UCase(withoutExtension) + "\"");
                            string Left = !current.Key.Contains(".") ? current.Key : Path.GetFileNameWithoutExtension(current.Key);
                            if (Operators.CompareString(Left, "", false) != 0 && Strings.UCase(withoutExtension).StartsWith(Strings.UCase(Left)))
                            {
                                str = current.Value;
                                TL?.LogMessage("ConditionPlatformVersion", "  Matched file: \"" + withoutExtension + "\" \"" + Left + "\"");
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator1 != null)
                            enumerator1.Dispose();
                    }
                    SortedList<string, string> sortedList2 = Profile.EnumProfile("ForcePlatformVersionSeparator");
                    IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator2 = null;
                    try
                    {
                        enumerator2 = sortedList2.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            System.Collections.Generic.KeyValuePair<string, string> current = enumerator2.Current;
                            TL?.LogMessage("ConditionPlatformVersion", "  ForcedFileName: \"" + current.Key + "\" \"" + current.Value + "\" \"" + Strings.UCase(Path.GetFileNameWithoutExtension(current.Key)) + "\" \"" + Strings.UCase(Path.GetFileName(current.Key)) + "\" \"" + Strings.UCase(current.Key) + "\" \"" + current.Key + "\" \"" + Strings.UCase(withoutExtension) + "\"");
                            if (!current.Key.Contains(".")) { }

                            if (Operators.CompareString(Strings.UCase(Path.GetFileNameWithoutExtension(current.Key)), Strings.UCase(withoutExtension), false) == 0)
                            {
                                if (string.IsNullOrEmpty(current.Value))
                                {
                                    str = str.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                                    TL?.LogMessage("ConditionPlatformVersion", "  String IsNullOrEmpty: \"" + str + "\"");
                                }
                                else
                                {
                                    str = str.Replace(".", current.Value);
                                    TL?.LogMessage("ConditionPlatformVersion", "  String Is: \"" + current.Value + "\" \"" + str + "\"");
                                }
                                TL?.LogMessage("ConditionPlatformVersion", "  Matched file: \"" + withoutExtension + "\" \"" + current.Key + "\"");
                            }
                        }
                    }
                    finally
                    {
                        if (enumerator2 != null)
                            enumerator2.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    TL?.LogMessageCrLf("ConditionPlatformVersion", "Exception: " + exception.ToString());
                    EventLogCode.LogEvent("ConditionPlatformVersion", "Exception: ", EventLogEntryType.Error, GlobalConstants.EventLogErrors.VB6HelperProfileException, exception.ToString());
                    //ProjectData.ClearProjectError();
                }
            }
            TL?.LogMessage("ConditionPlatformVersion", "  Returning: \"" + str + "\"");
            return str;
        }
    }
}
