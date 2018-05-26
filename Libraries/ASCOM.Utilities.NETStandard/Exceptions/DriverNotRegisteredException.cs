// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.DriverNotRegisteredException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.Exceptions
{
  //[ComVisible(true)]
  //[Guid("0D2B7199-622D-4244-88C3-2577308F82E2")]
  [ClassInterface(ClassInterfaceType.None)]
  [Serializable]
  public class DriverNotRegisteredException : HelperException
  {
    public DriverNotRegisteredException(string message)
      : base(message)
    {
    }

    public DriverNotRegisteredException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public DriverNotRegisteredException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
