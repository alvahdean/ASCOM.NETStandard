
using System;
using System.Linq;
using System.Text;

namespace ASCOM.Internal
{
  public static class StringExtensions
  {
    public static string Head(this string source, int length)
    {
      if (length > source.Length)
        throw new ArgumentOutOfRangeException("source", "The specified length is greater than the length of the string.");
      return source.Substring(0, length);
    }

    public static string Tail(this string source, int length)
    {
      int length1 = source.Length;
      if (length > length1)
        throw new ArgumentOutOfRangeException("source", "The specified length is greater than the length of the string.");
      return source.Substring(length1 - length, length);
    }

    public static string Clean(this string source, string allowedCharacters)
    {
      if (string.IsNullOrEmpty(source))
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder(source.Length);
      foreach (char ch in source)
      {
        if (allowedCharacters.Contains<char>(ch))
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    public static string RemoveHead(this string source, int length)
    {
      if (length < 1)
        return source;
      return source.Tail(source.Length - length);
    }

    public static string RemoveTail(this string source, int length)
    {
      if (length < 1)
        return source;
      return source.Head(source.Length - length);
    }
  }
}
