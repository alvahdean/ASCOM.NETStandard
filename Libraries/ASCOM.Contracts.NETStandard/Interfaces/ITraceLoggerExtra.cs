// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.ITraceLoggerExtra
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(false)]
  public interface ITraceLoggerExtra
  {
    void LogContinue(string Message);

    void LogFinish(string Message);

    void LogMessage(string Identifier, string Message);
  }
}
