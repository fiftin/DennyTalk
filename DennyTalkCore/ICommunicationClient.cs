using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public interface ICommunicationClient
    {
        void Send(byte[] telegramData, Address address);
        byte[] Receive(out System.Net.IPEndPoint address);
        int Port { get; set; }
        void Reconnect();
    }
}
