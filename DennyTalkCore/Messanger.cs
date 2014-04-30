using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Threading;
using System.Drawing;
using System.IO;

namespace DennyTalk
{
    public class Messanger
    {
        private TelegramListener telegramListener;
        private Account account = new Account();
        private ContactManager contactManager = new ContactManager();
        private ICommunicationClient client;

        private Thread threadListener;

        private IStorage contactStorage;
        private IStorage accountStorage;
        private IStorage optionStorage;
        private History history;
        private string imegesPath = "";

        private int serverPort;
        private string serverHost;
        private string updateServerHost;

        public string UpdateServerHost
        {
            get { return updateServerHost; }
            set
            {
                updateServerHost = value;
                optionStorage["UpdateServerHost"].Value = updateServerHost;
                optionStorage.Save();
            }
        }

        public int ServerPort
        {
            get
            {
                return serverPort;
            }
            set
            {
                serverPort = value;
                optionStorage["ServerPort"].Value = serverPort;
                optionStorage.Save();
            }
        }

        public string ServerHost
        {
            get
            {
                return serverHost;
            }
            set
            {
                serverHost = value;
                optionStorage["ServerHost"].Value = serverHost;
                optionStorage.Save();
            }
        }

        public History History
        {
            get
            {
                return history;
            }
        }

        public Account Account
        {
            get { return account; }
        }

        public ContactManager ContactManager
        {
            get { return contactManager; }
        }

        public TelegramListener TelegramListener
        {
            get
            {
                return telegramListener;
            }
        }

        public Messanger(IStorage optionStorage, IStorage contactStorage, IStorage accountStorage)
        {
            this.contactStorage = contactStorage;
            this.accountStorage = accountStorage;
            this.optionStorage = optionStorage;

            history = new History();

            threadListener = new Thread(threadListener_DoListen);
            threadListener.IsBackground = true;
            
        }

        private void threadListener_DoListen()
        {
            while (true)
            {
                telegramListener.Listen();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            client = new UDPClient(int.Parse(accountStorage["Address"]["Port"].Value.ToString()));
            telegramListener = new TelegramListener(client, accountStorage["Address"]["GUID"].Value.ToString());

            telegramListener.UserInfoReceived += new EventHandler<UserInfoReveivedEventArgs>(telegramListener_UserInfoReceived);
            telegramListener.UserStatusReceived += new EventHandler<UserStatusReceivedEventArgs>(telegramListener_UserStatusReceived);
            telegramListener.UserInfoRequest += new EventHandler<RequestReceivedEventArgs>(telegramListener_UserInfoRequest);
            telegramListener.UserStatusRequest += new EventHandler<RequestReceivedEventArgs>(telegramListener_UserStatusRequest);
            telegramListener.MessageReceived += new EventHandler<MessageReceivedEventArgs>(telegramListener_MessageReceived);
            if (string.IsNullOrEmpty((string)optionStorage["ImegesPath"].Value))
            {
                imegesPath = "images" + System.IO.Path.DirectorySeparatorChar;
                optionStorage["ImegesPath"].Value = imegesPath;
                optionStorage.Save();
            }
            else
            {
                imegesPath = (string)optionStorage["ImegesPath"].Value;
            }
            string rootPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            imegesPath = rootPath + System.IO.Path.DirectorySeparatorChar + imegesPath;

            if (!Directory.Exists(imegesPath))
            {
                Directory.CreateDirectory(imegesPath);
            }

            serverHost = (string)optionStorage["ServerHost"].Value;
            if (string.IsNullOrEmpty(serverHost))
            {
                serverHost = "192.168.63.137";
                optionStorage["ServerHost"].Value = serverHost;
                optionStorage.Save();
            }
            if (!int.TryParse((string)optionStorage["ServerPort"].Value, out serverPort))
            {
                serverPort = 2000;
                optionStorage["ServerPort"].Value = serverPort;
                optionStorage.Save();
            }
            updateServerHost = (string)optionStorage["UpdateServerHost"].Value;
            if (string.IsNullOrEmpty(updateServerHost))
            {
                updateServerHost = "http://192.168.63.137/dennytalk";
                optionStorage["UpdateServerHost"].Value = updateServerHost;
                optionStorage.Save();
            }

            string avatarFileName = (string)accountStorage["AvatarFileName"].Value;
            if (string.IsNullOrEmpty(avatarFileName))
            {
                account.Avatar = ImageHelper2.DefaultAvatar;
                accountStorage["AvatarFileName"].Value = "";
            }
            else
            {
                try
                {
                    account.Avatar = new Bitmap(imegesPath + avatarFileName);
                }
                catch (Exception)
                {
                    account.Avatar = ImageHelper2.DefaultAvatar;
                }
            }

            account.Nick = (string)accountStorage["Nick"].Value;
            account.StatusText = (string)accountStorage["StatusText"].Value;
            account.Address = new Address((string)accountStorage["Address"]["Host"].Value,
                              int.Parse(accountStorage["Address"]["Port"].Value.ToString()),
                              (string)accountStorage["Address"]["GUID"].Value);
            account.Status = (UserStatus)Enum.Parse(typeof(UserStatus), (string)accountStorage["Status"].Value);

            // Загружаю из хранилища список контактов
            foreach (KeyValuePair<string, IStorageNode> nodeKeyValuePair in contactStorage)
            {
                IStorageNode node = nodeKeyValuePair.Value;
                Contact contact = new Contact();

                string avatarFileName1 = (string)node["AvatarFileName"].Value;
                if (string.IsNullOrEmpty(avatarFileName1))
                {
                    contact.Avatar = ImageHelper2.DefaultAvatar;
                }
                else
                {
                    if (System.IO.File.Exists(imegesPath + avatarFileName1))
                    {
                        contact.Avatar = new Bitmap(imegesPath + avatarFileName1);
                    }
                    else
                    {
                        contact.Avatar = ImageHelper2.DefaultAvatar;
                    }

                }

                contact.Nick = (string)node["Nick"].Value;
                contact.StatusText = (string)node["StatusText"].Value;
                contact.Address = new Address((string)node["Address"]["Host"].Value,
                                              int.Parse(node["Address"]["Port"].Value.ToString()),
                                              (string)node["Address"]["GUID"].Value);
                contact.Tag.Add("LastReceivedTelegramTime", DateTime.Now);
                contactManager.AddContact(contact);
            }


            account.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(account_PropertyChanged);

            contactManager.ContactAdded += new EventHandler<ContactEventArgs>(contactManager_ContactAdded);
            contactManager.ContactRemoved += new EventHandler<ContactEventArgs>(contactManager_ContactRemoved);
            contactManager.ContactChanged += new EventHandler<ContactEventArgs>(contactManager_ContactChanged);
            
            // Обновляю информацию о пользователях из списка контактов
            RequestContactInfos();

            // Отправляю пользователям из списка контактов свой статус
            SendStatusToContactsAsync();

            threadListener.Start();
        }


        void telegramListener_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            telegramListener.SendTelegramDelivery(e.Address, e.ID);
        }

