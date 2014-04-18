namespace DennyTalk
{
    partial class DialogTabPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dialogUserControl1 = new DennyTalk.DialogUserControl();
            this.SuspendLayout();
            // 
            // dialogUserControl1
            // 
            this.dialogUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dialogUserControl1.Location = new System.Drawing.Point(0, 0);
            this.dialogUserControl1.Name = "dialogUserControl1";
            this.dialogUserControl1.Size = new System.Drawing.Size(459, 364);
            this.dialogUserControl1.TabIndex = 0;
            this.ResumeLayout(false);

        }

        #endregion

        private DialogUserControl dialogUserControl1;

    }
}
