// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Video.NativeHelpers
// Assembly: ASCOM.Utilities.Video, Version=6.1.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 7374B086-CBFD-4BA8-B5B2-C4B8C807F537
// Assembly location: C:\Program Files (x86)\ASCOM\Platform 6 Developer Components\Components\Platform6\ASCOM.Utilities.Video.dll

using ASCOM.Utilities.Exceptions;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ASCOM.Utilities.Video
{
    internal class NativeHelpers : IDisposable
    {
        private const string VIDEOUTILS32_DLL_NAME = "ASCOM.NativeVideo32.dll";
        private const string VIDEOUTILS64_DLL_NAME = "ASCOM.NativeVideo64.dll";
        private const string VIDEOUTILS_DLL_LOCATION = "\\ASCOM\\VideoUtilities\\";
        private TraceLogger TL;
        private int rc;
        private uint urc;
        private IntPtr VideoDllHandle;
        private bool disposedValue;

        public NativeHelpers()
        {
            this.TL = new TraceLogger("NativeHelpers");
            //this.TL.Enabled = RegistryCommonCode.GetBool("Trace Util", false);
            this.TL.Enabled = false;
            try
            {
                StringBuilder stringBuilder = new StringBuilder(260);
                string lpFileName = !NativeHelpers.Is64Bit() ? Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles) + "\\ASCOM\\VideoUtilities\\ASCOM.NativeVideo32.dll" : Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86) + "\\ASCOM\\VideoUtilities\\ASCOM.NativeVideo64.dll";
                this.TL.LogMessage("New", "Loading NativeHelpers library DLL: " + lpFileName);
                this.VideoDllHandle = NativeHelpers.LoadLibrary(lpFileName);
                int lastWin32Error = Marshal.GetLastWin32Error();
                if (this.VideoDllHandle != IntPtr.Zero)
                {
                    this.TL.LogMessage("New", "Loaded NativeHelpers library OK");
                    this.TL.LogMessage("NativeHelpers", "Created");
                }
                else
                {
                    this.TL.LogMessage("New", "Error loading NativeHelpers library: " + lastWin32Error.ToString("X8"));
                    throw new HelperException("Error code returned from LoadLibrary when loading NativeHelpers library: " + lastWin32Error.ToString("X8"));
                }
            }
            catch (Exception ex)
            {
                this.TL.LogMessageCrLf("NativeHelpers ctor", ex.ToString());
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (this.TL != null)
                    {
                        this.TL.Enabled = false;
                        this.TL.Dispose();
                        this.TL = (TraceLogger)null;
                    }
                }
                try
                {
                    NativeHelpers.FreeLibrary(this.VideoDllHandle);
                }
                catch
                {
                }
            }
            this.disposedValue = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        internal int GetBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[,] pixels, ref byte[] bitmapBytes)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetBitmapPixels32(width, height, bpp, flipMode, pixels, bitmapBytes) : NativeHelpers.GetBitmapPixels64(width, height, bpp, flipMode, pixels, bitmapBytes);
            return this.rc;
        }

        internal int GetColourBitmapPixels(int width, int height, int bpp, FlipMode flipMode, int[,,] pixels, ref byte[] bitmapBytes)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetColourBitmapPixels32(width, height, bpp, flipMode, pixels, bitmapBytes) : NativeHelpers.GetColourBitmapPixels64(width, height, bpp, flipMode, pixels, bitmapBytes);
            return this.rc;
        }

        internal int GetMonochromePixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, ref int[,] bitmapPixels, ref byte[] bitmapBytes, int mode)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetMonochromePixelsFromBitmap32(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes, mode) : NativeHelpers.GetMonochromePixelsFromBitmap64(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes, mode);
            return this.rc;
        }

        internal int GetColourPixelsFromBitmap(int width, int height, int bpp, FlipMode flipMode, IntPtr hBitmap, ref int[,,] bitmapPixels, ref byte[] bitmapBytes)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetColourPixelsFromBitmap32(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes) : NativeHelpers.GetColourPixelsFromBitmap64(width, height, bpp, flipMode, hBitmap, bitmapPixels, bitmapBytes);
            return this.rc;
        }

        internal int SetGamma(double gamma)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.SetGamma32(gamma) : NativeHelpers.SetGamma64(gamma);
            return this.rc;
        }

        internal int ApplyGammaBrightness(int width, int height, int bpp, ref int[,] pixelsIn, ref int[,] pixelsOut, short brightness)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.ApplyGammaBrightness32(width, height, bpp, pixelsIn, pixelsOut, brightness) : NativeHelpers.ApplyGammaBrightness64(width, height, bpp, pixelsIn, pixelsOut, brightness);
            return this.rc;
        }

        internal int GetBitmapBytes(int width, int height, IntPtr hBitmap, ref byte[] bitmapBytes)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetBitmapBytes32(width, height, hBitmap, bitmapBytes) : NativeHelpers.GetBitmapBytes64(width, height, hBitmap, bitmapBytes);
            return this.rc;
        }

        internal int InitFrameIntegration(int width, int height, int bpp)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.InitFrameIntegration32(width, height, bpp) : NativeHelpers.InitFrameIntegration64(width, height, bpp);
            return this.rc;
        }

        internal int AddFrameForIntegration(ref int[,] pixels)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.AddFrameForIntegration32(pixels) : NativeHelpers.AddFrameForIntegration64(pixels);
            return this.rc;
        }

        internal int GetResultingIntegratedFrame(ref int[,] pixels)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetResultingIntegratedFrame32(pixels) : NativeHelpers.GetResultingIntegratedFrame64(pixels);
            return this.rc;
        }

        internal int CreateNewAviFile(string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.CreateNewAviFile32(fileName, width, height, bpp, fps, showCompressionDialog) : NativeHelpers.CreateNewAviFile64(fileName, width, height, bpp, fps, showCompressionDialog);
            return this.rc;
        }

        internal int AviFileAddFrame(int[,] pixels)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.AviFileAddFrame32(pixels) : NativeHelpers.AviFileAddFrame64(pixels);
            return this.rc;
        }

        internal int AviFileClose()
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.AviFileClose32() : NativeHelpers.AviFileClose64();
            return this.rc;
        }

        internal int GetLastAviFileError(IntPtr errorMessage)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetLastAviFileError32(errorMessage) : NativeHelpers.GetLastAviFileError64(errorMessage);
            return this.rc;
        }

        internal uint GetUsedAviCompression()
        {
            this.urc = !NativeHelpers.Is64Bit() ? NativeHelpers.GetUsedAviCompression32() : NativeHelpers.GetUsedAviCompression64();
            return this.urc;
        }

        internal int SetWhiteBalance(int newWhiteBalance)
        {
            this.rc = !NativeHelpers.Is64Bit() ? NativeHelpers.SetWhiteBalance32(newWhiteBalance) : NativeHelpers.SetWhiteBalance64(newWhiteBalance);
            return this.rc;
        }

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetBitmapPixels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapPixels32(int width, int height, int bpp, FlipMode flipMode, [MarshalAs(UnmanagedType.LPArray), In] int[,] pixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetColourBitmapPixels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourBitmapPixels32(int width, int height, int bpp, FlipMode flipMode, [MarshalAs(UnmanagedType.LPArray), In] int[,,] pixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetMonochromePixelsFromBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMonochromePixelsFromBitmap32(int width, int height, int bpp, FlipMode flipMode, [In] IntPtr hBitmap, [MarshalAs(UnmanagedType.LPArray), In, Out] int[,] bitmapPixels, [In, Out] byte[] bitmapBytes, int mode);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetColourPixelsFromBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourPixelsFromBitmap32(int width, int height, int bpp, FlipMode flipMode, [In] IntPtr hBitmap, [MarshalAs(UnmanagedType.LPArray), In, Out] int[,,] bitmapPixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "SetGamma", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetGamma32(double gamma);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "ApplyGammaBrightness", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ApplyGammaBrightness32(int width, int height, int bpp, [In, Out] int[,] pixelsIn, [In, Out] int[,] pixelsOut, short brightness);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetBitmapBytes", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapBytes32(int width, int height, [In] IntPtr hBitmap, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "InitFrameIntegration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int InitFrameIntegration32(int width, int height, int bpp);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "AddFrameForIntegration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddFrameForIntegration32([In, Out] int[,] pixels);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetResultingIntegratedFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetResultingIntegratedFrame32([In, Out] int[,] pixels);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "CreateNewAviFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern int CreateNewAviFile32([MarshalAs(UnmanagedType.LPStr)] string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "AviFileAddFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AviFileAddFrame32([MarshalAs(UnmanagedType.LPArray), In] int[,] pixels);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "AviFileClose", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AviFileClose32();

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetLastAviFileError", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetLastAviFileError32(IntPtr errorMessage);

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "GetUsedAviCompression", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint GetUsedAviCompression32();

        [DllImport("ASCOM.NativeVideo32.dll", EntryPoint = "SetWhiteBalance", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetWhiteBalance32(int newWhiteBalance);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetBitmapPixels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapPixels64(int width, int height, int bpp, FlipMode flipMode, [MarshalAs(UnmanagedType.LPArray), In] int[,] pixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetColourBitmapPixels", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourBitmapPixels64(int width, int height, int bpp, FlipMode flipMode, [MarshalAs(UnmanagedType.LPArray), In] int[,,] pixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetMonochromePixelsFromBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetMonochromePixelsFromBitmap64(int width, int height, int bpp, FlipMode flipMode, [In] IntPtr hBitmap, [MarshalAs(UnmanagedType.LPArray), In, Out] int[,] bitmapPixels, [In, Out] byte[] bitmapBytes, int mode);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetColourPixelsFromBitmap", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetColourPixelsFromBitmap64(int width, int height, int bpp, FlipMode flipMode, [In] IntPtr hBitmap, [MarshalAs(UnmanagedType.LPArray), In, Out] int[,,] bitmapPixels, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "SetGamma", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetGamma64(double gamma);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "ApplyGammaBrightness", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ApplyGammaBrightness64(int width, int height, int bpp, [In, Out] int[,] pixelsIn, [In, Out] int[,] pixelsOut, short brightness);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetBitmapBytes", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetBitmapBytes64(int width, int height, [In] IntPtr hBitmap, [In, Out] byte[] bitmapBytes);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "InitFrameIntegration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int InitFrameIntegration64(int width, int height, int bpp);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "AddFrameForIntegration", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AddFrameForIntegration64([In, Out] int[,] pixels);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetResultingIntegratedFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetResultingIntegratedFrame64([In, Out] int[,] pixels);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "CreateNewAviFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern int CreateNewAviFile64([MarshalAs(UnmanagedType.LPStr)] string fileName, int width, int height, int bpp, double fps, bool showCompressionDialog);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "AviFileAddFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AviFileAddFrame64([MarshalAs(UnmanagedType.LPArray), In] int[,] pixels);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "AviFileClose", CallingConvention = CallingConvention.Cdecl)]
        private static extern int AviFileClose64();

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetLastAviFileError", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetLastAviFileError64(IntPtr errorMessage);

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "GetUsedAviCompression", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint GetUsedAviCompression64();

        [DllImport("ASCOM.NativeVideo64.dll", EntryPoint = "SetWhiteBalance", CallingConvention = CallingConvention.Cdecl)]
        private static extern int SetWhiteBalance64(int newWhiteBalance);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        internal byte[] PrepareBitmapForDisplay(int[,] imageArray, int width, int height, FlipMode flipMode)
        {
            return this.PrepareBitmapForDisplay((object)imageArray, width, height, false, flipMode);
        }

        internal byte[] PrepareBitmapForDisplay(object[,] imageArray, int width, int height, FlipMode flipMode)
        {
            return this.PrepareBitmapForDisplay((object)imageArray, width, height, true, flipMode);
        }

        internal byte[] PrepareColourBitmapForDisplay(int[,,] imageArray, int width, int height, FlipMode flipMode)
        {
            return this.PrepareColourBitmapForDisplay((object)imageArray, width, height, false, flipMode);
        }

        internal byte[] PrepareColourBitmapForDisplay(object[,,] imageArray, int width, int height, FlipMode flipMode)
        {
            return this.PrepareColourBitmapForDisplay((object)imageArray, width, height, true, flipMode);
        }

        internal object GetMonochromePixelsFromBitmap(Bitmap bitmap, LumaConversionMode conversionMode, FlipMode flipMode, out byte[] rawBitmapBytes)
        {
            int[,] bitmapPixels = new int[bitmap.Width, bitmap.Height];
            rawBitmapBytes = new byte[bitmap.Width * bitmap.Height * 3 + 40 + 14 + 1];
            IntPtr hbitmap = bitmap.GetHbitmap();
            try
            {
                this.GetMonochromePixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hbitmap, ref bitmapPixels, ref rawBitmapBytes, (int)conversionMode);
            }
            finally
            {
                NativeHelpers.DeleteObject(hbitmap);
            }
            return (object)bitmapPixels;
        }

        internal object GetColourPixelsFromBitmap(Bitmap bitmap, FlipMode flipMode, out byte[] rawBitmapBytes)
        {
            int[,,] bitmapPixels = new int[bitmap.Width, bitmap.Height, 3];
            rawBitmapBytes = new byte[bitmap.Width * bitmap.Height * 3 + 40 + 14 + 1];
            IntPtr hbitmap = bitmap.GetHbitmap();
            try
            {
                this.GetColourPixelsFromBitmap(bitmap.Width, bitmap.Height, 8, flipMode, hbitmap, ref bitmapPixels, ref rawBitmapBytes);
            }
            finally
            {
                NativeHelpers.DeleteObject(hbitmap);
            }
            return (object)bitmapPixels;
        }

        internal byte[] GetBitmapBytes(Bitmap bitmap)
        {
            byte[] bitmapBytes = new byte[bitmap.Width * bitmap.Height * 3 + 40 + 14 + 1];
            IntPtr hbitmap = bitmap.GetHbitmap();
            try
            {
                this.GetBitmapBytes(bitmap.Width, bitmap.Height, hbitmap, ref bitmapBytes);
            }
            finally
            {
                NativeHelpers.DeleteObject(hbitmap);
            }
            return bitmapBytes;
        }

        private byte[] PrepareBitmapForDisplay(object imageArray, int width, int height, bool useVariantPixels, FlipMode flipMode)
        {
            int[,] pixels = new int[height, width];
            if (useVariantPixels)
                Array.Copy((Array)imageArray, (Array)pixels, pixels.Length);
            else
                Array.Copy((Array)imageArray, (Array)pixels, pixels.Length);
            byte[] bitmapBytes = new byte[width * height * 3 + 40 + 14 + 1];
            this.GetBitmapPixels(width, height, 8, flipMode, pixels, ref bitmapBytes);
            return bitmapBytes;
        }

        private byte[] PrepareColourBitmapForDisplay(object imageArray, int width, int height, bool useVariantPixels, FlipMode flipMode)
        {
            int[,,] pixels = new int[height, width, 3];
            if (useVariantPixels)
                Array.Copy((Array)imageArray, (Array)pixels, pixels.Length);
            else
                Array.Copy((Array)imageArray, (Array)pixels, pixels.Length);
            byte[] bitmapBytes = new byte[width * height * 3 + 40 + 14 + 1];
            this.GetColourBitmapPixels(width, height, 8, flipMode, pixels, ref bitmapBytes);
            return bitmapBytes;
        }

        [DllImport("kernel32", EntryPoint = "LoadLibraryA", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static bool Is64Bit()
        {
            return IntPtr.Size == 8;
        }
    }
}
