using System;
using System.Linq;
using System.Text;

namespace RACI.Utils
{
    public static class RaciUtil
    {
        #region Internal implementations
        private static string GetBitsConvert(byte value) => Convert.ToString(value, 2).PadLeft(8, '0');
        private static string GetBitsLoop(byte value)
        {
            char[] b = new char[8];
            int pos = 7;
            int i = 0;
            while (i < 8)
            {
                if ((value & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
        #endregion

        #region Util enums
        [Flags]
        public enum BitComparison
        {
            //Use the default comparison
            Default=All,
            //Check if value has all of the bits set in the specified bitmask
            All=1,
            //Check if value has any of the bits set in the specified bitmask
            Any=2,
            //Check if value has no other bits set other than ones in the specified bitmask
            Only=4,
            //Check if value has no other bits set other than ones in the specified bitmask
            //and that all bits in the mask are present in the value
            //Note: This is simply x == y for integers
            Exact=8,
            //Check if value does NOT have any of the bits set in the specified bitmask
            None=16,
            NoEmptyValueAndMask=2^31
        }
        #endregion
        public static string GetBits(byte[] bArr)
        {
            StringBuilder sb = new StringBuilder();
            foreach(byte b in bArr.Reverse())
                sb.Append(GetBits(b));
            return sb.ToString();
        }
        public static string GetBits(byte value) => GetBitsConvert(value);
        public static string GetBits(bool value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(char value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(double value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(short value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(int value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(long value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(float value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(ushort value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(uint value) => GetBits(BitConverter.GetBytes(value));
        public static string GetBits(ulong value) => GetBits(BitConverter.GetBytes(value));

        public static bool IsEnumBitmask<T>() where T : struct => IsEnumBitmask(typeof(T));

        public static bool IsEnumBitmask(Type enumType)
        {
            return enumType != null
                && enumType.IsEnum
                && enumType.CustomAttributes.Select(t => t.AttributeType).Contains(typeof(FlagsAttribute));
        }
        public static bool IsSingleBit(int value)=> GetBits(value).Replace("0", "").Length < 2;
        /// <summary>
        /// Implements various bitwise comparisons on an int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bitmask"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static bool BitCompare(int value, int bitmask, BitComparison mode)
        {
            bool result = false;
            bool isEmptyValue = bitmask == 0;
            bool isEmptyMask = bitmask == 0;
            bool emptyResult = (mode & BitComparison.NoEmptyValueAndMask)==0;
            switch (mode)
            {
                case BitComparison.Any:
                    result = isEmptyValue 
                        ? isEmptyMask && emptyResult
                        : (value & bitmask) != 0;
                    break;
                case BitComparison.All:
                    result = isEmptyValue
                        ? isEmptyMask && emptyResult
                        : (value & bitmask) == bitmask;
                    break;
                case BitComparison.Only:
                    result = isEmptyValue
                        ? isEmptyMask && emptyResult
                        : (value ^ bitmask) != 0;
                    break;
                case BitComparison.Exact:
                    result = isEmptyValue
                        ? isEmptyMask && emptyResult
                        : value == bitmask;
                    break;
                case BitComparison.None:
                    result = (value & bitmask) == 0;
                    break;
            }
            return result;
        }
    }
}
