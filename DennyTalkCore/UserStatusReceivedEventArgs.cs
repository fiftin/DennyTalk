using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class UserStatusReceivedEventArgs : EventArgs
    {
        private string text;
        private UserStatus status;
        private Address address;
        public UserStatusReceivedEventArgs(Address address, UserStatus status, string text)
        {
            this.address = address;
            this.status = status;
            this.text = text;
        }
        public Address Address
        {
            get { return address; }
        }

        public UserStatus Status
        {
            get { return status; }
        }

        public string Text
        {
            get { return text; }
        }

    }
}
