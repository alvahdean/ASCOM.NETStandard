// Decompiled with JetBrains decompiler
// Type: ASCOM.Utilities.Serial
// Assembly: ASCOM.Utilities, Version=6.0.0.0, Culture=neutral, PublicKeyToken=565de7938946fba7
// MVID: 06F2ED6E-C559-496D-B7FF-DCA800F2230D
// Assembly location: C:\WINDOWS\assembly\GAC_MSIL\ASCOM.Utilities\6.0.0.0__565de7938946fba7\ASCOM.Utilities.dll


using ASCOM.Utilities.Exceptions;
using ASCOM.Utilities.Interfaces;
//using Microsoft.VisualBasic;
//using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ASCOM.Utilities
{
    [ClassInterface(ClassInterfaceType.None)]
    //[ComVisible(true)]
    //[Guid("0B8E6DC4-7451-4484-BE15-0D0727569FB8")]
    public class Serial : ISerial, IDisposable
    {
        private SerialPort m_Port;
        private string m_PortName;
        private int m_ReceiveTimeout;
        private SerialSpeed m_Speed;
        private bool m_Connected;
        private int m_DataBits;
        private bool m_DTREnable;
        private bool m_RTSEnable;
        private SerialHandshake m_Handshake;
        private SerialParity m_Parity;
        private SerialStopBits m_StopBits;
        private string TraceFile;
        private bool disposed;
        private bool DebugTrace;
        private Stopwatch ts;
        private TraceLogger Logger;
        private Encoding TextEncoding;
        private string m_SerTraceFile;
        private IAscomDataStore SerialProfile;
        private SortedList<string, string> ForcedCOMPorts;
        private SortedList<string, string> IgnoredCOMPorts;
        private Semaphore SerSemaphore;
        private bool SerPortInUse;
        private Semaphore CallCountSemaphore;
        private long CallCount;
        private const int TIMEOUT_NUMBER = -2147220478;
        private const string TIMEOUT_MESSAGE = "Timed out waiting for received data";
        private const int SEMAPHORE_TIMEOUT = 1000;
        private const int SERIALPORT_ENCODING = 1252;
        private const string SERIALPORT_DEFAULT_NAME = "COM1";
        private const int SERIALPORT_DEFAULT_TIMEOUT = 5000;
        private const SerialSpeed SERIALPORT_DEFAULT_SPEED = SerialSpeed.ps9600;
        private const int SERIALPORT_DEFAULT_DATABITS = 8;
        private const bool SERIALPORT_DEFAULT_DTRENABLE = true;
        private const bool SERIALPORT_DEFAULT_RTSENABLE = false;
        private const SerialHandshake SERIALPORT_DEFAULT_HANDSHAKE = SerialHandshake.None;
        private const SerialParity SERIALPORT_DEFAULT_PARITY = SerialParity.None;
        private const SerialStopBits SERIALPORT_DEFAULT_STOPBITS = SerialStopBits.One;
        private const int AVAILABLE_PORTS_SERIAL_TIMEOUT = 500;
        private const int AVAILABLE_PORTS_SERIAL_TIMEOUT_REPORT_THRESHOLD = 1000;
        private const bool SERIALPORT_DEFAULT_POLLING = false;
        private const string SERIAL_READ_POLLING = "ReadPolling";
        private bool UseReadPolling;
        private Serial.WaitType TypeOfWait;
        private const uint INFINITE = 4294967295;
        private const uint WAIT_ABANDONED = 128;
        private const uint WAIT_OBJECT_0 = 0;
        private const uint WAIT_TIMEOUT = 258;
        [SpecialName]
        private string __STATIC__FormatIDs__201EA__TransactionIDString;
        [SpecialName]
        private long __STATIC__FormatIDs__201EA__LastTransactionID;

        public int DataBits
        {
            get
            {
                return this.m_DataBits;
            }
            set
            {
                this.m_DataBits = value;
                this.Logger.LogMessage("DataBits", "Set to: " + value.ToString());
            }
        }

        public bool DTREnable
        {
            get
            {
                return this.m_DTREnable;
            }
            set
            {
                this.m_DTREnable = value;
                this.Logger.LogMessage("DTREnable", "Set to: " + value.ToString());
            }
        }

        public bool RTSEnable
        {
            get
            {
                return this.m_RTSEnable;
            }
            set
            {
                this.m_RTSEnable = value;
                this.Logger.LogMessage("RTSEnable", "Set to: " + value.ToString());
            }
        }

        public SerialHandshake Handshake
        {
            get
            {
                return this.m_Handshake;
            }
            set
            {
                this.m_Handshake = value;
                this.Logger.LogMessage("HandshakeType", "Set to: " + Enum.GetName(typeof(SerialHandshake), (object)value));
            }
        }

        public SerialParity Parity
        {
            get
            {
                return this.m_Parity;
            }
            set
            {
                this.m_Parity = value;
                this.Logger.LogMessage("Parity", "Set to: " + Enum.GetName(typeof(SerialParity), (object)value));
            }
        }

        public SerialStopBits StopBits
        {
            get
            {
                return this.m_StopBits;
            }
            set
            {
                this.m_StopBits = value;
                this.Logger.LogMessage("NumStopBits", "Set to: " + Enum.GetName(typeof(SerialStopBits), (object)value));
            }
        }

        public bool Connected
        {
            get
            {
                return this.m_Connected;
            }
            set
            {
                Serial.ThreadData TData = new Serial.ThreadData();
                Stopwatch stopwatch1 = new Stopwatch();
                Stopwatch stopwatch2 = Stopwatch.StartNew();
                bool result;
                bool flag1 = false;
                if (bool.TryParse(this.SerialProfile.GetProfile("COMPortSettings\\" + this.m_PortName, "RTSEnable"), out result))
                {
                    this.m_RTSEnable = result;
                    flag1 = true;
                }
                bool flag2 = false;
                if (bool.TryParse(this.SerialProfile.GetProfile("COMPortSettings\\" + this.m_PortName, "DTREnable"), out result))
                {
                    this.m_DTREnable = result;
                    flag2 = true;
                }
                if (bool.TryParse(this.SerialProfile.GetProfile("COMPortSettings", "ReadPolling"), out result))
                    this.UseReadPolling = result;
                try
                {
                    this.Logger.LogMessage("Set Connected", value.ToString());
                    if (value)
                    {
                        this.Logger.LogMessage("Set Connected", "Using COM port: " + this.m_PortName + " Baud rate: " + this.m_Speed.ToString() + " Timeout: " + this.m_ReceiveTimeout.ToString() + " DTR: " + this.m_DTREnable.ToString() + " ForcedDTR: " + flag2.ToString() + " RTS: " + this.m_RTSEnable.ToString() + " ForcedRTS: " + flag1.ToString() + " Handshake: " + this.m_Handshake.ToString() + " Encoding: " + 1252.ToString());
                        this.Logger.LogMessage("Set Connected", "Transmission format - Bits: " + Conversions.ToString(this.m_DataBits) + " Parity: " + Enum.GetName(this.m_Parity.GetType(), (object)this.m_Parity) + " Stop bits: " + Enum.GetName(this.m_StopBits.GetType(), (object)this.m_StopBits));
                        if (this.UseReadPolling)
                            this.Logger.LogMessage("Set Connected", "Reading COM Port through Read Polling");
                        else
                            this.Logger.LogMessage("Set Connected", "Reading COM port through Interrupt Handling");
                    }
                    TData.SerialCommand = Serial.SerialCommandType.Connected;
                    TData.Connecting = value;
                    TData.TransactionID = this.GetTransactionID("Set Connected");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ConnectedWorker), (object)TData);
                    this.WaitForThread(TData);
                    if (this.DebugTrace)
                        this.Logger.LogMessage("Set Connected", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                    if (TData.LastException != null)
                        throw TData.LastException;
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    this.Logger.LogMessage("Set Connected", ex.ToString());
                    throw;
                }
            }
        }

        public int Port
        {
            get
            {
                return Conversions.ToInteger(Strings.Mid(this.m_PortName, 4));
            }
            set
            {
                this.m_PortName = "COM" + value.ToString();
                this.Logger.LogMessage("Port", "Set to: " + value.ToString());
            }
        }

        public int ReceiveTimeout
        {
            get
            {
                return checked((int)Math.Round(unchecked((double)this.m_ReceiveTimeout / 1000.0)));
            }
            set
            {
                Serial.ThreadData TData = new Serial.ThreadData();
                try
                {
                    this.m_ReceiveTimeout = checked(value * 1000);
                    if (this.m_Connected)
                    {
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTimeout", "Start");
                        TData.SerialCommand = Serial.SerialCommandType.ReceiveTimeout;
                        TData.TimeoutValue = value;
                        TData.TransactionID = this.GetTransactionID("ReceiveTimeout");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveTimeoutWorker), (object)TData);
                        this.WaitForThread(TData);
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTimeout", "Completed: " + Conversions.ToString(TData.Completed));
                        if (TData.LastException != null)
                            throw TData.LastException;
                    }
                    else
                        this.Logger.LogMessage("ReceiveTimeout", "Set to: " + Conversions.ToString(value) + " seconds");
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    this.Logger.LogMessage("ReceiveTimeout", ex.ToString());
                    throw;
                }
            }
        }

        public int ReceiveTimeoutMs
        {
            get
            {
                return this.m_ReceiveTimeout;
            }
            set
            {
                Serial.ThreadData TData = new Serial.ThreadData();
                try
                {
                    this.m_ReceiveTimeout = value;
                    if (this.m_Connected)
                    {
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTimeoutMs", "Start");
                        TData.SerialCommand = Serial.SerialCommandType.ReceiveTimeoutMs;
                        TData.TimeoutValueMs = value;
                        TData.TransactionID = this.GetTransactionID("ReceiveTimeoutMs");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveTimeoutMsWorker), (object)TData);
                        this.WaitForThread(TData);
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTimeoutMs", "Completed: " + Conversions.ToString(TData.Completed));
                        if (TData.LastException != null)
                            throw TData.LastException;
                    }
                    else
                        this.Logger.LogMessage("ReceiveTimeoutMs", "Set to: " + Conversions.ToString(value) + " milli-seconds");
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    this.Logger.LogMessage("ReceiveTimeoutMs", ex.ToString());
                    throw;
                }
            }
        }

        public SerialSpeed Speed
        {
            get
            {
                return this.m_Speed;
            }
            set
            {
                this.m_Speed = value;
                this.Logger.LogMessage("Speed", "Set to: " + Enum.GetName(typeof(SerialSpeed), (object)value));
            }
        }

        public string PortName
        {
            get
            {
                return this.m_PortName;
            }
            set
            {
                this.m_PortName = value;
                this.Logger.LogMessage("PortName", "Set to: " + value);
            }
        }

        public string[] AvailableCOMPorts
        {
            get
            {
                Serial.PortNameComparer portNameComparer = new Serial.PortNameComparer();
                Serial.ThreadData TData = new Serial.ThreadData();
                try
                {
                    this.ForcedCOMPorts = this.SerialProfile.EnumProfile("COMPortSettings\\ForceCOMPorts");
                    try
                    {
                        this.ForcedCOMPorts.Remove("");
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator1 = null;
                    try
                    {
                        enumerator1 = this.ForcedCOMPorts.GetEnumerator();
                        while (enumerator1.MoveNext())
                        {
                            System.Collections.Generic.KeyValuePair<string, string> current = enumerator1.Current;
                            this.Logger.LogMessage("AvailableCOMPorts", "Forcing COM port " + current.Key + " " + current.Value + " to appear");
                        }
                    }
                    finally
                    {
                        if (enumerator1 != null)
                            enumerator1.Dispose();
                    }
                    this.IgnoredCOMPorts = this.SerialProfile.EnumProfile("COMPortSettings\\IgnoreCOMPorts");
                    try
                    {
                        this.IgnoredCOMPorts.Remove("");
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator2 = null;
                    try
                    {
                        enumerator2 = this.IgnoredCOMPorts.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            System.Collections.Generic.KeyValuePair<string, string> current = enumerator2.Current;
                            this.Logger.LogMessage("AvailableCOMPorts", "Ignoring COM port " + current.Key + " " + current.Value);
                        }
                    }
                    finally
                    {
                        if (enumerator2 != null)
                            enumerator2.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    this.Logger.LogMessage("AvailableCOMPorts Profile", ex.ToString());
                    //ProjectData.ClearProjectError();
                }
                if (this.DebugTrace)
                    this.Logger.LogMessage("AvailableCOMPorts", "Entered AvailableCOMPorts");
                List<string> stringList = new List<string>((IEnumerable<string>)SerialPort.GetPortNames());
                if (this.DebugTrace)
                    this.Logger.LogMessage("AvailableCOMPorts", "Retrieved port names using SerialPort.GetPortNames");
                IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator3 = null;
                try
                {
                    enumerator3 = this.ForcedCOMPorts.GetEnumerator();
                    while (enumerator3.MoveNext())
                    {
                        System.Collections.Generic.KeyValuePair<string, string> current = enumerator3.Current;
                        if (!stringList.Contains(Strings.Trim(current.Key)))
                            stringList.Add(Strings.Trim(current.Key));
                    }
                }
                finally
                {
                    if (enumerator3 != null)
                        enumerator3.Dispose();
                }
                IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator4 = null;
                try
                {
                    enumerator4 = this.IgnoredCOMPorts.GetEnumerator();
                    while (enumerator4.MoveNext())
                    {
                        System.Collections.Generic.KeyValuePair<string, string> current = enumerator4.Current;
                        if (!stringList.Contains(Strings.Trim(current.Key)))
                            stringList.Add(Strings.Trim(current.Key));
                    }
                }
                finally
                {
                    if (enumerator4 != null)
                        enumerator4.Dispose();
                }
                List<string> availableComPorts;
                try
                {
                    if (this.DebugTrace)
                        this.Logger.LogMessage("AvailableCOMPorts", "Start");
                    TData.SerialCommand = Serial.SerialCommandType.AvailableCOMPorts;
                    TData.AvailableCOMPorts = stringList;
                    TData.TransactionID = this.GetTransactionID("AvailableCOMPorts");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.AvailableCOMPortsWorker), (object)TData);
                    this.WaitForThread(TData);
                    if (this.DebugTrace)
                        this.Logger.LogMessage("AvailableCOMPorts", "Completed: " + Conversions.ToString(TData.Completed));
                    if (TData.LastException != null)
                        throw TData.LastException;
                    availableComPorts = TData.AvailableCOMPorts;
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    this.Logger.LogMessage("AvailableCOMPorts", ex.ToString());
                    throw;
                }
                IEnumerator<System.Collections.Generic.KeyValuePair<string, string>> enumerator5 = null;
                try
                {
                    enumerator5 = this.IgnoredCOMPorts.GetEnumerator();
                    while (enumerator5.MoveNext())
                    {
                        System.Collections.Generic.KeyValuePair<string, string> current = enumerator5.Current;
                        if (availableComPorts.Contains(Strings.Trim(current.Key)))
                            availableComPorts.Remove(Strings.Trim(current.Key));
                    }
                }
                finally
                {
                    if (enumerator5 != null)
                        enumerator5.Dispose();
                }
                availableComPorts.Sort((IComparer<string>)portNameComparer);
                List<string>.Enumerator enumerator6 = default(List<string>.Enumerator);
                try
                {
                    enumerator6 = availableComPorts.GetEnumerator();
                    while (enumerator6.MoveNext())
                        this.Logger.LogMessage("AvailableCOMPorts", enumerator6.Current);
                }
                finally
                {
                    enumerator6.Dispose();
                }
                if (this.DebugTrace)
                    this.Logger.LogMessage("AvailableCOMPorts", "Finished");
                return availableComPorts.ToArray();
            }
        }

        public Serial()
        {
            this.disposed = false;
            this.DebugTrace = false;
            this.ts = new Stopwatch();
            this.m_SerTraceFile = "C:\\SerialTrace.txt";
            this.SerialProfile = (RegistryAccess)null;
            this.SerPortInUse = false;
            this.CallCountSemaphore = new Semaphore(1, 1);
            this.UseReadPolling = false;
            this.TypeOfWait = Serial.WaitType.ManualResetEvent;
            this.SerSemaphore = new Semaphore(1, 1);
            this.m_Connected = false;
            this.m_PortName = "COM1";
            this.m_ReceiveTimeout = 5000;
            this.m_Speed = SerialSpeed.ps9600;
            this.m_DataBits = 8;
            this.m_DTREnable = true;
            this.m_RTSEnable = false;
            this.m_Handshake = SerialHandshake.None;
            this.m_Parity = SerialParity.None;
            this.m_StopBits = SerialStopBits.One;
            this.TextEncoding = Encoding.GetEncoding(1252);
            try
            {
                this.SerialProfile = new RegistryAccess();
                string profile = this.SerialProfile.GetProfile("", "SerTraceFile");
                this.Logger = new TraceLogger(profile, "Serial");
                if (Operators.CompareString(profile, "", false) != 0)
                    this.Logger.Enabled = true;
                this.DebugTrace = RegistryCommonCode.GetBool("Serial Trace Debug", false);
                this.TypeOfWait = RegistryCommonCode.GetWaitType("Serial Wait Type", Serial.WaitType.WaitForSingleObject);
                this.LogMessage("New", "Worker thread synchronisation by: " + this.TypeOfWait.ToString());
                int workerThreads;
                int completionPortThreads;
                ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
                if (this.DebugTrace)
                    this.Logger.LogMessage("New", "Minimum Threads: " + Conversions.ToString(workerThreads) + " " + Conversions.ToString(completionPortThreads));
                ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);
                if (this.DebugTrace)
                    this.Logger.LogMessage("New", "Maximum Threads: " + Conversions.ToString(workerThreads) + " " + Conversions.ToString(completionPortThreads));
                ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                if (!this.DebugTrace)
                    return;
                this.Logger.LogMessage("New", "Available Threads: " + Conversions.ToString(workerThreads) + " " + Conversions.ToString(completionPortThreads));
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                int num = (int)Interaction.MsgBox((object)("Serial:New exception " + ex.ToString()), MsgBoxStyle.OkOnly, (object)null);
                //ProjectData.ClearProjectError();
            }
        }

        ~Serial()
        {
            this.Dispose(false);
            // ISSUE: explicit finalizer call
            //base.Finalize();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (this.SerialProfile != null)
                {
                    try
                    {
                        this.SerialProfile.Dispose();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.SerialProfile = (RegistryAccess)null;
                }
                if (this.m_Port != null)
                {
                    try
                    {
                        this.m_Port.Dispose();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.m_Port = (SerialPort)null;
                }
                if (this.Logger != null)
                {
                    try
                    {
                        this.Logger.Dispose();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.Logger = (TraceLogger)null;
                }
                if (this.SerSemaphore != null)
                {
                    try
                    {
                        this.SerSemaphore.Release();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    try
                    {
                        this.SerSemaphore.Close();
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        //ProjectData.ClearProjectError();
                    }
                    this.SerSemaphore = (Semaphore)null;
                }
            }
            this.disposed = true;
        }

        private void ConnectedWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Connected: " + Conversions.ToString(threadData.Connecting));
                if (threadData.Connecting)
                {
                    if (!this.m_Connected)
                    {
                        if (this.m_Port == null)
                            this.m_Port = new SerialPort(this.m_PortName);
                        else
                            this.m_Port.PortName = this.m_PortName;
                        this.m_Port.BaudRate = (int)this.m_Speed;
                        this.m_Port.ReadTimeout = this.m_ReceiveTimeout;
                        this.m_Port.WriteTimeout = this.m_ReceiveTimeout;
                        this.m_Port.Encoding = Encoding.GetEncoding(1252);
                        this.m_Port.DtrEnable = this.m_DTREnable;
                        this.m_Port.RtsEnable = this.m_RTSEnable;
                        this.m_Port.Handshake = (System.IO.Ports.Handshake)this.m_Handshake;
                        this.m_Port.DataBits = this.m_DataBits;
                        this.m_Port.Parity = (System.IO.Ports.Parity)this.m_Parity;
                        this.m_Port.StopBits = (System.IO.Ports.StopBits)this.m_StopBits;
                        this.m_Port.Open();
                        this.m_Connected = true;
                        this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Port connected OK");
                    }
                    else
                        this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Port already connected");
                }
                else if (this.m_Connected)
                {
                    this.m_Connected = false;
                    this.m_Port.DiscardOutBuffer();
                    this.m_Port.DiscardInBuffer();
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Cleared buffers");
                    this.m_Port.Close();
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Closed port");
                    this.m_Port.Dispose();
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Port disposed OK");
                }
                else
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "Port already disconnected");
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ConnectedWorker", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: ConnectedWorker - " + exception.Message + " " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        private void ReceiveTimeoutWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                int timeoutValue = threadData.TimeoutValue;
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Set Start - TimeOut: " + Conversions.ToString(timeoutValue) + "seconds");
                int num = checked(timeoutValue * 1000);
                if (num <= 0 | num > 120000)
                    throw new ASCOM.InvalidValueException("ReceiveTimeout", Strings.Format((object)((double)num / 1000.0), "0.0"), "1 to 120 seconds");
                this.m_ReceiveTimeout = num;
                if (this.m_Connected)
                {
                    if (this.DebugTrace)
                        this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Connected so writing to serial port");
                    if (this.GetSemaphore("ReceiveTimeout", threadData.TransactionID))
                    {
                        try
                        {
                            this.m_Port.WriteTimeout = num;
                            this.m_Port.ReadTimeout = num;
                            this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Written to serial port OK");
                        }
                        catch (Exception ex)
                        {
                            //ProjectData.SetProjectError(ex);
                            Exception exception = ex;
                            this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: " + exception.ToString());
                            throw;
                        }
                        finally
                        {
                            this.ReleaseSemaphore("ReceiveTimeout", threadData.TransactionID);
                        }
                    }
                    else
                    {
                        this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                        throw new SerialPortInUseException("Serial:ReceiveTimeout - unable to get serial port semaphore before timeout.");
                    }
                }
                this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Set to: " + Conversions.ToString((double)num / 1000.0) + " seconds");
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveTimeout", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        private void ReceiveTimeoutMsWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                int timeoutValueMs = threadData.TimeoutValueMs;
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Set Start - TimeOut: " + timeoutValueMs.ToString() + "mS");
                if (timeoutValueMs <= 0 | timeoutValueMs > 120000)
                    throw new ASCOM.InvalidValueException("ReceiveTimeoutMs", timeoutValueMs.ToString(), "1 to 120000 milliseconds");
                this.m_ReceiveTimeout = timeoutValueMs;
                if (this.m_Connected)
                {
                    if (this.DebugTrace)
                        this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Connected so writing to serial port");
                    if (this.GetSemaphore("ReceiveTimeoutMs", threadData.TransactionID))
                    {
                        try
                        {
                            this.m_Port.WriteTimeout = timeoutValueMs;
                            this.m_Port.ReadTimeout = timeoutValueMs;
                            this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Written to serial port OK");
                        }
                        catch (Exception ex)
                        {
                            //ProjectData.SetProjectError(ex);
                            Exception exception = ex;
                            this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: " + exception.ToString());
                            throw;
                        }
                        finally
                        {
                            this.ReleaseSemaphore("ReceiveTimeoutMs", threadData.TransactionID);
                        }
                    }
                    else
                    {
                        this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                        throw new SerialPortInUseException("Serial:ReceiveTimeoutMs - unable to get serial port semaphore before timeout.");
                    }
                }
                this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Set to: " + timeoutValueMs.ToString() + "ms");
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveTimeoutMs", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public void ClearBuffers()
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            try
            {
                if (this.m_Connected)
                {
                    if (this.DebugTrace)
                        this.Logger.LogMessage("ClearBuffers", "Start");
                    TData.SerialCommand = Serial.SerialCommandType.ClearBuffers;
                    TData.TransactionID = this.GetTransactionID("ClearBuffers");
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.ClearBuffersWorker), (object)TData);
                    this.WaitForThread(TData);
                    if (this.DebugTrace)
                        this.Logger.LogMessage("ClearBuffers", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                    if (TData.LastException != null)
                        throw TData.LastException;
                }
                else
                    this.Logger.LogMessage("ClearBuffers", "***** ClearBuffers ignored because the port is not connected!");
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ClearBuffers", ex.ToString());
                throw;
            }
        }

        private void ClearBuffersWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + " " + Conversions.ToString(Thread.CurrentThread.ManagedThreadId) + "Start");
                if (this.GetSemaphore("ClearBuffersWorker", threadData.TransactionID))
                {
                    try
                    {
                        if (this.m_Port == null)
                            return;
                        if (this.m_Port.IsOpen)
                        {
                            this.m_Port.DiscardInBuffer();
                            this.m_Port.DiscardOutBuffer();
                            this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + "Completed");
                        }
                        else
                            this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + "Command ignored as port is not open");
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ClearBuffersWorker ", threadData.TransactionID);
                    }
                }
                else
                {
                    this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new COMException("Timed out waiting for received data", -2147220478);
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ClearBuffersWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public string Receive()
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.Receive command");
            string resultString;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("Receive", "Start");
                TData.SerialCommand = Serial.SerialCommandType.Receive;
                TData.TransactionID = this.GetTransactionID("Receive");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("Receive", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                resultString = TData.ResultString;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("Receive", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("Receive", ex.ToString());
                throw;
            }
            return resultString;
        }

        private void ReceiveWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            string str = "";
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + "Start");
                if (this.GetSemaphore("ReceiveWorker", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + "< ");
                        str = Conversions.ToString(this.ReadChar("ReceiveWorker", threadData.TransactionID));
                        str += this.m_Port.ReadExisting();
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + "< " + str);
                        else
                            this.Logger.LogFinish(str);
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + timeoutException.ToString());
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveWorker", threadData.TransactionID);
                    }
                    threadData.ResultString = str;
                }
                else
                {
                    this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("Serial:Receive - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public byte ReceiveByte()
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveByte command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveByte", "Start");
                TData.SerialCommand = Serial.SerialCommandType.Receivebyte;
                TData.TransactionID = this.GetTransactionID("ReceiveByte");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveByteWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveByte", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                return TData.ResultByte;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("ReceiveByte", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ReceiveByte", ex.ToString());
                throw;
            }
        }

        private void ReceiveByteWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "Start");
                if (this.GetSemaphore("ReceiveByteWorker", threadData.TransactionID))
                {
                    byte num;
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "< ");
                        num = this.ReadByte("ReceiveByteWorker", threadData.TransactionID);
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + " " + Conversions.ToString(Strings.Chr((int)num)), true);
                        else
                            this.Logger.LogFinish(Conversions.ToString(Strings.Chr((int)num)), true);
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: " + timeoutException.ToString());
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "EXCEPTION: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveByteWorker", threadData.TransactionID);
                    }
                    threadData.ResultByte = num;
                }
                else
                {
                    this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("Serial:ReceiveByte - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveByteWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public string ReceiveCounted(int Count)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveCounted command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCounted", "Start");
                TData.SerialCommand = Serial.SerialCommandType.ReceiveCounted;
                TData.Count = Count;
                TData.TransactionID = this.GetTransactionID("ReceiveCounted");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveCountedWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCounted", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                return TData.ResultString;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("ReceiveCounted", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ReceiveCounted", ex.ToString());
                throw;
            }
        }

        private void ReceiveCountedWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            string str = "";
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + "Start - count: " + threadData.Count.ToString());
                if (this.GetSemaphore("ReceiveCountedWorker", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveCountedWorker " + threadData.Count.ToString(), this.FormatIDs(threadData.TransactionID) + "< ");
                        int num1 = 1;
                        int count = threadData.Count;
                        int num2 = num1;
                        while (num2 <= count)
                        {
                            str += Conversions.ToString(this.ReadChar("ReceiveCountedWorker", threadData.TransactionID));
                            checked { ++num2; }
                        }
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + "< " + str);
                        else
                            this.Logger.LogFinish(str);
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + timeoutException.Message);
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + exception.Message);
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveCountedWorker", threadData.TransactionID);
                    }
                    threadData.ResultString = str;
                }
                else
                {
                    this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("ReceiveCounted - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveCountedWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public byte[] ReceiveCountedBinary(int Count)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            Stopwatch stopwatch1 = new Stopwatch();
            Stopwatch stopwatch2 = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveCountedBinary command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCountedBinary", "Start");
                TData.SerialCommand = Serial.SerialCommandType.ReceiveCountedBinary;
                TData.Count = Count;
                TData.TransactionID = this.GetTransactionID("ReceiveCountedBinary");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveCountedBinaryWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCountedBinary", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + stopwatch2.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                return TData.ResultByteArray;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("ReceiveCountedBinary", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ReceiveCountedBinary", ex.ToString());
                throw;
            }
        }

        private void ReceiveCountedBinaryWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            byte[] bytes = new byte[1];
            try
            {
                Encoding encoding = Encoding.GetEncoding(1252);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveCountedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Start - count: " + threadData.Count.ToString());
                if (this.GetSemaphore("ReceiveCountedBinaryWorker ", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveCountedBinaryWorker " + threadData.Count.ToString(), this.FormatIDs(threadData.TransactionID) + "< ");
                        int num1 = 0;
                        int num2 = checked(threadData.Count - 1);
                        int index = num1;
                        while (index <= num2)
                        {
                            List<byte> tmp = new List<byte>(bytes);
                            tmp.Add(0);
                            bytes = tmp.ToArray();
                            //(byte[])Microsoft.VisualBasic.CompilerServices.Utils.CopyArray((Array) bytes, (Array) new byte[checked (index + 1)]);
                            bytes[index] = this.ReadByte("ReceiveCountedBinaryWorker ", threadData.TransactionID);
                            checked { ++index; }
                        }
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveCountedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "< " + encoding.GetString(bytes), true);
                        else
                            this.Logger.LogFinish(encoding.GetString(bytes), true);
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveCountedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(encoding.GetString(bytes)) + "EXCEPTION: " + timeoutException.Message);
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveCountedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(encoding.GetString(bytes)) + "EXCEPTION: " + exception.Message);
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveCountedBinaryWorker ", threadData.TransactionID);
                    }
                    threadData.ResultByteArray = bytes;
                }
                else
                {
                    this.Logger.LogMessage("ReceiveCountedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new COMException("Timed out waiting for received data", -2147220478);
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveCountedBinaryWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public string ReceiveTerminated(string Terminator)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            this.ts = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveTerminated command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminated", "Start");
                TData.SerialCommand = Serial.SerialCommandType.ReceiveTerminated;
                TData.Terminator = Terminator;
                TData.TransactionID = this.GetTransactionID("ReceiveTerminated");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveTerminatedWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminated", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + this.ts.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                return TData.ResultString;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("ReceiveTerminated", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ReceiveTerminated", ex.ToString());
                throw;
            }
        }

        private void ReceiveTerminatedWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            string str = "";
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminatedWorker ", this.FormatIDs(threadData.TransactionID) + "Start - terminator: \"" + threadData.Terminator.ToString() + "\"");
                if (Operators.CompareString(threadData.Terminator, "", false) == 0)
                    throw new ASCOM.InvalidValueException("ReceiveTerminated Terminator", "Null or empty string", "Character or character string");
                if (this.GetSemaphore("ReceiveTerminatedWorker ", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveTerminatedWorker " + threadData.Terminator.ToString(), this.FormatIDs(threadData.TransactionID) + "< ");
                        int Length = Strings.Len(threadData.Terminator);
                        bool flag = false;
                        do
                        {
                            str += Conversions.ToString(this.ReadChar("ReceiveTerminatedWorker ", threadData.TransactionID));
                            if (Strings.Len(str) >= Length && Operators.CompareString(Strings.Right(str, Length), threadData.Terminator, false) == 0)
                                flag = true;
                        }
                        while (!flag);
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTerminatedWorker ", this.FormatIDs(threadData.TransactionID) + "< " + str);
                        else
                            this.Logger.LogFinish(str);
                    }
                    catch (ASCOM.InvalidValueException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        this.Logger.LogMessage("ReceiveTerminatedWorker ", this.FormatIDs(threadData.TransactionID) + "EXCEPTION - Terminator cannot be a null string");
                        throw;
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveTerminatedWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + timeoutException.ToString());
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveTerminatedWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str) + "EXCEPTION: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveTerminatedWorker ", threadData.TransactionID);
                    }
                    threadData.ResultString = str;
                }
                else
                {
                    this.Logger.LogMessage("ReceiveTerminatedWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("Serial:ReceiveTerminated - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveTerminatedWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public byte[] ReceiveTerminatedBinary(byte[] TerminatorBytes)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            this.ts = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.ReceiveTerminatedBinary command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminatedBinary", "Start");
                TData.SerialCommand = Serial.SerialCommandType.ReceiveCounted;
                TData.TerminatorBytes = TerminatorBytes;
                TData.TransactionID = this.GetTransactionID("ReceiveTerminatedBinary");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveTerminatedBinaryWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminatedBinary", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + this.ts.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                return TData.ResultByteArray;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("ReceiveTerminatedBinary", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("ReceiveTerminatedBinary", ex.ToString());
                throw;
            }
        }

        private void ReceiveTerminatedBinaryWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            string str1 = "";
            try
            {
                Encoding encoding = Encoding.GetEncoding(1252);
                string str2 = encoding.GetString(threadData.TerminatorBytes);
                if (this.DebugTrace)
                    this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Start - terminator: \"" + str2.ToString() + "\"");
                if (Operators.CompareString(str2, "", false) == 0)
                    throw new ASCOM.InvalidValueException("ReceiveTerminatedBinary Terminator", "Null or empty string", "Character or character string");
                if (this.GetSemaphore("ReceiveTerminatedBinaryWorker ", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogStart("ReceiveTerminatedBinaryWorker " + str2.ToString(), this.FormatIDs(threadData.TransactionID) + "< ");
                        int Length = Strings.Len(str2);
                        bool flag = false;
                        do
                        {
                            str1 += Conversions.ToString(this.ReadChar("ReceiveTerminatedBinaryWorker ", threadData.TransactionID));
                            if (Strings.Len(str1) >= Length && Operators.CompareString(Strings.Right(str1, Length), str2, false) == 0)
                                flag = true;
                        }
                        while (!flag);
                        if (this.DebugTrace)
                            this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "< " + str1, true);
                        else
                            this.Logger.LogFinish(str1, true);
                    }
                    catch (ASCOM.InvalidValueException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "EXCEPTION - Terminator cannot be a null string");
                        throw;
                    }
                    catch (TimeoutException ex)
                    {
                        //ProjectData.SetProjectError((Exception) ex);
                        TimeoutException timeoutException = ex;
                        this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str1) + "EXCEPTION: " + timeoutException.ToString());
                        throw new COMException("Timed out waiting for received data", -2147220478);
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + " " + this.FormatRXSoFar(str1) + "EXCEPTION: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("ReceiveTerminatedBinaryWorker ", threadData.TransactionID);
                    }
                    threadData.ResultByteArray = encoding.GetBytes(str1);
                }
                else
                {
                    this.Logger.LogMessage("ReceiveTerminatedBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new COMException("Timed out waiting for received data", -2147220478);
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("ReceiveTerminatedBinaryWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public void Transmit(string Data)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            this.ts = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.Transmit command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("Transmit", "Start");
                TData.SerialCommand = Serial.SerialCommandType.Transmit;
                TData.TransmitString = Data;
                TData.TransactionID = this.GetTransactionID("Transmit");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.TransmitWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("Transmit", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + this.ts.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("Transmit", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("Transmit", ex.ToString());
                throw;
            }
        }

        private void TransmitWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "> " + threadData.TransmitString);
                if (this.GetSemaphore("TransmitWorker", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "> " + threadData.TransmitString);
                        this.m_Port.Write(threadData.TransmitString);
                        if (!this.DebugTrace)
                            return;
                        this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "Completed");
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("TransmitWorker", threadData.TransactionID);
                    }
                }
                else
                {
                    this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("Serial:Transmit - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("TransmitWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        public void TransmitBinary(byte[] Data)
        {
            Serial.ThreadData TData = new Serial.ThreadData();
            this.ts = Stopwatch.StartNew();
            if (!this.m_Connected)
                throw new NotConnectedException("Serial port is not connected - you cannot use the Serial.TransmitBinary command");
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("TransmitBinary", "Start");
                TData.SerialCommand = Serial.SerialCommandType.ReceiveCounted;
                TData.TransmitBytes = Data;
                TData.TransactionID = this.GetTransactionID("TransmitBinary");
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.TransmitBinaryWorker), (object)TData);
                this.WaitForThread(TData);
                if (this.DebugTrace)
                    this.Logger.LogMessage("TransmitBinary", "Completed: " + Conversions.ToString(TData.Completed) + ", Duration: " + this.ts.Elapsed.TotalMilliseconds.ToString("0.0"));
                if (TData.LastException != null)
                    throw TData.LastException;
                string resultString = TData.ResultString;
            }
            catch (TimeoutException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                this.Logger.LogMessage("TransmitBinary", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                this.Logger.LogMessage("TransmitBinary", ex.ToString());
                throw;
            }
        }

        private void TransmitBinaryWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            try
            {
                string p_Msg = this.TextEncoding.GetString(threadData.TransmitBytes);
                if (this.DebugTrace)
                    this.Logger.LogMessage("TransmitBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "> " + p_Msg + " (HEX" + this.MakeHex(p_Msg) + ") ");
                if (this.GetSemaphore("TransmitBinaryWorker ", threadData.TransactionID))
                {
                    try
                    {
                        if (!this.DebugTrace)
                            this.Logger.LogMessage("TransmitBinaryWorker", this.FormatIDs(threadData.TransactionID) + "> " + p_Msg + " (HEX" + this.MakeHex(p_Msg) + ") ");
                        this.m_Port.Write(threadData.TransmitBytes, 0, threadData.TransmitBytes.Length);
                        if (!this.DebugTrace)
                            return;
                        this.Logger.LogMessage("TransmitBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Completed");
                    }
                    catch (Exception ex)
                    {
                        //ProjectData.SetProjectError(ex);
                        Exception exception = ex;
                        this.Logger.LogMessage("TransmitBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                        throw;
                    }
                    finally
                    {
                        this.ReleaseSemaphore("TransmitBinaryWorker ", threadData.TransactionID);
                    }
                }
                else
                {
                    this.Logger.LogMessage("TransmitBinaryWorker ", this.FormatIDs(threadData.TransactionID) + "Throwing SerialPortInUse exception");
                    throw new SerialPortInUseException("TransmitBinary - unable to get serial port semaphore before timeout.");
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("TransmitBinaryWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                threadData.ThreadCompleted();
            }
        }

        private void AvailableCOMPortsWorker(object TDataObject)
        {
            Serial.ThreadData threadData = (Serial.ThreadData)TDataObject;
            string str = "";
            Stopwatch stopwatch = new Stopwatch();
            SerialPort serialPort = new SerialPort();
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Started");
                try
                {
                    if (this.DebugTrace)
                        this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Port probe started");
                    List<string> availableComPorts = threadData.AvailableCOMPorts;
                    serialPort.ReadTimeout = 500;
                    serialPort.WriteTimeout = 500;
                    int num = 0;
                    do
                    {
                        try
                        {
                            str = "COM" + num.ToString();
                            if (!availableComPorts.Contains(str))
                            {
                                if (this.DebugTrace)
                                    this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Starting to probe port " + Conversions.ToString(num));
                                stopwatch.Reset();
                                stopwatch.Start();
                                serialPort.PortName = str;
                                serialPort.Open();
                                serialPort.Close();
                                stopwatch.Stop();
                                if (stopwatch.ElapsedMilliseconds >= 1000L)
                                    this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Probing port " + str + " took  a long time: " + Conversions.ToString(stopwatch.ElapsedMilliseconds) + "ms");
                                availableComPorts.Add(str);
                                if (this.DebugTrace)
                                    this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Port " + Conversions.ToString(num) + " exists, elapsed time: " + Conversions.ToString(stopwatch.ElapsedMilliseconds) + "ms");
                            }
                            else if (this.DebugTrace)
                                this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Skiping probe as port  " + str + " is already known to exist");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            //ProjectData.SetProjectError((Exception) ex);
                            availableComPorts.Add(str);
                            if (this.DebugTrace)
                                this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Port " + Conversions.ToString(num) + " UnauthorisedAccessException, elapsed time: " + Conversions.ToString(stopwatch.ElapsedMilliseconds) + "ms");
                            //ProjectData.ClearProjectError();
                        }
                        catch (Exception ex)
                        {
                            //ProjectData.SetProjectError(ex);
                            Exception exception = ex;
                            if (this.DebugTrace)
                                this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Port " + Conversions.ToString(num) + " Exception, found is, elapsed time: " + Conversions.ToString(stopwatch.ElapsedMilliseconds) + "ms " + exception.Message);
                            //ProjectData.ClearProjectError();
                        }
                        checked { ++num; }
                    }
                    while (num <= 32);
                    threadData.AvailableCOMPorts = availableComPorts;
                    if (!this.DebugTrace)
                        return;
                    this.Logger.LogMessage("AvailableCOMPortsWorker ", this.FormatIDs(threadData.TransactionID) + "Completed");
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    Exception exception = ex;
                    this.Logger.LogMessage("AvailableCOMPortsWorker ", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                    throw;
                }
            }
            catch (Exception ex1)
            {
                //ProjectData.SetProjectError(ex1);
                Exception exception = ex1;
                try
                {
                    this.Logger.LogMessage("AvailableCOMPortsWorker", this.FormatIDs(threadData.TransactionID) + "Exception: " + exception.ToString());
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    threadData.LastException = exception;
                }
                catch (Exception ex2)
                {
                    //ProjectData.SetProjectError(ex2);
                    //ProjectData.ClearProjectError();
                }
                //ProjectData.ClearProjectError();
            }
            finally
            {
                try
                {
                    serialPort.Dispose();
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    //ProjectData.ClearProjectError();
                }
                threadData.ThreadCompleted();
            }
        }

        public void LogMessage(string Caller, string Message)
        {
            this.Logger.LogMessage(Caller, Message);
        }

        private string FormatRXSoFar(string p_Chars)
        {
            if (Operators.CompareString(p_Chars, "", false) != 0)
                return p_Chars + " ";
            return "";
        }

        private string MakeHex(string p_Msg)
        {
            StringBuilder sb = new StringBuilder();
            byte[] barr = Encoding.ASCII.GetBytes(p_Msg);
            foreach (byte b in barr)
                sb.Append($"[{b:X2}]");
            return sb.ToString();
        }

        private bool GetSemaphore(string p_Caller, long MyCallNumber)
        {
            DateTime now = DateTime.Now;
            bool flag = false;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Entered GetSemaphore ");
                flag = this.SerSemaphore.WaitOne(checked(this.m_ReceiveTimeout + 2000), false);
                if (this.DebugTrace)
                {
                    if (flag)
                        this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Got Semaphore OK");
                    else
                        this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Failed to get Semaphore, returning: False");
                }
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                this.Logger.LogMessage("GetSemaphore", MyCallNumber.ToString() + Conversions.ToString(Thread.CurrentThread.ManagedThreadId) + " Exception: " + exception.ToString() + " " + exception.StackTrace);
                //ProjectData.ClearProjectError();
            }
            return flag;
        }

        private void ReleaseSemaphore(string p_Caller, long MyCallNumber)
        {
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Entered ReleaseSemaphore " + Conversions.ToString(Thread.CurrentThread.ManagedThreadId));
                this.SerSemaphore.Release();
                if (!this.DebugTrace)
                    return;
                this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Semaphore released OK");
            }
            catch (SemaphoreFullException ex)
            {
                //ProjectData.SetProjectError((Exception) ex);
                SemaphoreFullException semaphoreFullException = ex;
                this.Logger.LogMessage("ReleaseSemaphore", this.FormatIDs(MyCallNumber) + "SemaphoreFullException: " + semaphoreFullException.ToString() + " " + semaphoreFullException.StackTrace);
                //ProjectData.ClearProjectError();
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                this.Logger.LogMessage("ReleaseSemaphore", this.FormatIDs(MyCallNumber) + "Exception: " + exception.ToString() + " " + exception.StackTrace);
                //ProjectData.ClearProjectError();
            }
        }

        private long GetTransactionID(string p_Caller)
        {
            long callCount;
            try
            {
                if (this.DebugTrace)
                    this.Logger.LogMessage(p_Caller, this.FormatIDs(0L) + "Entered GetNextCount ");
                this.CallCountSemaphore.WaitOne();
                if (this.DebugTrace)
                    this.Logger.LogMessage(p_Caller, this.FormatIDs(0L) + "Got CallCountMutex");
                this.CallCount = this.CallCount == long.MaxValue ? 0L : checked(this.CallCount + 1L);
                callCount = this.CallCount;
                this.CallCountSemaphore.Release();
                if (this.DebugTrace)
                    this.Logger.LogMessage(p_Caller, this.FormatIDs(callCount) + "Released CallCountMutex");
            }
            catch (Exception ex)
            {
                //ProjectData.SetProjectError(ex);
                Exception exception = ex;
                this.Logger.LogMessage(p_Caller, "EXCEPTION: " + exception.ToString());
                throw;
            }
            return callCount;
        }

        private byte ReadByte(string p_Caller, long MyCallNumber)
        {
            byte[] numArray = new byte[11];
            DateTime now = DateTime.Now;
            if (this.DebugTrace)
                this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "Entered ReadByte: " + Conversions.ToString(this.UseReadPolling));
            if (this.UseReadPolling)
            {
                while (this.m_Port.BytesToRead == 0)
                {
                    if ((DateTime.Now - now).TotalMilliseconds > (double)this.m_ReceiveTimeout)
                    {
                        this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "ReadByte timed out waitng for a byte to read, throwing TimeoutException");
                        throw new TimeoutException("Serial port timed out waiting to read a byte");
                    }
                    Thread.Sleep(1);
                }
            }
            byte num = checked((byte)this.m_Port.ReadByte());
            if (this.DebugTrace)
                this.Logger.LogMessage(p_Caller, this.FormatIDs(MyCallNumber) + "ReadByte returning result - \"" + num.ToString() + "\"");
            return num;
        }

        private char ReadChar(string p_Caller, long TransactionID)
        {
            char[] chArray = new char[11];
            DateTime now = DateTime.Now;
            if (this.DebugTrace)
                this.Logger.LogMessage(p_Caller, this.FormatIDs(TransactionID) + "Entered ReadChar: " + Conversions.ToString(this.UseReadPolling));
            if (this.UseReadPolling)
            {
                while (this.m_Port.BytesToRead == 0)
                {
                    if ((DateTime.Now - now).TotalMilliseconds > (double)this.m_ReceiveTimeout)
                    {
                        this.Logger.LogMessage(p_Caller, this.FormatIDs(TransactionID) + "ReadByte timed out waitng for a character to read, throwing TimeoutException");
                        throw new TimeoutException("Serial port timed out waiting to read a character");
                    }
                    Thread.Sleep(1);
                }
            }
            char ch = Strings.Chr(this.m_Port.ReadByte());
            if (this.DebugTrace)
                this.Logger.LogMessage(p_Caller, this.FormatIDs(TransactionID) + "ReadChar returning result - \"" + Conversions.ToString(ch) + "\"");
            return ch;
        }

        internal string FormatIDs(long TransactionID)
        {
            if (!this.DebugTrace)
                return "";
            if (TransactionID != 0L)
            {
                this.__STATIC__FormatIDs__201EA__LastTransactionID = TransactionID;
                this.__STATIC__FormatIDs__201EA__TransactionIDString = TransactionID.ToString();
            }
            else
                this.__STATIC__FormatIDs__201EA__TransactionIDString = Strings.Left(Strings.Space(8), Strings.Len(this.__STATIC__FormatIDs__201EA__LastTransactionID.ToString()));
            return this.__STATIC__FormatIDs__201EA__TransactionIDString + " " + Conversions.ToString(Thread.CurrentThread.ManagedThreadId) + " ";
        }

        [DllImport("kernel32", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

        private void WaitForThread(Serial.ThreadData TData)
        {
            switch (this.TypeOfWait)
            {
                case Serial.WaitType.ManualResetEvent:
                    if (this.DebugTrace)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        TData.ManualResetEvent.WaitOne(-1);
                        this.LogMessage("WaitForThread", this.FormatIDs(TData.TransactionID) + "Completed ManualResetEvent OK, Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                        break;
                    }
                    TData.ManualResetEvent.WaitOne(-1);
                    break;
                case Serial.WaitType.Sleep:
                    if (this.DebugTrace)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        do
                        {
                            Thread.Sleep(1);
                            this.LogMessage("WaitForThread", this.FormatIDs(TData.TransactionID) + "Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                        }
                        while (!TData.Completed);
                        this.LogMessage("WaitForThread", this.FormatIDs(TData.TransactionID) + "Completed Sleep OK, Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                        break;
                    }
                    do
                    {
                        Thread.Sleep(1);
                    } while (!TData.Completed);
                    break;
                case Serial.WaitType.WaitForSingleObject:
                    if (this.DebugTrace)
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        uint num = Serial.WaitForSingleObject(TData.ManualResetEvent.SafeWaitHandle.DangerousGetHandle(), uint.MaxValue);
                        switch (num)
                        {
                            case 0:
                                this.LogMessage("WaitForThread", this.FormatIDs(TData.TransactionID) + "Completed WaitForSingleObject OK, Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                                break;
                            case 128:
                                this.LogMessage("***WaitForThread***", this.FormatIDs(TData.TransactionID) + "Completed WaitForSingleObject - ABANDONED, Return code: 0x" + num.ToString("X8") + ", Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                                break;
                            case 258:
                                this.LogMessage("***WaitForThread***", this.FormatIDs(TData.TransactionID) + "Completed WaitForSingleObject - TIMEOUT, Return code: 0x" + num.ToString("X8") + ", Command: " + TData.SerialCommand.ToString() + " Elapsed: " + Conversions.ToString(stopwatch.Elapsed.TotalMilliseconds));
                                break;
                        }
                    }
                    else
                    {
                        uint num = Serial.WaitForSingleObject(TData.ManualResetEvent.SafeWaitHandle.DangerousGetHandle(), uint.MaxValue);
                        switch (num)
                        {
                            case 128:
                                this.LogMessage("***WaitForThread***", this.FormatIDs(TData.TransactionID) + "Completed WaitForSingleObject - ABANDONED, Return code: 0x" + num.ToString("X8") + ", Command: " + TData.SerialCommand.ToString());
                                break;
                            case 258:
                                this.LogMessage("***WaitForThread***", this.FormatIDs(TData.TransactionID) + "Completed WaitForSingleObject - TIMEOUT, Return code: 0x" + num.ToString("X8") + ", Command: " + TData.SerialCommand.ToString());
                                break;
                        }
                    }
                    break;
            }
            if (!(this.DebugTrace & TData.TransactionID != this.CallCount))
                return;
            this.LogMessage("***WaitForThread***", "Transactions out of order! TransactionID CurrentCallCount: " + Conversions.ToString(TData.TransactionID) + " " + Conversions.ToString(this.CallCount));
        }

        private enum SerialCommandType
        {
            AvailableCOMPorts,
            ClearBuffers,
            Connected,
            Receive,
            Receivebyte,
            ReceiveCounted,
            ReceiveCountedBinary,
            ReceiveTerminated,
            ReceiveTerminatedBinary,
            ReceiveTimeout,
            ReceiveTimeoutMs,
            Transmit,
            TransmitBinary,
        }
#warning Internal enum exposed as public during porting: WaitType
        public enum WaitType
        //internal enum WaitType
        {
            ManualResetEvent,
            Sleep,
            WaitForSingleObject,
        }

        internal class PortNameComparer : IComparer<string>
        {
            [DebuggerNonUserCode]
            public PortNameComparer()
            {
            }

            int IComparer<string>.Compare(string x, string y)
            {
                string str1 = x.ToString();
                string str2 = y.ToString();
                if (Strings.Len(str1) >= 4 & Strings.Len(str2) >= 4 && Operators.CompareString(Strings.Left(str1, 3).ToUpper(), "COM", false) == 0 & Operators.CompareString(Strings.Left(str2, 3).ToUpper(), "COM", false) == 0 && Versioned.IsNumeric((object)Strings.Mid(str1, 4)) & Versioned.IsNumeric((object)Strings.Mid(str2, 4)))
                    return Comparer.Default.Compare((object)Conversions.ToInteger(Strings.Mid(str1, 4)), (object)Conversions.ToInteger(Strings.Mid(str2, 4)));
                return Comparer.Default.Compare((object)x, (object)y);
            }
        }

        private class ThreadData
        {
            public string TransmitString;
            public byte[] TransmitBytes;
            public string Terminator;
            public byte[] TerminatorBytes;
            public int Count;
            public Exception LastException;
            public byte ResultByte;
            public byte[] ResultByteArray;
            public string ResultString;
            public char ResultChar;
            public List<string> AvailableCOMPorts;
            public Serial.SerialCommandType SerialCommand;
            public bool Completed;
            public ManualResetEvent ManualResetEvent;
            public long TransactionID;
            public bool Connecting;
            public int TimeoutValueMs;
            public int TimeoutValue;

            public ThreadData()
            {
                this.Completed = false;
                this.LastException = (Exception)null;
                this.ManualResetEvent = new ManualResetEvent(false);
            }

            public void ThreadCompleted()
            {
                try
                {
                    this.Completed = true;
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    //ProjectData.ClearProjectError();
                }
                try
                {
                    this.ManualResetEvent.Set();
                }
                catch (Exception ex)
                {
                    //ProjectData.SetProjectError(ex);
                    //ProjectData.ClearProjectError();
                }
            }
        }

        public string[] AvailableComPorts
        {
            get
            {
                return System.IO.Ports.SerialPort.GetPortNames();
            }
        }
    }
}
