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
                    lblGuid.Enabled = false;
                    txtGuid.Enabled = false;
                }
                else if (action == AddChangeContactAction.Change)
                {
                    Text = "Change Contact";
                    lblGuid.Enabled = true;
                    txtGuid.Enabled = true;
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


    }


    public enum AddChangeContactAction
    {
        Add,
        Change
    }

}