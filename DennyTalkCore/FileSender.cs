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
    public class FileSenderConnection
    {
        private bool cancel;
        private Thread sendThread;
        public int TelegramId { get; private set; }
        public string[] FileNames { get; private set; }
        public IPEndPoint RemoteIP { get; private set; }
        public string AccountGuid { get; private set; }

        public FileSenderConnection(string accountGuid, string[] filenames, int telegramId)
        {
            FileNames = filenames;
            AccountGuid = accountGuid;
            sendThread = new Thread(DoSend);
            sendThread.IsBackground = true;
            TelegramId = telegramId;
        }

        public void SendAsync(IPEndPoint ip)
        {
            RemoteIP = ip;
            sendThread.Start();
        }

        private void DoSend()
        {
            SendByTcp(RemoteIP, FileNames);
        }

        public void Cancel()
        {
            cancel = true;
        }

        protected void SendByTcp(IPEndPoint remoteEP, string[] filenames)
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(remoteEP);
                NetworkStream stream = client.GetStream();
                AuthFileTransfering auth = new AuthFileTransfering();
                auth.guid = AccountGuid;
                auth.numberOfFiles = filenames.Length;
                MemoryHelper.WriteStructureToStream(auth, stream);
                foreach (string fn in filenames)
                {
                    if (cancel)
                        break;
                    FileInfo f = new FileInfo(fn);
                    TransferedFileInfo fi = new TransferedFileInfo();
                    fi.filename = Path.GetFileName(fn);
                    fi.size = f.Length;
                    MemoryHelper.WriteStructureToStream(fi, stream);
                    byte[] buffer = new byte[1000];
                    using (FileStream inputStream = f.OpenRead())
                    {
                        int read = inputStream.Read(buffer, 0, buffer.Length);
                        while (read > 0)
                        {
                            if (cancel)
                                break;
                            stream.Write(buffer, 0, read);
                            read = inputStream.Read(buffer, 0, buffer.Length);
                        }
                    }
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
                connections[e.RequestId].SendAsync(new IPEndPoint(e.Address.IP, e.Port));
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
