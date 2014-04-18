using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class TelegramReceivedEventArgs : EventArgs
    {
        private string content;

        private int id;
        private string host;
        public string Host
        {
            get { return host; }
        }

        public string Content
        {
            get { return content; }
        }

        public int ID
        {
            get { return id; }
        }

    }
}
