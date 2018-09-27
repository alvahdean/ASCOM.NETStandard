// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Exceptions.ProfilePersistenceException
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
  //[Guid("A38ABA4D-F872-4c2a-A19D-62DBBC761DD5")]
  [Serializable]
  public class ProfilePersistenceException : HelperException
  {
    public ProfilePersistenceException(string message)
      : base(message)
    {
    }

    public ProfilePersistenceException(string message, Exception inner)
      : base(message, inner)
    {
    }

    public ProfilePersistenceException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
