// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.CompatibilityException
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Astrometry.Exceptions
{
  //[Guid("FCE7DF74-B3AF-4ef6-AD7D-324B87492307")]
  //[ComVisible(true)]
  [ClassInterface(ClassInterfaceType.None)]
  [Serializable]
  public class CompatibilityException : HelperException
  {
    public CompatibilityException(string message)
      : base(message)
    {
    }

    public CompatibilityException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public CompatibilityException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
