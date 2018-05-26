// Decompiled with JetBrains decompiler
// Type: TiGra.Diagnostics
// Assembly: ASCOM.SettingsProvider, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 529D6A0A-5BB4-47DC-9909-E8E6ECFC7145
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.SettingsProvider\6.0.0.0__565de7938946fba7\ASCOM.SettingsProvider.dll

using System.Diagnostics;

namespace TiGra
{
  internal class Diagnostics
  {
    private static string[] TraceLevels = new string[5]
    {
      TraceLevel.Off.ToString(),
      TraceLevel.Error.ToString(),
      TraceLevel.Warning.ToString(),
      TraceLevel.Info.ToString(),
      TraceLevel.Verbose.ToString()
    };
    private static TiGra.Diagnostics theOne = new TiGra.Diagnostics();
    private static TraceSwitch ts;

    protected Diagnostics()
    {
      TiGra.Diagnostics.ts = new TraceSwitch("TiGra.ASCOM", "TiGra.ASCOM", "Warning");
      Trace.WriteLine(string.Format("===== TiGra.ASCOM Start Diagnostics: TraceLevel = {0} =====", (object) TiGra.Diagnostics.ts.Level));
    }

    public static TiGra.Diagnostics GetInstance()
    {
      if (TiGra.Diagnostics.theOne == null)
        TiGra.Diagnostics.theOne = new TiGra.Diagnostics();
      return TiGra.Diagnostics.theOne;
    }

    public static void TraceError(object msg)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceError, msg, string.Format("{0}[Error]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceError(string format, params object[] items)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceError, string.Format(format, items), string.Format("{0}[Error]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceWarning(object msg)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceWarning, msg, string.Format("{0}[Warn]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceWarning(string format, params object[] items)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceWarning, string.Format(format, items), string.Format("{0}[Warn]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceInfo(object msg)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceInfo, msg, string.Format("{0}[Info]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceInfo(string format, params object[] items)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceInfo, string.Format(format, items), string.Format("{0}[Info]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceVerbose(object msg)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceVerbose, msg, string.Format("{0}[Verb]", (object) TiGra.Diagnostics.ts.Description));
    }

    public static void TraceVerbose(string format, params object[] items)
    {
      Trace.WriteLineIf(TiGra.Diagnostics.ts.TraceVerbose, string.Format(format, items), string.Format("{0}[Verb]", (object) TiGra.Diagnostics.ts.Description));
    }
  }
}
