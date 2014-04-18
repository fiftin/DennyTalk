using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace DennyTalkServer
{
    public class RemoteServerListener
    {
        public RemoteServerListener(int port)
        {
            listener = new TcpListener(port);
            this.port = Port;
            listener.Start();
        }
        private int port;

        public int Port
        {
            get { return port; }
        }

        public RemoteServer Listen()
        {
            TcpClient client = listener.AcceptTcpClient();
            RemoteServer server = new RemoteServer(client);
            return server;
        }

        private TcpListener listener;
    }
}
