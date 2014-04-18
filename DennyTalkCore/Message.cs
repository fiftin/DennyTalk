using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DennyTalk
{
    public class Message : INotifyPropertyChanged
    {
        private string text;
        private int id;
        private DateTime time;
        private string senderNick;
        private Bitmap senderImage;
        private Address senderAddress;
        private  HistoryMessageDirection direction;
        private bool delivered;

        public bool Delivered
        {
            get { return delivered; }
            set { delivered = value; }
        }

        public HistoryMessageDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Address SenderAddress
        {
            get { return senderAddress; }
            set { senderAddress = value; }
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

        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (text != value)
                {
                    text = value;
                    NotifyPropertyChanged("Text");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
