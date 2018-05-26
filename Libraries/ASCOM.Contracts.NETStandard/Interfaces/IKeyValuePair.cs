// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IKeyValuePair
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
    /// <summary>
    /// Generic base type for string keyed pairs, original IKeyValuePair is now derived from this generic form
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    //[ComVisible(true)]
    //[Guid("CA653783-E47D-4e9d-9759-3B91BE0F4340")]
    public interface IKeyValuePair<TValue>
    {
        [DispId(1)]
        string Key { get; set; }
        [DispId(0)]
        TValue Value { get; set; }
    }

    //[ComVisible(true)]
    //[Guid("CA653783-E47D-4e9d-9759-3B91BE0F4340")]
    public interface IKeyValuePair : IKeyValuePair<String> { }
    //{
    //[DispId(1)]
    //string Key { get; set; }

    //[DispId(0)]
    //string Value { get; set; }
    //}

}
