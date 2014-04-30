using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{




    public partial class DialogForm : Form
    {



        public DialogForm()
        {
            InitializeComponent();
        }

        public DialogUserControl GetDialog(Address address)
        {
            foreach (DialogTabPage page in tabControl1.TabPages)
            {
                if (page.UserInfo.Address.Equals(address))
                {
                    return page.Dialog;
                }
            }
            return null;
        }

        public DialogUserControl AddDialog(ContactInfo contactInfo, IEnumerable<HistoryMessage> messages)
        {
            DialogTabPage page = new DialogTabPage();
            page.UserInfo = contactInfo;
            
            tabControl1.TabPages.Add(page);
            tabControl1.SelectedTab = page;
            SetText(contactInfo);
            if (!Visible)
            {
                page.ImageIndex = 14;
            }
            page.Dialog.AccountAddress = AccountAddress;
            page.Dialog.AccountNick = AccountNick;
            page.Dialog.AccountAvatar = AccountAvatar;
            page.Dialog.GotFocus += new EventHandler(Dialog_GotFocus);
            return page.Dialog;
        }

        void Dialog_GotFocus(object sender, EventArgs e)
        {
            ResetPageImage();
        }


        public bool HasDialog(Address address)
        {
            foreach (DialogTabPage page in tabControl1.TabPages)
            {
                if (page.UserInfo.Address.Equals(address))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasDialog(ContactInfo contactInfo)
        {
            foreach (DialogTabPage page in tabControl1.TabPages)
            {
                if (page.UserInfo.Equals(contactInfo))
                {
                    return true;
                }
            }
            return false;
        }

        public DialogUserControl SelectDialog(Address address)
        {
            foreach (DialogTabPage page in tabControl1.TabPages)
            {
                if (page.UserInfo.Address.Equals(address))
                {
                    tabControl1.SelectedTab = page;
                    return page.Dialog;
                }
            }
            if (!Visible)
                Show();
            return null;
        }

        public DialogUserControl SelectDialog(ContactInfo contactInfo)
        {
            foreach (DialogTabPage page in tabControl1.TabPages)
            {
                if (page.UserInfo.Equals(contactInfo))
                {
                    tabControl1.SelectedTab = page;
                    page.ImageIndex = (int)page.Dialog.UserInfo.Status;

                    return page.Dialog;
                }
            }
            return null;
        }

        void SetText(ContactInfo user)
        {
            if (string.IsNullOrEmpty(user.Nick))
                Text = user.Address.Host;
            else
                Text = user.Nick;
        }

        private void DialogForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
            textBox1.Focus();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
            if (tabControl1.SelectedTab != null)
            {
                ContactInfo user = ((DialogTabPage)tabControl1.SelectedTab).UserInfo;
                SetText(user);
                if (ContactSelected != null)
                {
                    ContactSelected(this, new ContactInfoEventArgs(user));
                }
            }
        }

        public DialogUserControl SelectedDialog
        {
            get
            {
                DialogTabPage page = (DialogTabPage)tabControl1.SelectedTab;
                if (page == null)
                {
                    return null;
                }
                return page.Dialog;
            }
        }


        public void ResetPageImage(Address address)
        {
            Control dialog = GetDialog(address);
            if (dialog != null)
            {
                DialogTabPage page = (DialogTabPage)dialog.Parent;
                ResetPageImage(page);
            }
        }
        public void ResetPageImage()
        {
            if (tabControl1.SelectedTab != null)
            {
                ResetPageImage(tabControl1.SelectedTab);
            }
        }
        public void ResetPageImage(TabPage page)
        {
            if (page != null)
                page.ImageIndex = (int)((DialogTabPage)page).Dialog.UserInfo.Status;
        }


        private void SendMessage()
        {
            if (MessageSend != null)
            {
                if (textBox1.Text == "")
                    return;

                MessageSendEventArgs e = new MessageSendEventArgs(textBox1.Text,
                    ((DialogTabPage)tabControl1.SelectedTab).UserInfo.Address);
                MessageSend(this, e);
                if (e.ID > 0)
                    textBox1.Text = "";
                else
                {
                }
            }
            else
            {
            }
        }


        public event EventHandler<MessageSendEventArgs> MessageSend;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (!e.Control)
                    {
                        SendMessage();
                        e.SuppressKeyPress = true;
                    }
                    break;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        public void CloseAllDialogs()
        {
            foreach (DialogTabPage page in this.tabControl1.TabPages)
            {
                page.Controls.Remove(page);
                page.Dialog.Dispose();
            }
            this.tabControl1.TabPages.Clear();
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
            set { accountNick = value; }
        }

        public Bitmap AccountAvatar
        {
            get { return accountAvatar; }
            set { accountAvatar = value; }
        }

        private void addToContactListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogTabPage tab = (DialogTabPage)tabControl1.SelectedTab;
            ContactAdd(this, new ContactInfoEventArgs(tab.Dialog.UserInfo));

        }

        protected virtual void OnContactAdd(ContactInfo user)
        {
            if (ContactAdd != null)
            {
                ContactAdd(this, new ContactInfoEventArgs(user));
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            DialogTabPage page = null;
            for (int i = 0; i < tabControl1.TabPages.Count;i++)
            {
                Rectangle rect = tabControl1.GetTabRect(i);
                if (rect.Contains(e.Location))
                {
                    page = (DialogTabPage)tabControl1.TabPages[i];
                    break;
                }
            }
            if (page != null)
            {
                OnTabMouseDown(e, page);
            }
        }

        protected virtual void OnTabMouseDown(MouseEventArgs e, DialogTabPage tabPage)
        {
            tabControl1.SelectedTab = tabPage;
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(Cursor.Position);
            }
        }

        public event EventHandler<ContactInfoEventArgs> ContactAdd;
        public event EventHandler<ContactInfoEventArgs> ContactSelected;

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogTabPage page = (DialogTabPage)tabControl1.SelectedTab;
            if (page == null)
                return;

            DialogUserControl dialog = page.Dialog;
            page.Controls.Remove(dialog);
            page.Dialog.Dispose();
            tabControl1.TabPages.Remove(page);
            if (tabControl1.TabPages.Count == 0)
                Close();
        }

        public void Initialize()
        {
            IntPtr formHandle = this.Handle;
            foreach (Control control in this.Controls)
            {
                IntPtr handle = control.Handle;
                InitControls(control);
            }
        }

        private void InitControls(Control control)
        {
            foreach (Control x in control.Controls)
            {
                IntPtr handle = x.Handle;
                InitControls(x);
            }
        }

        public void ShowTopMost()
        {
            //this.TopMost = true;
            this.Focus();
            //this.Select();
            //this.TopMost = false;
        }


        public void SetDialogImageAsNewMessage(Address address)
        {
            if (!WinFormsHelper.IsFocused(this))
            {
                WinFormsHelper.StartBlinking(this);
            }

            DialogUserControl dialog = GetDialog(address);
            if (dialog != null)
            {
                DialogTabPage page = (DialogTabPage)dialog.Parent;
                //if (page != tabControl1.SelectedTab && !WinFormsHelper.IsFocused(page))
                page.ImageIndex = 14;
            }
        }

        private void DialogForm_Activated(object sender, EventArgs e)
        {
            WinFormsHelper.StopBlinking(this);
            ResetPageImage();
        }
    }
}