using System;

namespace ASCOM.Utilities
{
    public class Conversion
    {
        public static string ErrorToString()
        {
            return "";
            //return Microsoft.VisualBasic.Conversion.ErrorToString();
        }

        public static string ErrorToString(int ErrorNumber)
        {
            return $"Error[{ErrorNumber}]";
            //return Microsoft.VisualBasic.Conversion.ErrorToString(ErrorNumber);
        }

        public static short Fix(short Number)
        {
            return (short)((int)Number);
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static int Fix(int Number)
        {
            return (int)Number;
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static long Fix(long Number)
        {
            return (long)((long)Number);
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static double Fix(double Number)
        {
            return (double)((int)Number);
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static float Fix(float Number)
        {
            return (float)((int)Number);
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static decimal Fix(decimal Number)
        {
            return (decimal)((int)Number);
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static object Fix(object Number)
        {
            return (int)Number;
            //return Microsoft.VisualBasic.Conversion.Fix(Number);
        }

        public static string Hex(object Number)
        {
            uint n = Convert.ToUInt32(Number);
            return $"{n:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(ulong Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(long Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(uint Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(int Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(ushort Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(byte Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(short Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static string Hex(sbyte Number)
        {
            return $"{Number:X}";
            //return Microsoft.VisualBasic.Conversion.Hex(Number);
        }

        public static object Int(object Number)
        {
            return Convert.ToInt32(Number);
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static decimal Int(decimal Number)
        {
            return Convert.ToInt32(Number);
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static float Int(float Number)
        {
            return Convert.ToInt32(Number);
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static double Int(double Number)
        {
            return Convert.ToInt32(Number);
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static long Int(long Number)
        {
            return Convert.ToInt32(Number);
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static int Int(int Number)
        {
            return Number;
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static short Int(short Number)
        {
            return Number;
            //return Microsoft.VisualBasic.Conversion.Int(Number);
        }

        public static string Oct(object Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(ulong Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(long Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(uint Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(short Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(ushort Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(byte Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(sbyte Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Oct(int Number)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Conversion.Oct(Number);
        }

        public static string Str(object Number)
        {
            return Number?.ToString() ?? "";
            //return Microsoft.VisualBasic.Conversion.Str(Number);
        }

        public static int Val(char Expression)
        {
            return (int)Expression;
            //return Microsoft.VisualBasic.Conversion.Val(Expression);
        }

        public static double Val(string InputStr)
        {
            return Double.Parse(InputStr);
            //return Microsoft.VisualBasic.Conversion.Val(InputStr);
        }

        public static double Val(object Expression)
        {
            return Convert.ToDouble(Expression);
            //return Microsoft.VisualBasic.Conversion.Val(Expression);
        }

    }
}