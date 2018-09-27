// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Interfaces.IUtilExtra
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(false)]
  public interface IUtilExtra
  {
    string DegreesToDMS(double Degrees);

    string DegreesToDMS(double Degrees, string DegDelim);

    string DegreesToDMS(double Degrees, string DegDelim, string MinDelim);

    string DegreesToDMS(double Degrees, string DegDelim, string MinDelim, string SecDelim);

    string HoursToHMS(double Hours);

    string HoursToHMS(double Hours, string HrsDelim);

    string HoursToHMS(double Hours, string HrsDelim, string MinDelim);

    string HoursToHMS(double Hours, string HrsDelim, string MinDelim, string SecDelim);

    string DegreesToDM(double Degrees);

    string DegreesToDM(double Degrees, string DegDelim);

    string DegreesToDM(double Degrees, string DegDelim, string MinDelim);

    string HoursToHM(double Hours);

    string HoursToHM(double Hours, string HrsDelim);

    string HoursToHM(double Hours, string HrsDelim, string MinDelim);

    string DegreesToHMS(double Degrees);

    string DegreesToHMS(double Degrees, string HrsDelim);

    string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim);

    string DegreesToHMS(double Degrees, string HrsDelim, string MinDelim, string SecDelim);

    string DegreesToHM(double Degrees);

    string DegreesToHM(double Degrees, string HrsDelim);

    string DegreesToHM(double Degrees, string HrsDelim, string MinDelim);
  }
}
