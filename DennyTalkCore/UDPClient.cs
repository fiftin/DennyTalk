using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace DennyTalk
{
    public class UDPClient : ICommunicationClient
    {
        private int port;
        private UdpClient client;
        private UdpClient outputClient;

        public void Send(byte[] telegramData, Address address)
        {
            int sent = outputClient.Send(telegramData, telegramData.Length,
                new System.Net.IPEndPoint(address.IPAddress, address.Port));
        }

        public byte[] Receive(out IPEndPoint address)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = client.Receive(ref remoteEP);
            address = remoteEP;
            return bytes;
        }

        public UDPClient(int port)
        {
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            outputClient = new UdpClient();
            this.port = port;
        }

        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        public void Reconnect()
        {
            client.Close();
            client = new UdpClient(new IPEndPoint(IPAddress.Any, port));
        }
    }
}
