// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.ProfileNotFoundException
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ASCOM.Utilities.Exceptions
{
  [ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  //[Guid("21AEDC6B-CC7F-4101-BC33-532DFEDEB7B5")]
  [Serializable]
  public class ProfileNotFoundException : HelperException
  {
    public ProfileNotFoundException(string message)
      : base(message)
    {
    }

    public ProfileNotFoundException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ProfileNotFoundException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
