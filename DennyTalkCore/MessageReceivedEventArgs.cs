using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private string text;
        private int id;
        private Address address;

        public MessageReceivedEventArgs(string text, int id, Address address)
        {
            this.text = text;
            this.id = id;
            this.address = address;
        }

        public Address Address
        {
            get { return address; }
        }

	
        public int ID
        {
            get { return id; }
        }

        public string Text
        {
            get { return text; }
        }

    }
}
