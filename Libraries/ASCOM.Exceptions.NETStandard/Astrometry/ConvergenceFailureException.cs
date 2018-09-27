// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.ConvergenceFailureException
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
  //[ComVisible(true)]
  //[Guid("34102500-664A-4C9E-92A2-0F72D773AEAE")]
  [Serializable]
  public class ConvergenceFailureException : HelperException
  {
    public ConvergenceFailureException(string message)
      : base(message)
    {
    }

    public ConvergenceFailureException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ConvergenceFailureException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
