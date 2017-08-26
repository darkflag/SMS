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
    public partial class AddSiteForm : Form
    {
        private const string EXAMPLE_USERNAME_NOT_APPLICABLE = "";
        private const string EXAMPLE_USERNAME_DEFAULT = "DOMAIN\\username or click \"Get your username\"";
        private const string EXAMPLE_USERNAME_SHAREPOINT_ONLINE = "username@company.onmicrosoft.com or click \"Get your username\"";
        private const string LAST_CONNECTION_PREFIX = "Last connection:";

        private SiteAuthentication _site;

        public AddSiteForm()
        {
            InitializeComponent();

            this.InitializeForm();
        }

        public AddSiteForm(SiteAuthentication site)
        {
            InitializeComponent();

            _site = site;

            InitializeForm();
        }

        private void AddSite_Load(object sender, EventArgs e)
        {
            lbWarning.Visible = Configuration.Current.LoadAllProperties;

            tbSiteUrl.SetCueBanner("https://company.sharepoint.com or https://company.sharepoint.com/sites/teamsite");
        }

        private void tbSiteUrl_TextChanged(object sender, EventArgs e)
        {
            // Set authentication to SharePoint Online when URL textbox contains SharePoint Online URL
            if (tbSiteUrl.Text.ToLower().Contains("sharepoint.com"))
            {
                cbAuthentication.SelectedIndex = (int)AuthenticationMode.SharePointOnline;
            }
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
            UpdateSite();
            LoadSite();
            SavePassword();
        }

        private void UpdateSite()
        {
            if (_site == null)
            {
                return;
            }

            _site.Authentication = (AuthenticationMode)cbAuthentication.SelectedIndex;
            if (cbUseCurrentUserCredentials.Checked)
            {
                _site.UserName = string.Empty;
                _site.Password = string.Empty;
            }
            else
            {
                _site.UserName = tbUsername.Text.Trim();
                _site.Password = tbUsername.Text.Trim();
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            this.RemoveSite();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeForm()
        {
            try
            {
                if (_site == null)
                {
                    cbAuthentication.SelectedIndex = (int)AuthenticationMode.Default;
                    lbLastConnectionTime.Text = string.Format("{0} no data", LAST_CONNECTION_PREFIX);

                    btRemove.Enabled = false;
                }
                else
                {
                    tbSiteUrl.Enabled = false;
                    tbSiteUrl.Text = _site.UrlAsString;
                    cbAuthentication.Enabled = false;
                    cbAuthentication.SelectedIndex = (int)_site.Authentication;
                    cbUseCurrentUserCredentials.Checked = _site.UseCurrentCredentials;
                    tbUsername.Text = _site.UserName;
                    lbLastConnectionTime.Text = string.Format("{0} {1} {2}", LAST_CONNECTION_PREFIX, _site.LoadDate.ToLongDateString(), _site.LoadDate.ToShortTimeString());

                    if (_site.UseCurrentCredentials)
                    {
                        tbUsername.Text = string.Empty;
                        tbPassword.Text = string.Empty;
                    }
                    else
                    {
                        // Try to retrieve credentials
                        string password;
                        if (CredManUtil.TryGetPassword(_site.UrlAsString, out password, StorageType.SiteCollection))
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
            cbRememberPassword.Enabled = true;

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
                    cbRememberPassword.Enabled = !cbUseCurrentUserCredentials.Checked;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_DEFAULT);
                    break;
                case AuthenticationMode.SharePointOnline:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    btGetUsername.Enabled = true;
                    cbRememberPassword.Enabled = true;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_SHAREPOINT_ONLINE);
                    break;
                case AuthenticationMode.Anonymous:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = false;
                    tbPassword.Enabled = false;
                    btGetUsername.Enabled = false;
                    cbRememberPassword.Enabled = false;
                    tbUsername.SetCueBanner(EXAMPLE_USERNAME_NOT_APPLICABLE);
                    break;
                case AuthenticationMode.Forms:
                    cbUseCurrentUserCredentials.Enabled = false;
                    cbUseCurrentUserCredentials.Checked = false;
                    tbUsername.Enabled = true;
                    tbPassword.Enabled = true;
                    btGetUsername.Enabled = false;
                    cbRememberPassword.Enabled = true;
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

        private void LoadSite()
        {
            try
            {
                // Validate input
                Regex regex = new Regex(@"((\w+:\/\/)[-a-zA-Z0-9:@;?&=\/%\+\.\*!'\(\),\$_\{\}\^~\[\]`#|]+)");
                if (!regex.IsMatch(tbSiteUrl.Text.Trim()) || tbSiteUrl.Text.Contains("_layouts"))
                {
                    MessageBox.Show("Please enter a valid site collection URL, like https://company.sharepoint.com or https://company.sharepoint.com/sites/teamsite.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add new site collection
                AuthenticationMode authentication = (AuthenticationMode)cbAuthentication.SelectedIndex;
                SiteAuthentication site = Globals.Sites.Add(new Uri(tbSiteUrl.Text), tbUsername.Text.Trim(), tbPassword.Text.Trim(), authentication);

                // Warning on mismatch SPCB and server build version
                Server server = null;
                if (!ProductUtil.DoesServerBuildVersionMatchThisRelease(site.BuildVersion, out server))
                {
                    if (MessageBox.Show($"The site you added is incompatible with this release of SharePoint Client Browser.\n\nThe site seems to be hosted on {server.ProductFullname} ({server.BuildVersion}) which is compatible with '{server.CompatibleRelease}', you can download this from {Constants.CODEPLEX_PROJECT_URL}.\n\nDo you want to download the '{server.CompatibleRelease}' release now?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Constants.CODEPLEX_PROJECT_URL);
                    }
                }

                // Close dialog
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (System.IO.FileNotFoundException ex)
            {
                // Handle the file not found exception "The specified module could not be found. (Exception from HRESULT: 0x8007007E)",
                // caused by not having installed the SharePoint Client Components
                string message = string.Format(Resources.ErrorMissingAssemblySDK, ex.Message, HelpUtil.GetSDKDownloadTitle(), Application.ProductName);

                ToggleButtons(true);
                if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
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

        private void RemoveSite()
        {
            if (MessageBox.Show($"Do you want to remove {_site.Url} from the Recent Sites?", "Remove from Recent Sites", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Globals.Sites.RemoveAll(s => s.UrlAsString == _site.UrlAsString);

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
                    CredManUtil.SaveCredentials(tbSiteUrl.Text.Trim(), tbUsername.Text.Trim(), tbPassword.Text.Trim(), StorageType.SiteCollection);
                }
                else
                {
                    // Delete credentials
                    CredManUtil.DeleteCredentials(tbSiteUrl.Text.Trim(), StorageType.SiteCollection);
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
            btRemove.Enabled = enable;
        }
    }
}
