namespace SPBrowser
{
    partial class AddWeb
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
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.tbSiteUrl = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbWebUrl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(486, 61);
            this.btOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(87, 26);
            this.btOk.TabIndex = 6;
            this.btOk.Text = "&OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(579, 61);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 26);
            this.btCancel.TabIndex = 7;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // tbSiteUrl
            // 
            this.tbSiteUrl.Location = new System.Drawing.Point(46, 12);
            this.tbSiteUrl.Name = "tbSiteUrl";
            this.tbSiteUrl.ReadOnly = true;
            this.tbSiteUrl.Size = new System.Drawing.Size(261, 23);
            this.tbSiteUrl.TabIndex = 8;
            this.tbSiteUrl.Text = "https://.../";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 15);
            this.label1.TabIndex = 9;
            this.label1.Text = "URL";
            // 
            // tbWebUrl
            // 
            this.tbWebUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWebUrl.Location = new System.Drawing.Point(313, 12);
            this.tbWebUrl.Name = "tbWebUrl";
            this.tbWebUrl.Size = new System.Drawing.Size(352, 23);
            this.tbWebUrl.TabIndex = 10;
            this.tbWebUrl.Text = "/";
            // 
            // AddWeb
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(678, 100);
            this.Controls.Add(this.tbWebUrl);
            this.Controls.Add(this.tbSiteUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddWeb";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Web";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox tbSiteUrl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbWebUrl;
    }
}