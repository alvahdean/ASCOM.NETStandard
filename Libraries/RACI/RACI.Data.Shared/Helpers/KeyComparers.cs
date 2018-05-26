using System;
using System.Collections.Generic;
using System.Text;

namespace RACI.Data
{
    public class CIKeyComparer : IEqualityComparer<String>
    {
        public bool Equals(string x, string y) => x.ToLower() == y.ToLower();
        public int GetHashCode(string obj) => obj?.ToLower().GetHashCode() ?? 0;
    }

    public class CSKeyComparer : IEqualityComparer<String>
    {
        public bool Equals(string x, string y) => x.Equals(y);
        public int GetHashCode(string obj) => obj?.GetHashCode() ?? 0;
    }
}
