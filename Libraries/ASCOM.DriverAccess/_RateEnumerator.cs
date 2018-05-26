// Decompiled with JetBrains decompiler
// Type: ASCOM.DriverAccess._RateEnumerator
// Assembly: ASCOM.DriverAccess, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 4B15C8A3-EB2C-468B-8912-4BBDBC985936
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.DriverAccess\6.0.0.0__565de7938946fba7\ASCOM.DriverAccess.dll

using System;
using System.Collections;
using System.Reflection;

namespace ASCOM.DriverAccess
{
  internal class _RateEnumerator : IEnumerator, IDisposable
  {
    private IEnumerator objEnumerator;

    public object Current
    {
      get
      {
        return (object) new _Rate(this.objEnumerator.Current);
      }
    }

    public _RateEnumerator(Type objTypeAxisRates, object objAxisRatesLateBound)
    {
      this.objEnumerator = (IEnumerator) objTypeAxisRates.InvokeMember("GetEnumerator", BindingFlags.InvokeMethod, (Binder) null, objAxisRatesLateBound, new object[0]);
    }

    public void Reset()
    {
      this.objEnumerator.Reset();
    }

    public bool MoveNext()
    {
      return this.objEnumerator.MoveNext();
    }

    public void Dispose()
    {
      if (this.objEnumerator == null)
        return;
      this.objEnumerator = (IEnumerator) null;
    }
  }
}
