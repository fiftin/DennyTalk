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
        private Bitmap accountAvatar;
        private string accountNick;
        private Address accountAddress;
        private ContactEx contactInfo;
        private BindingList<Message> messages = new BindingList<Message>();
        public event EventHandler<PropertyChangeNotifierEventArgs> PropertyChange;
        private Color evenMessageBackColor = Color.FromArgb(255, 240, 240, 240);
        private object oldValue;

        public DialogUserControl()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = messages;
            dataGridView1.AutoSize = true;

            UpdateColumn1Width();
        }

        public Message FindMessage(int id, MessageType type)
        {
            lock (messages)
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == id && msg.Type == type)
                        return msg;
                }
            }
            return null;
        }

        public void AddMessage(Message msg)
        {
            if (msg.Direction == MessageDirection.In)
            {
                msg.SenderNick = UserInfo.Nick;
                msg.SenderAvatar = UserInfo.Avatar;
            }
            else if (msg.Direction == MessageDirection.Out)
            {
                msg.SenderNick = AccountNick;
                msg.SenderAvatar = AccountAvatar;
            }
            Invoke(new MethodInvoker(() =>
            {
                lock (messages)
                {
                    this.messages.Add(msg);
                }
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
                int o = dataGridView1.Rows.Count % 2;
                if (o == 0)
                {
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = evenMessageBackColor;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1].Style.BackColor = evenMessageBackColor;
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Style.BackColor = evenMessageBackColor;
                }
            }));
            UpdateColumn1Width();
        }

        public ContactEx UserInfo
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

        protected virtual void NotifyPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            if (PropertyChange != null)
            {
                PropertyChange(this, new PropertyChangeNotifierEventArgs(propertyName, oldValue, newValue));
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && !dataGridView1.IsCurrentCellInEditMode)
            {
                Message copyMessage = (Message)dataGridView1.SelectedRows[0].DataBoundItem;
                string text = copyMessage.Text;
                Clipboard.Clear();
                Clipboard.SetText(text);
            }
        }

        public void SetMessageDelivered(int messageID)
        {
            lock (messages)
            {
                foreach (Message msg in messages)
                {
                    if (msg.ID == messageID)
                    {
                        msg.Delivered = true;
                        Invoke(new MethodInvoker(() => msg.NotifyPropertyChanged("Text")));
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                row.Selected = true;
                Message msg = (Message)row.DataBoundItem;
                if (msg.Type == MessageType.FilesRequest && msg.Direction == MessageDirection.In)
                {
                    if (e.Location.X > 10 && e.Location.X < 50
                        && e.Location.Y < row.Height - 10
                        && e.Location.Y > row.Height - 25)
                    {
                        DennyTalk.DialogManager.FilePortRequestInfo req = (DennyTalk.DialogManager.FilePortRequestInfo)msg.Tag;
                        req.TelegramListener.SendFilePort(req.Address, req.FileReceivingPort, msg.ID);
                        req.IsAcknowledged = true;
                        req.IsAccepted = true;
                    }
                    else if (e.Location.X > 60 && e.Location.X < 100
                        && e.Location.Y < row.Height - 10
                        && e.Location.Y > row.Height - 25)
                    {
                        DennyTalk.DialogManager.FilePortRequestInfo req = (DennyTalk.DialogManager.FilePortRequestInfo)msg.Tag;
                        req.TelegramListener.SendFilePort(req.Address, -1, msg.ID);
                        req.IsAcknowledged = true;
                        req.IsAccepted = false;
                    }
                }
                else if (msg.Type == MessageType.Files)
                {
                    if (e.Location.X > 10 && e.Location.X < 50
                        && e.Location.Y < row.Height - 10
                        && e.Location.Y > row.Height - 25)
                    {
                        FileTransferClient client = (FileTransferClient)msg.Tag;
                        client.Cancel();
                    }
                }
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                Message msg = (Message)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                if (msg.Type == MessageType.Message)
                    dataGridView1.BeginEdit(true);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = oldValue;
                copyToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                oldValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                copyToolStripMenuItem.ShortcutKeys = Keys.None;
            }
        }

        private void DialogUserControl_SizeChanged(object sender, EventArgs e)
        {
            UpdateColumn1Width();
        }

        private void UpdateColumn1Width()
        {
            Column1.Width = panel1.ClientSize.Width - (ColumnAvatar.Width + ColumnTime.Width + 20);
        }

        int oldDataGridViewHeight = 0;

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {
            if (Math.Abs(oldDataGridViewHeight-dataGridView1.Height) > 30)
            {
                panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;
                oldDataGridViewHeight = dataGridView1.Height;
            }
        }
    }
}
