using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class FilesSendEventArgs : EventArgs
    {
        public string[] FileNames { get; private set; }
        public ContactInfo ReceiverContectInfo { get; private set; }
        public FilesSendEventArgs(string[] fileNames, ContactInfo receiver)
        {
            FileNames = fileNames;
            ReceiverContectInfo = receiver;
        }
    }
}
