using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class UserListener : IDisposable
    {
        private string host;
        private string nick;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        //public event EventHandler<MessageDeliveredEventArgs> MessageDelivered;

        //public event EventHandler<UserInfoReveivedEventArgs> UserInfoReceived;

        //public event EventHandler<UserStatusReceivedEventArgs> UserStatusReceived;

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public bool IsOnline
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public void SendMessage(string text, string[] fileNames)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Close()
        {
        }


        protected internal virtual void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (MessageReceived != null)
            {
                MessageReceived(sender, e);
            }
        }
    }
}
