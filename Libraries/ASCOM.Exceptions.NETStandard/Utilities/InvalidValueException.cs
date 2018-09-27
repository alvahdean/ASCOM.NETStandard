// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.InvalidValueException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.Exceptions
{
  //[ComVisible(true)]
  //[Guid("A9C2CF73-C139-4fae-B47B-36F18C49B527")]
  [ClassInterface(ClassInterfaceType.None)]
  [Serializable]
  public class InvalidValueException : HelperException
  {
    public InvalidValueException(string message)
      : base(message)
    {
    }

    public InvalidValueException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public InvalidValueException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
