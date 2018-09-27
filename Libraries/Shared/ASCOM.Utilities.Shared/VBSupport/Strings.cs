using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace ASCOM.Utilities
{
    public class Strings
    {
        public static int Asc(char c)
        {
            return Microsoft.VisualBasic.Strings.AscW(c);
        }
        public static int Asc(string s)
        {
            return Microsoft.VisualBasic.Strings.AscW(s);
        }
        public static int AscW(string s)
        {
            return Microsoft.VisualBasic.Strings.AscW(s);
        }
        public static int AscW(char c)
        {
            return Microsoft.VisualBasic.Strings.AscW(c);
        }
        public static char Chr(int CharCode)
        {
            return Microsoft.VisualBasic.Strings.ChrW(CharCode);
        }
        public static char ChrW(int CharCode)
        {
            return Microsoft.VisualBasic.Strings.ChrW(CharCode);
        }
        public static string[] Filter(string[] Source, string Match, bool Include = true, CompareMethod Compare = CompareMethod.Binary)
        {
            List<String> result = new List<string>();
            foreach (string s in Source)
            {
                bool match = s.Contains(Match);
                if ((match && Include) || !match && !Include)
                    result.Add(s);
            }
            return result.ToArray();
            //return Microsoft.VisualBasic.Strings.Filter(Source, Match, Include, (Microsoft.VisualBasic.CompareMethod)Compare);
            //return Microsoft.VisualBasic.Strings.Filter(Source, Match, Include, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static string Format(object Expression, string Style = "")
        {
               if (Expression == null)
                return "";
            if (string.IsNullOrWhiteSpace(Style))
                return Convert.ToString(Expression);
            if (Expression.GetType() == typeof(DateTime))
                return ((DateTime)Expression).ToString(Style);
            string fmt = $"{{0:{Style}}}";
            return String.Format(fmt, Expression);
            //return Microsoft.VisualBasic.Strings.Format(Expression, Style);
        }
        public static string FormatCurrency(double Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            CultureInfo cult = CultureInfo.CurrentCulture;
            NumDigitsAfterDecimal = NumDigitsAfterDecimal >= 0 ? NumDigitsAfterDecimal : cult.NumberFormat.CurrencyDecimalDigits;
            //There are 15 NumberFormat.CurrencyNegativePattern options... 
            //  0 ="($n)" seems the best match for a default to match useParens==true
            //  1 ="-$n" seems the best match for a default to match useParens==false
            int useParens = UseParensForNegativeNumbers==TriState.UseDefault 
                ? (cult.NumberFormat.CurrencyNegativePattern == 0) ?  0 : 1
                : UseParensForNegativeNumbers==TriState.True ? 0 : 1;

            if (NumDigitsAfterDecimal > 99)
                throw new ArgumentOutOfRangeException($"{nameof(NumDigitsAfterDecimal)} must be less than 100");
            double amt = Math.Round(Expression, NumDigitsAfterDecimal);
            CultureInfo adjCult = new CultureInfo(cult.Name);
            adjCult.NumberFormat.CurrencyNegativePattern = useParens;
            return amt.ToString("C",adjCult);
           //return Microsoft.VisualBasic.Strings.FormatCurrency(Expression, NumDigitsAfterDecimal, (Microsoft.VisualBasic.TriState)IncludeLeadingDigit, (Microsoft.VisualBasic.TriState)UseParensForNegativeNumbers, (Microsoft.VisualBasic.TriState)GroupDigits);
        }
        public static string FormatDateTime(DateTime Expression, DateFormat NamedFormat = DateFormat.GeneralDate)
        {
            switch(NamedFormat)
            {
                case DateFormat.LongDate:
                    return Expression.ToLongDateString();
                case DateFormat.LongTime:
                    return Expression.ToLongTimeString();
                case DateFormat.ShortDate:
                    return Expression.ToShortDateString();
                case DateFormat.ShortTime:
                    return Expression.ToShortTimeString();
                case DateFormat.GeneralDate:
                default:
                    return Expression.ToString();
            }
            //return Microsoft.VisualBasic.Strings.FormatDateTime(Expression, (Microsoft.VisualBasic.DateFormat)NamedFormat);
        }
        public static string FormatNumber(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            double num = Convert.ToDouble(Expression);
            if (NumDigitsAfterDecimal >= 0)
                num = Math.Round(num, NumDigitsAfterDecimal);
            if (num < 0 && UseParensForNegativeNumbers == TriState.True)
                return $"({num})";
            return $"{num}";
            //return Microsoft.VisualBasic.Strings.FormatNumber(Expression, NumDigitsAfterDecimal, (Microsoft.VisualBasic.TriState)IncludeLeadingDigit, (Microsoft.VisualBasic.TriState)UseParensForNegativeNumbers, (Microsoft.VisualBasic.TriState)GroupDigits);
        }
        public static string FormatPercent(object Expression, int NumDigitsAfterDecimal = -1, TriState IncludeLeadingDigit = TriState.UseDefault, TriState UseParensForNegativeNumbers = TriState.UseDefault, TriState GroupDigits = TriState.UseDefault)
        {
            double num = Convert.ToDouble(Expression);
            if (NumDigitsAfterDecimal >= 0)
                num = Math.Round(num,NumDigitsAfterDecimal);
            if (num < 0 && UseParensForNegativeNumbers == TriState.True)
                return $"({num*100}%)";
            return $"{num*100}%";
            //return Microsoft.VisualBasic.Strings.FormatPercent(Expression, NumDigitsAfterDecimal, (Microsoft.VisualBasic.TriState)IncludeLeadingDigit, (Microsoft.VisualBasic.TriState)UseParensForNegativeNumbers, (Microsoft.VisualBasic.TriState)GroupDigits);
        }
        public static char GetChar(string str, int Index)
        {
            //Adjust for VB 1 based index
            Index--;
            if (Index < 0 || Index >= str.Length)
                throw new ArgumentOutOfRangeException($"Index '{Index}' is out of range (1 based)");
            return str[Index];
            //return Microsoft.VisualBasic.Strings.GetChar(str, Index);
        }
        public static int InStr(int Start, string String1, string String2, CompareMethod Compare = CompareMethod.Binary)
        {
            //Adjust for VB 1 based index
            Start--;

            if (Compare == CompareMethod.Binary)
                return String1.IndexOf(String2, Start,StringComparison.Ordinal);
            else 
                return String1.IndexOf(String2, Start, StringComparison.InvariantCulture);
            //return Microsoft.VisualBasic.Strings.InStr(Start, String1, String2, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static int InStr(string String1, string String2, CompareMethod Compare = CompareMethod.Binary)
        {
            return InStr(1, String1, String2, Compare);
            //return Microsoft.VisualBasic.Strings.InStr(String1, String2, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static int InStrRev(string StringCheck, string StringMatch, int Start = 1, CompareMethod Compare = CompareMethod.Binary)
        {
            return InStr(Start, StrReverse(StringCheck), StringMatch, Compare);
            //return Microsoft.VisualBasic.Strings.InStrRev(StringCheck, StringMatch, Start, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static string Join(string[] SourceArray, string Delimiter = " ")
        {
            return String.Join(Delimiter, SourceArray);
            //return Microsoft.VisualBasic.Strings.Join(SourceArray, Delimiter);
        }
        public static string Join(object[] SourceArray, string Delimiter = " ")
        {
            return String.Join(Delimiter, SourceArray.Select(t=>t.ToString()));
            //return Microsoft.VisualBasic.Strings.Join(SourceArray, Delimiter);
        }
        public static char LCase(char Value)
        {
            return Value.ToString().ToLowerInvariant().ToCharArray()[0];
            //return Microsoft.VisualBasic.Strings.LCase(Value);
        }
        public static string LCase(string Value)
        {
            return Value?.ToLowerInvariant();
            //return Microsoft.VisualBasic.Strings.LCase(Value);
        }
        public static string Left(string str, int Length)
        {
            return str?.Substring(0, Length);
            //return Microsoft.VisualBasic.Strings.Left(str, Length);
        }
        public static int Len(DateTime Expression)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(object Expression)
        {
            return Len(Expression.ToString());
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(string Expression)
        {
            return Expression?.Length??0;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(double Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(char Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(decimal Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(byte Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(short Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }

        public static int Len(ushort Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(int Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }

        public static int Len(uint Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(float Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(sbyte Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(bool Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(long Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static int Len(ulong Expression)
        {
            return $"{Expression}".Length;
            //return Microsoft.VisualBasic.Strings.Len(Expression);
        }
        public static string LSet(string Source, int Length)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.LSet(Source, Length);
        }
        public static string LTrim(string str)
        {
            return str?.TrimStart();
            //return Microsoft.VisualBasic.Strings.LTrim(str);
        }
        public static string Mid(string str, int Start, int Length)
        {
            //Adjust for VB 1 based index
            Start--;
            return str?.Substring(Start, Length);
            //return Microsoft.VisualBasic.Strings.Mid(str, Start, Length);
        }
        public static string Mid(string str, int Start)
        {
            //Adjust for VB 1 based index
            Start--;
            return str?.Substring(Start);
            
            //return Microsoft.VisualBasic.Strings.Mid(str, Start);
        }
        public static string Replace(string Expression, string Find, string Replacement, int Start = 1, int Count = -1, CompareMethod Compare = CompareMethod.Binary)
        {
            //Adjust for VB 1 based index
            Start--;
            if (Start == 0 && Count==-1)
                return Expression.Replace(Find, Replacement);
            throw new NotImplementedException("Replace only handles full string search/replace currently...");
            //return Microsoft.VisualBasic.Strings.Replace(Expression, Find, Replacement, Start, Count, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static string Right(string str, int Length)
        {
            return str?.Substring(str.Length - Length);
            //return Microsoft.VisualBasic.Strings.Right(str, Length);
        }
        public static string RSet(string Source, int Length)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.RSet(Source, Length);
        }
        public static string RTrim(string str)
        {
            return str?.TrimEnd();
            //return Microsoft.VisualBasic.Strings.RTrim(str);
        }
        public static string Space(int Number)
        {
            return StrDup(Number, ' ');
            //return Microsoft.VisualBasic.Strings.Space(Number);
        }
        public static string[] Split(string Expression, string Delimiter = " ", int Limit = -1, CompareMethod Compare = CompareMethod.Binary)
        {
            return Expression?.Split(new string[] { Delimiter }, Limit, StringSplitOptions.RemoveEmptyEntries);
            //return Microsoft.VisualBasic.Strings.Split(Expression, Delimiter, Limit, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static int StrComp(string String1, string String2, CompareMethod Compare = CompareMethod.Binary)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.StrComp(String1, String2, (Microsoft.VisualBasic.CompareMethod)Compare);
        }
        public static string StrConv(string str, VbStrConv Conversion, int LocaleID = 0)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.StrConv(str, (Microsoft.VisualBasic.VbStrConv)Conversion, LocaleID);
        }
        public static string StrDup(int Number, string Character)
        {
            string s = "";
            for (int i = 0; i < Number; i++)
                s += Character;
            return s;
            //return Microsoft.VisualBasic.Strings.StrDup(Number, Character);
        }
        public static string StrDup(int Number, char Character)
        {
            string s = "";
            for (int i = 0; i < Number; i++)
                s += Character;
            return s;
            //return Microsoft.VisualBasic.Strings.StrDup(Number, Character);
        }
        public static object StrDup(int Number, object Character)
        {
            throw new NotImplementedException();
            //return Microsoft.VisualBasic.Strings.StrDup(Number, Character);
        }
        public static string StrReverse(string Expression)
        {
            return new String(Expression.ToCharArray().Reverse().ToArray());
            //return Microsoft.VisualBasic.Strings.StrReverse(Expression);
        }
        public static string Trim(string str)
        {
            return str?.Trim();
            //return Microsoft.VisualBasic.Strings.Trim(str);
        }
        public static string UCase(string Value)
        {
            return Value?.ToUpperInvariant();
            //return Microsoft.VisualBasic.Strings.UCase(Value);
        }
        public static char UCase(char Value)
        {
            return Value.ToString().ToUpperInvariant().ToCharArray()[0];
            //return Microsoft.VisualBasic.Strings.UCase(Value);
        }
    }
}