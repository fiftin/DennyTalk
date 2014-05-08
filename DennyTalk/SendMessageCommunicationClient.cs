using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using Common;

namespace DennyTalk
{
    public class SendMessageCommunicationClient : ICommunicationClient
    {
        public SendMessageCommunicationClient()
        {
        }
        
        public void Send(byte[] data, Address address)
        {
            this.address = address;

            //
            // Обработка полученных данных
            //
            
            // Заголовок телеграммы
            int headerSize = Marshal.SizeOf(typeof(TelegramHeader));
            header = MemoryHelper.ByteArrayToStructure<TelegramHeader>(data, 0, headerSize);
            // Тело телеграммы (сообщение)
            byte[] telegramData = new byte[data.Length - headerSize];
            Buffer.BlockCopy(data, headerSize, telegramData, 0, telegramData.Length);
            messageText = Encoding.Unicode.GetString(telegramData);
            receiveLocker.Set();
        }

        public byte[] Receive(out System.Net.IPEndPoint address)
        {
            if (!receiveLocker.WaitOne())
                throw new Exception();
            address = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
            TelegramHeader replyHeader = new TelegramHeader();
            replyHeader.id = header.id;
            replyHeader.type = (int)TelegramHeaderType.MessageDelivered;
            byte[] bytes = MemoryHelper.StructureToByteArray(replyHeader);
            return bytes;
        }

        private Address address;

        TelegramHeader header;

        private string messageText;

        private AutoResetEvent receiveLocker = new AutoResetEvent(false);

        public int Port
        {
            get
            {
                return 100;
            }
            set
            {
            }
        }

        public void Reconnect()
        {
        }

    }
}
