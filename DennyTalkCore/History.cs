using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class History
    {
        public Message[] GetMessage(Address fromAddress)
        {
            return GetMessage(fromAddress, "");
        }
        public Message[] GetMessage(Address fromAddress, string filter)
        {
            return GetMessage(fromAddress, filter, 0, -1);
        }
        public Message[] GetMessage(Address fromAddress, string filter, int startIndex, int count)
        {
            return new Message[0];
        }
    }
}
