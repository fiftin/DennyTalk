using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DennyTalk
{
    public partial class DialogTabPage : TabPage
    {
        private ContactEx contactInfo;

        public DialogTabPage()
        {
            InitializeComponent();
            ImageIndex = 0;
            Controls.Add(dialogUserControl1);
            dialogUserControl1.PropertyChange += new EventHandler<PropertyChangeNotifierEventArgs>(dialogUserControl1_PropertyChange);
        }

        void dialogUserControl1_PropertyChange(object sender, PropertyChangeNotifierEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "AccountNick":
                    this.Invoke(new MethodInvoker(delegate()
                    {
                    }));
                    break;
            }
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

                    if (string.IsNullOrEmpty(contactInfo.Nick))
                    {
                        Text = contactInfo.Address.Host;
                    }
                    else
                    {
                        Text = contactInfo.Nick;
                    }
                    this.ImageIndex = (int)contactInfo.Status;
                    this.dialogUserControl1.UserInfo = value;
                }
            }
        }

        void contactInfo_PropertyChange(object sender, PropertyChangeNotifierEventArgs e)
        {
            Invoke(new MethodInvoker(delegate()
            {
                switch (e.PropertyName)
                {
                    case "Status":
                        this.ImageIndex = (int)contactInfo.Status;
                        break;
                    case "Nick":
                        if (string.IsNullOrEmpty(contactInfo.Nick))
                            Text = contactInfo.Address.Host;
                        else
                            Text = contactInfo.Nick;
                        break;
                }
            }));
        }

        public DialogUserControl Dialog
        {
            get { return this.dialogUserControl1; }
        }

    }
}
