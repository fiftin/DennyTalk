using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public partial class DialogUserControl : UserControl, IPropertyChangeNotifier
    {
        public DialogUserControl()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = messages;
        }

        private void DialogUserControl_Load(object sender, EventArgs e)
        {
        }

        public void AddMessages(HistoryMessage[] historyMessages)
        {
            foreach (HistoryMessage historyMessage in historyMessages)
            {
                AddMessage(historyMessage);
            }
        }

        public void AddMessage(HistoryMessage historyMessage)
        {
            Message msg = new Message();
            msg.Text = historyMessage.Text;
            msg.Time = historyMessage.Time;
            msg.ID = historyMessage.ID;
            msg.Direction = historyMessage.Direction;

            if (historyMessage.Direction == HistoryMessageDirection.In)
            {
                msg.SenderAddress = historyMessage.FromAddress;
                msg.SenderNick = UserInfo.Nick;
                msg.SenderAvatar = UserInfo.Avatar;
                
            }
            else if (historyMessage.Direction == HistoryMessageDirection.Out)
            {
                msg.SenderAddress = AccountAddress;
                msg.SenderNick = AccountNick;
                msg.SenderAvatar = AccountAvatar;
            }

            this.messages.Add(msg);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        public ContactInfo UserInfo
        {
            get { return contactInfo; }
            set
            {
                if (contactInfo != value)
                {
                    if (contactInfo != null)
                    {
                        contactInfo.PropertyChange -= new EventHandler<PropertyChangeNotifierEventArgs>(contactInfo_PropertyChange);
                    }
                    contactInfo = value;
                    if (contactInfo != null)
                    {
                        contactInfo.PropertyChange += new EventHandler<PropertyChangeNotifierEventArgs>(contactInfo_PropertyChange);
                    }
                }
            }
        }

        void contactInfo_PropertyChange(object sender, PropertyChangeNotifierEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Status":

                    break;
            }
        }

        private Bitmap accountAvatar;
        private string accountNick;
        private Address accountAddress;

        public Address AccountAddress
        {
            get { return accountAddress; }
            set { accountAddress = value; }
        }

        public string AccountNick
        {
            get { return accountNick; }
            set
            {
                if (accountNick != value)
                {
                    object oldValue = accountNick;
                    accountNick = value;
                    NotifyPropertyChanged("AccountNick", oldValue, value);
                }
            }
        }

        public Bitmap AccountAvatar
        {
            get { return accountAvatar; }
            set { accountAvatar = value; }
        }


        private ContactInfo contactInfo;
        private BindingList<Message> messages = new BindingList<Message>();

        protected virtual void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new PropertyChangeNotifierEventArgs(propertyName, oldValue, newValue));
            }
        }

        public event EventHandler<PropertyChangeNotifierEventArgs> PropertyChange;

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Message copyMessage = (Message)dataGridView1.SelectedRows[0].DataBoundItem;
                string text = copyMessage.Text;
                Clipboard.Clear();
                Clipboard.SetText(text);
            }
        }



        public void SetMessageDelivered(int messageID)
        {
            foreach (Message msg in messages)
            {
                if (msg.ID == messageID)
                {
                    msg.Delivered = true;
                    Invoke(new MethodInvoker(() =>  msg.NotifyPropertyChanged("Text")));
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                dataGridView1.Rows[e.RowIndex].Selected = true;
        }
    }
}
