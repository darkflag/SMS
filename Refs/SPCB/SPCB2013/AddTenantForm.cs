using SPBrowser.Entities;
using SPBrowser.Extentions;
using SPBrowser.Properties;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SPBrowser
{
    public partial class AddTenantForm : Form
    {
        private const string EXAMPLE_USERNAME_NOT_APPLICABLE = "";
        private const string EXAMPLE_USERNAME_DEFAULT = "DOMAIN\\username or click \"Get your username\"";
        private const string EXAMPLE_USERNAME_SHAREPOINT_ONLINE = "username@company.onmicrosoft.com or click \"Get your username\"";
        private const string LAST_CONNECTION_PREFIX = "Last connection:";

        private TenantAuthentication _tenant;

        public AddTenantForm()
        {
            InitializeComponent();

            InitializeForm();
        }

        public AddTenantForm(TenantAuthentication tenant)
        {
            InitializeComponent();

            _tenant = tenant;

            InitializeForm();
        }

        private void AddTenant_Load(object sender, EventArgs e)
        {
            lbWarning.Visible = Configuration.Current.LoadAllProperties;

            tbTenantAdminUrl.SetCueBanner("https://company-admin.sharepoint.com");

#if !CLIENTSDKV161 
            // Disable the use of Office 365 Tenants within the SharePoint Server 201X Client Browser.
            if (MessageBox.Show(string.Format("The {0} does not support Office 365 Tenants, please use the SharePoint Online Client Browser which supports the latest CSOM version supporting Office 365 and SharePoint Online.\n\n Would you like to download this now?", Application.ProductName), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(Constants.CODEPLEX_PROJECT_URL);
            }
            this.Close();
#endif
        }

        private void tbTenantAdminUrl_TextChanged(object sender, EventArgs e)
        {
            // Set authentication to SharePoint Online when URL textbox contains SharePoint Online URL
            if (tbTenantAdminUrl.Text.ToLower().Contains("sharepoint.com"))
                cbAuthentication.SelectedIndex = (int)AuthenticationMode.SharePointOnline;
        }

        private void cbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetVisibilityForAuthentication();
        }

        private void cbUseCurrentUserCredentials_CheckedChanged(object sender, EventArgs e)
        {
            SetVisibilityForAuthentication();
        }

        private void btGetUsername_Click(object sender, EventArgs e)
        {
            switch (cbAuthentication.SelectedIndex)
            {
                case (int)AuthenticationMode.Default:
                    tbUsername.Text = string.Format("{0}\\{1}", Environment.UserDomainName, Environment.UserName);
                    break;
                case (int)AuthenticationMode.SharePointOnline:
                    tbUsername.Text = System.DirectoryServices.AccountManagement.UserPrincipal.Current.UserPrincipalName;
                    break;
                default:
                    break;
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            // Disable buttons
            ToggleButtons(false);

            // Do the work
            UpdateTenant();
            LoadTenant();
            SavePassword();
        }

        private void UpdateTenant()
        {
            if (_tenant == null)
            {
                return;
            }

            if (cbUseCurrentUserCredentials.Checked)
            {
                _tenant.UserName = string.Empty;
                _tenant.Password = string.Empty;
            }
            else
            {
                _tenant.UserName = tbUsername.Text.Trim();
                _tenant.Password = tbUsername.Text.Trim();
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            this.RemoveTenant();
        }

        private void InitializeForm()
        {
            try
            {
                if (_tenant == null)
                {
                    cbAuthentication.Enabled = false;
                    cbAuthentication.SelectedIndex = (int)AuthenticationMode.SharePointOnline;

                    btRemove.Enabled = false;

                    lbLastConnectionTime.Text = string.Format("{0} no data", LAST_CONNECTION_PREFIX);
                }
                else
                {
                    tbTenantAdminUrl.Enabled = false;
                    tbTenantAdminUrl.Text = _tenant.AdminUrlAsString;
                    cbAuthentication.Enabled = false;
                    cbAuthentication.SelectedIndex = (int)AuthenticationMode.SharePointOnline;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Text = _tenant.UserName;
                    lbLastConnectionTime.Text = string.Format("{0} {1} {2}", LAST_CONNECTION_PREFIX, _tenant.LoadDate.ToLongDateString(), _tenant.LoadDate.ToShortTimeString());
                    
                    // Try to retrieve credentials
                    string password;
                    if (CredManUtil.TryGetPassword(_tenant.AdminUrlAsString, out password, StorageType.Tenant))
                    {
                        tbPassword.Text = password;
                        cbRememberPassword.Checked = true;
                    }
                    else
                    {
                        tbPassword.Select();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void SetVisibilityForAuthentication()
        {
            // Set default visibility
            cbUseCurrentUserCredentials.Enabled = true;
            tbUsername.Enabled = true;
            tbPassword.Enabled = true;
            btGetUsername.Enabled = true;

            // Clear input from controls
            tbUsername.Text = string.Empty;
            tbPassword.Text = string.Empty;

            switch ((AuthenticationMode)cbAuthentication.SelectedIndex)
            {
                case AuthenticationMode.Default:
                    cbUseCurrentUserCredentials.Enabled = true;
                    tbUsername.Enabled = !cbUseCurrentUserCredentials.Checked;
                    tbPassword.Enabled = !cbUseCurrentUserCredentials.Checked;
                    btGetUsername.Enabled = !cbUseCurrentUserCredentials.Checked;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_DEFAULT);
                    break;
                case AuthenticationMode.SharePointOnline:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    btGetUsername.Enabled = true;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_SHAREPOINT_ONLINE);
                    break;
                case AuthenticationMode.Anonymous:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = false;
                    tbPassword.Enabled = false;
                    btGetUsername.Enabled = false;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_NOT_APPLICABLE);
                    break;
                case AuthenticationMode.Forms:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    btGetUsername.Enabled = false;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_NOT_APPLICABLE);
                    break;
                case AuthenticationMode.Claims:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = false;
                    tbPassword.Enabled = false;
                    btGetUsername.Enabled = false;
                    cbRememberPassword.Enabled = false;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_NOT_APPLICABLE);
                    break;
                default:
                    break;
            }
        }

        private void LoadTenant()
        {
            try
            {
                // Validate input
                Regex regex = new Regex(@"((\w+:\/\/)[-a-zA-Z0-9:@;?&=\/%\+\.\*!'\(\),\$_\{\}\^~\[\]`#|]+)");
                if (!regex.IsMatch(tbTenantAdminUrl.Text.Trim()) || tbTenantAdminUrl.Text.Contains("_layouts"))
                {
                    MessageBox.Show("Please enter a valid tenant admin URL, like https://company-admin.sharepoint.com.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add new tenant
                Globals.Tenants.Add(new Uri(tbTenantAdminUrl.Text), tbUsername.Text.Trim(), tbPassword.Text.Trim());

                // Close dialog
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                // Handle the file not found exception "The specified module could not be found. (Exception from HRESULT: 0x8007007E)",
                // caused by not having installed the SharePoint Client Components. 
                string message = string.Format(Resources.ErrorMissingAssemblySDK, ex.Message, HelpUtil.GetSDKDownloadTitle(), Application.ProductName);

                ToggleButtons(true);
                if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(HelpUtil.GetSDKDownloadUrl());
                }

                LogUtil.LogException(ex);
            }
            catch (Exception ex)
            {
                ToggleButtons(true);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void RemoveTenant()
        {
            if (MessageBox.Show($"Do you want to remove {_tenant.AdminUrl} from the Recent Tenants?", "Remove from Recent Tenants", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Globals.Tenants.RemoveAll(t => t.AdminUrlAsString == _tenant.AdminUrlAsString);

                // Close dialog
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SavePassword()
        {
            try
            {
                if (cbRememberPassword.Checked)
                {
                    // Save credentials
                    CredManUtil.SaveCredentials(tbTenantAdminUrl.Text.Trim(), tbUsername.Text.Trim(), tbPassword.Text.Trim(), StorageType.Tenant);
                }
                else
                {
                    // Delete credentials
                    CredManUtil.DeleteCredentials(tbTenantAdminUrl.Text.Trim(), StorageType.Tenant);
                }
            }
            catch (Exception ex)
            {
                ToggleButtons(true);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void ToggleButtons(bool enable)
        {
            btOk.Enabled = enable;
            btCancel.Enabled = enable;
        }
    }
}
