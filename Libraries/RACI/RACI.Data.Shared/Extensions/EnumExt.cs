using RACI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RACI.Data
{
    public static class EnumExt
    {
        #region RaciStringComparison extensions

        public static RaciStringComparison Normalize(this RaciStringComparison value)
        {
            return value.HasFlag(RaciStringComparison.Ordinal) && value.HasFlag(RaciStringComparison.Invariant)
                ? value ^ RaciStringComparison.Invariant
                : value;
        }
        #endregion
        #region Generic Enum extensions

        public static String EnumText(this RaciStringComparison value, string delimiter = " | ") =>
            EnumText<RaciStringComparison>(value, delimiter);
        public static String EnumText(this StringComparison value, string delimiter = " | ") =>
            EnumText<StringComparison>(value, delimiter);
        private static String EnumText<T>(T value, string delimiter = " | ")
            where T : struct
        {
            List<string> arr = new List<string>();
            Type eType = typeof(T);
            if (!eType.IsEnum)
                return null;
            int ival = Convert.ToInt32(value);
            bool isMaskable = RaciUtil.IsEnumBitmask(eType);

            foreach (var ev in Enum.GetValues(eType))
            {
                int iev = (int)ev;
                if (isMaskable && RaciUtil.IsSingleBit(iev) && (ival & (int)ev) != 0)
                    arr.Add(Enum.GetName(eType, ev));
                else if (!isMaskable && ival == iev)
                    arr.Add(Enum.GetName(eType, ev));
            }
            return arr.Count>0 ? String.Join(delimiter, arr) : "Default";
        }
        #endregion

    }
}
