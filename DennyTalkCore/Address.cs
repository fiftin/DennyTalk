using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DennyTalk
{
    public class Address : IComparable<Address>
    {
        private int port;
        private string host;
        private string guid;

        public Address(IPEndPoint endPoint)
        {
            port = endPoint.Port;
            host = endPoint.Address.ToString();
        }

        public Address(string host, int port, string guid)
        {
            this.host = host;
            this.port = port;
            this.guid = guid;
        }

        public string Guid
        {
            get { return guid; }
        }

        public string Host
        {
            get { return host; }
        }

        public int Port
        {
            get { return port; }
        }

        public int CompareTo(Address other)
        {
            if (string.IsNullOrEmpty(Guid) || string.IsNullOrEmpty(other.Guid))
            {
                return IPAddress.Equals(other.IPAddress) && Port == other.Port ? 0 : -1;
            }
            return Guid == other.Guid ? 0 : -1;
        }

        public IPAddress IPAddress
        {
            get
            {
                IPAddress addr;
                if (IPAddress.TryParse(host, out addr))
                    return addr;
                else
                    return IPAddress.Loopback;
            }
        }
	
    }
}
