// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.TransformUninitialisedException
// Assembly: ASCOM.Astrometry, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 695EF5AA-123E-4689-AAF5-64FD1324F6BB
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Astrometry\6.0.0.0__565de7938946fba7\ASCOM.Astrometry.dll

using ASCOM.Utilities.Exceptions;
using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Astrometry.Exceptions
{
  //[ComVisible(true)]
  //[Guid("A8B9A15E-0F01-46ce-AF6E-BEFD3CB9E2BC")]
  [ClassInterface(ClassInterfaceType.None)]
  [Serializable]
  public class TransformUninitialisedException : HelperException
  {
    public TransformUninitialisedException(string message)
      : base(message)
    {
    }

    public TransformUninitialisedException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public TransformUninitialisedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
