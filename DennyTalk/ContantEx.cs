using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace DennyTalk
{
    public class ContactEx : Contact
    {
        private Bitmap statusImage;
        private Bitmap avatarSmall;

        public ContactEx()
        {
            statusImage = ImageHelper.GetUserStatusImage(UserStatus.Offline);
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

        public bool Equals(ContactEx other)
        {
            return Address.Equals(other.Address);
        }

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

        protected override void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (propertyName == "Status")
                StatusImage = ImageHelper.GetUserStatusImage((UserStatus)newValue);
            else if (propertyName == "Avatar")
                AvatarSmall = new Bitmap((Bitmap)newValue, 30, 30);
            base.NotifyPropertyChanged(propertyName, oldValue, newValue);
        }
    }
}
