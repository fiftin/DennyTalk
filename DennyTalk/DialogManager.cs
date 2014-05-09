using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace DennyTalk
{
    public class DialogManager
    {
        //
        private Messanger messanger;
        private TelegramListener telegramListener;
        private ContactManager contactManager;
        private Account account;
        private History history;
        //
        private MainForm mainForm = new MainForm();
        private DialogForm dialogForm = new DialogForm();

        public event EventHandler CheckUpdates;


        private ContactInfo ConvertToContactInfo(Contact cont)
        {
            ContactInfo contactInfo = new ContactInfo();
            contactInfo.Avatar = cont.Avatar;
            contactInfo.Nick = cont.Nick;
            contactInfo.Status = cont.Status;
            contactInfo.StatusText = cont.StatusText;
            contactInfo.Address = cont.Address;
            return contactInfo;
        }


        private Contact ConvertToContact(ContactInfo contInfo)
        {
            Contact contact = new Contact();
            contact.Avatar = contInfo.Avatar;
            contact.Nick = contInfo.Nick;
            contact.Status = contInfo.Status;
            contact.StatusText = contInfo.StatusText;
            contact.Address = contInfo.Address;
            return contact;
        }

        private void InitializeContacts()
        {
            Contact[] contacts = contactManager.GetContacts();
            List<ContactInfo> contactInfos = new List<ContactInfo>();
            foreach (Contact cont in contacts)
            {
                ContactInfo contactInfo = ConvertToContactInfo(cont);
                contactInfos.Add(contactInfo);
            }
            mainForm.ContactList.AddContacts(contactInfos);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public DialogManager(Messanger messanger)
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
        }

        private void InitializeContactManager()
        {
            contactManager.ContactAdded += new EventHandler<ContactEventArgs>(contactManager_ContactAdded);
            contactManager.ContactRemoved += new EventHandler<ContactEventArgs>(contactManager_ContactRemoved);
            contactManager.ContactChanged+=new EventHandler<ContactEventArgs>(contactManager_ContactChanged);
        }

        void  contactManager_ContactChanged(object sender, ContactEventArgs e)
        {
            Address addr = e.Contact.Address;
            if (e.PropertyName == "Address")
                addr = (Address)e.OldValue;
            ContactInfo cont = MainForm.ContactList.GetContactByAddress(addr);
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
            MainForm.ContactList.AddContacts(new ContactInfo[] { ConvertToContactInfo(e.Contact) });
        }

        private void InitializeTelegramListener()
        {
            telegramListener.MessageReceived += new EventHandler<MessageReceivedEventArgs>(telegramListener_MessageReceived);
            telegramListener.UserInfoReceived += new EventHandler<UserInfoReveivedEventArgs>(telegramListener_UserInfoReceived);
            telegramListener.MessageDelivered += new EventHandler<MessageDeliveredEventArgs>(telegramListener_MessageDelivered);
            telegramListener.UserStatusReceived += new EventHandler<UserStatusReceivedEventArgs>(telegramListener_UserStatusReceived);
        }

        void telegramListener_UserStatusReceived(object sender, UserStatusReceivedEventArgs e)
        {
            ContactInfo cont = MainForm.ContactList.GetContactByAddress(e.Address);
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
            HistoryMessage histMessage = new HistoryMessage(DateTime.Now, e.Text, HistoryMessageDirection.In, e.Address, HistoryMessageType.Message);
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
                    ContactInfo contactInfo = ConvertToContactInfo(contact);
                    MainForm.Invoke(new MethodInvoker(delegate()
                    {
                        dialog = DialogForm.AddDialog(contactInfo, new HistoryMessage[] { histMessage });
                    }));
                }
                else
                {
                    ContactInfo contactInfo = new ContactInfo();
                    contactInfo.Address = e.Address;
                    contactInfo.Avatar = ImageHelper2.DefaultAvatar;

                    MainForm.Invoke(new MethodInvoker(delegate()
                    {
                        dialog = DialogForm.AddDialog(contactInfo, new HistoryMessage[] { histMessage });
                    }));

                    telegramListener.RequestUserInfo(e.Address);
                }
            }
            dialog.Invoke(new MethodInvoker(delegate()
            {
                dialog.AddMessage(histMessage);
            }));
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


        void dialogForm_ContactSelected(object sender, ContactInfoEventArgs e)
        {
            MainForm.ResetContactImage(e.ContactInfo.Address);
            DialogForm.ResetPageImage(e.ContactInfo.Address);
        }

        void dialogForm_ContactAdd(object sender, ContactInfoEventArgs e)
        {
            if (contactManager.GetContactByAddress(e.ContactInfo.Address) == null)
            {
                contactManager.AddContact(ConvertToContact(e.ContactInfo));
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

        void dialog_CheckUpdates(object sender, EventArgs e)
        {
            if (CheckUpdates != null)
                CheckUpdates(sender, e);
        }

        protected void AddMessage(HistoryMessage msg, ContactInfo contectInfo)
        {
            DialogUserControl dialog = null;
            if (!DialogForm.HasDialog(contectInfo.Address))
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    dialog = DialogForm.AddDialog(contectInfo, new HistoryMessage[] { msg });
                }));
            else
                MainForm.Invoke(new MethodInvoker(delegate()
                {
                    dialog = DialogForm.SelectDialog(contectInfo.Address);
                }));
            if (dialog != null)
                dialog.AddMessage(msg);
        }

        void dialogForm_FilesSend(object sender, FilesSendEventArgs e)
        {
            int telegramId = messanger.SendFiles(ConvertToContact(e.ReceiverContectInfo), e.FileNames);
            HistoryMessage msg = new HistoryMessage(DateTime.Now, string.Join("\n", e.FileNames), HistoryMessageDirection.Out, e.ReceiverContectInfo.Address, HistoryMessageType.Files);
            msg.ID = telegramId;
            AddMessage(msg, e.ReceiverContectInfo);
        }

        void DialogForm_MessageSend(object sender, MessageSendEventArgs e)
        {
            TelegramSendResult result = telegramListener.SendMessage(e.Address, e.Text);
            e.ID = result.ID;
            Contact contact = contactManager.GetContactByAddress(e.Address);
            ContactInfo contactInfo = new ContactInfo();
            contactInfo.Address = e.Address;
            if (contact != null)
            {
                contactInfo.Nick = contact.Nick;
                contactInfo.Avatar = contact.Avatar;
            }
            HistoryMessage msg = new HistoryMessage(DateTime.Now, e.Text, HistoryMessageDirection.Out, e.Address, HistoryMessageType.Message);
            msg.ID = e.ID;
            AddMessage(msg, contactInfo);
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

        void ResetContact(Address address)
        {
            MainForm.ResetContactImage(address);
            DialogForm.ResetPageImage(address);
        }

        void MainForm_AddContactButtonClick(object sender, EventArgs e)
        {
            Contact contact = new Contact();
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
