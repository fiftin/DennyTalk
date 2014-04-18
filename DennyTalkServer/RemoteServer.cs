using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using DennyTalk;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace DennyTalkServer
{
    public class RemoteServer
    {
        public void Send(TelegramHeader header, Address address, byte[] data)
        {
            //Console.WriteLine("TCP send start");
            if (client == null)
                return;
            if (!client.Connected)
            {
                OnDisconnected();
                client = null;
                return;
            }
            NetworkStream stream = client.GetStream();

            byte[] bytes = new byte[Marshal.SizeOf(header) + data.Length];
            byte[] headerBuffer = MemoryHelper.StructureToByteArray(header);
            Buffer.BlockCopy(headerBuffer, 0, bytes, 0, headerBuffer.Length);
            Buffer.BlockCopy(data, 0, bytes, headerBuffer.Length, data.Length);
            stream.Write(bytes, 0, bytes.Length);
            //Console.WriteLine("TCP sent");
            //stream.Close();
        }

        internal RemoteServer(TcpClient client)
        {
            this.client = client;
            IPEndPoint ep = (IPEndPoint)client.Client.RemoteEndPoint;
            address = new Address(ep);
            receiverThread = new Thread(DoReceive);
            receiverThread.Start();
            Console.WriteLine("TCP connected");
        }

        public RemoteServer(Address address)
        {
            this.address = address;
            receiverThread = new Thread(DoReceive);
        }

        protected byte[] Receive(NetworkStream stream, out Address address, out TelegramHeader header)
        {
            int headerBufferSize = Marshal.SizeOf(typeof(TelegramHeader));
            byte[] headerBuffer = new byte[headerBufferSize];
            int nRead = stream.Read(headerBuffer, 0, headerBufferSize);
            if (nRead == 0)
            {
                address = null;
                header = new TelegramHeader();
                return null;
            }
            header = MemoryHelper.ByteArrayToStructure<TelegramHeader>(headerBuffer);
            if ((header.type < 0 || header.type > 20)
                || (header.dataSize < 0 || header.dataSize > 65000)
                || header.id == 0)
            {
                address = null;
                header = new TelegramHeader();
                byte[] dataTemp = new byte[1000000];
                nRead = stream.Read(dataTemp, 0, dataTemp.Length);

                return null;
            }
            
            byte[] data = new byte[header.dataSize];
            int nReadTotal = 0;
            while (nReadTotal < header.dataSize)
            {
                nRead = stream.Read(data, nReadTotal, header.dataSize - nReadTotal);
                nReadTotal += nRead;
            }

            address = new Address(Address.Host, Address.Port, header.fromGuid);
            return data;
        }

        private void DoReceive()
        {
            NetworkStream stream = client.GetStream();
            try
            {
                while (Connected)
                {
                    Address addr;
                    TelegramHeader header;
                    byte[] bytes = Receive(stream, out addr, out header);
                    if (bytes == null)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    if (TelegramReceived != null)
                    {
                        TelegramReceived(this, new TelegramReceivedEventArgs(header, addr, bytes));
                    }
                    //Console.WriteLine("TCP received");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                stream.Close();
            }
        }

        public bool Connected
        {
            get
            {
                if (client != null)
                {
                    if (client.Connected)
                    {
                        return true;
                    }
                    else
                    {
                        client = null;
                        OnDisconnected();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void Connect()
        {
            if (client != null)
            {
                Disconnect();
            }
            try
            {
                client = new TcpClient(Address.Host, Address.Port);
                Console.WriteLine("TCP connetced");
                receiverThread.Start();
                //client.Connect(address.Host, address.Port);
            }
            catch (Exception)
            {
                client = null;
            }
            
        }

        public void Disconnect()
        {
            if (client != null)
            {
                client.Close();
                client = null;
                OnDisconnected();
            }
        }

        protected virtual void OnDisconnected()
        {
            if (Disconnected != null)
            {
                Disconnected(this, new EventArgs());
            }
            Console.WriteLine("TCP disconnected");
        }


        public Address Address
        {
            get { return address; }
        }

        public event EventHandler Disconnected;
        public event EventHandler<TelegramReceivedEventArgs> TelegramReceived;

        private Address address;
        private TcpClient client = null;
        private Thread receiverThread;
    }
}
