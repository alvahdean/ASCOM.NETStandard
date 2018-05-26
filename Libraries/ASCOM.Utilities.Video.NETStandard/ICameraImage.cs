// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Video.ICameraImage
// Assembly: ASCOM.Utilities.Video, Version=6.1.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 7374B086-CBFD-4BA8-B5B2-C4B8C807F537
// Assembly location: C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Components\Platform6\ASCOM.Utilities.Video.dll

using ASCOM.DeviceInterface;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Video
{
  //[Guid("BD925113-3B58-4C5F-984E-FBCE7C6A93BE")]
  //[ComVisible(true)]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  public interface ICameraImage
  {
    bool FlipHorizontally { get; set; }

    bool FlipVertically { get; set; }

    void SetImageArray(object imageArray, int imageWidth, int imageHeight, SensorType sensorType);

    int GetPixel(int x, int y);

    int GetPixel(int x, int y, int plane);

    byte[] GetDisplayBitmapBytes();
  }
}
