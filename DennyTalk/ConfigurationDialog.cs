using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public partial class ConfigurationDialog : Form
    {
        public ConfigurationDialog()
        {
            InitializeComponent();
        }

        private int port;
        private int serverPort;

        public string UpdateServerHost
        {
            get { return txtUpdateServer.Text; }
            set { txtUpdateServer.Text = value; }
        }

        public string ServerHost
        {
            get { return txtServerHost.Text; }
            set { txtServerHost.Text = value; }
        }

        public string Guid
        {
            get { return txtGuid.Text; }
            set { txtGuid.Text = value; }
        }

        public int Port
        {
            get { return port; }
            set
            {
                port = value;
                txtPort.Text = value.ToString();
            }
        }

        public int ServerPort
        {
            get { return serverPort; }
            set
            {
                serverPort = value;
                txtServerPort.Text = value.ToString();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            port = int.Parse(txtPort.Text);
            serverPort = int.Parse(txtServerPort.Text);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (CheckUpdates != null)
                CheckUpdates(this, new EventArgs());
        }

        public event EventHandler CheckUpdates;
    }
}