using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.IO;

namespace DennyTalk
{
    public enum TelegramHeaderType : int
    {
        Message = 0,
        UserInfo = 1,
        UserStatus = 2,
        MessageDelivered = 3,
        UserInfoRequest = 4,
        UserStatusRequest = 5
    }

    [StructLayout(LayoutKind.Explicit, CharSet=CharSet.Ansi, Pack=2)]
    public struct TelegramHeader
    {
        [FieldOffset(0)]
        public int id;
        [FieldOffset(4)]
        public int type;
        [FieldOffset(8)]
        public int dataSize;
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string toGuid;
        [FieldOffset(56)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string fromGuid;
        [FieldOffset(96)]
        public int port;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct UserInfo
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        public string nick;

        [FieldOffset(40)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = ImageHelper2.AvatarWidth * ImageHelper2.AvatarHeight)]
        public int[] avatar;

        [FieldOffset(40 + 4 * (ImageHelper2.AvatarWidth * ImageHelper2.AvatarHeight))]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string statusText;

        [FieldOffset(40 + 4 * (ImageHelper2.AvatarWidth * ImageHelper2.AvatarHeight) + 2000)]
        public int status;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UserStatusSturct
    {
        public int status;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1000)]
        public string statusText;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TelegramDelivery
    {
        public int telegramID;
    }

    public class TelegramReceivedEventArgs : EventArgs
    {

        private TelegramHeader header;
        private byte[] data;
        Address address;

        public TelegramReceivedEventArgs(TelegramHeader header, Address address, byte[] data)
        {
            this.header = header;
            this.address = address;
            this.data = data;
        }

        public Address Address
        {
            get
            {
                return address;
            }
        }
        public byte[] Data
        {
            get
            {
                return data;
            }
        }
        public TelegramHeader Header
        {
            get
            {
                return header;
            }
        }
    }

    public class RequestReceivedEventArgs : EventArgs
    {
        public RequestReceivedEventArgs(Address address)
        {
            this.address = address;
        }

        private Address address;
        public Address Address
        {
            get { return address; }
        }

    }

    public class TelegramListener
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public event EventHandler<MessageDeliveredEventArgs> MessageDelivered;
       
        public event EventHandler<UserStatusReceivedEventArgs> UserStatusReceived;

        public event EventHandler<UserInfoReveivedEventArgs> UserInfoReceived;

        public event EventHandler<RequestReceivedEventArgs> UserInfoRequest;

        public event EventHandler<RequestReceivedEventArgs> UserStatusRequest;

        public event EventHandler<TelegramReceivedEventArgs> TelegramReceived;

        private int lastID = 1000;

        private IDictionary<int, ResponceWaiter> responceWaiters = new Dictionary<int, ResponceWaiter>();
        
        private ICommunicationClient client;

        private string guid = "";

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public TelegramListener(ICommunicationClient client, string guid)
        {
            this.client = client;
            this.guid = guid;
        }

        public TelegramSendResult SendMessage(Address address, string text)
        {
            byte[] data = Encoding.Unicode.GetBytes(text);
            return Send(address, data, TelegramHeaderType.Message);
        }

        public TelegramSendResult Send(Address address, byte[] data, TelegramHeaderType type)
        {
            return Send(address, data, (int)type);
        }

        public TelegramSendResult Send<T>(Address address, T data) where T : struct
        {
            TelegramHeaderType type;
            if (typeof(T) == typeof(UserInfo))
            {
                type = TelegramHeaderType.UserInfo;
            }
            else if (typeof(T) == typeof(TelegramDelivery))
            {
                type = TelegramHeaderType.MessageDelivered;
            }
            else if (typeof(T) == typeof(UserStatusSturct))
            {
                type = TelegramHeaderType.UserStatus;
            }
            else
            {
                throw new Exception("T is unknown type");
            }
            return Send(address, data, (int)type);
        }

        protected TelegramSendResult Send<T>(Address address, T data, int type) where T : struct
        {
            byte[] bytes = MemoryHelper.StructureToByteArray<T>(data);
            return Send(address, bytes, type);
        }

