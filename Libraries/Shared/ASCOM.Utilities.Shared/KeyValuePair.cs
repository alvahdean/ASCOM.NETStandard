// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.KeyValuePair
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using ASCOM.Utilities.Interfaces;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
  //[ClassInterface(ClassInterfaceType.None)]
  //[ComVisible(true)]
  //[Guid("69CFE7E6-E64F-4045-8D0D-C61F50F31CAC")]
  public class KeyValuePair : IKeyValuePair
  {
    private string m_Key;
    private string m_Value;

    public string Key
    {
      get
      {
        return this.m_Key;
      }
      set
      {
        this.m_Key = value;
      }
    }

    public string Value
    {
      get
      {
        return this.m_Value;
      }
      set
      {
        this.m_Value = value;
      }
    }

    public KeyValuePair()
    {
      this.m_Key = "";
      this.m_Value = "";
    }

    public KeyValuePair(string Key, string Value)
    {
      this.m_Key = Key;
      this.m_Value = Value;
    }
  }
}
