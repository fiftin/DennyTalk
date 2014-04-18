using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DennyTalk
{
    public class UserInfoReveivedEventArgs : EventArgs
    {
        public UserInfoReveivedEventArgs(string nick, Bitmap avatar, UserStatus status, string statusText, Address address)
        {
            this.nick = nick;
            this.avatar = avatar;
            this.address = address;
            this.status = status;
            this.statusText = statusText;
        }

        private string nick;
        private Bitmap avatar;
        private UserStatus status;
        private string statusText;
        private Address address;

        public Address Address
        {
            get { return address; }
            set { address = value; }
        }

        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; }
        }

        public UserStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public Bitmap Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

    }
}
