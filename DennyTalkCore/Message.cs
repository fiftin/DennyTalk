using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace DennyTalk
{
    public enum MessageDirection
    {
        In,
        Out
    }

    public enum MessageType
    {
        Message,
        Files,
        FilesRequest
    }

    public class Message : INotifyPropertyChanged
    {
        private int id;
        private DateTime time;
        private string text;
        private MessageDirection direction;
        private Address fromAddress;
        private MessageType type;
        private string[] filenames;
        private bool delivered;
        private string senderNick;
        private Bitmap senderImage;

        public Message(DateTime time, string text, MessageDirection direction, Address fromAddress, MessageType type)
        {
            this.time = time;
            this.text = text;
            this.direction = direction;
            this.fromAddress = fromAddress;
            this.type = type;
        }

        public Message(DateTime time, string[] filenames, MessageDirection direction, Address fromAddress, MessageType type)
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

        public bool Delivered
        {
            get { return delivered; }
            set { delivered = value; }
        }

        public Address FromAddress
        {
            get { return fromAddress; }
        }

        public MessageDirection Direction
        {
            get { return direction; }
            set
            {
                direction = value;
            }
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

        public Bitmap SenderAvatar
        {
            get { return senderImage; }
            set { senderImage = value; }
        }

        public string SenderNick
        {
            get { return senderNick; }
            set { senderNick = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                catch { }
            }
        }

    }
}
