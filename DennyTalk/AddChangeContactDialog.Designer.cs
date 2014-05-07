namespace DennyTalk
{
    partial class AddChangeContactDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtGuid = new System.Windows.Forms.TextBox();
            this.lblGuid = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(207, 86);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 31);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(126, 86);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 31);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtGuid
            // 
            this.txtGuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuid.Enabled = false;
            this.txtGuid.Location = new System.Drawing.Point(70, 50);
            this.txtGuid.Name = "txtGuid";
            this.txtGuid.Size = new System.Drawing.Size(212, 20);
            this.txtGuid.TabIndex = 3;
            // 
            // lblGuid
            // 
            this.lblGuid.AutoSize = true;
            this.lblGuid.Enabled = false;
            this.lblGuid.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblGuid.Location = new System.Drawing.Point(12, 53);
            this.lblGuid.Name = "lblGuid";
            this.lblGuid.Size = new System.Drawing.Size(51, 13);
            this.lblGuid.TabIndex = 2;
            this.lblGuid.Text = "Global ID";
            // 
            // txtHost
            // 
            this.txtHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHost.Location = new System.Drawing.Point(70, 12);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(108, 20);
            this.txtHost.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Host";
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.AutoCompleteCustomSource.AddRange(new string[] {
            "test",
            "text",
            "tess"});
            this.txtPort.Location = new System.Drawing.Point(218, 11);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(64, 20);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "1000";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPort.Location = new System.Drawing.Point(189, 15);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(26, 13);
            this.lblPort.TabIndex = 6;
            this.lblPort.Text = "Port";
            // 
            // AddChangeContactDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(294, 129);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGuid);
            this.Controls.Add(this.lblGuid);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddChangeContactDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Add Contact";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddChangeContactDialog_FormClosing);
            this.Load += new System.EventHandler(this.AddContactDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtGuid;
        private System.Windows.Forms.Label lblGuid;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
    }
}