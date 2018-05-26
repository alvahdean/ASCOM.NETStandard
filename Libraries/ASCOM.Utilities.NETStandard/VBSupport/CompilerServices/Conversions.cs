
using System;
using System.Globalization;

namespace ASCOM.Utilities
{
    public class Conversions
    {
        public static object ChangeType(object Expression, Type TargetType)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ChangeType(Expression,TargetType);
        }

        public static string FromCharAndCount(char Value, int Count)
        {
            String s = "";
            for (int i = 0; i < Count; i++)
                s += Value;
            return s;
            //return Microsoft.VisualBasic.CompilerServices.Conversions.FromCharAndCount(Value,Count);
        }

        public static string FromCharArray(char[] Value)
        {
            return new string(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.FromCharArray(Value);
        }

        public static string FromCharArraySubset(char[] Value, int StartIndex, int Length)
        {
            String s = "";
            for (int i = StartIndex; i < StartIndex+Length; i++)
                s += Value[i];
            return s;
            //return Microsoft.VisualBasic.CompilerServices.Conversions.FromCharArraySubset(Value,StartIndex,Length);
        }

        public static bool ToBoolean(string Value)
        {
            return Convert.ToBoolean(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToBoolean(Value);
        }

        public static bool ToBoolean(object Value)
        {
            return Convert.ToBoolean(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToBoolean(Value);
        }

        public static byte ToByte(string Value)
        {
            return Convert.ToByte(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToByte(Value);
        }

        public static byte ToByte(object Value)
        {
            return Convert.ToByte(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToByte(Value);
        }

        public static char ToChar(object Value)
        {
            return Convert.ToChar(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToChar(Value);
        }

        public static char ToChar(string Value)
        {
            return Convert.ToChar(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToChar(Value);
        }

        public static char[] ToCharArrayRankOne(object Value)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToCharArrayRankOne(Value);
        }

        public static char[] ToCharArrayRankOne(string Value)
        {
            return Value?.ToCharArray();
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToCharArrayRankOne(Value);
        }

        public static DateTime ToDate(object Value)
        {
            return Convert.ToDateTime(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDate(Value);
        }

        public static DateTime ToDate(string Value)
        {
            return Convert.ToDateTime(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDate(Value);
        }

        public static decimal ToDecimal(bool Value)
        {
            return Convert.ToDecimal(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDecimal(Value);
        }

        public static decimal ToDecimal(string Value)
        {
            return Convert.ToDecimal(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDecimal(Value);
        }

        public static decimal ToDecimal(object Value)
        {
            return Convert.ToDecimal(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDecimal(Value);
        }

        public static double ToDouble(string Value)
        {
            return Convert.ToDouble(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDouble(Value);
        }

        public static double ToDouble(object Value)
        {
            return Convert.ToDouble(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToDouble(Value);
        }

        public static T ToGenericParameter<T>(object Value)
        {
            return (T)Convert.ChangeType(Value,typeof(T));
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToGenericParameter<T>(Value);
        }

        public static int ToInteger(string Value)
        {
            return Convert.ToInt32(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToInteger(Value);
        }

        public static int ToInteger(object Value)
        {
            return Convert.ToInt32(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToInteger(Value);
        }

        public static long ToLong(string Value)
        {
            return Convert.ToInt64(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToLong(Value);
        }

        public static long ToLong(object Value)
        {
            return Convert.ToInt64(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToLong(Value);
        }

        public static sbyte ToSByte(string Value)
        {
            return Convert.ToSByte(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToSByte(Value);
        }

        public static sbyte ToSByte(object Value)
        {
            return Convert.ToSByte(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToSByte(Value);
        }

        public static short ToShort(object Value)
        {
            return Convert.ToInt16(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToShort(Value);
        }

        public static short ToShort(string Value)
        {
            return Convert.ToInt16(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToShort(Value);
        }

        public static float ToSingle(string Value)
        {
            return Convert.ToSingle(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToSingle(Value);
        }

        public static float ToSingle(object Value)
        {
            return Convert.ToSingle(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToSingle(Value);
        }

        public static string ToString(int Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(object Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(decimal Value, NumberFormatInfo NumberFormat)
        {
            return Convert.ToString(Value,NumberFormat);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(decimal Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(DateTime Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(double Value, NumberFormatInfo NumberFormat)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(float Value, NumberFormatInfo NumberFormat)
        {
            return Convert.ToString(Value,NumberFormat);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value,NumberFormat);
        }

        public static string ToString(double Value)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(float Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(ulong Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(byte Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(uint Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(short Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(char Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(bool Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static string ToString(long Value)
        {
            return Convert.ToString(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToString(Value);
        }

        public static uint ToUInteger(object Value)
        {
            return Convert.ToUInt32(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToUInteger(Value);
        }

        public static uint ToUInteger(string Value)
        {
            return Convert.ToUInt32(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToUInteger(Value);
        }

        public static ulong ToULong(string Value)
        {
            return Convert.ToUInt64(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToULong(Value);
        }

        public static ulong ToULong(object Value)
        {
            return Convert.ToUInt64(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToULong(Value);
        }

        public static ushort ToUShort(string Value)
        {
            return Convert.ToUInt16(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToUShort(Value);
        }

        public static ushort ToUShort(object Value)
        {
            return Convert.ToUInt16(Value);
            //return Microsoft.VisualBasic.CompilerServices.Conversions.ToUShort(Value);
        }
    }
}