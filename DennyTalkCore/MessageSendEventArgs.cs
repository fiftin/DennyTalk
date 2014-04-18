using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class MessageSendEventArgs : EventArgs
    {
        private string text;
        private int id  = 0;
        private Address address;

        public MessageSendEventArgs(string text, Address address)
        {
            this.text = text;
            this.address = address;
        }

        public Address Address
        {
            get { return address; }
        }

	
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Text
        {
            get { return text; }
        }

    }
}
