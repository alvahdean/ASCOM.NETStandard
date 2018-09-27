
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(true)]
  //[Guid("8828511A-05C1-43c7-8970-00D23595930A")]
  public interface ISerial
  {
    [DispId(1)]
    string[] AvailableComPorts { get; }

    [DispId(2)]
    int DataBits { get; set; }

    [DispId(3)]
    bool DTREnable { get; set; }

    [DispId(4)]
    SerialParity Parity { get; set; }

    [DispId(5)]
    SerialStopBits StopBits { get; set; }

    [DispId(6)]
    SerialHandshake Handshake { get; set; }

    [DispId(7)]
    bool Connected { get; set; }

    [DispId(8)]
    int Port { get; set; }

    [DispId(9)]
    int ReceiveTimeout { get; set; }

    [DispId(10)]
    int ReceiveTimeoutMs { get; set; }

    [DispId(11)]
    string PortName { get; set; }

    [DispId(12)]
    SerialSpeed Speed { get; set; }

    [DispId(23)]
    bool RTSEnable { get; set; }

    [DispId(13)]
    void ClearBuffers();

    [DispId(14)]
    void Transmit(string Data);

    [DispId(15)]
    void TransmitBinary(byte[] Data);

    [DispId(16)]
    void LogMessage(string Caller, string Message);

    [DispId(17)]
    string Receive();

    [DispId(18)]
    byte ReceiveByte();

    [DispId(19)]
    string ReceiveCounted(int Count);

    [DispId(20)]
    byte[] ReceiveCountedBinary(int Count);

    [DispId(21)]
    string ReceiveTerminated(string Terminator);

    [DispId(22)]
    byte[] ReceiveTerminatedBinary(byte[] TerminatorBytes);
  }
}
