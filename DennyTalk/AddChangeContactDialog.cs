using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{

    public partial class AddChangeContactDialog : Form
    {
        public AddChangeContactDialog()
        {
            InitializeComponent();
        }

        private string nick = "";
        private string guid = "";
        private string host = "";
        private int port = 0;
        private AddChangeContactAction action;

        public AddChangeContactAction Action
        {
            get { return action; }
            set
            {
                action = value;
                if (action == AddChangeContactAction.Add)
                {
                    Text = "Add Contact";
                    txtGuid.Enabled = true;
                    txtGuid.UseSystemPasswordChar = false;
                }
                else if (action == AddChangeContactAction.Change)
                {
                    Text = "Change Contact";
                    txtGuid.Enabled = true;
                    txtGuid.UseSystemPasswordChar = false;
                }
            }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public string Host
        {
            get { return host; }
            set { host = value; }
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string Nick
        {
            get { return nick; }
            set { nick = value; }
        }

        private Form owner;

        public DialogResult ShowDialog(Form owner)
        {
            this.owner = owner;
            return ShowDialog((IWin32Window)owner);
        }

        private void AddContactDialog_Load(object sender, EventArgs e)
        {
            this.Left = owner.Left + (owner.Width - this.Width) / 2;
            this.Top = owner.Top + (owner.Height - this.Height) / 2;

            txtNick.Text = nick;
            txtGuid.Text = guid;
            txtHost.Text = host;
            if (port > 0)
                txtPort.Text = port.ToString();
        }



        private bool canClose = true;

        private void AddChangeContactDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (canClose == false)
            {
                e.Cancel = true;
                canClose = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            nick = txtNick.Text.Trim();
            guid = txtGuid.Text.Trim();
            host = txtHost.Text.Trim();
            if (!int.TryParse(txtPort.Text.Trim(), out port))
            {
                port = 0;
            }
            if (host == "" && guid == "")
            {
                MessageBox.Show("Укажите host или GUID!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                canClose = false;
            }
        }

        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            Height = 221;
            lblNick.Visible = true;
            lblPort.Visible = true;
            txtNick.Visible = true;
            txtPort.Visible = true;
            btnAdvanced.Enabled = false;
        }

    }


    public enum AddChangeContactAction
    {
        Add,
        Change
    }

}