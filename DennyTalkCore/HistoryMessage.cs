using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public enum HistoryMessageDirection
    {
        In,
        Out
    }

    public class HistoryMessage
    {
        private DateTime time;
        private string text;
        private HistoryMessageDirection direction;
        private Address fromAddress;
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public Address FromAddress
        {
            get { return fromAddress; }
        }

        public HistoryMessage(DateTime time, string text, HistoryMessageDirection direction, Address fromAddress)
        {
            this.time = time;
            this.text = text;
            this.direction = direction;
            this.fromAddress = fromAddress;
        }

        public HistoryMessageDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public DateTime Time
        {
            get { return time; }
        }

    }
}
