
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASCOM.Internal
{
  public static class FileInfoExtensions
  {
    private static readonly List<string> deviceClasses = new List<string>()
    {
      "Telescope",
      "Focuser",
      "Camera",
      "FilterWheel",
      "Switch",
      "Dome",
      "Rotator",
      "MultiPortSelector"
    };

    public static bool IsDeviceSpecific(this FileInfo file)
    {
      string[] strArray = file.Name.Split('.');
      return    strArray.Count() >= 3
                && !string.IsNullOrEmpty(strArray[0]) 
                && FileInfoExtensions.deviceClasses.Contains(strArray[0]);
    }
  }
}