        protected TelegramSendResult Send(Address address, byte[] data, int type)
        {
            TelegramHeader header = new TelegramHeader();
            header.id = lastID;
            lastID++;
            header.type = type;
            header.dataSize = data.Length;
            byte[] guidBytes = Encoding.ASCII.GetBytes(address.Guid);
            header.toGuid = address.Guid;
            header.fromGuid = guid;
            header.port = client.Port;
            /*
            byte[] headerBytes = MemoryHelper.StructureToByteArray(header);
            byte[] telegramBytes = new byte[headerBytes.Length + header.dataSize];
            Buffer.BlockCopy(headerBytes, 0, telegramBytes, 0, headerBytes.Length);
            Buffer.BlockCopy(data, 0, telegramBytes, headerBytes.Length, data.Length);
            client.Send(telegramBytes, address);
            return new TelegramSendResult(header.id);
             */
            return Send(header, address, data);
        }

        public TelegramSendResult Send(TelegramHeader header, Address address, byte[] data)
        {
            byte[] headerBytes = MemoryHelper.StructureToByteArray(header);
            byte[] telegramBytes = new byte[headerBytes.Length + header.dataSize];
            Buffer.BlockCopy(headerBytes, 0, telegramBytes, 0, headerBytes.Length);
            Buffer.BlockCopy(data, 0, telegramBytes, headerBytes.Length, data.Length);
            client.Send(telegramBytes, address);
            return new TelegramSendResult(header.id);
        }

        public TelegramSendResult RequestUserInfo(Address address)
        {
            return Send(address, new byte[0], TelegramHeaderType.UserInfoRequest);
        }

        public TelegramSendResult RequestUserStatus(Address address)
        {
            return Send(address, new byte[0], TelegramHeaderType.UserStatusRequest);
        }

        public TelegramSendResult SendTelegramDelivery(Address address, int telegramID)
        {
            TelegramDelivery tel = new TelegramDelivery();
            tel.telegramID = telegramID;
            return Send(address, tel);
        }

