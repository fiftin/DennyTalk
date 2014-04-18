using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class TelegramDeliveredEventArgs : EventArgs
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

    }
}
