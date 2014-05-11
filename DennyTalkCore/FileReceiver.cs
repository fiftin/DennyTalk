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
        //[FieldOffset(0)]
        //public int port;

        [FieldOffset(0)]
        public int numberOfFiles;

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

    public class FileDownloadStartOrCompleteEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public FileDownloadStartOrCompleteEventArgs(string filename)
        {
            FileName = filename;
        }
    }

    public class AuthFileTransferingEventArgs : EventArgs
    {
        public string Guid { get; private set; }
        public AuthFileTransferingEventArgs(string guid)
        {
            Guid = guid;
        }
    }

    public class FileDownloadingEventArgs : EventArgs
    {
        public string FileName { get; private set; }
        public float Percent { get; private set; }
        public FileDownloadingEventArgs(string filename, float percent)
        {
            FileName = filename;
            Percent = percent;
        }
    }

    public class FileReceiverClient
    {
        public string PathForSave { get; private set; }
        public TcpClient Client { get; private set; }
        private Thread Thread { get; set; }

        public event EventHandler<ExceptionEventArgs> Error;
        public event EventHandler<FileDownloadStartOrCompleteEventArgs> DownloadStart;
        public event EventHandler<FileDownloadStartOrCompleteEventArgs> DownloadComplete;
        public event EventHandler<FileDownloadingEventArgs> Downloading;
        public event EventHandler<AuthFileTransferingEventArgs> Auth;

        public FileReceiverClient(TcpClient client, string pathForSave)
        {
            Client = client;
            Thread = new Thread(Thread_DoReceive);
            Thread.Name = "FileReceiverClientThread";
            Thread.IsBackground = true;
            PathForSave = pathForSave;
            Thread.Start();
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
                OnAuth(auth.guid);
                int numberOfDownloadedFiles = 0;
                while (numberOfDownloadedFiles < auth.numberOfFiles)
                {
                    TransferedFileInfo fileInfo = MemoryHelper.ReadStructureFromStream<TransferedFileInfo>(stream);
                    OnDownloadStart(fileInfo.filename);
                    using (FileStream outStream = File.OpenWrite(PathForSave + Path.DirectorySeparatorChar + fileInfo.filename))
                    {
                        int totalRead = 0;
                        byte[] buffer = new byte[1000];
                        while (totalRead <= fileInfo.size)
                        {
                            int n = (int)Math.Min(buffer.LongLength, fileInfo.size - totalRead);
                            int read = stream.Read(buffer, 0, n);
                            if (read == 0)
                                break;
                            totalRead += read;
                            outStream.Write(buffer, 0, read);
                            OnDownloading(fileInfo.filename, fileInfo.size / totalRead);
                        }
                    }
                    OnDownloadComplete(fileInfo.filename);
                    ++numberOfDownloadedFiles;
                }
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        protected virtual void OnAuth(string guid)
        {
            if (Auth != null)
                Auth(this, new AuthFileTransferingEventArgs(guid));
        }

        protected virtual void OnDownloadComplete(string filename)
        {
            Console.WriteLine("OnDownloadComplete");
            if (DownloadComplete != null)
                DownloadComplete(this, new FileDownloadStartOrCompleteEventArgs(filename));
        }

        protected virtual void OnDownloading(string filename, float percent)
        {
            Console.WriteLine("OnDownloading");
            if (Downloading != null)
                Downloading(this, new FileDownloadingEventArgs(filename, percent));
        }

        protected virtual void OnDownloadStart(string filename)
        {
            Console.WriteLine("OnDownloadStart");
            if (DownloadStart != null)
                DownloadStart(this, new FileDownloadStartOrCompleteEventArgs(filename));
        }

        protected virtual void OnError(Exception ex)
        {
            if (Error != null)
                Error(this, new ExceptionEventArgs(ex));
            Console.WriteLine(ex.Message);
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
        private IDictionary<string, FileReceiverClient> clients = new SortedDictionary<string, FileReceiverClient>();
        private string PathForSave { get; set; }

        public FileReceiver(string pathForSave)
        {
            this.PathForSave = pathForSave;
            listener = new TcpListener(IPAddress.Loopback, 2000);
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
            listenThread.IsBackground = true;
            listenThread.Start();
            Thread.Sleep(100);
        }

        private void listenThread_DoWork()
        {
            while (true)
            {
                TcpClient c = listener.AcceptTcpClient();
                var client = new FileReceiverClient(c, PathForSave);
                client.Auth += new EventHandler<AuthFileTransferingEventArgs>(client_Auth);
                unauthenticatedClients.Add(client);
            }
        }

        void client_Auth(object sender, AuthFileTransferingEventArgs e)
        {
            FileReceiverClient c = (FileReceiverClient)sender;
            if (unauthenticatedClients.Remove(c))
                clients.Add(e.Guid, c);
        }
    }
}
