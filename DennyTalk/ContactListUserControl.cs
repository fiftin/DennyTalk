using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public partial class ContactListUserControl : UserControl
    {
        BindingList<ContactInfo> contacts = new BindingList<ContactInfo>();
        BindingSource contactsBindingSource = new BindingSource();
        public ContactListUserControl()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            contactsBindingSource.DataSource = contacts;
            dataGridView1.DataSource = contactsBindingSource;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contacts"></param>
        public void AddContacts(IEnumerable<ContactInfo> contacts)
        {
            foreach (ContactInfo contact in contacts)
            {
                this.contacts.Add(contact);
                contact.PropertyChanged += new PropertyChangedEventHandler(contact_PropertyChanged);
            }
        }

        void contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ContactInfo cont = (ContactInfo)sender;
            if (e.PropertyName == "StatusText")
            {
                cont.NotifyPropertyChanged("Nick");
            }
        }



        public ContactInfo GetContactByAddress(Address address)
        {
            foreach (ContactInfo cont in contacts)
            {
                if (cont.Address.Equals(address))
                    return cont;
            }
            return null;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ContactInfo contactInfo = (ContactInfo)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            OnContactDoubleClick(contactInfo);
        }

        private void OnContactDoubleClick(ContactInfo contactInfo)
        {
            if (ContactDoubleClick != null)
            {
                ContactDoubleClick(this, new ContactInfoEventArgs(contactInfo));
            }
        }

        public event EventHandler<ContactInfoEventArgs> ContactDoubleClick;
        public event EventHandler<ContactInfoEventArgs> ContactRemoveRequest;
        public event EventHandler<ContactInfoEventArgs> ContactEditRequest;
        public event EventHandler<ContactInfoEventArgs> ContactShowInfoRequest;

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            ContactInfo contactInfo = (ContactInfo)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            switch (e.ColumnIndex)
            {
                case 3: // Show info
                    OnContactShowInfoRequest(contactInfo);
                    break;
                case 4: // Edit
                    OnContactEditRequest(contactInfo);
                    break;
                case 5: // Remove
                    OnContactRemoveRequest(contactInfo);
                    break;
            }
        }

        private void OnContactRemoveRequest(ContactInfo contactInfo)
        {
            if(ContactRemoveRequest!=null)
                ContactRemoveRequest(this, new ContactInfoEventArgs(contactInfo));
        }

        private void OnContactEditRequest(ContactInfo contactInfo)
        {
            if(ContactEditRequest!=null)
                ContactEditRequest(this, new ContactInfoEventArgs(contactInfo));
        }

        private void OnContactShowInfoRequest(ContactInfo contactInfo)
        {
            if (ContactShowInfoRequest != null)
                ContactShowInfoRequest(this, new ContactInfoEventArgs(contactInfo));
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 3: // Show info
                case 4: // Edit
                case 5: // Remove
                    //Cursor.Current = Cursors.Hand;
                    //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGray;
                    break;
            }
        }

        private void dataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 3: // Show info
                case 4: // Edit
                case 5: // Remove
                    //Cursor.Current = Cursors.Default;
                    //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    break;
            }
        }


        internal void RemoveContactByAddress(Address address)
        {
            ContactInfo cont = GetContactByAddress(address);
            contacts.Remove(cont);
            
        }
    }
}
