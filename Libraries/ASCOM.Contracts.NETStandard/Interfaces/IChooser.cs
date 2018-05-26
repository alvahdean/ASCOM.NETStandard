// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IChooser
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[Guid("D398FD76-F4B8-48a2-9CA3-2EF0DD8B98E1")]
  //[ComVisible(true)]
  public interface IChooser : IDisposable
  {
    [DispId(1)]
    string DeviceType { get; set; }

    [DispId(2)]
    string Choose(string DriverProgID);
  }
}
