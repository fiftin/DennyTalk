using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DennyTalk
{
    public class Address
    {
        private int port;
        private string host;
        private string guid;

        public Address(IPEndPoint endPoint)
        {
            port = endPoint.Port;
            host = endPoint.Address.ToString();
        }

        public Address(Address address, string guid)
        {
            this.host = address.host;
            this.port = address.port;
            this.guid = guid;
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

        private IPHostEntry hostEntry;

        public IPHostEntry HostEntry
        {
            get
            {
                if (hostEntry == null)
                {
                    hostEntry = Dns.GetHostEntry(host);
                }
                return hostEntry;
            }
        }

        public bool Equals(Address other)
        {
            return Guid == other.Guid || EqualIPAddress(other);
        }

        public bool EqualIPAddress(Address other)
        {
            bool eq = false;
            foreach (IPAddress addr in HostEntry.AddressList)
            {
                if (Array.Exists(other.HostEntry.AddressList, x => x.Equals(addr)))
                {
                    eq = true;
                    break;
                }
            }
            eq = eq
                || Array.Exists(other.HostEntry.AddressList, x => x.Equals(IPAddress))
                || Array.Exists(HostEntry.AddressList, x => x.Equals(other.IPAddress))
                || HostEntry.HostName.Equals(other.HostEntry.HostName) 
                || IPAddress.Equals(other.IPAddress) 
                || Host.Equals(other.Host, StringComparison.InvariantCultureIgnoreCase);
            return eq && Port == other.Port;
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
