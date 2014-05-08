using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;
using Common;
using System.IO;

namespace DennyTalk
{
    /// <summary>
    /// Идентификация клиета инициировавшего передачу файла.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct AuthFileTransfering
    {
        [FieldOffset(0)]
        public int port;
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string guid;
    }

    /// <summary>
    /// Файл.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct TransferedFileInfo
    {
        [FieldOffset(0)]
        public long size;
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 300)]
        public string filename;
    }

    /// <summary>
    /// 
    /// </summary>
    class FileReceiverClient
    {
        public string PathForSave { get; private set; }
        public TcpClient Client { get; private set; }
        public Thread Thread { get; private set; }
        public event EventHandler<ExceptionEventArgs> Error;

        public FileReceiverClient(TcpClient client, string pathForSave)
        {
            Client = client;
            Thread = new Thread(Thread_DoReceive);
            PathForSave = pathForSave;
        }

        public bool IsClosed
        {
            get
            {
                return !Client.Connected;
            }
        }

        public void Thread_DoReceive()
        {
            try
            {
                var stream = Client.GetStream();
                AuthFileTransfering auth = MemoryHelper.ReadStructureFromStream<AuthFileTransfering>(stream);
                while (true)
                {
                    TransferedFileInfo fileInfo = MemoryHelper.ReadStructureFromStream<TransferedFileInfo>(stream);
                    using (FileStream outStream = File.OpenWrite(PathForSave + Path.PathSeparator + fileInfo.filename))
                    {
                        int totalRead = 0;
                        byte[] buffer = new byte[1000];
                        while (totalRead <= fileInfo.size)
                        {
                            int n = (int)Math.Min(buffer.LongLength, fileInfo.size - totalRead);
                            int read = stream.Read(buffer, 0, n);
                            if (read == -1)
                                break;
                            totalRead += read;
                            outStream.Write(buffer, 0, read);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void OnError(Exception ex)
        {
            if (Error != null)
                Error(this, new ExceptionEventArgs(ex));
        }
    }

    public class FileSender
    {
        public int Port { get; set; }
        public string Guid { get; set; }
        private TelegramListener telegramListener;

        public FileSender(TelegramListener telegramListener, int port, string guid)
        {
            Port = port;
            Guid = guid;
        }

        public void Send(Address address, string[] filenames)
        {

        }

        public void Send(IPEndPoint ep, string[] filenames)
        {
            using (TcpClient client = new TcpClient(ep))
            {
                NetworkStream stream = client.GetStream();
                AuthFileTransfering auth = new AuthFileTransfering();
                auth.port = Port;
                auth.guid = Guid;
                MemoryHelper.WriteStructureToStream(auth, stream);
                foreach (string fn in filenames)
                {
                    FileInfo f = new FileInfo(fn);
                    TransferedFileInfo fi = new TransferedFileInfo();
                    fi.filename = Path.GetFileName(fn);
                    fi.size = f.Length;
                    MemoryHelper.WriteStructureToStream(fi, stream);
                    byte[] buffer = new byte[1000];
                    using (FileStream inputStream = f.OpenRead())
                    {
                        int read = inputStream.Read(buffer, 0, buffer.Length);
                        while (read != -1)
                        {
                            stream.Write(buffer, 0, read);
                            read = inputStream.Read(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileReceiver
    {

        private TcpListener listener;
        private Thread listenThread;
        private List<FileReceiverClient> unauthenticatedClients = new List<FileReceiverClient>();
        private IDictionary<Contact, FileReceiverClient> clients = new SortedDictionary<Contact, FileReceiverClient>();
        private string PathForSave { get; set; }

        public FileReceiver(string pathForSave)
        {
            this.PathForSave = pathForSave;
            listener = new TcpListener(0);
        }

        public bool IsClosed
        {
            get
            {
                return !listener.Server.IsBound;
            }
        }

        public int Port
        {
            get
            {
                return ((IPEndPoint)listener.Server.LocalEndPoint).Port;
            }
        }

        public void Start()
        {
            listener.Start();
            listenThread = new Thread(listenThread_DoWork);
            listenThread.Start();
        }

        private void listenThread_DoWork()
        {
            while (true)
            {
                var client = listener.AcceptTcpClient();
                unauthenticatedClients.Add(new FileReceiverClient(client, PathForSave));
            }
        }
    }
}
