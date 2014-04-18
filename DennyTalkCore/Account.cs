using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DennyTalk
{
    public class Account : System.ComponentModel.INotifyPropertyChanged
    {
        private string nick;
        private UserStatus status;
        private Bitmap avatar;
        private string statusText;
        private Address address;

        public Address Address
        {
            get { return address; }
            set
            {
                address = value;
                NotifyPropertyChanged("Address");
            }
        }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }

        public Bitmap Avatar
        {
            get { return avatar; }
            set
            {
                if (avatar != value)
                {
                    avatar = value;
                    NotifyPropertyChanged("Avatar");
                }
            }
        }

        public UserStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string Nick
        {
            get { return nick; }
            set {

                if (nick != value)
                {
                    nick = value;
                    NotifyPropertyChanged("Nick");
                }
            }
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }

        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

    }
}
