// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Video.AviTools
// Assembly: ASCOM.Utilities.Video, Version=6.1.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 7374B086-CBFD-4BA8-B5B2-C4B8C807F537
// Assembly location: C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Components\Platform6\ASCOM.Utilities.Video.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Video
{
    //TODO: Why is this internal? The simultator seems to reference it...
  public class AviTools
  {
    private NativeHelpers nativeHelpers;

    public AviTools()
    {
      this.nativeHelpers = new NativeHelpers();
    }

    public void SetNewGamma(double newGamma)
    {
      this.nativeHelpers.SetGamma(newGamma);
    }

    public void SetNewWhiteBalance(int newWhiteBalance)
    {
      this.nativeHelpers.SetWhiteBalance((int) byte.MaxValue - newWhiteBalance);
    }

    public void ApplyGammaBrightness(int[,] pixelsIn, int[,] pixelsOut, int width, int height, short brightness)
    {
      this.nativeHelpers.ApplyGammaBrightness(width, height, 8, ref pixelsIn, ref pixelsOut, brightness);
    }

    public void InitFrameIntegration(int width, int height)
    {
      this.nativeHelpers.InitFrameIntegration(width, height, 8);
    }

    public void AddIntegrationFrame(int[,] pixelsIn)
    {
      this.nativeHelpers.AddFrameForIntegration(ref pixelsIn);
    }

    public int[,] GetResultingIntegratedFrame(int width, int height)
    {
      int[,] pixels = new int[height, width];
      this.nativeHelpers.GetResultingIntegratedFrame(ref pixels);
      return pixels;
    }

    public string GetLastAviErrorMessage()
    {
      string str = (string) null;
      IntPtr num = IntPtr.Zero;
      try
      {
        byte[] source = new byte[200];
        num = Marshal.AllocHGlobal(source.Length + 1);
        Marshal.Copy(source, 0, num, source.Length);
        Marshal.WriteByte(num + source.Length, (byte) 0);
        this.nativeHelpers.GetLastAviFileError(num);
        str = Marshal.PtrToStringAnsi(num);
        if (str != null)
          str = str.Trim();
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
      return str;
    }

    private void TraceLastNativeError()
    {
      Trace.WriteLine(this.GetLastAviErrorMessage(), "VideoNativeException");
    }

    public void StartNewAviFile(string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog)
    {
      if (this.nativeHelpers.CreateNewAviFile(fileName, width, height, bpp, fps, showCompressionDialog) == 0)
        return;
      this.TraceLastNativeError();
    }

    public void AddAviVideoFrame(int[,] pixels)
    {
      if (this.nativeHelpers.AviFileAddFrame(pixels) == 0)
        return;
      this.TraceLastNativeError();
    }

    public void CloseAviFile()
    {
      if (this.nativeHelpers.AviFileClose() == 0)
        return;
      this.TraceLastNativeError();
    }

    public string GetUsedAviFourCC()
    {
      uint usedAviCompression = this.nativeHelpers.GetUsedAviCompression();
      if ((int) usedAviCompression == 0)
        return string.Empty;
      return (Convert.ToString((char) (usedAviCompression & (uint) byte.MaxValue)) + Convert.ToString((char) (usedAviCompression >> 8 & (uint) byte.MaxValue)) + Convert.ToString((char) (usedAviCompression >> 16 & (uint) byte.MaxValue)) + Convert.ToString((char) (usedAviCompression >> 24 & (uint) byte.MaxValue))).ToUpper();
    }
  }
}
