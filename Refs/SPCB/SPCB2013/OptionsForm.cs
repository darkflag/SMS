using SPBrowser.Utils;
using SPBrowser.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPBrowser
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            InitializeControls();
            LoadConfiguration();
            LoadAssemblies();
        }

        private void cbShowAllAssemblies_CheckedChanged(object sender, EventArgs e)
        {
            this.LoadAssemblies();
        }

        private void btOpenLogsFolder_Click(object sender, EventArgs e)
        {
            LogUtil.OpenLogsFolderLocation();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConfiguration();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                LogUtil.LogException("Error while saving configuration.", ex);
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeControls()
        {
            // Initialize the log levels
            foreach (string value in Enum.GetNames(typeof(LogLevel)))
                cbLoggingLevel.Items.Add(value);
        }

        private void LoadAssemblies()
        {
            tbAssemblies.Text = string.Empty;

            foreach (AssemblyInfo assemblyInfo in AssemblyInfo.GetAssemblies().OrderBy(a => a.FullName))
            {
                if (cbShowAllAssemblies.Checked || assemblyInfo.PublicKeyToken.Equals(Constants.SHAREPOINT_CLIENT_PUBLIC_KEY_TOKEN))
                {
                    tbAssemblies.Text += string.Format("{1}{0} Version: {3}{0} Location: {2}{0}{0}",
                        Environment.NewLine,
                        assemblyInfo.FullName,
                        string.IsNullOrEmpty(assemblyInfo.Location) ? "n/a" : assemblyInfo.Location,
                        assemblyInfo.FileVersion == null ? "n/a" : assemblyInfo.FileVersion.ToString());
                }
            }
        }

        private void LoadConfiguration()
        {
            cbInitializeAllProperties.Checked = Configuration.Current.LoadAllProperties;
            tbLogTruncateFilesAfterNumberOfDays.Text = Configuration.Current.LogTruncateAfterNumberOfFiles.ToString();
            cbLoggingLevel.SelectedItem = Configuration.Current.LogLevel.ToString();
            cbCheckUpdates.Checked = Configuration.Current.CheckUpdatesOnStartup;

            LogUtil.LogMessage("Configuration loaded", LogLevel.Information, 0, LogCategory.General);
        }

        private void SaveConfiguration()
        {
            // Update runtime configuration
            Configuration.Current.LoadAllProperties = cbInitializeAllProperties.Checked;
            Configuration.Current.LogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), cbLoggingLevel.SelectedItem.ToString());

            // Save configuration to app.config
            Configuration.Current.LogTruncateAfterNumberOfFiles = ConvertToInteger(tbLogTruncateFilesAfterNumberOfDays.Text.Trim());
            Configuration.Current.CheckUpdatesOnStartup = cbCheckUpdates.Checked;

            LogUtil.LogMessage("Configuration saved", LogLevel.Information, 0, LogCategory.General);
        }

        private static int ConvertToInteger(string value)
        {
            int configValue = -1;
            if (int.TryParse(value, out configValue))
                return configValue;
            else
                throw new ArgumentException(string.Format("Incorrect number value for '{0}', please provide a valid number.", value));
        }
    }
}
