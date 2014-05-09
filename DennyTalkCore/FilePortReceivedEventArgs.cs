using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class FilePortReceivedEventArgs : EventArgs
    {
        public int Port { get; private set; }
        public Address Address { get; private set; }
        public int RequestId { get; private set; }
        public FilePortReceivedEventArgs(Address address, int port, int requestId)
        {
            Port = port;
            Address = address;
            RequestId = requestId;
        }
    }
}
