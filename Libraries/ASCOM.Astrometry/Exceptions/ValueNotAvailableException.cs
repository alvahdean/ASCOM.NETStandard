// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.ValueNotAvailableException
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Astrometry.Exceptions
{
  //[Guid("F934C471-CFA7-478c-A25E-CED11236EF1A")]
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  [Serializable]
  public class ValueNotAvailableException : HelperException
  {
    public ValueNotAvailableException(string message)
      : base(message)
    {
    }

    public ValueNotAvailableException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ValueNotAvailableException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
