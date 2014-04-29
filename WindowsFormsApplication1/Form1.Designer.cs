namespace WindowsFormsApplication1
{
    partial class Form1
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
            WinFormsPopupAlerts.CornerRadius cornerRadius1 = new WinFormsPopupAlerts.CornerRadius();
            this.alertManager1 = new WinFormsPopupAlerts.AlertManager(this.components);
            this.customTooltipAlertRenderer1 = new WinFormsPopupAlerts.CustomTooltipAlertRenderer();
            this.SuspendLayout();
            // 
            // alertManager1
            // 
            this.alertManager1.AlertFactory = null;
            this.alertManager1.ContainerControl = this;
            // 
            // customTooltipAlertRenderer1
            // 
            this.customTooltipAlertRenderer1.BackColor = System.Drawing.Color.Gray;
            this.customTooltipAlertRenderer1.BorderColor = System.Drawing.Color.Black;
            this.customTooltipAlertRenderer1.BorderThickness = 1;
            cornerRadius1.BottomLeft = 0;
            cornerRadius1.BottomRight = 15;
            cornerRadius1.TopLeft = 15;
            cornerRadius1.TopRight = 0;
            this.customTooltipAlertRenderer1.CornerRadius = cornerRadius1;
            this.customTooltipAlertRenderer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.customTooltipAlertRenderer1.ForeColor = System.Drawing.Color.White;
            this.customTooltipAlertRenderer1.IconPadding = new System.Windows.Forms.Padding(3, 4, 5, 4);
            this.customTooltipAlertRenderer1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.customTooltipAlertRenderer1.TitleForeColor = System.Drawing.Color.White;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WinFormsPopupAlerts.AlertManager alertManager1;
        private WinFormsPopupAlerts.CustomTooltipAlertRenderer customTooltipAlertRenderer1;

    }
}

