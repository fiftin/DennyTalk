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
        BindingList<ContactEx> contacts = new BindingList<ContactEx>();
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
        public void AddContacts(IEnumerable<ContactEx> contacts)
        {
            foreach (ContactEx contact in contacts)
            {
                this.contacts.Add(contact);
                contact.PropertyChanged += new PropertyChangedEventHandler(contact_PropertyChanged);
            }
        }

        void contact_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ContactEx cont = (ContactEx)sender;
            if (e.PropertyName == "StatusText")
            {
                cont.NotifyPropertyChanged("Nick");
            }
        }



        public ContactEx GetContactByAddress(Address address)
        {
            foreach (ContactEx cont in contacts)
            {
                if (cont.Address.Equals(address))
                    return cont;
            }
            return null;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ContactEx contactInfo = (ContactEx)dataGridView1.Rows[e.RowIndex].DataBoundItem;
            OnContactDoubleClick(contactInfo);
        }

        private void OnContactDoubleClick(ContactEx contactInfo)
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
            ContactEx contactInfo = (ContactEx)dataGridView1.Rows[e.RowIndex].DataBoundItem;
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

        private void OnContactRemoveRequest(ContactEx contactInfo)
        {
            if(ContactRemoveRequest!=null)
                ContactRemoveRequest(this, new ContactInfoEventArgs(contactInfo));
        }

        private void OnContactEditRequest(ContactEx contactInfo)
        {
            if(ContactEditRequest!=null)
                ContactEditRequest(this, new ContactInfoEventArgs(contactInfo));
        }

        private void OnContactShowInfoRequest(ContactEx contactInfo)
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
            ContactEx cont = GetContactByAddress(address);
            contacts.Remove(cont);
            
        }
    }
}
