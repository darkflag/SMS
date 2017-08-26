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
    public partial class AddSite : Form
    {
        public AddSite()
        {
            InitializeComponent();

            cbAuthentication.SelectedIndex = 0;
        }

        public AddSite(SiteAuth site)
        {
            InitializeComponent();

            tbSiteUrl.Text = site.UrlAsString;
            tbUsername.Text = site.Username;
            tbPassword.Text = site.Password;

            tbSiteUrl.Enabled = false;
            cbAuthentication.Enabled = false;

            if (site.Authentication == AuthN.Default)
                cbAuthentication.SelectedIndex = 0;
            else
                cbAuthentication.SelectedIndex = 1;

            if (tbUsername.Text.Length > 0)
                tbPassword.Select();
        }

        private void LoadSite()
        {
            try
            {
                // Validate input
                Regex regex = new Regex(@"((\w+:\/\/)[-a-zA-Z0-9:@;?&=\/%\+\.\*!'\(\),\$_\{\}\^~\[\]`#|]+)");
                if (!regex.IsMatch(tbSiteUrl.Text.Trim()))
                {
                    MessageBox.Show("Please enter a valid URL...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Add new site collection
                AuthN authentication = cbAuthentication.SelectedIndex == 0 ? AuthN.Default : AuthN.SharePointOnline;
                Globals.SiteCollections.Add(new Uri(tbSiteUrl.Text), tbUsername.Text.Trim(), tbPassword.Text.Trim(), authentication);

                // Close dialog
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            LoadSite();
        }

        private void tbSiteUrl_TextChanged(object sender, EventArgs e)
        {
            if (tbSiteUrl.Text.ToLower().Contains("sharepoint.com"))
                cbAuthentication.SelectedIndex = 1;
        }

        private void cbAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAuthentication.SelectedIndex == 1)
            {
                tbUsername.Enabled = false;
                tbPassword.Enabled = false;
            }
            else
            {
                tbUsername.Enabled = true;
                tbPassword.Enabled = true;
            }
        }
    }
}
