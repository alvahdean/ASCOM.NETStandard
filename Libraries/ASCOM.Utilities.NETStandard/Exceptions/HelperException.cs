// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.HelperException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ASCOM;

namespace ASCOM.Utilities.Exceptions
{
  //[Guid("A29FB43E-28C5-4ed0-8C8A-889DC7170A82")]
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  [Serializable]
  public class HelperException : AscomException
  {
    public HelperException(string message)
      : base(message)
    {
    }

    public HelperException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public HelperException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
