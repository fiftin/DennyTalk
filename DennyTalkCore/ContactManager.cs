using System;
using System.Collections.Generic;
using System.Text;

namespace DennyTalk
{
    public class ContactEventArgs : EventArgs
    {
        public ContactEventArgs(Contact contact)
        {
            this.contact = contact;
        }
        
        public ContactEventArgs(Contact contact, string propertyName, object oldValue, object newValue)
        {
            this.contact = contact;
            this.propertyName = propertyName;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        private Contact contact;
        private string propertyName = null;
        private object oldValue;
        private object newValue;
        public object NewValue
        {
            get { return newValue; }
        }


        public object OldValue
        {
            get { return oldValue; }
        }

        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        public Contact Contact
        {
            get { return contact; }
        }

    }

    public class ContactManager
    {
        public Contact GetContactByAddress(Address address)
        {
            foreach (Contact cont in contacts.ToArray())
            {
                if (cont.Address.Equals(address))
                {
                    return cont;
                }
            }

            return null;
        }

        public Contact GetContactByGuid(string guid)
        {
            foreach (Contact cont in contacts)
            {
                if (cont.Address.Guid == guid)
                {
                    return cont;
                }
            }

            return null;
        }

        public Contact[] GetContacts()
        {
            return contacts.ToArray();
        }

        public bool AddContact(Contact contact)
        {
            if (Array.Exists(GetContacts(), x => x.Address.Equals(contact.Address)))
                return false;
            contacts.Add(contact);
            contact.PropertyChange += new EventHandler<PropertyChangeNotifierEventArgs>(contact_PropertyChange);
            OnContactAdded(contact);
            return true;
        }

        void contact_PropertyChange(object sender, PropertyChangeNotifierEventArgs e)
        {
            Contact contact = (Contact)sender;
            if (ContactChanged != null)
            {
                ContactChanged(this, new ContactEventArgs(contact, e.PropertyName, e.OldValue, e.NewValue));
            }
        }

        protected virtual void OnContactAdded(Contact contact)
        {
            if (ContactAdded != null)
                ContactAdded(this, new ContactEventArgs(contact));
        }

        public bool RemoveContactByAddress(Address address)
        {
            Contact cont = GetContactByAddress(address);
            if (cont == null)
                return false;
            return RemoveContact(cont);
        }

        public bool RemoveContact(Contact contact)
        {
            bool ret = contacts.Remove(contact);
            contact.PropertyChange -= contact_PropertyChange;
            if (ret)
            {
                if (ContactRemoved != null)
                    ContactRemoved(this, new ContactEventArgs(contact));
            }
            return ret;
        }

        public event EventHandler<ContactEventArgs> ContactAdded;
        public event EventHandler<ContactEventArgs> ContactRemoved;
        public event EventHandler<ContactEventArgs> ContactChanged;

        private List<Contact> contacts = new List<Contact>();

    }
}
