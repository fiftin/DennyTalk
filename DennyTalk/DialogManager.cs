using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DialogManager
    {
        public class FilePortRequestInfo
        {
            public bool IsAcknowledged { get; set; }
            public bool IsAccepted { get; set; }
            public Address Address { get; private set; }
            public int NumberOfFiles { get; private set; }
            public TelegramListener TelegramListener { get; private set; }
            public int FileReceivingPort { get; private set; }
            public FilePortRequestInfo(int nFiles, Address address, TelegramListener l, int fileReceivingPort)
            {
                NumberOfFiles = nFiles;
                Address = address;
                TelegramListener = l;
                FileReceivingPort = fileReceivingPort;
            }
        }

        //
        private Messenger messanger;
        private TelegramListener telegramListener;
        private ContactManager contactManager;
        private Account account;
        private History history;
        //
        private MainForm mainForm = new MainForm();
        private DialogForm dialogForm = new DialogForm();

        public event EventHandler CheckUpdates;

        private void InitializeContacts()
        {
            Contact[] contacts = contactManager.GetContacts();
            List<ContactEx> contactInfos = new List<ContactEx>();
            foreach (Contact cont in contacts)
            {
                contactInfos.Add((ContactEx)cont);
            }
            mainForm.ContactList.AddContacts(contactInfos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public DialogManager(Messenger messanger)
        {
            this.messanger = messanger;
            telegramListener = this.messanger.TelegramListener;
            contactManager = this.messanger.ContactManager;
            account = this.messanger.Account;
            history = this.messanger.History;
            Initialize();
        }

        private void Initialize()
        {
            InitializeContactManager();

            InitializeTelegramListener();

            InitializeDialogs();

            InitializeContacts();

            MainForm.Avatar = messanger.Account.Avatar;
            MainForm.Nick = account.Nick;
            MainForm.StatusText = account.StatusText;
            MainForm.Status = account.Status;

            messanger.NewClient += new EventHandler<FileReceiverClientEventArgs>(messanger_NewClient);
        }

        private void InitializeDialogs()
        {
            MainForm.AddContactButtonClick += new EventHandler(MainForm_AddContactButtonClick);
            MainForm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(MainForm_PropertyChanged);
            MainForm.ContactDoubleClick += new EventHandler<ContactInfoEventArgs>(MainForm_ContactDoubleClick);
            MainForm.ConfigButtonClick += new EventHandler(MainForm_ConfigButtonClick);
            MainForm.ContactList.ContactEditRequest += new EventHandler<ContactInfoEventArgs>(ContactList_ContactEditRequest);
            MainForm.ContactList.ContactRemoveRequest += new EventHandler<ContactInfoEventArgs>(ContactList_ContactRemoveRequest);
            MainForm.ContactList.ContactShowInfoRequest += new EventHandler<ContactInfoEventArgs>(ContactList_ContactShowInfoRequest);
            MainForm.CheckUpdates += new EventHandler(dialog_CheckUpdates);
            dialogForm.AccountAddress = account.Address;
            dialogForm.AccountAvatar = account.Avatar;
            dialogForm.AccountNick = account.Nick;
            dialogForm.MessageSend += new EventHandler<MessageSendEventArgs>(DialogForm_MessageSend);
            dialogForm.FormClosing += new FormClosingEventHandler(dialogForm_FormClosing);
            dialogForm.ContactAdd += new EventHandler<ContactInfoEventArgs>(dialogForm_ContactAdd);
            dialogForm.ContactSelected += new EventHandler<ContactInfoEventArgs>(dialogForm_ContactSelected);
            dialogForm.FilesSend += new EventHandler<FilesSendEventArgs>(dialogForm_FilesSend);
            DialogForm.Initialize();
        }

        private void InitializeContactManager()
        {
            contactManager.ContactAdded += new EventHandler<ContactEventArgs>(contactManager_ContactAdded);
            contactManager.ContactRemoved += new EventHandler<ContactEventArgs>(contactManager_ContactRemoved);
            contactManager.ContactChanged+=new EventHandler<ContactEventArgs>(contactManager_ContactChanged);
        }

        private void InitializeTelegramListener()
        {
            telegramListener.FilePortReceived += new EventHandler<FilePortReceivedEventArgs>(telegramListener_FilePortReceived);
            telegramListener.FilePortRequestReceived += new EventHandler<FilePortRequestReceivedEventArgs>(telegramListener_FilePortRequest);
            telegramListener.MessageReceived += new EventHandler<MessageReceivedEventArgs>(telegramListener_MessageReceived);
            telegramListener.UserInfoReceived += new EventHandler<UserInfoReveivedEventArgs>(telegramListener_UserInfoReceived);
            telegramListener.MessageDelivered += new EventHandler<MessageDeliveredEventArgs>(telegramListener_MessageDelivered);
            telegramListener.UserStatusReceived += new EventHandler<UserStatusReceivedEventArgs>(telegramListener_UserStatusReceived);
        }

        void messanger_NewClient(object sender, FileReceiverClientEventArgs e)
        {
            Message msg = FindMessage(e.Client.RequestId, MessageType.FilesRequest,
                (ContactEx)contactManager.GetContactByGuid(e.Client.ContactGuid));
            if (msg != null)
            {
                msg.SetTypeAndTag(MessageType.Files, e.Client);
                e.Client.Message = msg;
            }
        }

        /// <summary>
        /// Получен ответ на запрос порта для передачи файлов.
        /// </summary>
        void telegramListener_FilePortReceived(object sender, FilePortReceivedEventArgs e)
        {
			/*
            Message msg = FindMessage(e.RequestId, MessageType.FilesRequest, (ContactEx)contactManager.GetContactByAddress(e.Address));
            if (msg != null)
            {
				FilePortRequestInfo info = (FilePortRequestInfo)msg.Tag;
                if (info.IsAcknowledged && info.IsAccepted)
                {
                    StringBuilder b = new StringBuilder();
                    for (int i = 1; i <= info.NumberOfFiles; i++)
                        b.Append("\n" + i + ". ???");
                    msg.Text += b.ToString();
                }
                else
                    msg.Text += "\nTransfering canceled";
            }
			*/
        }

        /// <summary>
        /// Получен запрос на передачу файлов.
        /// </summary>
        void telegramListener_FilePortRequest(object sender, FilePortRequestReceivedEventArgs e)
        {
            //String s = string.Format("Received request for transfering {0} file(s)", e.NumberOfFiles);
            Message msg = new Message(DateTime.Now, "", 
                MessageDirection.In, e.Address, MessageType.FilesRequest);
            msg.Tag = new FilePortRequestInfo(e.NumberOfFiles, e.Address, telegramListener, messanger.FileReceivingPort);
            msg.ID = e.RequestId;
            AddMessage(msg, (ContactEx)contactManager.GetContactByAddress(e.Address));
        }

        void contactManager_ContactChanged(object sender, ContactEventArgs e)
        {
            Address addr = e.Contact.Address;
            if (e.PropertyName == "Address")
                addr = (Address)e.OldValue;
            ContactEx cont = MainForm.ContactList.GetContactByAddress(addr);
            if (cont != null)
            {
                switch (e.PropertyName)
                {
                    case "Avatar":
                        cont.Avatar = e.Contact.Avatar;
                        break;
                    case "Address":
                        cont.Address = e.Contact.Address;
                        break;
                    case "Nick":
                        cont.Nick = e.Contact.Nick;
                        break;
                    case "Status":
                        cont.Status = e.Contact.Status;
                        break;
                    case "StatusText":
                        cont.StatusText = e.Contact.StatusText;
                        break;
                }                
            }
        }

        void contactManager_ContactRemoved(object sender, ContactEventArgs e)
        {
            MainForm.ContactList.RemoveContactByAddress(e.Contact.Address);
        }

        void contactManager_ContactAdded(object sender, ContactEventArgs e)
        {
            MainForm.ContactList.AddContacts(new ContactEx[] { (ContactEx)e.Contact });
        }

        void telegramListener_UserStatusReceived(object sender, UserStatusReceivedEventArgs e)
        {
            ContactEx cont = MainForm.ContactList.GetContactByAddress(e.Address);
            if (cont != null)
            {
                cont.Status = e.Status;
                cont.StatusText = e.Text;
            }
        }

        void telegramListener_MessageDelivered(object sender, MessageDeliveredEventArgs e)
        {
            DialogUserControl dialog = DialogForm.GetDialog(e.Address);
            if (dialog != null)
            {
                dialog.SetMessageDelivered(e.ID);
            }
        }

        void telegramListener_UserInfoReceived(object sender, UserInfoReveivedEventArgs e)
        {
            DialogUserControl dialog = DialogForm.GetDialog(e.Address);
            if (dialog != null)
            {
                dialog.AccountAvatar = e.Avatar;
                dialog.AccountNick = e.Nick;
                dialog.AccountAddress = e.Address;
            }
        }

        void telegramListener_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Message histMessage = new Message(DateTime.Now, e.Text, MessageDirection.In, e.Address, MessageType.Message);
            histMessage.ID = e.ID;
            if (!DialogForm.Visible)
            {
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    MainForm.SetContactImageAsNewMessage(histMessage, e.Address);
                }));
            }
            else
            {
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    DialogForm.SetDialogImageAsNewMessage(e.Address);
                    if (!WinFormsHelper.IsFocused(DialogForm))
                        MainForm.SetContactImageAsNewMessage(histMessage, e.Address);
                }));
            }
            DialogUserControl dialog = DialogForm.GetDialog(e.Address);
            if (dialog == null)
            {
                Contact contact = contactManager.GetContactByAddress(e.Address);
                if (contact != null)
                {
                    MainForm.Invoke(new MethodInvoker(delegate()
                    {
                        dialog = DialogForm.AddDialog((ContactEx)contact, new Message[] { histMessage });
                    }));
                }
                else
                {
                    ContactEx contactInfo = new ContactEx();
                    contactInfo.Address = e.Address;
                    contactInfo.Avatar = ImageHelper2.DefaultAvatar;

                    MainForm.Invoke(new MethodInvoker(delegate()
                    {
                        dialog = DialogForm.AddDialog(contactInfo, new Message[] { histMessage });
                    }));

                    telegramListener.RequestUserInfo(e.Address);
                }
            }
            dialog.Invoke(new MethodInvoker(delegate()
            {
                dialog.AddMessage(histMessage);
            }));
        }

        void dialogForm_FilesSend(object sender, FilesSendEventArgs e)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < e.FileNames.Length; i++)
                b.AppendLine(string.Format("{0}. {1}", i + 1, System.IO.Path.GetFileName(e.FileNames[i])));
            Message msg = new Message(DateTime.Now, string.Format("Sending file(s):\n{0}", b.ToString()), MessageDirection.Out, e.ReceiverContectInfo.Address, MessageType.Files);
            FileSenderConnection conn = messanger.SendFiles(e.ReceiverContectInfo, e.FileNames);
            msg.ID = conn.RequestId;
            msg.Tag = conn;
            conn.Message = msg;
            AddMessage(msg, e.ReceiverContectInfo);
        }

        void DialogForm_MessageSend(object sender, MessageSendEventArgs e)
        {
            TelegramSendResult result = telegramListener.SendMessage(e.Address, e.Text);
            e.ID = result.ID;
            Contact contact = contactManager.GetContactByAddress(e.Address);
            ContactEx contactInfo = new ContactEx();
            contactInfo.Address = e.Address;
            if (contact != null)
            {
                contactInfo.Nick = contact.Nick;
                contactInfo.Avatar = contact.Avatar;
            }
            Message msg = new Message(DateTime.Now, e.Text, MessageDirection.Out, e.Address, MessageType.Message);
            msg.ID = e.ID;
            AddMessage(msg, contactInfo);
        }

        void dialogForm_ContactSelected(object sender, ContactInfoEventArgs e)
        {
            MainForm.ResetContactImage(e.ContactInfo.Address);
            DialogForm.ResetPageImage(e.ContactInfo.Address);
        }

        void dialogForm_ContactAdd(object sender, ContactInfoEventArgs e)
        {
            if (contactManager.GetContactByAddress(e.ContactInfo.Address) == null)
            {
                contactManager.AddContact(e.ContactInfo);
                telegramListener.RequestUserInfo(e.ContactInfo.Address);
            }
        }

        void dialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall
                && e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                DialogForm.CloseAllDialogs();
                DialogForm.Hide();
            }
        }

        void MainForm_ContactDoubleClick(object sender, ContactInfoEventArgs e)
        {
            DialogForm.Focus();
            if (!DialogForm.HasDialog(e.ContactInfo))
            {
                DialogForm.Show();
                DialogForm.AddDialog(e.ContactInfo, history.GetMessage(e.ContactInfo.Address));
            }
            else
            {
                DialogForm.Show();
                DialogForm.SelectDialog(e.ContactInfo);
            }
            ResetContact(e.ContactInfo.Address);
        }

        void MainForm_AddContactButtonClick(object sender, EventArgs e)
        {
            Contact contact = new ContactEx();
            AddChangeContactDialog dialog = new AddChangeContactDialog();
            if (dialog.ShowDialog(MainForm) == DialogResult.OK)
            {
                contact.Nick = dialog.Host != "" ? dialog.Host : "Unnamed";
                string host = dialog.Host;
                int port = dialog.Port;
                string guid = dialog.Guid;
                if (string.IsNullOrEmpty(host))
                {
                    host = messanger.ServerHost;
                    port = messanger.ServerPort;
                }
                contact.Address = new Address(host, port, guid);
                contact.Avatar = ImageHelper2.DefaultAvatar;
                if (contactManager.AddContact(contact))
                    messanger.TelegramListener.RequestUserInfo(contact.Address);
                else
                    MessageBox.Show("User with this address already exists in your contact list", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void MainForm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nick":
                    account.Nick = MainForm.Nick;
                    break;
                case "Avatar":
                    account.Avatar = MainForm.Avatar;
                    break;
                case "Status":
                    account.Status = MainForm.Status;
                    break;
                case "StatusText":
                    account.StatusText = MainForm.StatusText;
                    break;
            }
        }

        void MainForm_ConfigButtonClick(object sender, EventArgs e)
        {
            ConfigurationDialog dialog = new ConfigurationDialog();
            dialog.Port = account.Address.Port;
            dialog.Guid = account.Address.Guid;
            dialog.CheckUpdates += dialog_CheckUpdates;
            dialog.ServerHost = messanger.ServerHost;
            dialog.ServerPort = messanger.ServerPort;
            dialog.UpdateServerHost = messanger.UpdateServerHost;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                account.Address = new Address("127.0.0.1", dialog.Port, dialog.Guid);
                messanger.ServerHost = dialog.ServerHost;
                messanger.ServerPort = dialog.ServerPort;
                messanger.UpdateServerHost = dialog.UpdateServerHost;

            }
            dialog.CheckUpdates -= dialog_CheckUpdates;
        }

        void ContactList_ContactShowInfoRequest(object sender, ContactInfoEventArgs e)
        {
            messanger.TelegramListener.RequestUserInfo(e.ContactInfo.Address);
        }

        void ContactList_ContactRemoveRequest(object sender, ContactInfoEventArgs e)
        {
            if (MessageBox.Show("Remove contact?", "Remove Contact", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                contactManager.RemoveContactByAddress(e.ContactInfo.Address);
            }
        }

        void ContactList_ContactEditRequest(object sender, ContactInfoEventArgs e)
        {
            Contact contact = contactManager.GetContactByAddress(e.ContactInfo.Address);
            if (contact == null)
            {
                MessageBox.Show("Contact not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AddChangeContactDialog dialog = new AddChangeContactDialog();
            dialog.Action = AddChangeContactAction.Change;
            dialog.Guid = contact.Address.Guid;
            dialog.Host = contact.Address.Host;
            dialog.Port = contact.Address.Port;
            if (dialog.ShowDialog(MainForm) == DialogResult.OK)
            {
                contact.Nick = dialog.Host != "" ? dialog.Host :  "Unnamed";
                string host = dialog.Host;
                int port = dialog.Port;
                string guid = dialog.Guid;
                if (string.IsNullOrEmpty(host))
                {
                    host = messanger.ServerHost;
                    port = messanger.ServerPort;
                }
                else if (port == 0)
                {
                    port = 1000;
                }
                contact.Address = new Address(host, port, guid);
                e.ContactInfo.Address = new Address(host, port, guid);
                e.ContactInfo.Nick = contact.Nick;
            }
        }

        void dialog_CheckUpdates(object sender, EventArgs e)
        {
            if (CheckUpdates != null)
                CheckUpdates(sender, e);
        }

        protected Message FindMessage(int requestId, MessageType type, ContactEx contactInfo)
        {
            if (DialogForm.HasDialog(contactInfo.Address))
            {
                DialogUserControl dialog = null;
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    dialog = DialogForm.SelectDialog(contactInfo.Address);
                }));
                return dialog.FindMessage(requestId, type);
            }
            return null;
        }

        protected void AddMessage(Message msg, ContactEx contactInfo)
        {
            DialogUserControl dialog = null;
            if (!DialogForm.HasDialog(contactInfo.Address))
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    dialog = DialogForm.AddDialog(contactInfo, new Message[] { msg });
                }));
            else
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    dialog = DialogForm.SelectDialog(contactInfo.Address);
                }));
            if (dialog != null)
                dialog.AddMessage(msg);
        }

        void ResetContact(Address address)
        {
            MainForm.ResetContactImage(address);
            DialogForm.ResetPageImage(address);
        }

        /// <summary>
        /// 
        /// </summary>
        public MainForm MainForm
        {
            get
            {
                return mainForm;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DialogForm DialogForm
        {
            get
            {
                return dialogForm;
            }
        }

        public void NewVersionAvaliable(Version newVersion)
        {
            MainForm.NewVersionAvaliable(newVersion);
        }
    }
}
