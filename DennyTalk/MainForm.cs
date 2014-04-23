using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using WinFormsPopupAlerts;

namespace DennyTalk
{
    public partial class MainForm : Form, System.ComponentModel.INotifyPropertyChanged
    {
        public MainForm()
        {
            InitializeComponent();
            Version appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Text += " " + appVersion.ToString();
        }

        public ContactListUserControl ContactList
        {
            get { return contactListUserControl1; }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            EndEditNick(false);
            EndEditStatusText(false);
            contextMenuStrip1.Show(picStatus, new Point(-5, -5));
        }

        private void label2_MouseDown(object sender, MouseEventArgs e)
        {
            pnlStatusText.Visible = true;
            lblStatusText.Visible = false;
            txtStatusText.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    EndEditStatusText(false);
                    break;
                case Keys.Enter:
                    EndEditStatusText(true);
                    break;
            }
        }

        private void txtNick_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    EndEditNick(false);
                    break;
                case Keys.Enter:
                    EndEditNick(true);
                    break;
            }
        }

        public void BeginEditNick()
        {
            txtNick.Text = lblNick.Text;
            pnlNick.Visible = true;
            lblNick.Visible = false;
            EndEditStatusText();
        }

        public void EndEditNick(bool saveChanges)
        {
            pnlNick.Visible = false;
            lblNick.Visible = true;
            if (saveChanges)
            {
                lblNick.Text = txtNick.Text;
                NotifyPropertyChanged("Nick");
            }
        }

        public void EndEditNick()
        {
            EndEditNick(false);
        }

        public void BeginEditStatusText()
        {
            txtStatusText.Text = "";
            EndEditNick();
            pnlStatusText.Visible = true;
            lblStatusText.Visible = false;
        }

        public void EndEditStatusText()
        {
            EndEditStatusText(false);
        }

        public void EndEditStatusText(bool saveChanges)
        {
            pnlStatusText.Visible = false;
            lblStatusText.Visible = true;
            
            if (saveChanges)
            {
                StatusText = txtStatusText.Text;
                NotifyPropertyChanged("StatusText");
            }
            txtStatusText.Text = "";
        }

        private void txtNick_Leave(object sender, EventArgs e)
        {
            EndEditNick(false);
        }

        private void txtStatusText_Leave(object sender, EventArgs e)
        {
            EndEditStatusText(false);
        }


        private void lblStatusText_MouseEnter(object sender, EventArgs e)
        {
            lblStatusText.BackColor = SystemColors.ControlLight;
        }

        private void lblStatusText_MouseLeave(object sender, EventArgs e)
        {
            lblStatusText.BackColor = SystemColors.Control;
        }

        private void lblNick_MouseEnter(object sender, EventArgs e)
        {
            lblNick.BackColor = SystemColors.ControlLight;
        }

        private void lblNick_MouseLeave(object sender, EventArgs e)
        {
            lblNick.BackColor = SystemColors.Control;
        }

        private void lblNick_MouseDown(object sender, MouseEventArgs e)
        {
            lblNick.Visible = false;
            pnlNick.Visible = true;
            txtNick.Text = lblNick.Text;
            txtNick.Focus();
        }

        private Bitmap avatar;

        public Bitmap Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                picAvatar.Image = value;
            }
        }


        private void picAvatar_MouseDown(object sender, MouseEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(openFileDialog1.FileName);
                avatar = new Bitmap(bmp, 50, 50);
                picAvatar.Image = avatar;
                NotifyPropertyChanged("Avatar");
            }
        }

        private void toolStripButtonAddContact_Click(object sender, EventArgs e)
        {
            //notifyIcon1.Icon = Icon.FromHandle(DennyTalk.Properties.Resources.email.GetHicon());
            //notifyIcon1.ShowBalloonTip(1, "Fandorin", "Hello, Fif", ToolTipIcon.None);
            if (AddContactButtonClick != null)
                AddContactButtonClick(this, new EventArgs());

        }

        private void toolStripButtonConfig_Click(object sender, EventArgs e)
        {
            OnConfigButtonClick();
        }

        private void OnConfigButtonClick()
        {
            if (ConfigButtonClick != null)
                ConfigButtonClick(this, new EventArgs());
        }
        public event EventHandler ConfigButtonClick;
        public event EventHandler AddContactButtonClick;


        private void contactListUserControl1_ContactDoubleClick(object sender, ContactInfoEventArgs e)
        {
            OnContactDoubleClick(e);
        }

        protected virtual void OnContactDoubleClick(ContactInfoEventArgs e)
        {
            if (ContactDoubleClick != null)
                ContactDoubleClick(this, e);
        }

        public string Nick
        {
            get
            {
                return lblNick.Text;
            }
            set
            {
                lblNick.Text = value;
            }
        }

        public string statusText = "";
        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                if (value == "")
                {
                    lblStatusText.Text = "set status message";
                }
                else
                    lblStatusText.Text = value;
                statusText = value;
            }
        }

        private UserStatus status = UserStatus.Offline;

        public UserStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                ResetNotifyIcon();
                NotifyPropertyChanged("Status");
            }
        }
        public event EventHandler<ContactInfoEventArgs> ContactDoubleClick;



        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Online;
        }

        private void offlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Offline;
        }

        private void picAvatar_Click(object sender, EventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        private void awayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Away;
        }

        private void dndToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Dns;

        }

        private void notAvaliableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.NotAvaliable;

        }

        private void occupiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Occupied;

        }

        private void eatingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Eating;
        }

        private void freeForChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.FreeForChat;
        }

        private void atWorkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.AtWork;
        }

        private void atHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.AtHome;
        }

        private void angryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.Angry;
        }

        private void badMoodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = UserStatus.BadMood;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
        }

        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
        }

        private FormWindowState lastWindowState = FormWindowState.Normal;

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (Visible == false)
            {
                Show();


            }
            else
            {
                //Hide();
            }

            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = lastWindowState;
            }
            ShowTopMost();
        }
        public void ShowTopMost()
        {
            this.TopMost = true;
            this.Focus();
            this.TopMost = false;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Address addr = (Address)notifyIcon1.Tag;
            ContactInfo cont = ContactList.GetContactByAddress(addr);
            if (cont != null)
            {
                OnContactDoubleClick(new ContactInfoEventArgs(cont));
            }
            else
            {
                cont = new ContactInfo();
                cont.Address = addr;
                OnContactDoubleClick(new ContactInfoEventArgs(cont));
            }
            ResetNotifyIcon();
        }

        public void SetContactImageAsNewMessage(HistoryMessage message, Address address)
        {
            ContactInfo cont = ContactList.GetContactByAddress(address);
            notifyIcon1.Icon = Icon.FromHandle(ImageHelper.MessageImage.GetHicon());
            notifyIcon1.BalloonTipText = message.Text;
            if (cont != null)
            {
                cont.StatusImage = ImageHelper.MessageImage;
                notifyIcon1.BalloonTipTitle = cont.Nick;
            }
            else
            {
                notifyIcon1.BalloonTipTitle = address.Host;
            }
            notifyIcon1.Tag = address;
            //notifyIcon1.ShowBalloonTip(100);
            WinFormsPopupAlerts.PopupAlert alert;
            if (cont == null)
            {
                alert = popupAlertManager1.Alert(new TooltipAlertArg(address.IPAddress.ToString(), message.Text, ImageHelper2.DefaultAvatar));
            }
            else
            {
                alert = popupAlertManager1.Alert(new TooltipAlertArg(cont.Nick, message.Text, cont.Avatar));
            }
            alert.Tag = address;
        }


        private List<string> contactsMarkedAsNewMessage = new List<string>();

        private void MainForm_Activated(object sender, EventArgs e)
        {
            ResetNotifyIcon();
        }

        public void SetNotifyIconAsNewMessage(Address address)
        {
            if (!contactsMarkedAsNewMessage.Contains(address.Guid))
            {
                contactsMarkedAsNewMessage.Add(address.Guid);
            }
        }

        public void ResetNotifyIcon(Address address)
        {
            contactsMarkedAsNewMessage.Remove(address.Guid);
            if (contactsMarkedAsNewMessage.Count == 0)
                ResetNotifyIcon();
        }

        public void ResetNotifyIcon()
        {
            Bitmap bmp =ImageHelper.GetUserStatusImage(Status);
            picStatus.Image = bmp;
            notifyIcon1.Icon = Icon.FromHandle(bmp.GetHicon());
            statusToolStripMenuItem.Image = bmp;
        }

        public void ResetContactImage(Address address)
        {
            ContactInfo cont = ContactList.GetContactByAddress(address);
            if (cont != null)
            {
                cont.StatusImage = ImageHelper.GetUserStatusImage(cont.Status);
            }
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            if (statusToolStripMenuItem.DropDown.Name == "")
                statusToolStripMenuItem.DropDown = contextMenuStrip1;
        }

        private void toolStripButtonUpdate_Click(object sender, EventArgs e)
        {
            if (CheckUpdates != null)
                CheckUpdates(this, new EventArgs());
        }

        public event EventHandler CheckUpdates;

        public void NewVersionAvaliable(Version newVersion)
        {
            toolStripButtonUpdate.Visible = true;
            toolStripButtonUpdate.Text = string.Format("Avaliable {0}", newVersion.ToString(2));
        }


        private void tooltipAlertFactory1_AlertMouseDown(object sender, MouseEventArgs e)
        {
            WinFormsPopupAlerts.PopupAlert alert = (WinFormsPopupAlerts.PopupAlert)sender;
            Address addr = (Address)alert.Tag;
            ContactInfo cont = ContactList.GetContactByAddress(addr);
            if (cont != null)
            {
                OnContactDoubleClick(new ContactInfoEventArgs(cont));
            }
            else
            {
                cont = new ContactInfo();
                cont.Address = addr;
                OnContactDoubleClick(new ContactInfoEventArgs(cont));
            }
            ResetNotifyIcon();
        }
    }
}