// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.PEReader
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System;

namespace ASCOM.Utilities
{
#warning Internal class exposed as public during porting: Operators
    public class Operators
    //internal class Operators
    {
        public static int CompareString(object p, string v1, bool v2)
        {
            return StringComparer.InvariantCulture.Compare(p, v1);
        }
    }
}