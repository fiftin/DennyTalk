using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class MessageDeliveredEventArgs : EventArgs
    {
        private int id;
        private Address address;
        public MessageDeliveredEventArgs(int id, Address address)
        {
            this.id = id;
            this.address = address;
        }
        public int ID
        {
            get { return id; }
        }
        public Address Address
        {
            get { return address; }
        }

	
    }
}
