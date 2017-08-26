namespace SPBrowser
{
    partial class WhatsNewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WhatsNewForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.tbReleaseNotes = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbProductName = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.llCodeplex = new System.Windows.Forms.LinkLabel();
            this.llTwitter = new System.Windows.Forms.LinkLabel();
            this.llWeb = new System.Windows.Forms.LinkLabel();
            this.btnMoreInfo = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(362, 402);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // tbReleaseNotes
            // 
            this.tbReleaseNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbReleaseNotes.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReleaseNotes.Location = new System.Drawing.Point(12, 97);
            this.tbReleaseNotes.Multiline = true;
            this.tbReleaseNotes.Name = "tbReleaseNotes";
            this.tbReleaseNotes.ReadOnly = true;
            this.tbReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbReleaseNotes.Size = new System.Drawing.Size(437, 266);
            this.tbReleaseNotes.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbProductName);
            this.panel1.Controls.Add(this.pbIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 90);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(54, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "New in this release!";
            // 
            // lbProductName
            // 
            this.lbProductName.AutoSize = true;
            this.lbProductName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProductName.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbProductName.Location = new System.Drawing.Point(58, 16);
            this.lbProductName.Name = "lbProductName";
            this.lbProductName.Size = new System.Drawing.Size(157, 17);
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
            // llCodeplex
            // 
            this.llCodeplex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llCodeplex.AutoSize = true;
            this.llCodeplex.Location = new System.Drawing.Point(9, 414);
            this.llCodeplex.Name = "llCodeplex";
            this.llCodeplex.Size = new System.Drawing.Size(144, 15);
            this.llCodeplex.TabIndex = 2;
            this.llCodeplex.TabStop = true;
            this.llCodeplex.Text = "http://spcb.codeplex.com";
            this.llCodeplex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCodeplex_LinkClicked);
            // 
            // llTwitter
            // 
            this.llTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llTwitter.AutoSize = true;
            this.llTwitter.Location = new System.Drawing.Point(9, 395);
            this.llTwitter.Name = "llTwitter";
            this.llTwitter.Size = new System.Drawing.Size(85, 15);
            this.llTwitter.TabIndex = 1;
            this.llTwitter.TabStop = true;
            this.llTwitter.Text = "@bramdejager";
            this.llTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llTwitter_LinkClicked);
            // 
            // llWeb
            // 
            this.llWeb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llWeb.AutoSize = true;
            this.llWeb.Location = new System.Drawing.Point(9, 376);
            this.llWeb.Name = "llWeb";
            this.llWeb.Size = new System.Drawing.Size(193, 15);
            this.llWeb.TabIndex = 0;
            this.llWeb.TabStop = true;
            this.llWeb.Text = "http://bramdejager.wordpress.com";
            this.llWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llWeb_LinkClicked);
            // 
            // btnMoreInfo
            // 
            this.btnMoreInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoreInfo.Location = new System.Drawing.Point(362, 369);
            this.btnMoreInfo.Name = "btnMoreInfo";
            this.btnMoreInfo.Size = new System.Drawing.Size(87, 27);
            this.btnMoreInfo.TabIndex = 3;
            this.btnMoreInfo.Text = "More &Info";
            this.btnMoreInfo.UseVisualStyleBackColor = true;
            this.btnMoreInfo.Click += new System.EventHandler(this.btnMoreInfo_Click);
            // 
            // WhatsNewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(461, 441);
            this.Controls.Add(this.llCodeplex);
            this.Controls.Add(this.llTwitter);
            this.Controls.Add(this.llWeb);
            this.Controls.Add(this.tbReleaseNotes);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnMoreInfo);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WhatsNewForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "What\'s new in this release?";
            this.Load += new System.EventHandler(this.WhatsNewForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbReleaseNotes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbProductName;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel llCodeplex;
        private System.Windows.Forms.LinkLabel llTwitter;
        private System.Windows.Forms.LinkLabel llWeb;
        private System.Windows.Forms.Button btnMoreInfo;
    }
}