        public void Listen()
        {
            while (true)
            {
                IPEndPoint remoteEP;
                byte[] telegramBytes = new byte[0];
                try
                {
                    telegramBytes = client.Receive(out remoteEP);
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                int headerSize = Marshal.SizeOf(typeof(TelegramHeader));

                //Console.WriteLine("headerSize:{0} telegramBytes:{1}" + Environment.NewLine, headerSize, telegramBytes.Length);
                //File.AppendAllText("log.txt", string.Format("headerSize:{0} telegramBytes:{1}" + Environment.NewLine, headerSize, telegramBytes.Length));
                
                TelegramHeader header = MemoryHelper.ByteArrayToStructure<TelegramHeader>(telegramBytes, 0, headerSize);
                byte[] data = new byte[telegramBytes.Length - headerSize];
                Buffer.BlockCopy(telegramBytes, headerSize, data, 0, data.Length);
                Address address = new Address(remoteEP.Address.ToString(), header.port, header.fromGuid);

                OnTelegramReceived(header, address, data);
            }
        }

        protected virtual void OnTelegramReceived(TelegramHeader header, Address address, byte[] data)
        {
            ResponceWaiter waiter;
            if (responceWaiters.TryGetValue(header.id, out waiter))
            {
                waiter.Data = data;
                waiter.Set();
            }
            if (TelegramReceived != null)
            {
                TelegramReceived(this, new TelegramReceivedEventArgs(header, address, data));
            }

            switch ((TelegramHeaderType)header.type)
            {
                case TelegramHeaderType.Message:
                    string message = Encoding.Unicode.GetString(data);
                    OnMessageReceived(header, address, message);
                    break;
                case TelegramHeaderType.MessageDelivered:
                    OnMessageDelivered(header, address, data);
                    break;
                case TelegramHeaderType.UserInfoRequest:
                    OnUserInfoRequest(header, address);
                    break;
                case TelegramHeaderType.UserStatusRequest:
                    OnUserStatusRequest(header, address);
                    break;
                case TelegramHeaderType.UserInfo:
                    OnUserInfoReceived(header, address, data);
                    break;
                case TelegramHeaderType.UserStatus:
                    OnUserStatusReceived(header, address, data);
                    break;
            }
        }


        protected virtual void OnUserInfoReceived(TelegramHeader header, Address address, byte[] data)
        {
            UserInfo info = MemoryHelper.ByteArrayToStructure<UserInfo>(data);
            if (UserInfoReceived != null)
            {
                Bitmap bitmap = ImageHelper2.IntArrayToAvatar(info.avatar);
                UserInfoReveivedEventArgs e = new UserInfoReveivedEventArgs(info.nick, bitmap, (UserStatus)info.status, info.statusText, address);
                if (UserInfoReceived != null)
                {
                    UserInfoReceived(this, e);
                }
            }
        }

        protected virtual void OnUserStatusReceived(TelegramHeader header, Address address, byte[] data)
        {
            UserStatusSturct userStatus = MemoryHelper.ByteArrayToStructure<UserStatusSturct>(data);
            if (UserStatusReceived != null)
            {
                UserStatusReceived(this, new UserStatusReceivedEventArgs(address, (UserStatus)userStatus.status, userStatus.statusText));
            }
        }

        protected virtual void OnUserStatusRequest(TelegramHeader header, Address address)
        {
            if (UserStatusRequest != null)
            {
                UserStatusRequest(this, new RequestReceivedEventArgs(address));
            }
        }

        protected virtual void OnUserInfoRequest(TelegramHeader header, Address address)
        {
            if (UserInfoRequest != null)
            {
                UserInfoRequest(this, new RequestReceivedEventArgs(address));
            }
        }

        protected virtual void OnMessageDelivered(TelegramHeader header, Address address, byte[] bytes)
        {
            TelegramDelivery tel = MemoryHelper.ByteArrayToStructure<TelegramDelivery>(bytes);
            MessageDeliveredEventArgs e = new MessageDeliveredEventArgs(tel.telegramID, address);
            if (MessageDelivered != null)
                MessageDelivered(this, e);
        }

        protected virtual void OnMessageReceived(TelegramHeader header, Address address, string text)
        {
            MessageReceivedEventArgs e = new MessageReceivedEventArgs(text, header.id, address);
            if (MessageReceived != null)
                MessageReceived(this, e);
        }


        public TelegramSendResult SendSync(Address address, byte[] data, int type, out byte[] responce)
        {
            TelegramSendResult result = Send(address, data, type);
            ResponceWaiter responceWaiter = new ResponceWaiter();
            responceWaiters.Add(result.ID, responceWaiter);
            try
            {
                if (responceWaiter.Wait())
                {
                    responce = responceWaiter.Data;
                }
                else
                {
                    responce = null;
                }
            }
            finally
            {
                responceWaiters.Remove(result.ID);
            }
            return result;
        }

        private class ResponceWaiter
        {
            public bool Wait()
            {
                if (setted)
                    throw new Exception();
                if (closed)
                    throw new Exception();
                return locker.WaitOne(2000, false);
            }

            public byte[] Data
            {
                get { return data; }
                set { data = value; }
            }

            bool IsClosed
            {
                get
                {
                    return closed;
                }
            }

            bool IsSetted
            {
                get
                {
                    return setted;
                }
            }

            public void Close()
            {
                if(!setted)
                    locker.Set();
                locker.Close();
                setted = true;
                closed = true;
            }

            public void Set()
            {
                if (setted)
                    throw new Exception();
                if (closed)
                    throw new Exception();
                locker.Set();
                setted = true;
            }

            private byte[] data;
            private bool setted = false;
            private bool closed = false;
            private ManualResetEvent locker = new ManualResetEvent(false);
        }

        public void SendStatus(Address address, UserStatus userStatus, string statusText)
        {
            UserStatusSturct status = new UserStatusSturct();
            status.status = (int)userStatus;
            status.statusText = statusText;
            Send(address, status);
        }

        public void SendInfo(Address address, Bitmap avatar, string nick, UserStatus status, string statusText)
        {
            int[] avatarPixels = ImageHelper2.AvatarToIntArray(avatar);

            UserInfo userInfo = new UserInfo();
            userInfo.avatar = avatarPixels;
            userInfo.nick = nick;
            userInfo.status = (int)status;
            userInfo.statusText = statusText;
            Send(address, userInfo);
        }
    }
}
