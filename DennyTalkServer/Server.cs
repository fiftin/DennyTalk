using System;
using System.Collections.Generic;
using System.Text;
using DennyTalk;
using System.Net.Sockets;
using System.Threading;

namespace DennyTalkServer
{

    public class Server
    {
        /// <summary>
        /// список подключенных по TCP удаленных серверов
        /// </summary>
        private List<RemoteServer> remoteServers = new List<RemoteServer>();

        /// <summary>
        /// 
        /// </summary>
        private RemoteServerListener remoteServerListener;

        /// <summary>
        /// получение и отправка телеграмм по UDP
        /// </summary>
        private Messanger messanger;

        /// <summary>
        /// поток на ожидание подключения новых клиентов
        /// </summary>
        private Thread listenerThread;


        private TelegramListener listener;

        private ContactManager contacts;

        private void DoListen()
        {
            RemoteServer server = remoteServerListener.Listen();
            while (server != null)
            {
                AddRemoteServer(server);
                server = remoteServerListener.Listen();
            }
        }

        public void AddRemoteServer(RemoteServer server)
        {
            server.TelegramReceived += new EventHandler<TelegramReceivedEventArgs>(server_TelegramReceived);
            remoteServers.Add(server);
        }

        /// <summary>
        /// От одного из удаленных серверов пришло новое сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void server_TelegramReceived(object sender, TelegramReceivedEventArgs e)
        {
            Send(e, false, false);
        }

        public Server(Messanger messanger, RemoteServerListener remoteServerListener)
        {
            this.messanger = messanger;
            this.listener = messanger.TelegramListener;
            this.contacts = messanger.ContactManager;
            this.remoteServerListener = remoteServerListener;
            listenerThread = new Thread(DoListen);
            listenerThread.IsBackground = true;
        }


        public void Initialize()
        {
            listener.TelegramReceived += new EventHandler<TelegramReceivedEventArgs>(listener_TelegramReceived);
            listenerThread.Start();
        }

        /// <summary>
        /// Рассылает телеграмму всем подключенным серверам
        /// </summary>
        /// <param name="header"></param>
        /// <param name="addr"></param>
        /// <param name="data"></param>
        void SendToRemoteServers(TelegramHeader header, Address addr, byte[]data)
        {
            foreach (RemoteServer serv in remoteServers)
            {
                serv.Send(header, addr, data);
            }
        }


        void Send(TelegramReceivedEventArgs e)
        {
            Send(e, true, true);
        }
        /// <summary>
        /// Отправляем телеграмму пользователю по UDP, если он есть в списке контактов. Иначе рассылает ее другим серверам
        /// </summary>
        /// <param name="e"></param>
        void Send(TelegramReceivedEventArgs e, bool sendToTcp, bool addSenderToContacts)
        {
            // ищем отправителя телеграммы в списке контактов.
            Contact fromCont = contacts.GetContactByGuid(e.Address.Guid);
            if (fromCont == null && addSenderToContacts)
            {
                fromCont = new Contact();
                fromCont.Address = e.Address;
                contacts.AddContact(fromCont);
            }
            else
            {

            }

            // если this является получателем телеграммы
            if (e.Header.toGuid == messanger.Account.Address.Guid)
            {
                // обработка полученной телеграммы
                ;
            }
            else
            {
                // если GUID получателя есть в списке контактов,
                // то отправляем ему телеграмму
                Contact cont = contacts.GetContactByGuid(e.Header.toGuid);
                TelegramHeader header = e.Header;
                header.port = messanger.Account.Address.Port;
                if (cont != null)
                {
                    listener.Send(header, cont.Address, e.Data);
                }
                else if(sendToTcp) // если GUID'а нету в списке контактов, то отправляем по TCP на другой сервер
                {
                    //Console.WriteLine("SendToRemoteServers");
                    SendToRemoteServers(header, e.Address, e.Data);
                }
            }
        }

        /// <summary>
        /// Прослушиватель UDP телегамм получил новую телеграмму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void listener_TelegramReceived(object sender, TelegramReceivedEventArgs e)
        {
            if (e.Header.type == 4 || e.Header.type==1)
            {
            }
            Send(e);
        }
    }
}
