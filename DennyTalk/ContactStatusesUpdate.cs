using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DennyTalk
{
    public class ContactStatusesUpdate
    {
        ContactManager contactManager;
        TelegramListener telegramListener;
        Account account;

        public ContactStatusesUpdate(ContactManager contactManager, TelegramListener telegramListener, Account account)
        {
            this.contactManager = contactManager;
            this.telegramListener = telegramListener;
            this.account = account;

            thread = new Thread(DoWork);
            thread.IsBackground = true;
        }

        public void Start()
        {
            thread.Start();
        }

        private void UpdateContactStatuses()
        {
            Contact[] contacts = contactManager.GetContacts();
            foreach (Contact contact in contacts)
            {
                DateTime lastReceivedTelegramTime;
                object obj;
                if (!contact.Tag.TryGetValue("LastReceivedTelegramTime", out obj))
                {
                    DateTime now = DateTime.Now;
                    contact.Tag.Add("LastReceivedTelegramTime", now);
                    obj = now;
                }

                lastReceivedTelegramTime = (DateTime)obj;

                TimeSpan d = DateTime.Now - lastReceivedTelegramTime;
                if (d.TotalSeconds > 10 && contact.Status != UserStatus.Offline)
                {
                    contact.Status = UserStatus.Offline;
                }
            }
        }

        public void SendStatusToContact()
        {
            Contact[] contacts = contactManager.GetContacts();
            foreach (Contact contact in contacts)
            {
                telegramListener.SendStatus(contact.Address, account.Status, account.StatusText);
            }
        }

        private void DoWork()
        {
            while (true)
            {
                UpdateContactStatuses();

                SendStatusToContact();

                Thread.Sleep(1000);
            }
        }

        private Thread thread;
    }
}
