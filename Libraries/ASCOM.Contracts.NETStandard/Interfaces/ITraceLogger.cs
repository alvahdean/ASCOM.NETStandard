
using System.Runtime.InteropServices;

namespace ASCOM.Utilities.Interfaces
{
  //[ComVisible(true)]
  //[Guid("1C7ABC95-8B63-475e-B5DB-D0CE7ADC436B")]
  public interface ITraceLogger
  {
    [DispId(5)]
    bool Enabled { get; set; }

    [DispId(9)]
    string LogFileName { get; }

    [DispId(1)]
    void LogStart(string Identifier, string Message);

    [DispId(2)]
    void LogContinue(string Message, bool HexDump);

    [DispId(3)]
    void LogFinish(string Message, bool HexDump);

    [DispId(4)]
    void LogMessage(string Identifier, string Message, bool HexDump=false);

    [DispId(6)]
    void LogIssue(string Identifier, string Message);

    [DispId(7)]
    void SetLogFile(string LogFileName, string LogFileType);

    [DispId(8)]
    void BlankLine();

    [DispId(10)]
    void LogMessageCrLf(string Identifier, string Message);
    
    }
}
