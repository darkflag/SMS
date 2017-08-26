namespace SPBrowser
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.cbCheckUpdates = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btOpenLogsFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLogTruncateFilesAfterNumberOfDays = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbLoggingLevel = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbInitializeAllProperties = new System.Windows.Forms.CheckBox();
            this.tbTechnicalData = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tbAssemblies = new System.Windows.Forms.TextBox();
            this.cbShowAllAssemblies = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbTechnicalData.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(314, 404);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(87, 27);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "&Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.Location = new System.Drawing.Point(221, 404);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(87, 27);
            this.btOk.TabIndex = 0;
            this.btOk.Text = "&OK";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbGeneral);
            this.tabControl1.Controls.Add(this.tbTechnicalData);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(389, 386);
            this.tabControl1.TabIndex = 4;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.cbCheckUpdates);
            this.tbGeneral.Controls.Add(this.groupBox2);
            this.tbGeneral.Controls.Add(this.groupBox1);
            this.tbGeneral.Location = new System.Drawing.Point(4, 24);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(381, 358);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
            // 
            // cbCheckUpdates
            // 
            this.cbCheckUpdates.AutoSize = true;
            this.cbCheckUpdates.Location = new System.Drawing.Point(13, 277);
            this.cbCheckUpdates.Name = "cbCheckUpdates";
            this.cbCheckUpdates.Size = new System.Drawing.Size(179, 19);
            this.cbCheckUpdates.TabIndex = 3;
            this.cbCheckUpdates.Text = "Check for updates on startup";
            this.cbCheckUpdates.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btOpenLogsFolder);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbLogTruncateFilesAfterNumberOfDays);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cbLoggingLevel);
            this.groupBox2.Location = new System.Drawing.Point(6, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 137);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Logging";
            // 
            // btOpenLogsFolder
            // 
            this.btOpenLogsFolder.Location = new System.Drawing.Point(7, 98);
            this.btOpenLogsFolder.Name = "btOpenLogsFolder";
            this.btOpenLogsFolder.Size = new System.Drawing.Size(131, 27);
            this.btOpenLogsFolder.TabIndex = 2;
            this.btOpenLogsFolder.Text = "Open logs folder...";
            this.btOpenLogsFolder.UseVisualStyleBackColor = true;
            this.btOpenLogsFolder.Click += new System.EventHandler(this.btOpenLogsFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "files";
            // 
            // tbLogTruncateFilesAfterNumberOfDays
            // 
            this.tbLogTruncateFilesAfterNumberOfDays.Location = new System.Drawing.Point(210, 68);
            this.tbLogTruncateFilesAfterNumberOfDays.Name = "tbLogTruncateFilesAfterNumberOfDays";
            this.tbLogTruncateFilesAfterNumberOfDays.Size = new System.Drawing.Size(47, 23);
            this.tbLogTruncateFilesAfterNumberOfDays.TabIndex = 1;
            this.tbLogTruncateFilesAfterNumberOfDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(202, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Truncate the log files after more than";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Logging level:";
            // 
            // cbLoggingLevel
            // 
            this.cbLoggingLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoggingLevel.FormattingEnabled = true;
            this.cbLoggingLevel.Location = new System.Drawing.Point(7, 41);
            this.cbLoggingLevel.Name = "cbLoggingLevel";
            this.cbLoggingLevel.Size = new System.Drawing.Size(204, 23);
            this.cbLoggingLevel.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbInitializeAllProperties);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 122);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Loading object properties";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(356, 66);
            this.label4.TabIndex = 1;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // cbInitializeAllProperties
            // 
            this.cbInitializeAllProperties.AutoSize = true;
            this.cbInitializeAllProperties.Location = new System.Drawing.Point(7, 23);
            this.cbInitializeAllProperties.Name = "cbInitializeAllProperties";
            this.cbInitializeAllProperties.Size = new System.Drawing.Size(227, 19);
            this.cbInitializeAllProperties.TabIndex = 0;
            this.cbInitializeAllProperties.Text = "Force loading all &properties on objects";
            this.cbInitializeAllProperties.UseVisualStyleBackColor = true;
            // 
            // tbTechnicalData
            // 
            this.tbTechnicalData.Controls.Add(this.groupBox4);
            this.tbTechnicalData.Location = new System.Drawing.Point(4, 24);
            this.tbTechnicalData.Name = "tbTechnicalData";
            this.tbTechnicalData.Padding = new System.Windows.Forms.Padding(3);
            this.tbTechnicalData.Size = new System.Drawing.Size(381, 358);
            this.tbTechnicalData.TabIndex = 2;
            this.tbTechnicalData.Text = "Technical Data";
            this.tbTechnicalData.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.tbAssemblies);
            this.groupBox4.Controls.Add(this.cbShowAllAssemblies);
            this.groupBox4.Location = new System.Drawing.Point(7, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(368, 345);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Loaded assemblies";
            // 
            // tbAssemblies
            // 
            this.tbAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAssemblies.Location = new System.Drawing.Point(7, 48);
            this.tbAssemblies.Multiline = true;
            this.tbAssemblies.Name = "tbAssemblies";
            this.tbAssemblies.ReadOnly = true;
            this.tbAssemblies.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbAssemblies.Size = new System.Drawing.Size(355, 291);
            this.tbAssemblies.TabIndex = 1;
            this.tbAssemblies.WordWrap = false;
            // 
            // cbShowAllAssemblies
            // 
            this.cbShowAllAssemblies.AutoSize = true;
            this.cbShowAllAssemblies.Location = new System.Drawing.Point(6, 22);
            this.cbShowAllAssemblies.Name = "cbShowAllAssemblies";
            this.cbShowAllAssemblies.Size = new System.Drawing.Size(130, 19);
            this.cbShowAllAssemblies.TabIndex = 0;
            this.cbShowAllAssemblies.Text = "Show all assemblies";
            this.cbShowAllAssemblies.UseVisualStyleBackColor = true;
            this.cbShowAllAssemblies.CheckedChanged += new System.EventHandler(this.cbShowAllAssemblies_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(413, 443);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbTechnicalData.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btOpenLogsFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLogTruncateFilesAfterNumberOfDays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbLoggingLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbInitializeAllProperties;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbCheckUpdates;
        private System.Windows.Forms.TabPage tbTechnicalData;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tbAssemblies;
        private System.Windows.Forms.CheckBox cbShowAllAssemblies;
    }
}