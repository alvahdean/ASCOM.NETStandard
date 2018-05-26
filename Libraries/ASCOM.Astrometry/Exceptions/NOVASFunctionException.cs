// Decompiled with JetBrains decompiler
// Type: ASCOM.Astrometry.Exceptions.NOVASFunctionException
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
  [ClassInterface(ClassInterfaceType.None)]
  //[Guid("7E2164AD-F002-4b30-98A1-BE1CEC954260")]
  [Serializable]
  public class NOVASFunctionException : HelperException
  {
    public NOVASFunctionException(string message, string FuncName, short ErrCode)
      : base(message + " Error returned from function " + FuncName + " - error code: " + ErrCode.ToString())
    {
    }

    public NOVASFunctionException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public NOVASFunctionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
