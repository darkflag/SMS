namespace SPBrowser
{
    partial class AboutForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbProductName = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lbProductInfo = new System.Windows.Forms.Label();
            this.btExit = new System.Windows.Forms.Button();
            this.llWeb = new System.Windows.Forms.LinkLabel();
            this.llTwitter = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbReleaseNotes = new System.Windows.Forms.TextBox();
            this.llCodeplex = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel1.Controls.Add(this.lbProductName);
            this.panel1.Controls.Add(this.pbIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 65);
            this.panel1.TabIndex = 0;
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProductName.Location = new System.Drawing.Point(58, 16);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(289, 32);
            this.lbProductName.TabIndex = 2;
            this.lbProductName.Text = "SharePoint Client Browser";
            // 
            // pbIcon
            // 
            this.pbIcon.Image = global::SPBrowser.Properties.Resources.SharePoint;
            this.pbIcon.Location = new System.Drawing.Point(14, 16);
            this.pbIcon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(32, 32);
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            // 
            // lbProductInfo
            // 
            this.lbProductInfo.AutoSize = true;
            this.lbProductInfo.Location = new System.Drawing.Point(12, 81);
            this.lbProductInfo.Name = "lbProductInfo";
            this.lbProductInfo.Size = new System.Drawing.Size(189, 17);
            this.lbProductInfo.TabIndex = 1;
            this.lbProductInfo.Text = "SharePoint 2013 Client Browser";
            // 
            // btExit
            // 
            this.btExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btExit.Location = new System.Drawing.Point(540, 381);
            this.btExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(87, 30);
            this.btExit.TabIndex = 2;
            this.btExit.Text = "&OK";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // llWeb
            // 
            this.llWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llWeb.AutoSize = true;
            this.llWeb.Location = new System.Drawing.Point(12, 388);
            this.llWeb.Name = "llWeb";
            this.llWeb.Size = new System.Drawing.Size(213, 17);
            this.llWeb.TabIndex = 3;
            this.llWeb.TabStop = true;
            this.llWeb.Text = "http://bramdejager.wordpress.com";
            this.llWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWeb_LinkClicked);
            // 
            // llTwitter
            // 
            this.llTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llTwitter.AutoSize = true;
            this.llTwitter.Location = new System.Drawing.Point(231, 388);
            this.llTwitter.Name = "llTwitter";
            this.llTwitter.Size = new System.Drawing.Size(96, 17);
            this.llTwitter.TabIndex = 4;
            this.llTwitter.TabStop = true;
            this.llTwitter.Text = "@bramdejager";
            this.llTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llTwitter_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Release Notes:";
            // 
            // tbReleaseNotes
            // 
            this.tbReleaseNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReleaseNotes.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReleaseNotes.Location = new System.Drawing.Point(14, 177);
            this.tbReleaseNotes.Multiline = true;
            this.tbReleaseNotes.Name = "tbReleaseNotes";
            this.tbReleaseNotes.ReadOnly = true;
            this.tbReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReleaseNotes.Size = new System.Drawing.Size(613, 197);
            this.tbReleaseNotes.TabIndex = 7;
            // 
            // llCodeplex
            // 
            this.llCodeplex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llCodeplex.AutoSize = true;
            this.llCodeplex.Location = new System.Drawing.Point(333, 388);
            this.llCodeplex.Name = "llCodeplex";
            this.llCodeplex.Size = new System.Drawing.Size(156, 17);
            this.llCodeplex.TabIndex = 4;
            this.llCodeplex.TabStop = true;
            this.llCodeplex.Text = "http://spcb.codeplex.com";
            this.llCodeplex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCodeplex_LinkClicked);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btExit;
            this.ClientSize = new System.Drawing.Size(642, 427);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbReleaseNotes);
            this.Controls.Add(this.llCodeplex);
            this.Controls.Add(this.llTwitter);
            this.Controls.Add(this.llWeb);
            this.Controls.Add(this.btExit);
            this.Controls.Add(this.lbProductInfo);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SharePoint Client Browser";
            this.Load += new System.EventHandler(this.About_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lbProductInfo;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.LinkLabel llWeb;
        private System.Windows.Forms.LinkLabel llTwitter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbReleaseNotes;
        private System.Windows.Forms.LinkLabel llCodeplex;
    }
}