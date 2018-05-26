using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASCOM.Havla
{
    public enum TcpMessageFormat { Binary, TextLine, TextRaw, Json };


    public interface IBasicAsyncTcpServer
    {
        TcpMessageFormat MessageFormat { get; set; }
        Encoding MessageEncoding { get; set; }
        bool IsRunning { get; }
        bool IsListening { get; }
        bool Accept { get; set; }
        bool CanAccept { get; }
        uint ClientCount { get; }
        uint MaxConnections { get; set; }
        String Welcome { get; set; }
        bool Start(int port, IPAddress addr = null);
        Task Listen();
        void Shutdown();
        void DisconnectSession(string sessionName);
    }

    public abstract class BasicAsyncTcpServer : IBasicAsyncTcpServer, IDisposable
    {
        private uint maxConnections;
        private Encoding messageEncoding;
        private Dictionary<String, TcpClient> sessions;
        private Dictionary<String, List<Task<bool>>> transactions;
        public BasicAsyncTcpServer()
        {
            Welcome = "Welcome. What brings you here today? (Just say 'bye' before you leave)";
            ClientCount = 0;
            MaxConnections = 5;
            Accept = true;
            sessions = new Dictionary<string, TcpClient>();
            transactions = new Dictionary<string, List<Task<bool>>>();
        }

        public Encoding MessageEncoding
        {
            get => messageEncoding ?? (messageEncoding = Encoding.ASCII);
            set
            {
                if (IsRunning)
                    throw new InvalidOperationException($"MessageEncoding cannot be set while server is running");
                if (MessageEncoding != value)
                    messageEncoding = value;
            }
        }

        public TcpMessageFormat MessageFormat { get; set; }

        public String Welcome { get; set; }

        protected TcpClient SessionClient(String sessionKey)
            => sessions.ContainsKey(sessionKey)
            ? sessions[sessionKey]
            : null;

        public bool CanAccept => (IsListening && Accept && ClientCount < MaxConnections);

        public bool IsListening => Listener?.Server.IsBound ?? false;

        public bool IsRunning => Listener != null;

        public bool Accept { get; set; }

        public uint ClientCount { get; protected set; }

        public uint MaxConnections
        {
            get => maxConnections;
            set
            {
                //TODO: Implement sync lock
                if (MaxConnections != value && value >= ClientCount)
                    maxConnections = value;
                else
                    throw new InvalidOperationException($"MaxConnection cannot be set less than the number of current connections");
            }
        }

        protected TcpListener Listener { get; private set; }

        protected void AddSessionTx(String sessionKey, Task<bool> tx)
        {
            if (String.IsNullOrWhiteSpace(sessionKey))
                throw new ArgumentException("Invalid session key");
            if (tx == null)
                throw new ArgumentNullException("Transaction task argument 'tx' cannot be null");

            if (!transactions.ContainsKey(sessionKey))
                transactions.Add(sessionKey, new List<Task<bool>>());
            transactions[sessionKey].Add(tx);

        }

        public void DisconnectSession(string sessionKey)
        {
            Console.WriteLine($"SESSION[{sessionKey}]: Disconnecting session");
            TcpClient client = null;
            if (sessions.ContainsKey(sessionKey))
            {
                client = sessions[sessionKey];
                if (client?.Connected ?? false)
                {
                    Console.WriteLine($"SESSION[{sessionKey}]: Closing client connection");
                    client.Close();
                }
                else
                    Console.WriteLine($"SESSION[{sessionKey}]: Client not connected");
                client?.Dispose();
                client = null;
                ClientCount--;
                Console.WriteLine($"SESSION[{sessionKey}]: Deregistering session");
                sessions.Remove(sessionKey);
            }
            else
                Console.WriteLine($"SESSION[{sessionKey}]: Session not registered");


            if (transactions.ContainsKey(sessionKey))
            {
                Console.WriteLine($"SESSION[{sessionKey}]: Reviewing transaction log...");
                transactions[sessionKey].Clear();
                transactions.Remove(sessionKey);
            }
            else
                Console.WriteLine($"SESSION[{sessionKey}]: No transaction registations");
            Console.WriteLine($"SESSION[{sessionKey}]: Client disconnect complete");
        }

        public void Shutdown()
        {
            if (!IsRunning)
                return;
            Console.WriteLine($"[Server]: Shutdown requested");
            Console.WriteLine($"[Server]: {sessions.Count} sessions active");
            foreach (string key in sessions.Keys)
            {
                if (sessions[key]?.Connected ?? false)
                {
                    Console.WriteLine($"[Server]: Closing active Session[{key}]");
                    sessions[key].Close();
                }
                else
                    Console.WriteLine($"[Server]: Session[{key}] already closed");
            }
            if (IsRunning || IsListening)
            {
                Console.WriteLine($"Shutting down listener....");
                Listener.Stop();
                if (IsListening)
                    Console.WriteLine($"Listener shutdown called but not complete....");
            }
            return;
        }

        public bool Start(int port, IPAddress addr = null)
        {
            if (addr == null)
                addr = IPAddress.Any;
            Listener = new TcpListener(addr, port);
            Console.WriteLine($"[Server]: {(IsRunning ? "running" : "start run failed")}");
            if (IsRunning)
            {
                Listener.Start();
                if (IsRunning)
                    Console.WriteLine($"[Server]: Listening to TCP clients at {addr}:{port}");
                else
                    Console.WriteLine($"[Server]: Listener failed to bind to local address={addr}, port={port}");
            }
            return IsListening;
        }

        public async Task Listen()
        {
            if (!IsListening)
            {
                Console.WriteLine($"Server must be bound to a socket before listening, did you call Start(...) first?");
                return;
            }
            DateTime startTime = DateTime.Now;
            DateTime lastNopReset = startTime;
            ulong sessionId = 0;
            ulong connId = 0;
            // Continue listening.  
            while (IsRunning && IsListening)
            {
                TcpClient client = null;
                Console.WriteLine("Waiting for client...");
                try
                {
                    client = await Listener.AcceptTcpClientAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Listen[Accept]: {ex.GetType().Name}: {ex.Message}");
                    Console.WriteLine($"Listen: exiting...");
                    return;
                }
                connId++;
                Console.WriteLine($"New client connection: {connId}");
                if (client == null)
                {
                    Console.WriteLine($"Connection[{connId}]: Connection failed to complete");
                }

                else if (!CanAccept)
                {
                    Console.WriteLine($"Connection[{connId}]: Server not accepting new clients, closing connection");
                    Console.WriteLine($"Connection[{connId}]: Informing client of impending disconnect...");
                    await WriteLineAsync(client, "Server busy, call back later.Bye.");
                    Console.WriteLine($"Connection[{connId}]: Disconnecting...");
                    client.Close();
                    client = null;
                    Console.WriteLine($"Connection[{connId}]: Connection terminated");
                }
                else
                {
                    ClientCount++;
                    sessionId++;
                    String sessionName = $"{sessionId}";
                    sessions.Add(sessionName, client);
                    transactions.Add(sessionName, new List<Task<bool>>());
                    Console.WriteLine($"SESSION[{sessionName}]: Created");
                    Console.WriteLine($"SESSION[{sessionName}]: Spawning session handler");
                    await HandleSessionAsync(sessionName);
                }
                Console.WriteLine($"[Listener]: Accept processing complete");
            }
        }

        protected object Read(TcpClient client, int maxLen = 4096)
        {
            Task<object> t = ReadAsync(client, maxLen);
            t.Wait();
            return t.Status == TaskStatus.RanToCompletion ? t.Result : null;
        }

        protected async Task<object> ReadAsync(TcpClient client, int maxLen = 4096)
        {
            object result = null;
            String errMsg = "";
            NetworkStream rs = null;
            String sresult;
            rs = client?.GetStream();
            if (!(rs?.CanRead ?? false))
            {
                errMsg = $"Client state invalid for reading";
                Console.WriteLine($"[Server] Read: {errMsg}");
                throw new Exception(errMsg);
            }
            switch (MessageFormat)
            {
                case TcpMessageFormat.Binary:
                    byte[] buf = new byte[maxLen];
                    int len = rs.Read(buf, 0, maxLen);
                    if (len > 0)
                    {
                        byte[] data = new byte[len];
                        Buffer.BlockCopy(buf, 0, data, 0, len);
                        result = data;
                    }
                    break;
                case TcpMessageFormat.TextLine:
                    sresult = (await new StreamReader(rs, MessageEncoding)?.ReadLineAsync());
                    result = sresult.TrimEnd(new char[] { '\n' }) ?? "";
                    break;
                case TcpMessageFormat.TextRaw:
                    result = await new StreamReader(rs, MessageEncoding)?.ReadToEndAsync();
                    break;
                case TcpMessageFormat.Json:
                    String json = await new StreamReader(rs, MessageEncoding)?.ReadToEndAsync();
                    if ((json?.Length ?? 0) > 0)
                        result = JsonConvert.DeserializeObject(json);
                    break;
                default:
                    Console.WriteLine($"[Read]: Unknown message format '{MessageFormat}' not supported");
                    break;
            }
            return result;
        }

        protected async Task<T> ReadAsync<T>(TcpClient client, int maxLen = 4096)
        {
            return (T)await ReadAsync(client, maxLen);
        }

        protected int Write<T>(TcpClient client, T obj)
        {
            Task<int> t = WriteAsync(client, obj);
            t.Wait();
            return t.Status == TaskStatus.RanToCompletion ? t.Result : 0;
        }

        protected int WriteLine(TcpClient client, String msg) => Write(client, $"{msg}\n");

        protected async Task<int> WriteAsync<T>(TcpClient client, T obj)
        {
            int result = 0;
            Type oType = typeof(T);
            NetworkStream oStream = client?.GetStream();

            Console.WriteLine($"[Server] Write<{oType.Name}>: Initializing");
            if (!(oStream?.CanWrite ?? false))
                Console.WriteLine($"[Server] Write: Client state invalid for writing");
            else if (obj == null)
            {
                Console.WriteLine($"[Server] Write: No data to write ({oType.Name})");
                return result;
            }
            byte[] buf;
            switch (MessageFormat)
            {
                case TcpMessageFormat.Binary:
                    if (typeof(byte[]).IsAssignableFrom(oType))
                    {
                        buf = obj as byte[];
                        if (buf.Length <= 0)
                            Console.WriteLine("Write: No data to write");
                        else
                        {
                            await oStream.WriteAsync(buf, 0, buf.Length);
                            result = buf.Length;
                        }
                    }
                    else
                    {
                        string errMsg = $"MessageFormat[{MessageFormat}] is not compatible with type '{oType.Name}'";
                        Console.WriteLine($"[Server] Write Error: {errMsg}");
                        throw new InvalidCastException(errMsg);
                    }
                    break;
                case TcpMessageFormat.TextLine:
                case TcpMessageFormat.TextRaw:
                    String text = obj.ToString();
                    if (String.IsNullOrEmpty(text))
                        Console.WriteLine("Write: No data to write");
                    else
                    {
                        if (MessageFormat == TcpMessageFormat.TextLine)
                            text += "\n";
                        buf = MessageEncoding.GetBytes(text);
                        await oStream.WriteAsync(buf, 0, buf.Length);
                        result = buf.Length;
                    }
                    break;
                case TcpMessageFormat.Json:
                    string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    buf = MessageEncoding.GetBytes(json);
                    await oStream.WriteAsync(buf, 0, buf.Length);
                    result = buf.Length;
                    break;
                default:
                    Console.WriteLine($"[Write]: Unknown message format '{MessageFormat}' not supported");
                    break;
            }
            return result;
        }

        protected async Task<int> WriteLineAsync(TcpClient client, String msg) => await WriteAsync(client, $"{msg}\n");

        virtual protected async Task HandleSessionAsync(String session)
        {
            TcpClient client = SessionClient(session);
            if (client == null)
            {
                Console.WriteLine($"SESSION[{session}]: No session registered");
                return;
            }
            Console.WriteLine($"SESSION[{session}]: Client session started");
            if (!String.IsNullOrWhiteSpace(Welcome))
                await WriteLineAsync(client, Welcome);
            ulong msgId = 0;
            while (client?.Connected ?? false)
            {
                try
                {
                    //StreamReader reader = new StreamReader(client.GetStream(), MessageEncoding);
                    string cData = await ReadAsync<String>(client);
                    msgId++;
                    Console.WriteLine($"SESSION[{session}]: Msg[{msgId}] recieved");
                    Task<bool> tx = HandleMessageAsync(session, cData);
                    if (tx != null)
                        AddSessionTx(session, tx);
                    Console.WriteLine($"SESSION[{session}]: Msg[{msgId}] task queued");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SESSION[{session}]: Exception[{ex.GetType()}]: {ex.Message}");

                }
            }
            DisconnectSession(session);
            Console.WriteLine($"SESSION[{session}]: Handler completed");
        }

        protected bool HandleMessage(String session, object msg)
        {
            Task<bool> t = HandleMessageAsync(session, msg.ToString());
            t.Wait();
            return t.Result;
        }

        virtual protected async Task<bool> HandleMessageAsync(String session, object msg)
        {
            Console.WriteLine($"SESSION[{session}]{nameof(HandleMessageAsync)}: Processing message");
            if(msg==null)
                Console.WriteLine($"SESSION[{session}]: NULL message, exiting");
            Task<bool> task = Task.Run(() => { return HandleMessage(session, msg); });
            return await task;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Shutdown();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class BasicTextServer : BasicAsyncTcpServer
    {
        private int msgId = 0;

        public BasicTextServer() : base() { }


        override protected async Task<bool> HandleMessageAsync(String session, object msgObj)
        {
            msgId++;
            bool terminate = false;
            string msg = msgObj.ToString();
            TcpClient client = SessionClient(session);
            if (client == null)
            {
                Console.WriteLine($"SESSION[{session}]: No session registered");
                return false;
            }
            Console.WriteLine($"SESSION[{session}][{msgId}]: Processing message '{msg}'");
            string response=null;
            if (msg.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
            {
                response = "Ok, I enjoyed talking with you. Bye now";
                terminate = true;
            }
            else if (msg == "")
                response = $"Sorry, I didn't catch that, please repeat it.";
            else if (msg.EndsWith("?"))
                response = $"Well... what you think the answer is?";
            else if (msg.StartsWith("~"))
            {
                response = $"ACCESS DENIED: Internal Command Pathways\nLet's talk about you instead...";
            }
            else
                response = $"I see... Tell me more.";
            try
            {

                if (response != null)
                {
                    Console.WriteLine($"SESSION[{session}][{msgId}]: Sending response '{response}'");
                    int length=await WriteLineAsync(client,response);
                    Console.WriteLine($"SESSION[{session}][{msgId}]: Response sent ({length} bytes)");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SESSION[{session}][{msgId}]: {ex.GetType().Name} '{ex.Message}'");
                response = null;
            }
            finally
            {
                if(response==null)
                    Console.WriteLine($"SESSION[{session}][{msgId}]: No response sent");
                if (terminate)
                {
                    Console.WriteLine($"SESSION[{session}][{msgId}]: Connection termination indicated...");
                    DisconnectSession(session);
                    Console.WriteLine($"SESSION[{session}][{msgId}]: Connection terminated");
                }
            }
            bool result = response != null;
            Console.WriteLine($"SESSION[{session}][{msgId}]: Message Handler complete (Result={result})");
            return result;
        }
    }
}