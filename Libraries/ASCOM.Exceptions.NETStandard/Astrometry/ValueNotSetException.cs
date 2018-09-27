// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.ValueNotSetException
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Astrometry.Exceptions
{
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("4CFCC2FF-6348-4268-B481-E92BE3B30039")]
  //[ComVisible(true)]
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
