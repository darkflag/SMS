namespace SPBrowser
{
    partial class SplashScreen
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
            this.lbProductInfo = new System.Windows.Forms.Label();
            this.llWeb = new System.Windows.Forms.LinkLabel();
            this.llTwitter = new System.Windows.Forms.LinkLabel();
            this.lbStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbProductInfo
            // 
            this.lbProductInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbProductInfo.BackColor = System.Drawing.Color.Transparent;
            this.lbProductInfo.ForeColor = System.Drawing.Color.White;
            this.lbProductInfo.Location = new System.Drawing.Point(12, 192);
            this.lbProductInfo.Name = "lbProductInfo";
            this.lbProductInfo.Size = new System.Drawing.Size(515, 56);
            this.lbProductInfo.TabIndex = 1;
            this.lbProductInfo.Text = "SharePoint 2013 Client Browser by Bram de Jager";
            this.lbProductInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // llWeb
            // 
            this.llWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llWeb.BackColor = System.Drawing.Color.Transparent;
            this.llWeb.LinkColor = System.Drawing.Color.White;
            this.llWeb.Location = new System.Drawing.Point(224, 9);
            this.llWeb.Name = "llWeb";
            this.llWeb.Size = new System.Drawing.Size(303, 17);
            this.llWeb.TabIndex = 3;
            this.llWeb.TabStop = true;
            this.llWeb.Text = "http://bramdejager.wordpress.com";
            this.llWeb.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWeb_LinkClicked);
            // 
            // llTwitter
            // 
            this.llTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llTwitter.BackColor = System.Drawing.Color.Transparent;
            this.llTwitter.LinkColor = System.Drawing.Color.White;
            this.llTwitter.Location = new System.Drawing.Point(274, 26);
            this.llTwitter.Name = "llTwitter";
            this.llTwitter.Size = new System.Drawing.Size(253, 21);
            this.llTwitter.TabIndex = 4;
            this.llTwitter.TabStop = true;
            this.llTwitter.Text = "@bramdejager";
            this.llTwitter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.llTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llTwitter_LinkClicked);
            // 
            // lbStatus
            // 
            this.lbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStatus.AutoSize = true;
            this.lbStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.White;
            this.lbStatus.Location = new System.Drawing.Point(40, 156);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(57, 13);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "Starting...";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.BackgroundImage = global::SPBrowser.Properties.Resources.SplashScreenSharePoint2013;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(539, 257);
            this.ControlBox = false;
            this.Controls.Add(this.llTwitter);
            this.Controls.Add(this.llWeb);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbProductInfo);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashScreen";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Splashscreen SharePoint Client Browser";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbProductInfo;
        private System.Windows.Forms.LinkLabel llWeb;
        private System.Windows.Forms.LinkLabel llTwitter;
        private System.Windows.Forms.Label lbStatus;
    }
}