using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class TelegramSendResult
    {
        private int id;
        public TelegramSendResult(int id)
        {
            this.id = id;
        }
        public int ID
        {
            get { return id; }
        }

    }
}
