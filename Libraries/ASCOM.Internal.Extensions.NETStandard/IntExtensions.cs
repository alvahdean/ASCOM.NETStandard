
using System;

namespace ASCOM.Internal
{
  public static class IntExtensions
  {
    private static readonly uint[] bitMask = new uint[32]
    {
      1U,
      2U,
      4U,
      8U,
      16U,
      32U,
      64U,
      128U,
      256U,
      512U,
      1024U,
      2048U,
      4096U,
      8192U,
      16384U,
      32768U,
      65536U,
      131072U,
      262144U,
      524288U,
      1048576U,
      2097152U,
      4194304U,
      8388608U,
      16777216U,
      33554432U,
      67108864U,
      134217728U,
      268435456U,
      536870912U,
      1073741824U,
      2147483648U
    };

    public static bool Bit(this uint register, int bitPosition)
    {
      if (bitPosition < 0 || bitPosition > 31)
        throw new ArgumentOutOfRangeException("bitPosition", "Valid bit positions are 0..31");
      uint num = IntExtensions.bitMask[bitPosition];
      return (register & num) > 0U;
    }

    public static bool Bit(this int register, int bitPosition)
    {
      return ((uint) register).Bit(bitPosition);
    }
  }
}
