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
        private IPHostEntry hostEntry;

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


        public IPAddress[] Addresses
        {
            get
            {
                IPHostEntry hostEntry = HostEntry;
                if (hostEntry == null)
                    return new IPAddress[0];
                return HostEntry.AddressList;
            }
        }

        private static List<string> invalidHosts = new List<string>();

        public IPHostEntry HostEntry
        {
            get
            {
                if (hostEntry == null)
                {
                    bool isInvalidHost;
                    lock (invalidHosts)
                    {
                        isInvalidHost = invalidHosts.Contains(host.ToLower());
                    }
                    if (!isInvalidHost)
                    {
                        try
                        {
                            hostEntry = Dns.GetHostEntry(host);
                        }
                        catch (Exception)
                        {
                            lock (invalidHosts)
                            {
                                invalidHosts.Add(host.ToLower());
                            }
                        }
                    }
                }
                return hostEntry;
            }
        }

        public bool Equals(Address other)
        {
            return !string.IsNullOrEmpty(Guid) && !string.IsNullOrEmpty(other.Guid) && Guid == other.Guid 
                || EqualHostOrIP(other);
        }

        public bool EqualHostOrIP(Address other)
        {
            bool eq = Host.Equals(other.Host, StringComparison.InvariantCultureIgnoreCase)
                || Array.Exists(other.Addresses, x => x.Equals(IP))
                || Array.Exists(Addresses, x => x.Equals(other.IP))
                || HostEntry != null && other.HostEntry != null && HostEntry.HostName.Equals(other.HostEntry.HostName) 
                || IP.Equals(other.IP);
            if (!eq && HostEntry != null)
            {
                foreach (IPAddress addr in HostEntry.AddressList)
                {
                    if (Array.Exists(other.Addresses, x => x.Equals(addr)))
                    {
                        eq = true;
                        break;
                    }
                }
            }
            if (eq && Port == other.Port)
                return true;
            return false;
        }

        public bool IsLocalHost
        {
            get
            {
                return Array.IndexOf(HostEntry.AddressList, IPAddress.Loopback) != -1;
            }
        }

        public IPAddress IP
        {
            get
            {
                IPAddress addr;
                if (IPAddress.TryParse(host, out addr))
                    return addr;
                else if (IsLocalHost)
                    return IPAddress.Loopback;
                else
                    return HostEntry.AddressList[0];
            }
        }
	
    }
}
