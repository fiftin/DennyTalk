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

    public enum HistoryMessageType
    {
        Message,
        Files
    }

    public class HistoryMessage
    {
        private DateTime time;
        private string text;
        private string[] filenames;
        private HistoryMessageDirection direction;
        private Address fromAddress;
        private int id;
        private HistoryMessageType type;

        public HistoryMessage(DateTime time, string text, HistoryMessageDirection direction, Address fromAddress, HistoryMessageType type)
        {
            this.time = time;
            this.text = text;
            this.direction = direction;
            this.fromAddress = fromAddress;
            this.type = type;
        }

        public HistoryMessage(DateTime time, string[] filenames, HistoryMessageDirection direction, Address fromAddress, HistoryMessageType type)
        {
            this.time = time;
            this.filenames = filenames;
            this.direction = direction;
            this.fromAddress = fromAddress;
            this.type = type;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public Address FromAddress
        {
            get { return fromAddress; }
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
