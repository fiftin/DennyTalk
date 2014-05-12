using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Common;
using System.IO;
using System.Threading;

namespace DennyTalk
{
    public class FileSenderConnection : FileTransferClient
    {
        public string[] FileNames { get; private set; }
        public string RemoteHost { get; private set; }
        public int RemotePort { get; private set; }
        public string AccountGuid { get; private set; }

        private Thread transferThread;

        public FileSenderConnection(string accountGuid, string[] filenames, int requestId)
        {
            FileNames = filenames;
            AccountGuid = accountGuid;
            transferThread = new Thread(DoSend);
            transferThread.Name = "FileSenderClientThread";
            transferThread.IsBackground = true;
            RequestId = requestId;
        }

        public void SendAsync(string remoteHost, int remotePort)
        {
            RemoteHost = remoteHost;
            RemotePort = remotePort;
            transferThread.Start();
        }

        private void DoSend()
        {
            SendByTcp(RemoteHost, RemotePort, FileNames, RequestId);
        }

        protected void SendByTcp(string remoteHost, int remotePort, string[] filenames, int requestId)
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(remoteHost, remotePort);
                    NetworkStream stream = client.GetStream();
                    AuthFileTransfering auth = new AuthFileTransfering();
                    auth.guid = AccountGuid;
                    auth.numberOfFiles = filenames.Length;
                    auth.requestId = requestId;
                    MemoryHelper.WriteStructureToStream(auth, stream);
                    foreach (string fn in filenames)
                    {
                        CurrentFileNumber++;
                        if (IsCanceled)
                            break;
                        FileInfo f = new FileInfo(fn);
                        TransferedFileInfo fi = new TransferedFileInfo();
                        fi.filename = Path.GetFileName(fn);
                        fi.size = f.Length;
                        MemoryHelper.WriteStructureToStream(fi, stream);
                        byte[] buffer = new byte[1000];
                        int totalSent = 0;
                        using (FileStream inputStream = f.OpenRead())
                        {
                            OnLoadStart(fi.filename);
                            int read = inputStream.Read(buffer, 0, buffer.Length);
                            while (read > 0)
                            {
                                if (IsCanceled)
                                    break;
                                stream.Write(buffer, 0, read);
                                totalSent += read;
                                OnLoading(fi.filename, (totalSent / (float)fi.size) * 100f);
                                read = inputStream.Read(buffer, 0, buffer.Length);
                            }
                            OnLoadComplete(fi.filename);
                        }
                    }
                    client.Close();
                }
                catch (Exception ex)
                {
                    Reject();
                    OnError(ex);
                }
                finally
                {
                    OnFinished();
                }
            }
        }
    }

    /// <summary>
    /// Отправляет файл другому пользователю.
    /// </summary>
    public class FileSender
    {
        public string AccountGuid { get; set; }
        private TelegramListener telegramListener;
        private IDictionary<int, FileSenderConnection> connections = new SortedDictionary<int, FileSenderConnection>();

        public FileSender(TelegramListener telegramListener, string accountGuid)
        {
            AccountGuid = accountGuid;
            this.telegramListener = telegramListener;
            this.telegramListener.FilePortReceived += new EventHandler<FilePortReceivedEventArgs>(telegramListener_FilePortReceived);
        }

        void telegramListener_FilePortReceived(object sender, FilePortReceivedEventArgs e)
        {
            if (connections.ContainsKey(e.RequestId))
            {
                if (e.Port != -1)
                {
                    connections[e.RequestId].SendAsync(e.Address.Host, e.Port);
                }
                else
                    connections[e.RequestId].Reject();
                connections.Remove(e.RequestId);
            }
        }

        /// <summary>
        /// Отправляет телеграмму на адрес запрашивающую сеанс передачи файлов.
        /// </summary>
        public FileSenderConnection Send(Address address, string[] filenames)
        {
            FilePortRequest req = new FilePortRequest();
            req.numberOfFiles = filenames.Length;
            var res = telegramListener.Send(address, req);
            connections.Add(res.ID, new FileSenderConnection(AccountGuid, filenames, res.ID));
            return connections[res.ID];
        }
    }
}
