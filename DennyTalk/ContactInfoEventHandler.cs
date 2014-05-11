using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class ContactInfoEventArgs : EventArgs
    {
        public ContactInfoEventArgs(ContactEx contactInfo)
        {
            this.contactInfo = contactInfo;
        }

        private ContactEx contactInfo;
        public ContactEx ContactInfo
        {
            get { return contactInfo; }
        }

    }
}
