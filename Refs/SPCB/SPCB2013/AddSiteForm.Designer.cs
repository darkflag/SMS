namespace SPBrowser
{
    partial class AddSiteForm
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
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSiteUrl = new System.Windows.Forms.TextBox();
            this.cbAuthentication = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbUseCurrentUserCredentials = new System.Windows.Forms.CheckBox();
            this.btGetUsername = new System.Windows.Forms.Button();
            this.lbWarning = new System.Windows.Forms.Label();
            this.gbAuthenticationDetails = new System.Windows.Forms.GroupBox();
            this.cbRememberPassword = new System.Windows.Forms.CheckBox();
            this.lbLastConnectionTime = new System.Windows.Forms.Label();
            this.btRemove = new System.Windows.Forms.Button();
            this.gbAuthenticationDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(643, 239);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 26);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(548, 239);
            this.btOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(87, 26);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "&OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(579, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL of site collection (https://company.sharepoint.com or https://company.sharepo" +
    "int.com/sites/teamsite):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Authentication mode:";
            // 
            // tbSiteUrl
            // 
            this.tbSiteUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSiteUrl.Location = new System.Drawing.Point(12, 27);
            this.tbSiteUrl.Name = "tbSiteUrl";
            this.tbSiteUrl.Size = new System.Drawing.Size(718, 23);
            this.tbSiteUrl.TabIndex = 0;
            this.tbSiteUrl.TextChanged += new System.EventHandler(this.tbSiteUrl_TextChanged);
            // 
            // cbAuthentication
            // 
            this.cbAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAuthentication.FormattingEnabled = true;
            this.cbAuthentication.Items.AddRange(new object[] {
            "Windows Authentication (NTLM or Kerberos)",
            "SharePoint Online (Office 365)",
            "Anonymous",
            "Forms Based",
            "Claims (AD FS)"});
            this.cbAuthentication.Location = new System.Drawing.Point(132, 28);
            this.cbAuthentication.Name = "cbAuthentication";
            this.cbAuthentication.Size = new System.Drawing.Size(580, 23);
            this.cbAuthentication.TabIndex = 0;
            this.cbAuthentication.SelectedIndexChanged += new System.EventHandler(this.cbAuthentication_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Username:";
            // 
            // tbUsername
            // 
            this.tbUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUsername.Location = new System.Drawing.Point(132, 82);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(437, 23);
            this.tbUsername.TabIndex = 2;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(132, 111);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(437, 23);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // cbUseCurrentUserCredentials
            // 
            this.cbUseCurrentUserCredentials.AutoSize = true;
            this.cbUseCurrentUserCredentials.Checked = true;
            this.cbUseCurrentUserCredentials.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseCurrentUserCredentials.Location = new System.Drawing.Point(132, 57);
            this.cbUseCurrentUserCredentials.Name = "cbUseCurrentUserCredentials";
            this.cbUseCurrentUserCredentials.Size = new System.Drawing.Size(171, 19);
            this.cbUseCurrentUserCredentials.TabIndex = 1;
            this.cbUseCurrentUserCredentials.Text = "Use current user credentials";
            this.cbUseCurrentUserCredentials.UseVisualStyleBackColor = true;
            this.cbUseCurrentUserCredentials.CheckedChanged += new System.EventHandler(this.cbUseCurrentUserCredentials_CheckedChanged);
            // 
            // btGetUsername
            // 
            this.btGetUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btGetUsername.Location = new System.Drawing.Point(575, 81);
            this.btGetUsername.Name = "btGetUsername";
            this.btGetUsername.Size = new System.Drawing.Size(124, 23);
            this.btGetUsername.TabIndex = 5;
            this.btGetUsername.Text = "Get your username";
            this.btGetUsername.UseVisualStyleBackColor = true;
            this.btGetUsername.Click += new System.EventHandler(this.btGetUsername_Click);
            // 
            // lbWarning
            // 
            this.lbWarning.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbWarning.Image = global::SPBrowser.Properties.Resources.Warning;
            this.lbWarning.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lbWarning.Location = new System.Drawing.Point(108, 239);
            this.lbWarning.Name = "lbWarning";
            this.lbWarning.Size = new System.Drawing.Size(396, 32);
            this.lbWarning.TabIndex = 8;
            this.lbWarning.Text = "       Warning: initialize all object properties is enabled! This requires a high" +
    "        level of permissions. Possibly an \"Access Denied\"-message is shown.";
            // 
            // gbAuthenticationDetails
            // 
            this.gbAuthenticationDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAuthenticationDetails.Controls.Add(this.cbRememberPassword);
            this.gbAuthenticationDetails.Controls.Add(this.lbLastConnectionTime);
            this.gbAuthenticationDetails.Controls.Add(this.cbAuthentication);
            this.gbAuthenticationDetails.Controls.Add(this.label2);
            this.gbAuthenticationDetails.Controls.Add(this.btGetUsername);
            this.gbAuthenticationDetails.Controls.Add(this.tbUsername);
            this.gbAuthenticationDetails.Controls.Add(this.cbUseCurrentUserCredentials);
            this.gbAuthenticationDetails.Controls.Add(this.tbPassword);
            this.gbAuthenticationDetails.Controls.Add(this.label4);
            this.gbAuthenticationDetails.Controls.Add(this.label3);
            this.gbAuthenticationDetails.Location = new System.Drawing.Point(12, 56);
            this.gbAuthenticationDetails.Name = "gbAuthenticationDetails";
            this.gbAuthenticationDetails.Size = new System.Drawing.Size(718, 167);
            this.gbAuthenticationDetails.TabIndex = 1;
            this.gbAuthenticationDetails.TabStop = false;
            this.gbAuthenticationDetails.Text = "Connection Details";
            // 
            // cbRememberPassword
            // 
            this.cbRememberPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRememberPassword.AutoSize = true;
            this.cbRememberPassword.Location = new System.Drawing.Point(575, 113);
            this.cbRememberPassword.Name = "cbRememberPassword";
            this.cbRememberPassword.Size = new System.Drawing.Size(137, 19);
            this.cbRememberPassword.TabIndex = 4;
            this.cbRememberPassword.Text = "Remember password";
            this.cbRememberPassword.UseVisualStyleBackColor = true;
            // 
            // lbLastConnectionTime
            // 
            this.lbLastConnectionTime.AutoSize = true;
            this.lbLastConnectionTime.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lbLastConnectionTime.Location = new System.Drawing.Point(6, 149);
            this.lbLastConnectionTime.Name = "lbLastConnectionTime";
            this.lbLastConnectionTime.Size = new System.Drawing.Size(229, 15);
            this.lbLastConnectionTime.TabIndex = 7;
            this.lbLastConnectionTime.Text = "Last connection: September 15, 2014 08:26";
            this.lbLastConnectionTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btRemove
            // 
            this.btRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRemove.Location = new System.Drawing.Point(15, 239);
            this.btRemove.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btRemove.Name = "btRemove";
            this.btRemove.Size = new System.Drawing.Size(87, 26);
            this.btRemove.TabIndex = 1;
            this.btRemove.Text = "&Remove";
            this.btRemove.UseVisualStyleBackColor = true;
            this.btRemove.Click += new System.EventHandler(this.btRemove_Click);
            // 
            // AddSiteForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(744, 280);
            this.Controls.Add(this.gbAuthenticationDetails);
            this.Controls.Add(this.lbWarning);
            this.Controls.Add(this.tbSiteUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btRemove);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddSiteForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Site Collection";
            this.Load += new System.EventHandler(this.AddSite_Load);
            this.gbAuthenticationDetails.ResumeLayout(false);
            this.gbAuthenticationDetails.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSiteUrl;
        private System.Windows.Forms.ComboBox cbAuthentication;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbUseCurrentUserCredentials;
        private System.Windows.Forms.Button btGetUsername;
        private System.Windows.Forms.Label lbWarning;
        private System.Windows.Forms.GroupBox gbAuthenticationDetails;
        private System.Windows.Forms.Label lbLastConnectionTime;
        private System.Windows.Forms.CheckBox cbRememberPassword;
        private System.Windows.Forms.Button btRemove;
    }
}