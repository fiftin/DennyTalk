using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class ContactInfoEventArgs : EventArgs
    {
        public ContactInfoEventArgs(ContactInfo contactInfo)
        {
            this.contactInfo = contactInfo;
        }

        private ContactInfo contactInfo;
        public ContactInfo ContactInfo
        {
            get { return contactInfo; }
        }

    }
}
