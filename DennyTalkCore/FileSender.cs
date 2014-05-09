using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Common;
using System.IO;

namespace DennyTalk
{

    /// <summary>
    /// Отправляет файл другому пользователю.
    /// </summary>
    public class FileSender
    {
        public int Port { get; set; }
        public string Guid { get; set; }
        private TelegramListener telegramListener;
        private IDictionary<int, string[]> filenames = new SortedDictionary<int, string[]>();

        public FileSender(TelegramListener telegramListener, int port, string guid)
        {
            Port = port;
            Guid = guid;
            this.telegramListener = telegramListener;
            this.telegramListener.FilePortReceived += new EventHandler<FilePortReceivedEventArgs>(telegramListener_FilePortReceived);
        }

        void telegramListener_FilePortReceived(object sender, FilePortReceivedEventArgs e)
        {
            if (filenames.ContainsKey(e.RequestId))
            {
                string[] fns = filenames[e.RequestId];
                filenames.Remove(e.RequestId);
                Send(new IPEndPoint(e.Address.IP, e.Port), fns);
            }
        }

        public int Send(Address address, string[] filenames)
        {
            FilePortRequest req = new FilePortRequest();
            req.numberOfFiles = filenames.Length;
            var res = telegramListener.Send(address, req);
            return res.ID;
        }

        protected void Send(IPEndPoint ep, string[] filenames)
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
}
