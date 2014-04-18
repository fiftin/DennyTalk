using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DennyTalk
{
    public class Contact : System.ComponentModel.INotifyPropertyChanged, IPropertyChangeNotifier
    {
        private Address address;
        private string nick;
        private UserStatus status;
        private string statusText;
        private Bitmap avatar;

        private IDictionary<string, object> tag = new Dictionary<string, object>();

        public IDictionary<string, object> Tag
        {
            get { return tag; }
        }

        public Bitmap Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                if (avatar != value)
                {
                    object oldValue = avatar;
                    avatar = value;
                    NotifyPropertyChanged("Avatar", oldValue, value);
                }
            }
        }

        public string StatusText
        {
            get { return statusText; }
            set
            {
                if (statusText != value)
                {
                    object oldValue = statusText;
                    statusText = value;
                    NotifyPropertyChanged("StatusText", oldValue, value);
                }
            }
        }

        public UserStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    object oldValue = status;
                    status = value;
                    NotifyPropertyChanged("Status", oldValue, value);
                }
            }
        }

        public string Nick
        {
            get { return nick; }
            set
            {
                if (nick != value)
                {
                    object oldValue = nick;
                    nick = value;
                    NotifyPropertyChanged("Nick", oldValue, value);
                }
            }
        }


        public Address Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    object oldValue = address;
                    address = value;
                    NotifyPropertyChanged("Address", oldValue, value);
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

        protected virtual void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new PropertyChangeNotifierEventArgs(propertyName, oldValue, newValue));
            }
            NotifyPropertyChanged(propertyName);
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<PropertyChangeNotifierEventArgs> PropertyChange;

    }
}
