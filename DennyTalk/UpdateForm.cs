using System.Windows.Forms;

namespace DennyTalk
{
    public partial class UpdateForm : Form
    {
        public UpdateForm()
        {
            InitializeComponent();
        }

        public string moreInfoLink = null;
        public string Info { get { return lblInfo.Text; } set { lblInfo.Text = value; } }
        public string MoreInfoLink { get { return moreInfoLink; } set { moreInfoLink = value; } }

        private void linkInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkInfo.LinkVisited = true;
            System.Diagnostics.Process.Start(MoreInfoLink);
        }
    }
}