        void telegramListener_UserStatusRequest(object sender, RequestReceivedEventArgs e)
        {
            telegramListener.SendStatus(e.Address, account.Status, account.StatusText);
        }

        void telegramListener_UserInfoRequest(object sender, RequestReceivedEventArgs e)
        {
            telegramListener.SendInfo(e.Address, account.Avatar, account.Nick, account.Status, account.StatusText);
        }


        void account_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Avatar":
                    string avatarFileName = Guid.NewGuid().ToString() + ".bmp";
                    string pathName = imegesPath + avatarFileName;
                    account.Avatar.Save(pathName);
                    accountStorage["AvatarFileName"].Value = avatarFileName;
                    accountStorage.Save();
                    SendInfoToContactsAsync();
                    break;
                case "Nick":
                    accountStorage["Nick"].Value = account.Nick;
                    accountStorage.Save();
                    SendInfoToContactsAsync();
                    break;
                case "StatusText":
                    accountStorage["StatusText"].Value = account.StatusText;
                    accountStorage.Save();
                    SendStatusToContactsAsync();
                    break;
                case "Status":
                    accountStorage["Status"].Value = account.Status;
                    accountStorage.Save();
                    SendStatusToContactsAsync();
                    break;
                case "Address":
                    if (client.Port != account.Address.Port)
                    {
                        client.Port = account.Address.Port;
                        client.Reconnect();
                        optionStorage.Save();
                    }
                    telegramListener.Guid = account.Address.Guid;
                    accountStorage["Address"]["Host"].Value = account.Address.Host;
                    accountStorage["Address"]["Port"].Value = account.Address.Port;
                    accountStorage["Address"]["GUID"].Value = account.Address.Guid;
                    accountStorage.Save();
                    SendInfoToContactsAsync();
                    break;
            }
        }

        void contactManager_ContactChanged(object sender, ContactEventArgs e)
        {
            IStorageNode nodeForUpdate = null;
            foreach (IStorageNode node in contactStorage.GetNodes("Contact"))
            {
                IStorageNode addressNode = node["Address"];
                int port = int.Parse(addressNode["Port"].Value.ToString());
                Address address = new Address((string)addressNode["Host"].Value, port, (string)addressNode["GUID"].Value);
                Address otherAddr;
                if (e.PropertyName == "Address")
                    otherAddr = (Address)e.OldValue;
                else
                    otherAddr = e.Contact.Address;
                if (address.Equals(otherAddr))
                {
                    nodeForUpdate = node;
                    break;
                }
            }

            if (nodeForUpdate != null)
            {
                switch (e.PropertyName)
                {
                    case "Avatar":
                        if (e.Contact.Avatar != ImageHelper2.DefaultAvatar)
                        {
                            string avatarFileName = Guid.NewGuid().ToString() + ".bmp";
                            e.Contact.Avatar.Save(imegesPath + avatarFileName);

                            string oldFileName = (string)nodeForUpdate["AvatarFileName"].Value;

                            //if (System.IO.File.Exists(imegesPath + oldFileName))
                            //    System.IO.File.Delete(imegesPath + oldFileName);

                            nodeForUpdate["AvatarFileName"].Value = avatarFileName;
                        }
                        break;
                    case "Nick":
                        nodeForUpdate["Nick"].Value = e.Contact.Nick;
                        break;
                    case "Address":
                        IStorageNode address = nodeForUpdate["Address"];
                        address["GUID"].Value = e.Contact.Address.Guid;
                        address["Host"].Value = e.Contact.Address.Host;
                        address["Port"].Value = e.Contact.Address.Port.ToString();
                        break;
                    case "StatusText":
                        nodeForUpdate["StatusText"].Value = e.Contact.StatusText;
                        break;
                }
                
                contactStorage.Save();
            }
        }

        void contactManager_ContactRemoved(object sender, ContactEventArgs e)
        {
            IStorageNode nodeForRemove = null;
            foreach (IStorageNode node in contactStorage.GetNodes("Contact"))
            {
                IStorageNode addressNode = node["Address"];
                int port = int.Parse(addressNode["Port"].Value.ToString());
                Address address = new Address((string)addressNode["Host"].Value, port, (string)addressNode["GUID"].Value);
                if (address.Equals(e.Contact.Address))
                {
                    nodeForRemove = node;
                    break;
                }
            }
            if (nodeForRemove != null)
            {
                contactStorage.Remove(nodeForRemove);
                contactStorage.Save();
            }
        }

        void contactManager_ContactAdded(object sender, ContactEventArgs e)
        {
            IStorageNode nodeContact = contactStorage.AddNode("Contact");
            nodeContact.AddNode("Nick").Value = e.Contact.Nick;
            IStorageNode addressNode = nodeContact.AddNode("Address");
            addressNode.AddNode("Host").Value = e.Contact.Address.Host;
            addressNode.AddNode("GUID").Value = e.Contact.Address.Guid;
            addressNode.AddNode("Port").Value = e.Contact.Address.Port;
            contactStorage.Save();
        }

        private void RequestContactInfos()
        {
            Contact[] contacts = contactManager.GetContacts();
            foreach (Contact contact in contacts)
            {
                telegramListener.RequestUserInfo(contact.Address);
            }
        }

        public void SendStatusToContacts()
        {
            Contact[] contacts = contactManager.GetContacts();
            foreach (Contact contact in contacts)
            {
                telegramListener.SendStatus(contact.Address, account.Status, account.StatusText);
            }
        }

        public delegate void Procedure0();

        public void SendStatusToContactsAsync()
        {
            Procedure0 proc = SendStatusToContacts;
            IAsyncResult res = proc.BeginInvoke(delegate(IAsyncResult ar)
            {
                proc.EndInvoke(ar); 
            }, null);
             
        }

        public void SendInfoToContacts()
        {
            Contact[] contacts = contactManager.GetContacts();
            foreach (Contact contact in contacts)
            {
                telegramListener.SendInfo(contact.Address,
                    account.Avatar,
                    account.Nick,
                    account.Status,
                    account.StatusText);
            }
        }

        public void SendInfoToContactsAsync()
        {
            Procedure0 proc = new Procedure0(SendInfoToContacts);
            proc.Invoke();
        }


        void telegramListener_UserStatusReceived(object sender, UserStatusReceivedEventArgs e)
        {
            Contact contact = contactManager.GetContactByAddress(e.Address);
            if (contact != null)
            {
                if (contact.Status != e.Status)
                    contact.Status = e.Status;
                if (contact.StatusText != e.Text)
                    contact.StatusText = e.Text;
                contact.Tag["LastReceivedTelegramTime"] = DateTime.Now;
            }
        }

        void telegramListener_UserInfoReceived(object sender, UserInfoReveivedEventArgs e)
        {
            Contact contact = contactManager.GetContactByAddress(e.Address);
            if (contact != null)
            {
                contact.Status = e.Status;
                contact.StatusText = e.StatusText;
                if (!contact.Address.EqualIPAddress(e.Address)) // Это условие выполнятеся, когда контакт был найден по GUID.
                {                                               // В этом случае нужно предупредить пользователя, что контакт с таким-то GUID изменил свой Host.
                    
                    contact.Address = e.Address; 
               
                }
                if (contact.Address.Guid != e.Address.Guid) // Это условие выполнятеся, когда контакт был найден по Host.
                {

                    contact.Address = new Address(contact.Address, e.Address.Guid);

                }
                contact.Nick = e.Nick;
                contact.Avatar = e.Avatar;
                contact.Tag["LastReceivedTelegramTime"] = DateTime.Now;
            }
        }

    }
}
