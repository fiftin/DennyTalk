using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DennyTalk
{
    public class ContactInfo : IComparable<ContactInfo>, INotifyPropertyChanged, IPropertyChangeNotifier
    {
        private string nick;
        private UserStatus status;
        private string statusText;
        private Address address;
        private Bitmap avatar;
        private Bitmap statusImage;
        private Bitmap avatarSmall;

        public ContactInfo()
        {
            status = UserStatus.Offline;
            statusImage = ImageHelper.GetUserStatusImage(UserStatus.Offline);
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
                    NotifyPropertyChanged("Address", oldValue, address);
                }
            }
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
                    Bitmap oldValue = avatar;
                    avatar = value;
                    NotifyPropertyChanged("Avatar", oldValue, avatar);
                    AvatarSmall = new Bitmap(avatar, 30, 30);
                }
            }
        }

        public Bitmap AvatarSmall
        {
            private set
            {
                Bitmap oldValue = avatarSmall;
                avatarSmall = value;
                NotifyPropertyChanged("AvatarSmall", oldValue, avatarSmall);
            }
            get
            {
                return avatarSmall;
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
                    StatusImage = ImageHelper.GetUserStatusImage(value);
                    NotifyPropertyChanged("Status", oldValue, value);
                }
            }
        }

        public Bitmap StatusImage
        {
            get
            {
                return statusImage;
            }
            set
            {
                if (statusImage != value)
                {
                    object oldValue = statusImage;
                    statusImage = value;
                    NotifyPropertyChanged("StatusImage", oldValue, value);
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

        public int CompareTo(ContactInfo other)
        {
            return address.CompareTo(other.Address);
        }

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<PropertyChangeNotifierEventArgs> PropertyChange;


        public Bitmap InfoImage
        {
            get
            {
                return ImageHelper.ContactInfoImage;
            }
        }

        public Bitmap EditImage
        {
            get
            {
                return ImageHelper.ContactEditImage;
            }
        }

        public Bitmap RemoveImage
        {
            get
            {
                return ImageHelper.ContactRemoveImage;
            }
        }
    }
}
