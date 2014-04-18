namespace DennyTalk
{
    partial class ContactListUserControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactListUserControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewContactInfoColumn1 = new DennyTalk.DataGridViewContactInfoColumn();
            this.ColumnAvatar = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnContactInfo = new DennyTalk.DataGridViewContactInfoColumn();
            this.ColumnShowInfo = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnEdit = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColumnRemove = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnAvatar,
            this.ColumnStatus,
            this.ColumnContactInfo,
            this.ColumnShowInfo,
            this.ColumnEdit,
            this.ColumnRemove});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 34;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(261, 297);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseLeave);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellMouseEnter);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // dataGridViewContactInfoColumn1
            // 
            this.dataGridViewContactInfoColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewContactInfoColumn1.DataPropertyName = "Nick";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            this.dataGridViewContactInfoColumn1.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewContactInfoColumn1.HeaderText = "Info";
            this.dataGridViewContactInfoColumn1.Name = "dataGridViewContactInfoColumn1";
            // 
            // ColumnAvatar
            // 
            this.ColumnAvatar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnAvatar.DataPropertyName = "AvatarSmall";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle1.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle1.NullValue")));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ColumnAvatar.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnAvatar.FillWeight = 50.76142F;
            this.ColumnAvatar.HeaderText = "Avatar";
            this.ColumnAvatar.MinimumWidth = 34;
            this.ColumnAvatar.Name = "ColumnAvatar";
            this.ColumnAvatar.ReadOnly = true;
            this.ColumnAvatar.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnAvatar.Width = 34;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnStatus.DataPropertyName = "StatusImage";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle2.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle2.NullValue")));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ColumnStatus.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnStatus.FillWeight = 149.2386F;
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.MinimumWidth = 25;
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            this.ColumnStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnStatus.Width = 25;
            // 
            // ColumnContactInfo
            // 
            this.ColumnContactInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnContactInfo.DataPropertyName = "Nick";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2, 2, 0, 0);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnContactInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnContactInfo.FillWeight = 147.7957F;
            this.ColumnContactInfo.HeaderText = "Info";
            this.ColumnContactInfo.Name = "ColumnContactInfo";
            this.ColumnContactInfo.ReadOnly = true;
            // 
            // ColumnShowInfo
            // 
            this.ColumnShowInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnShowInfo.DataPropertyName = "InfoImage";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle4.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle4.NullValue")));
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ColumnShowInfo.DefaultCellStyle = dataGridViewCellStyle4;
            this.ColumnShowInfo.FillWeight = 86.22027F;
            this.ColumnShowInfo.HeaderText = "Show Info";
            this.ColumnShowInfo.Image = global::DennyTalk.Properties.Resources.user;
            this.ColumnShowInfo.MinimumWidth = 25;
            this.ColumnShowInfo.Name = "ColumnShowInfo";
            this.ColumnShowInfo.ReadOnly = true;
            this.ColumnShowInfo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnShowInfo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnShowInfo.Width = 25;
            // 
            // ColumnEdit
            // 
            this.ColumnEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnEdit.DataPropertyName = "EditImage";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle5.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle5.NullValue")));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ColumnEdit.DefaultCellStyle = dataGridViewCellStyle5;
            this.ColumnEdit.FillWeight = 81.96722F;
            this.ColumnEdit.HeaderText = "Edit";
            this.ColumnEdit.Image = global::DennyTalk.Properties.Resources.pencil;
            this.ColumnEdit.MinimumWidth = 25;
            this.ColumnEdit.Name = "ColumnEdit";
            this.ColumnEdit.ReadOnly = true;
            this.ColumnEdit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnEdit.Width = 25;
            // 
            // ColumnRemove
            // 
            this.ColumnRemove.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnRemove.DataPropertyName = "RemoveImage";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle6.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle6.NullValue")));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.ColumnRemove.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnRemove.FillWeight = 84.0168F;
            this.ColumnRemove.HeaderText = "Remove";
            this.ColumnRemove.Image = global::DennyTalk.Properties.Resources.delete;
            this.ColumnRemove.MinimumWidth = 25;
            this.ColumnRemove.Name = "ColumnRemove";
            this.ColumnRemove.ReadOnly = true;
            this.ColumnRemove.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnRemove.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnRemove.Width = 25;
            // 
            // ContactListUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "ContactListUserControl";
            this.Size = new System.Drawing.Size(261, 297);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private DataGridViewContactInfoColumn dataGridViewContactInfoColumn1;
        private System.Windows.Forms.DataGridViewImageColumn ColumnAvatar;
        private System.Windows.Forms.DataGridViewImageColumn ColumnStatus;
        private DataGridViewContactInfoColumn ColumnContactInfo;
        private System.Windows.Forms.DataGridViewImageColumn ColumnShowInfo;
        private System.Windows.Forms.DataGridViewImageColumn ColumnEdit;
        private System.Windows.Forms.DataGridViewImageColumn ColumnRemove;
    }
}
