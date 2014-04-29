namespace DennyTalk
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.onlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.awayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notAvaliableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.occupiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eatingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.freeForChatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atWorkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.atHomeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.badMoodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatusText = new System.Windows.Forms.Label();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.lblNick = new System.Windows.Forms.Label();
            this.picAvatar = new System.Windows.Forms.PictureBox();
            this.pnlStatusText = new System.Windows.Forms.Panel();
            this.txtStatusText = new System.Windows.Forms.TextBox();
            this.pnlNick = new System.Windows.Forms.Panel();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonConfig = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUpdate = new System.Windows.Forms.ToolStripButton();
            this.popupAlertManager1 = new WinFormsPopupAlerts.AlertManager(this.components);
            this.tooltipAlertFactory1 = new WinFormsPopupAlerts.TooltipAlertFactory(this.components);
            this.contactListUserControl1 = new DennyTalk.ContactListUserControl();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).BeginInit();
            this.pnlStatusText.SuspendLayout();
            this.pnlNick.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip2;
            this.notifyIcon1.Text = "DennyTalk";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            this.notifyIcon1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDown);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(107, 48);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.offline;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.statusToolStripMenuItem.Text = "Status";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineToolStripMenuItem,
            this.offlineToolStripMenuItem,
            this.awayToolStripMenuItem,
            this.dndToolStripMenuItem,
            this.notAvaliableToolStripMenuItem,
            this.occupiedToolStripMenuItem,
            this.eatingToolStripMenuItem,
            this.freeForChatToolStripMenuItem,
            this.atWorkToolStripMenuItem,
            this.atHomeToolStripMenuItem,
            this.angryToolStripMenuItem,
            this.badMoodToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(177, 268);
            // 
            // onlineToolStripMenuItem
            // 
            this.onlineToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.comments;
            this.onlineToolStripMenuItem.Name = "onlineToolStripMenuItem";
            this.onlineToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.onlineToolStripMenuItem.Text = "В сети";
            this.onlineToolStripMenuItem.Click += new System.EventHandler(this.onlineToolStripMenuItem_Click);
            // 
            // offlineToolStripMenuItem
            // 
            this.offlineToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.offline;
            this.offlineToolStripMenuItem.Name = "offlineToolStripMenuItem";
            this.offlineToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.offlineToolStripMenuItem.Text = "Не в сети";
            this.offlineToolStripMenuItem.Click += new System.EventHandler(this.offlineToolStripMenuItem_Click);
            // 
            // awayToolStripMenuItem
            // 
            this.awayToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.away;
            this.awayToolStripMenuItem.Name = "awayToolStripMenuItem";
            this.awayToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.awayToolStripMenuItem.Text = "Отошел";
            this.awayToolStripMenuItem.Click += new System.EventHandler(this.awayToolStripMenuItem_Click);
            // 
            // dndToolStripMenuItem
            // 
            this.dndToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.dnd;
            this.dndToolStripMenuItem.Name = "dndToolStripMenuItem";
            this.dndToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.dndToolStripMenuItem.Text = "Не беспокоить";
            this.dndToolStripMenuItem.Click += new System.EventHandler(this.dndToolStripMenuItem_Click);
            // 
            // notAvaliableToolStripMenuItem
            // 
            this.notAvaliableToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.not_available;
            this.notAvaliableToolStripMenuItem.Name = "notAvaliableToolStripMenuItem";
            this.notAvaliableToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.notAvaliableToolStripMenuItem.Text = "Не доступен";
            this.notAvaliableToolStripMenuItem.Click += new System.EventHandler(this.notAvaliableToolStripMenuItem_Click);
            // 
            // occupiedToolStripMenuItem
            // 
            this.occupiedToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.occupied;
            this.occupiedToolStripMenuItem.Name = "occupiedToolStripMenuItem";
            this.occupiedToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.occupiedToolStripMenuItem.Text = "Занят";
            this.occupiedToolStripMenuItem.Click += new System.EventHandler(this.occupiedToolStripMenuItem_Click);
            // 
            // eatingToolStripMenuItem
            // 
            this.eatingToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.eating;
            this.eatingToolStripMenuItem.Name = "eatingToolStripMenuItem";
            this.eatingToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.eatingToolStripMenuItem.Text = "Ем";
            this.eatingToolStripMenuItem.Click += new System.EventHandler(this.eatingToolStripMenuItem_Click);
            // 
            // freeForChatToolStripMenuItem
            // 
            this.freeForChatToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.free_for_chat;
            this.freeForChatToolStripMenuItem.Name = "freeForChatToolStripMenuItem";
            this.freeForChatToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.freeForChatToolStripMenuItem.Text = "Готов пообщаться";
            this.freeForChatToolStripMenuItem.Click += new System.EventHandler(this.freeForChatToolStripMenuItem_Click);
            // 
            // atWorkToolStripMenuItem
            // 
            this.atWorkToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.at_work;
            this.atWorkToolStripMenuItem.Name = "atWorkToolStripMenuItem";
            this.atWorkToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.atWorkToolStripMenuItem.Text = "На работе";
            this.atWorkToolStripMenuItem.Click += new System.EventHandler(this.atWorkToolStripMenuItem_Click);
            // 
            // atHomeToolStripMenuItem
            // 
            this.atHomeToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.at_home;
            this.atHomeToolStripMenuItem.Name = "atHomeToolStripMenuItem";
            this.atHomeToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.atHomeToolStripMenuItem.Text = "Дома";
            this.atHomeToolStripMenuItem.Click += new System.EventHandler(this.atHomeToolStripMenuItem_Click);
            // 
            // angryToolStripMenuItem
            // 
            this.angryToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.angry;
            this.angryToolStripMenuItem.Name = "angryToolStripMenuItem";
            this.angryToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.angryToolStripMenuItem.Text = "Злой";
            this.angryToolStripMenuItem.Click += new System.EventHandler(this.angryToolStripMenuItem_Click);
            // 
            // badMoodToolStripMenuItem
            // 
            this.badMoodToolStripMenuItem.Image = global::DennyTalk.Properties.Resources.bad_mood;
            this.badMoodToolStripMenuItem.Name = "badMoodToolStripMenuItem";
            this.badMoodToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.badMoodToolStripMenuItem.Text = "Депрессия";
            this.badMoodToolStripMenuItem.Click += new System.EventHandler(this.badMoodToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatusText);
            this.panel1.Controls.Add(this.picStatus);
            this.panel1.Controls.Add(this.lblNick);
            this.panel1.Controls.Add(this.picAvatar);
            this.panel1.Controls.Add(this.pnlStatusText);
            this.panel1.Controls.Add(this.pnlNick);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 60);
            this.panel1.TabIndex = 0;
            // 
            // lblStatusText
            // 
            this.lblStatusText.BackColor = System.Drawing.SystemColors.Control;
            this.lblStatusText.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblStatusText.Location = new System.Drawing.Point(63, 28);
            this.lblStatusText.Name = "lblStatusText";
            this.lblStatusText.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblStatusText.Size = new System.Drawing.Size(123, 17);
            this.lblStatusText.TabIndex = 3;
            this.lblStatusText.Text = "set status message";
            this.lblStatusText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label2_MouseDown);
            this.lblStatusText.MouseEnter += new System.EventHandler(this.lblStatusText_MouseEnter);
            this.lblStatusText.MouseLeave += new System.EventHandler(this.lblStatusText_MouseLeave);
            // 
            // picStatus
            // 
            this.picStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.picStatus.Image = global::DennyTalk.Properties.Resources.offline;
            this.picStatus.InitialImage = null;
            this.picStatus.Location = new System.Drawing.Point(62, 8);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(16, 16);
            this.picStatus.TabIndex = 2;
            this.picStatus.TabStop = false;
            this.picStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // lblNick
            // 
            this.lblNick.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNick.Location = new System.Drawing.Point(81, 8);
            this.lblNick.Name = "lblNick";
            this.lblNick.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.lblNick.Size = new System.Drawing.Size(108, 17);
            this.lblNick.TabIndex = 1;
            this.lblNick.Text = "lblNick";
            this.lblNick.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblNick_MouseDown);
            this.lblNick.MouseEnter += new System.EventHandler(this.lblNick_MouseEnter);
            this.lblNick.MouseLeave += new System.EventHandler(this.lblNick_MouseLeave);
            // 
            // picAvatar
            // 
            this.picAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picAvatar.Location = new System.Drawing.Point(5, 5);
            this.picAvatar.Name = "picAvatar";
            this.picAvatar.Size = new System.Drawing.Size(50, 50);
            this.picAvatar.TabIndex = 0;
            this.picAvatar.TabStop = false;
            this.picAvatar.Click += new System.EventHandler(this.picAvatar_Click);
            this.picAvatar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picAvatar_MouseDown);
            // 
            // pnlStatusText
            // 
            this.pnlStatusText.AutoSize = true;
            this.pnlStatusText.BackColor = System.Drawing.Color.White;
            this.pnlStatusText.Controls.Add(this.txtStatusText);
            this.pnlStatusText.Location = new System.Drawing.Point(63, 26);
            this.pnlStatusText.Name = "pnlStatusText";
            this.pnlStatusText.Size = new System.Drawing.Size(126, 21);
            this.pnlStatusText.TabIndex = 6;
            this.pnlStatusText.Visible = false;
            // 
            // txtStatusText
            // 
            this.txtStatusText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStatusText.Location = new System.Drawing.Point(3, 4);
            this.txtStatusText.Name = "txtStatusText";
            this.txtStatusText.Size = new System.Drawing.Size(120, 13);
            this.txtStatusText.TabIndex = 4;
            this.txtStatusText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            this.txtStatusText.Leave += new System.EventHandler(this.txtStatusText_Leave);
            // 
            // pnlNick
            // 
            this.pnlNick.AutoSize = true;
            this.pnlNick.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlNick.BackColor = System.Drawing.Color.White;
            this.pnlNick.Controls.Add(this.txtNick);
            this.pnlNick.Location = new System.Drawing.Point(81, 6);
            this.pnlNick.Name = "pnlNick";
            this.pnlNick.Padding = new System.Windows.Forms.Padding(1);
            this.pnlNick.Size = new System.Drawing.Size(109, 21);
            this.pnlNick.TabIndex = 7;
            this.pnlNick.Visible = false;
            // 
            // txtNick
            // 
            this.txtNick.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNick.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtNick.Location = new System.Drawing.Point(4, 4);
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(101, 13);
            this.txtNick.TabIndex = 5;
            this.txtNick.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNick_KeyDown);
            this.txtNick.Leave += new System.EventHandler(this.txtNick_Leave);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonConfig,
            this.toolStripButton2,
            this.toolStripButtonUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 284);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 1, 0);
            this.toolStrip1.Size = new System.Drawing.Size(198, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonConfig
            // 
            this.toolStripButtonConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonConfig.Image = global::DennyTalk.Properties.Resources.wrench;
            this.toolStripButtonConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConfig.Name = "toolStripButtonConfig";
            this.toolStripButtonConfig.Size = new System.Drawing.Size(23, 21);
            this.toolStripButtonConfig.Text = "toolStripButtonConfig";
            this.toolStripButtonConfig.Click += new System.EventHandler(this.toolStripButtonConfig_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::DennyTalk.Properties.Resources.add;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton2.Text = "toolStripButtonAddContact";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButtonAddContact_Click);
            // 
            // toolStripButtonUpdate
            // 
            this.toolStripButtonUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripButtonUpdate.ForeColor = System.Drawing.Color.Green;
            this.toolStripButtonUpdate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpdate.Image")));
            this.toolStripButtonUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpdate.Name = "toolStripButtonUpdate";
            this.toolStripButtonUpdate.Size = new System.Drawing.Size(57, 21);
            this.toolStripButtonUpdate.Text = "Update";
            this.toolStripButtonUpdate.Visible = false;
            this.toolStripButtonUpdate.Click += new System.EventHandler(this.toolStripButtonUpdate_Click);
            // 
            // popupAlertManager1
            // 
            this.popupAlertManager1.AlertAlignment = WinFormsPopupAlerts.AlertAlignment.BottomLeft;
            this.popupAlertManager1.AlertFactory = this.tooltipAlertFactory1;
            this.popupAlertManager1.AlertsMaxCount = 15;
            this.popupAlertManager1.ContainerControl = this;
            // 
            // tooltipAlertFactory1
            // 
            this.tooltipAlertFactory1.MaximumSize = new System.Drawing.Size(500, 300);
            this.tooltipAlertFactory1.MinimumSize = new System.Drawing.Size(150, 0);
            this.tooltipAlertFactory1.Padding = new System.Windows.Forms.Padding(5);
            this.tooltipAlertFactory1.AlertMouseDown += new System.EventHandler<System.Windows.Forms.MouseEventArgs>(this.tooltipAlertFactory1_AlertMouseDown);
            // 
            // contactListUserControl1
            // 
            this.contactListUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contactListUserControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.contactListUserControl1.Location = new System.Drawing.Point(0, 60);
            this.contactListUserControl1.Name = "contactListUserControl1";
            this.contactListUserControl1.Size = new System.Drawing.Size(198, 224);
            this.contactListUserControl1.TabIndex = 1;
            this.contactListUserControl1.ContactDoubleClick += new System.EventHandler<DennyTalk.ContactInfoEventArgs>(this.contactListUserControl1_ContactDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 309);
            this.Controls.Add(this.contactListUserControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DennyTalk";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAvatar)).EndInit();
            this.pnlStatusText.ResumeLayout(false);
            this.pnlStatusText.PerformLayout();
            this.pnlNick.ResumeLayout(false);
            this.pnlNick.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Panel panel1;
        private ContactListUserControl contactListUserControl1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.PictureBox picAvatar;
        private System.Windows.Forms.PictureBox picStatus;
        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.Label lblStatusText;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem onlineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offlineToolStripMenuItem;
        private System.Windows.Forms.TextBox txtStatusText;
        private System.Windows.Forms.TextBox txtNick;
        private System.Windows.Forms.Panel pnlStatusText;
        private System.Windows.Forms.Panel pnlNick;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonConfig;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem awayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem notAvaliableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem occupiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eatingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem freeForChatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atWorkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem atHomeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem angryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem badMoodToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpdate;
        private WinFormsPopupAlerts.AlertManager popupAlertManager1;
        private WinFormsPopupAlerts.TooltipAlertFactory tooltipAlertFactory1;
    }
}

