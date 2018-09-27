// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Video.CameraImage
// Assembly: ASCOM.Utilities.Video, Version=6.1.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 7374B086-CBFD-4BA8-B5B2-C4B8C807F537
// Assembly location: C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Components\Platform6\ASCOM.Utilities.Video.dll

using ASCOM.DeviceInterface;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Video
{
  //[Guid("41244296-BED8-4AC9-AA24-D4E90C6C95FA")]
  //[ProgId("ASCOM.Utilities.CameraImage")]
  //[ComSourceInterfaces(typeof (ICameraImage))]
  //[ComVisible(true)]
  //[ClassInterface(ClassInterfaceType.None)]
  public class CameraImage : ICameraImage
  {
    private object imageArray;
    private int[,] intPixelArray;
    private object[,] objPixelArray;
    private int[,,] intColourPixelArray;
    private object[,,] objColourPixelArray;
    private int imageWidth;
    private int imageHeight;
    private SensorType sensorType;
    private bool isColumnMajor;
    private bool isRowMajor;
    private NativeHelpers nativeHelpers;

    public bool FlipHorizontally { get; set; }

    public bool FlipVertically { get; set; }

    public CameraImage()
    {
      this.nativeHelpers = new NativeHelpers();
    }

    public void SetImageArray(object imageArray, int imageWidth, int imageHeight, SensorType sensorType)
    {
      this.imageArray = imageArray;
      this.imageWidth = imageWidth;
      this.imageHeight = imageHeight;
      this.sensorType = sensorType;
      this.objPixelArray = (object[,]) null;
      this.intPixelArray = (int[,]) null;
      this.intColourPixelArray = (int[,,]) null;
      this.objColourPixelArray = (object[,,]) null;
      if (sensorType == SensorType.Monochrome)
      {
        if (imageArray is int[,])
        {
          this.intPixelArray = (int[,]) imageArray;
          this.isColumnMajor = this.intPixelArray.GetLength(0) == imageWidth;
          this.isRowMajor = this.intPixelArray.GetLength(0) == imageHeight;
          return;
        }
        if (imageArray is object[,])
        {
          this.objPixelArray = (object[,]) imageArray;
          this.isColumnMajor = this.objPixelArray.GetLength(0) == imageWidth;
          this.isRowMajor = this.objPixelArray.GetLength(0) == imageHeight;
          return;
        }
      }
      else if (sensorType == SensorType.Color)
      {
        if (imageArray is int[,,])
        {
          this.intColourPixelArray = (int[,,]) imageArray;
          this.isColumnMajor = this.intColourPixelArray.GetLength(0) == imageWidth;
          this.isRowMajor = this.intColourPixelArray.GetLength(0) == 3;
          return;
        }
        if (imageArray is object[,,])
        {
          this.objColourPixelArray = (object[,,]) imageArray;
          this.isColumnMajor = this.objColourPixelArray.GetLength(0) == imageWidth;
          this.isRowMajor = this.objColourPixelArray.GetLength(0) == 3;
          return;
        }
      }
      throw new NotSupportedException("Only Monochrome and Color sensor types are supported.");
    }

    private FlipMode GetFlipMode()
    {
      if (this.FlipHorizontally && this.FlipVertically)
        return FlipMode.FlipBoth;
      if (this.FlipHorizontally)
        return FlipMode.FlipHorizontally;
      return this.FlipVertically ? FlipMode.FlipVertically : FlipMode.None;
    }

    public byte[] GetDisplayBitmapBytes()
    {
      if (this.sensorType == SensorType.Monochrome)
      {
        if (this.intPixelArray != null)
          return this.nativeHelpers.PrepareBitmapForDisplay(this.intPixelArray, this.imageWidth, this.imageHeight, this.GetFlipMode());
        if (this.objPixelArray != null)
          return this.nativeHelpers.PrepareBitmapForDisplay(this.objPixelArray, this.imageWidth, this.imageHeight, this.GetFlipMode());
      }
      else
      {
        if (this.sensorType != SensorType.Color)
          throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", (object) this.sensorType));
        if (this.intColourPixelArray != null)
          return this.nativeHelpers.PrepareColourBitmapForDisplay(this.intColourPixelArray, this.imageWidth, this.imageHeight, this.GetFlipMode());
        if (this.objColourPixelArray != null)
          return this.nativeHelpers.PrepareColourBitmapForDisplay(this.objColourPixelArray, this.imageWidth, this.imageHeight, this.GetFlipMode());
      }
      return (byte[]) null;
    }

    public int GetPixel(int x, int y)
    {
      if (this.intPixelArray != null)
      {
        if (this.isRowMajor)
          return this.intPixelArray[y, x];
        if (this.isColumnMajor)
          return this.intPixelArray[x, y];
      }
      else if (this.objPixelArray != null)
      {
        if (this.isRowMajor)
          return (int) this.objPixelArray[y, x];
        if (this.isColumnMajor)
          return (int) this.objPixelArray[x, y];
      }
      else if (this.intColourPixelArray != null || this.objColourPixelArray != null)
        throw new ASCOM.InvalidOperationException("Use the GetPixel(int, int, int) overload to get a pixel value in a colour image.");
      throw new ASCOM.InvalidOperationException();
    }

    public int GetPixel(int x, int y, int plane)
    {
      if (this.intPixelArray != null || this.objPixelArray != null)
        throw new ASCOM.InvalidOperationException("Use the GetPixel(int, int) overload to get a pixel value in a monochrome image.");
      if (this.intColourPixelArray != null)
      {
        if (this.isRowMajor)
          return this.intColourPixelArray[plane, y, x];
        if (this.isColumnMajor)
          return this.intColourPixelArray[x, y, plane];
      }
      else if (this.objColourPixelArray != null)
      {
        if (this.isRowMajor)
          return (int) this.objColourPixelArray[plane, y, x];
        if (this.isColumnMajor)
          return (int) this.objColourPixelArray[x, y, plane];
      }
      throw new ASCOM.InvalidOperationException();
    }

    public object GetImageArray(Bitmap bmp, SensorType sensorType, LumaConversionMode conversionMode, out byte[] bitmapBytes)
    {
      this.imageWidth = bmp.Width;
      this.imageHeight = bmp.Height;
      if (sensorType == SensorType.Monochrome)
        return this.nativeHelpers.GetMonochromePixelsFromBitmap(bmp, conversionMode, this.GetFlipMode(), out bitmapBytes);
      if (sensorType == SensorType.Color)
        return this.nativeHelpers.GetColourPixelsFromBitmap(bmp, this.GetFlipMode(), out bitmapBytes);
      throw new NotSupportedException(string.Format("Sensor type {0} is not currently supported.", (object) sensorType));
    }

    public byte[] GetBitmapBytes(Bitmap bmp)
    {
      return this.nativeHelpers.GetBitmapBytes(bmp);
    }

        System.Drawing.Bitmap sdBitmap;
        
    }
}
