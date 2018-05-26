// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.ValueNotSetException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.Exceptions
{
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("C893C94C-3D48-4068-8BCE-6CED6AEF2512")]
  [Serializable]
  public class ValueNotSetException : HelperException
  {
    public ValueNotSetException(string message)
      : base(message)
    {
    }

    public ValueNotSetException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ValueNotSetException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
