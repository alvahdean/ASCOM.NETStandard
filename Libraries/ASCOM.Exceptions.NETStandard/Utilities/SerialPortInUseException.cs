// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.SerialPortInUseException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.Exceptions
{
  //[ComVisible(true)]
  //[Guid("7A3CFD64-D7E3-48b0-BEB6-5696CF7599B3")]
  [ClassInterface(ClassInterfaceType.None)]
  [Serializable]
  public class SerialPortInUseException : HelperException
  {
    public SerialPortInUseException(string message)
      : base(message)
    {
    }

    public SerialPortInUseException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public SerialPortInUseException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
