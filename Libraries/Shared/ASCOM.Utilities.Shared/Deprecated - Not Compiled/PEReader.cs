// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.PEReader
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll

//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ASCOM.Utilities
{
  internal class PEReader : IDisposable
  {
    internal const int CLR_HEADER = 14;
    private const int MAX_HEADERS_TO_CHECK = 1000;
    private const int COR_E_BADIMAGEFORMAT = -2147024885;
    private const int CLDB_E_FILE_OLDVER = -2146234105;
    private const int CLDB_E_INDEX_NOTFOUND = -2146234076;
    private const int CLDB_E_FILE_CORRUPT = -2146234098;
    private const int COR_E_NEWER_RUNTIME = -2146234341;
    private const int COR_E_ASSEMBLYEXPECTED = -2146234344;
    private const int ERROR_BAD_EXE_FORMAT = -2147024703;
    private const int ERROR_EXE_MARKED_INVALID = -2147024704;
    private const int CORSEC_E_INVALID_IMAGE_FORMAT = -2146233315;
    private const int ERROR_NOACCESS = -2147023898;
    private const int ERROR_INVALID_ORDINAL = -2147024714;
    private const int ERROR_INVALID_DLL = -2147023742;
    private const int ERROR_FILE_CORRUPT = -2147023504;
    private const int COR_E_LOADING_REFERENCE_ASSEMBLY = -2146234280;
    private const int META_E_BAD_SIGNATURE = -2146233966;
    private const ushort IMAGE_FILE_MACHINE_I386 = 332;
    private const ushort IMAGE_FILE_MACHINE_IA64 = 512;
    private const ushort IMAGE_FILE_MACHINE_AMD64 = 34404;
    private readonly PEReader.IMAGE_DOS_HEADER dosHeader;
    private PEReader.IMAGE_NT_HEADERS ntHeaders;
    private readonly PEReader.IMAGE_COR20_HEADER CLR;
    private readonly IList<PEReader.IMAGE_SECTION_HEADER> sectionHeaders;
    private uint TextBase;
    private BinaryReader reader;
    private Stream stream;
    private bool IsAssembly;
    private AssemblyName AssemblyInfo;
    private Assembly SuppliedAssembly;
    private string AssemblyDeterminationType;
    private bool OS32BitCompatible;
    private VersionCode.Bitness ExecutableBitness;
    private TraceLogger TL;
    private bool disposedValue;

    internal VersionCode.Bitness BitNess
    {
      get
      {
        this.TL.LogMessage("PE.BitNess", $"Returning: {this.ExecutableBitness}");
        return this.ExecutableBitness;
      }
    }

    internal PEReader(string FileName, TraceLogger TLogger)
    {
      this.sectionHeaders = (IList<PEReader.IMAGE_SECTION_HEADER>) new List<PEReader.IMAGE_SECTION_HEADER>();
      this.IsAssembly = false;
      this.OS32BitCompatible = false;
      this.TL = TLogger;
      this.TL.LogMessage("PEReader", "Running within CLR version: " + RuntimeEnvironment.GetSystemVersion());
      if (Operators.CompareString(Strings.Left(FileName, 5).ToUpper(), "FILE:", false) == 0)
      {
        Uri uri = new Uri(FileName);
        FileName = uri.LocalPath + Uri.UnescapeDataString(uri.Fragment).Replace("/", "\\\\");
      }
      this.TL.LogMessage("PEReader", "Filename to check: " + FileName);
      if (!File.Exists(FileName))
        throw new FileNotFoundException("PEReader - File not found: " + FileName);
      this.TL.LogMessage("PEReader", "Determining whether this is an assembly");
      try
      {
        this.SuppliedAssembly = Assembly.ReflectionOnlyLoadFrom(FileName);
        this.IsAssembly = true;
        this.TL.LogMessage("PEReader.IsAssembly", "Found an assembly because it loaded Ok to the reflection context: " + Conversions.ToString(this.IsAssembly));
      }
      catch (FileNotFoundException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.TL.LogMessage("PEReader.IsAssembly", "FileNotFoundException: File not found so this is NOT an assembly: " + Conversions.ToString(this.IsAssembly));
        //ProjectData.ClearProjectError();
      }
      catch (BadImageFormatException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        int hrForException = Marshal.GetHRForException((Exception) ex);
        switch (hrForException)
        {
          case -2147024885:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - COR_E_BADIMAGEFORMAT. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234105:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - CLDB_E_FILE_OLDVER. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234076:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - CLDB_E_INDEX_NOTFOUND. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234098:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - CLDB_E_FILE_CORRUPT. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234341:
            this.IsAssembly = true;
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - COR_E_NEWER_RUNTIME. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234344:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - COR_E_ASSEMBLYEXPECTED. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147024703:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_BAD_EXE_FORMAT. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147024704:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_EXE_MARKED_INVALID. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146233315:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - CORSEC_E_INVALID_IMAGE_FORMAT. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147023898:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_NOACCESS. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147024714:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_INVALID_ORDINAL. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147023742:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_INVALID_DLL. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2147023504:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - ERROR_FILE_CORRUPT. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146234280:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - COR_E_LOADING_REFERENCE_ASSEMBLY. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          case -2146233966:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - META_E_BAD_SIGNATURE. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
          default:
            this.TL.LogMessage("PEReader.IsAssembly", "BadImageFormatException. hResult: " + hrForException.ToString("X8") + " - Meaning of error code is unknown. Setting IsAssembly to: " + Conversions.ToString(this.IsAssembly));
            break;
        }
        //ProjectData.ClearProjectError();
      }
      catch (FileLoadException ex)
      {
        //ProjectData.SetProjectError((Exception) ex);
        this.IsAssembly = true;
        this.TL.LogMessage("PEReader.IsAssembly", "FileLoadException: Assembly already loaded so this is an assembly: " + Conversions.ToString(this.IsAssembly));
        //ProjectData.ClearProjectError();
      }
      this.TL.LogMessage("PEReader", "Determining PE Machine type");
      this.stream = (Stream) new FileStream(FileName, FileMode.Open, FileAccess.Read);
      this.reader = new BinaryReader(this.stream);
      this.reader.BaseStream.Seek(0L, SeekOrigin.Begin);
      this.dosHeader = PEReader.MarshalBytesTo<PEReader.IMAGE_DOS_HEADER>(this.reader);
      if ((int) this.dosHeader.e_magic != 23117)
        throw new ASCOM.InvalidOperationException("File is not a portable executable.");
      this.reader.BaseStream.Seek((long) this.dosHeader.e_lfanew, SeekOrigin.Begin);
      this.ntHeaders.Signature = PEReader.MarshalBytesTo<uint>(this.reader);
      if ((long) this.ntHeaders.Signature != 17744L)
        throw new ASCOM.InvalidOperationException("Invalid portable executable signature in NT header.");
      this.ntHeaders.FileHeader = PEReader.MarshalBytesTo<PEReader.IMAGE_FILE_HEADER>(this.reader);
      switch (this.ntHeaders.FileHeader.Machine)
      {
        case 332:
          this.OS32BitCompatible = true;
          this.TL.LogMessage("PEReader.MachineType", "Machine - found \"Intel 32bit\" executable. Characteristics: " + this.ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + Conversions.ToString(this.OS32BitCompatible));
          break;
        case 512:
          this.OS32BitCompatible = false;
          this.TL.LogMessage("PEReader.MachineType", "Machine - found \"Itanium 64bit\" executable. Characteristics: " + this.ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + Conversions.ToString(this.OS32BitCompatible));
          break;
        case 34404:
          this.OS32BitCompatible = false;
          this.TL.LogMessage("PEReader.MachineType", "Machine - found \"Intel 64bit\" executable. Characteristics: " + this.ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + Conversions.ToString(this.OS32BitCompatible));
          break;
        default:
          this.TL.LogMessage("PEReader.MachineType", "Found Unknown machine type: " + this.ntHeaders.FileHeader.Machine.ToString("X8") + ". Characteristics: " + this.ntHeaders.FileHeader.Characteristics.ToString("X8") + ", OS32BitCompatible: " + Conversions.ToString(this.OS32BitCompatible));
          break;
      }
      if (this.OS32BitCompatible)
      {
        this.TL.LogMessage("PEReader.MachineType", "Reading optional 32bit header");
        this.ntHeaders.OptionalHeader32 = PEReader.MarshalBytesTo<PEReader.IMAGE_OPTIONAL_HEADER32>(this.reader);
      }
      else
      {
        this.TL.LogMessage("PEReader.MachineType", "Reading optional 64bit header");
        this.ntHeaders.OptionalHeader64 = PEReader.MarshalBytesTo<PEReader.IMAGE_OPTIONAL_HEADER64>(this.reader);
      }
      if (this.IsAssembly)
      {
        this.TL.LogMessage("PEReader", "This is an assembly, determining Bitness through the CLR header");
        int num1 = 1000;
        if (this.OS32BitCompatible)
        {
          this.TL.LogMessage("PEReader.Bitness", "This is a 32 bit assembly, reading the CLR Header");
          if ((long) this.ntHeaders.OptionalHeader32.NumberOfRvaAndSizes < 1000L)
            num1 = checked ((int) this.ntHeaders.OptionalHeader32.NumberOfRvaAndSizes);
          this.TL.LogMessage("PEReader.Bitness", "Checking " + Conversions.ToString(num1) + " headers");
          int num2 = 0;
          int num3 = checked (num1 - 1);
          int index = num2;
          while (index <= num3)
          {
            if ((long) this.ntHeaders.OptionalHeader32.DataDirectory[index].Size > 0L)
              this.sectionHeaders.Add(PEReader.MarshalBytesTo<PEReader.IMAGE_SECTION_HEADER>(this.reader));
            checked { ++index; }
          }
          IEnumerator<PEReader.IMAGE_SECTION_HEADER> enumerator = null;
          try
          {
            enumerator = this.sectionHeaders.GetEnumerator();
            while (enumerator.MoveNext())
            {
              PEReader.IMAGE_SECTION_HEADER current = enumerator.Current;
              if (Operators.CompareString(current.Name, ".text", false) == 0)
                this.TextBase = current.PointerToRawData;
            }
          }
          finally
          {
            if (enumerator != null)
              enumerator.Dispose();
          }
          if (num1 >= 15 && (long) this.ntHeaders.OptionalHeader32.DataDirectory[14].VirtualAddress > 0L)
          {
            this.reader.BaseStream.Seek((long) checked (this.ntHeaders.OptionalHeader32.DataDirectory[14].VirtualAddress - this.ntHeaders.OptionalHeader32.BaseOfCode + this.TextBase), SeekOrigin.Begin);
            this.CLR = PEReader.MarshalBytesTo<PEReader.IMAGE_COR20_HEADER>(this.reader);
          }
        }
        else
        {
          this.TL.LogMessage("PEReader.Bitness", "This is a 64 bit assembly, reading the CLR Header");
          if ((long) this.ntHeaders.OptionalHeader64.NumberOfRvaAndSizes < 1000L)
            num1 = checked ((int) this.ntHeaders.OptionalHeader64.NumberOfRvaAndSizes);
          this.TL.LogMessage("PEReader.Bitness", "Checking " + Conversions.ToString(num1) + " headers");
          int num2 = 0;
          int num3 = checked (num1 - 1);
          int index = num2;
          while (index <= num3)
          {
            if ((long) this.ntHeaders.OptionalHeader64.DataDirectory[index].Size > 0L)
              this.sectionHeaders.Add(PEReader.MarshalBytesTo<PEReader.IMAGE_SECTION_HEADER>(this.reader));
            checked { ++index; }
          }
          IEnumerator<PEReader.IMAGE_SECTION_HEADER> enumerator = null;
          try
          {
            enumerator = this.sectionHeaders.GetEnumerator();
            while (enumerator.MoveNext())
            {
              PEReader.IMAGE_SECTION_HEADER current = enumerator.Current;
              if (Operators.CompareString(current.Name, ".text", false) == 0)
              {
                this.TL.LogMessage("PEReader.Bitness", "Found TEXT section");
                this.TextBase = current.PointerToRawData;
              }
            }
          }
          finally
          {
            if (enumerator != null)
              enumerator.Dispose();
          }
          if (num1 >= 15 && (long) this.ntHeaders.OptionalHeader64.DataDirectory[14].VirtualAddress > 0L)
          {
            this.reader.BaseStream.Seek((long) checked (this.ntHeaders.OptionalHeader64.DataDirectory[14].VirtualAddress - this.ntHeaders.OptionalHeader64.BaseOfCode + this.TextBase), SeekOrigin.Begin);
            this.CLR = PEReader.MarshalBytesTo<PEReader.IMAGE_COR20_HEADER>(this.reader);
            this.TL.LogMessage("PEReader.Bitness", "Read CLR header successfully");
          }
        }
        if (this.OS32BitCompatible)
        {
          if (((long) this.CLR.Flags & 2L) > 0L)
          {
            this.TL.LogMessage("PEReader.Bitness", "Found \"32bit Required\" assembly");
            this.ExecutableBitness = VersionCode.Bitness.Bits32;
          }
          else
          {
            this.TL.LogMessage("PEReader.Bitness", "Found \"MSIL\" assembly");
            this.ExecutableBitness = VersionCode.Bitness.BitsMSIL;
          }
        }
        else
        {
          this.TL.LogMessage("PEReader.Bitness", "Found \"64bit Required\" assembly");
          this.ExecutableBitness = VersionCode.Bitness.Bits64;
        }
        this.TL.LogMessage("PEReader", "Assembly required Runtime version: " + Conversions.ToString((uint) this.CLR.MajorRuntimeVersion) + "." + Conversions.ToString((uint) this.CLR.MinorRuntimeVersion));
      }
      else
      {
        this.TL.LogMessage("PEReader", "This is not an assembly, determining Bitness through the executable bitness flag");
        if (this.OS32BitCompatible)
        {
          this.TL.LogMessage("PEReader.Bitness", "Found 32bit executable");
          this.ExecutableBitness = VersionCode.Bitness.Bits32;
        }
        else
        {
          this.TL.LogMessage("PEReader.Bitness", "Found 64bit executable");
          this.ExecutableBitness = VersionCode.Bitness.Bits64;
        }
      }
    }

    internal bool IsDotNetAssembly()
    {
      this.TL.LogMessage("PE.IsDotNetAssembly", "Returning: " + Conversions.ToString(this.IsAssembly));
      return this.IsAssembly;
    }

    internal PEReader.SubSystemType SubSystem()
    {
      if (this.OS32BitCompatible)
      {
        this.TL.LogMessage("PE.SubSystem", "Returning 32bit value: " + ((PEReader.SubSystemType) this.ntHeaders.OptionalHeader32.Subsystem).ToString());
        return (PEReader.SubSystemType) this.ntHeaders.OptionalHeader32.Subsystem;
      }
      this.TL.LogMessage("PE.SubSystem", "Returning 64bit value: " + ((PEReader.SubSystemType) this.ntHeaders.OptionalHeader64.Subsystem).ToString());
      return (PEReader.SubSystemType) this.ntHeaders.OptionalHeader64.Subsystem;
    }

    private static T MarshalBytesTo<T>(BinaryReader reader)
    {
      GCHandle gcHandle = GCHandle.Alloc((object) reader.ReadBytes(Marshal.SizeOf(typeof (T))), GCHandleType.Pinned);
      T structure = (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof (T));
      gcHandle.Free();
      return structure;
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!this.disposedValue)
      {
        if (disposing)
        {
          try
          {
            this.reader.Close();
            this.stream.Close();
            this.stream.Dispose();
            this.stream = (Stream) null;
          }
          catch (Exception ex)
          {
            //ProjectData.SetProjectError(ex);
            //ProjectData.ClearProjectError();
          }
        }
      }
      this.disposedValue = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    internal enum CLR_FLAGS
    {
      CLR_FLAGS_ILONLY = 1,
      CLR_FLAGS_32BITREQUIRED = 2,
      CLR_FLAGS_IL_LIBRARY = 4,
      CLR_FLAGS_STRONGNAMESIGNED = 8,
      CLR_FLAGS_NATIVE_ENTRYPOINT = 16,
      CLR_FLAGS_TRACKDEBUGDATA = 65536,
    }

    internal enum SubSystemType
    {
      NATIVE = 1,
      WINDOWS_GUI = 2,
      WINDOWS_CUI = 3,
      UNKNOWN_4 = 4,
      OS2_CUI = 5,
      UNKNOWN_6 = 6,
      POSIX_CUI = 7,
      NATIVE_WINDOWS = 8,
      WINDOWS_CE_GUI = 9,
      EFI_APPLICATION = 10,
      EFI_BOOT_SERVICE_DRIVER = 11,
      EFI_RUNTIME_DRIVER = 12,
      EFI_ROM = 13,
      XBOX = 14,
      UNKNOWN_15 = 15,
      WINDOWS_BOOT_APPLICATION = 16,
    }

    internal struct IMAGE_DOS_HEADER
    {
      internal ushort e_magic;
      internal ushort e_cblp;
      internal ushort e_cp;
      internal ushort e_crlc;
      internal ushort e_cparhdr;
      internal ushort e_minalloc;
      internal ushort e_maxalloc;
      internal ushort e_ss;
      internal ushort e_sp;
      internal ushort e_csum;
      internal ushort e_ip;
      internal ushort e_cs;
      internal ushort e_lfarlc;
      internal ushort e_ovno;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
      internal ushort[] e_res1;
      internal ushort e_oemid;
      internal ushort e_oeminfo;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
      internal ushort[] e_res2;
      internal uint e_lfanew;
    }

    internal struct IMAGE_NT_HEADERS
    {
      internal uint Signature;
      internal PEReader.IMAGE_FILE_HEADER FileHeader;
      internal PEReader.IMAGE_OPTIONAL_HEADER32 OptionalHeader32;
      internal PEReader.IMAGE_OPTIONAL_HEADER64 OptionalHeader64;
    }

    internal struct IMAGE_FILE_HEADER
    {
      internal ushort Machine;
      internal ushort NumberOfSections;
      internal uint TimeDateStamp;
      internal uint PointerToSymbolTable;
      internal uint NumberOfSymbols;
      internal ushort SizeOfOptionalHeader;
      internal ushort Characteristics;
    }

    internal struct IMAGE_OPTIONAL_HEADER32
    {
      internal ushort Magic;
      internal byte MajorLinkerVersion;
      internal byte MinorLinkerVersion;
      internal uint SizeOfCode;
      internal uint SizeOfInitializedData;
      internal uint SizeOfUninitializedData;
      internal uint AddressOfEntryPoint;
      internal uint BaseOfCode;
      internal uint BaseOfData;
      internal uint ImageBase;
      internal uint SectionAlignment;
      internal uint FileAlignment;
      internal ushort MajorOperatingSystemVersion;
      internal ushort MinorOperatingSystemVersion;
      internal ushort MajorImageVersion;
      internal ushort MinorImageVersion;
      internal ushort MajorSubsystemVersion;
      internal ushort MinorSubsystemVersion;
      internal uint Win32VersionValue;
      internal uint SizeOfImage;
      internal uint SizeOfHeaders;
      internal uint CheckSum;
      internal ushort Subsystem;
      internal ushort DllCharacteristics;
      internal uint SizeOfStackReserve;
      internal uint SizeOfStackCommit;
      internal uint SizeOfHeapReserve;
      internal uint SizeOfHeapCommit;
      internal uint LoaderFlags;
      internal uint NumberOfRvaAndSizes;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
      internal PEReader.IMAGE_DATA_DIRECTORY[] DataDirectory;
    }

    internal struct IMAGE_OPTIONAL_HEADER64
    {
      internal ushort Magic;
      internal byte MajorLinkerVersion;
      internal byte MinorLinkerVersion;
      internal uint SizeOfCode;
      internal uint SizeOfInitializedData;
      internal uint SizeOfUninitializedData;
      internal uint AddressOfEntryPoint;
      internal uint BaseOfCode;
      internal ulong ImageBase;
      internal uint SectionAlignment;
      internal uint FileAlignment;
      internal ushort MajorOperatingSystemVersion;
      internal ushort MinorOperatingSystemVersion;
      internal ushort MajorImageVersion;
      internal ushort MinorImageVersion;
      internal ushort MajorSubsystemVersion;
      internal ushort MinorSubsystemVersion;
      internal uint Win32VersionValue;
      internal uint SizeOfImage;
      internal uint SizeOfHeaders;
      internal uint CheckSum;
      internal ushort Subsystem;
      internal ushort DllCharacteristics;
      internal ulong SizeOfStackReserve;
      internal ulong SizeOfStackCommit;
      internal ulong SizeOfHeapReserve;
      internal ulong SizeOfHeapCommit;
      internal uint LoaderFlags;
      internal uint NumberOfRvaAndSizes;
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
      internal PEReader.IMAGE_DATA_DIRECTORY[] DataDirectory;
    }

    internal struct IMAGE_DATA_DIRECTORY
    {
      internal uint VirtualAddress;
      internal uint Size;
    }

    internal struct IMAGE_SECTION_HEADER
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
      internal string Name;
      internal PEReader.Misc Misc;
      internal uint VirtualAddress;
      internal uint SizeOfRawData;
      internal uint PointerToRawData;
      internal uint PointerToRelocations;
      internal uint PointerToLinenumbers;
      internal ushort NumberOfRelocations;
      internal ushort NumberOfLinenumbers;
      internal uint Characteristics;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct Misc
    {
      [FieldOffset(0)]
      internal uint PhysicalAddress;
      [FieldOffset(0)]
      internal uint VirtualSize;
    }

    internal struct IMAGE_COR20_HEADER
    {
      internal uint cb;
      internal ushort MajorRuntimeVersion;
      internal ushort MinorRuntimeVersion;
      internal PEReader.IMAGE_DATA_DIRECTORY MetaData;
      internal uint Flags;
      internal uint EntryPointToken;
      internal PEReader.IMAGE_DATA_DIRECTORY Resources;
      internal PEReader.IMAGE_DATA_DIRECTORY StrongNameSignature;
      internal PEReader.IMAGE_DATA_DIRECTORY CodeManagerTable;
      internal PEReader.IMAGE_DATA_DIRECTORY VTableFixups;
      internal PEReader.IMAGE_DATA_DIRECTORY ExportAddressTableJumps;
      internal PEReader.IMAGE_DATA_DIRECTORY ManagedNativeHeader;
    }
  }
}
