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

    public class FileReceiverClient : FileTransferClient
    {
        public string PathForSave { get; private set; }
        public TcpClient Client { get; private set; }
        public string ContactGuid { get; set; }

        private Thread transferThread;
        public event EventHandler<AuthFileTransferingEventArgs> Auth;

        public FileReceiverClient(TcpClient client, string pathForSave)
        {
            Client = client;
            transferThread = new Thread(Thread_DoReceive);
            transferThread.Name = "FileReceiverClientThread";
            transferThread.IsBackground = true;
            PathForSave = pathForSave;
            transferThread.Start();
        }

        public bool IsClosed
        {
            get
            {
                return !Client.Connected;
            }
        }

        private string GetFreeFileName(string filename)
        {
            if (File.Exists(PathForSave + Path.DirectorySeparatorChar + filename))
            {
                string fnWithoutExt = Path.GetFileNameWithoutExtension(filename);
                string ext = Path.GetExtension(filename);
                int i = 1;
                string newFilename = string.Format("{0} ({1}){2}", fnWithoutExt, i, ext);
                while (File.Exists(PathForSave + Path.DirectorySeparatorChar + newFilename))
                {
                    i++;
                    newFilename = string.Format("{0} ({1}){2}", fnWithoutExt, i, ext);
                }
                return newFilename;
            }
            return filename;
        }

        public void Thread_DoReceive()
        {
            try
            {
                var stream = Client.GetStream();
                AuthFileTransfering auth = MemoryHelper.ReadStructureFromStream<AuthFileTransfering>(stream);
                OnAuth(auth.guid, auth.requestId);
                int numberOfDownloadedFiles = 0;
                while (numberOfDownloadedFiles < auth.numberOfFiles && !IsCanceled)
                {
                    TransferedFileInfo fileInfo = MemoryHelper.ReadStructureFromStream<TransferedFileInfo>(stream);
                    string filename = GetFreeFileName(fileInfo.filename);
                    OnLoadStart(filename);
                    CurrentFileNumber = numberOfDownloadedFiles;
                    using (FileStream outStream = File.OpenWrite(PathForSave + Path.DirectorySeparatorChar + filename))
                    {
                        int totalRead = 0;
                        byte[] buffer = new byte[1000];
                        while (totalRead <= fileInfo.size)
                        {
                            if (IsCanceled)
                                break;
                            int n = (int)Math.Min(buffer.LongLength, fileInfo.size - totalRead);
                            int read = stream.Read(buffer, 0, n);
                            if (read == 0)
                                break;
                            totalRead += read;
                            outStream.Write(buffer, 0, read);
                            OnLoading(fileInfo.filename, (totalRead / (float)fileInfo.size) * 100f);
                        }
                    }
                    OnLoadComplete(fileInfo.filename);
                    ++numberOfDownloadedFiles;
                }
            }
            catch (Exception ex)
            {
                Reject();
                OnError(ex);
            }
            finally
            {
                Client.Close();
                OnFinished();
            }
        }

        protected virtual void OnAuth(string guid, int requestId)
        {
            if (Auth != null)
                Auth(this, new AuthFileTransferingEventArgs(guid, requestId));
        }
    }

    public class FileReceiverClientEventArgs : EventArgs
    {
        public FileReceiverClient Client { get; private set; }
        public FileReceiverClientEventArgs(FileReceiverClient client)
        {
            Client = client;
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

        public event EventHandler<FileReceiverClientEventArgs> NewClient;
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
                client.LoadComplete += new EventHandler<FileLoadStartOrCompleteEventArgs>(client_DownloadComplete);
                unauthenticatedClients.Add(client);
            }
        }

        void client_DownloadComplete(object sender, FileLoadStartOrCompleteEventArgs e)
        {
            FileReceiverClient c = (FileReceiverClient)sender;
            c.Auth -= client_Auth;
            c.LoadComplete -= client_DownloadComplete;
            clients.Remove(c.ContactGuid);
        }

        void client_Auth(object sender, AuthFileTransferingEventArgs e)
        {
            FileReceiverClient c = (FileReceiverClient)sender;
            if (unauthenticatedClients.Remove(c))
            {
                clients.Add(e.Guid, c);
                c.ContactGuid = e.Guid;
                c.RequestId = e.RequestId;
                if (NewClient != null)
                    NewClient(this, new FileReceiverClientEventArgs((FileReceiverClient)sender));
            }
        }
    }
}
