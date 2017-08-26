using SPBrowser.Properties;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPBrowser
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            this.Text = string.Format("About {0}", Application.ProductName);
        }

        private void About_Load(object sender, EventArgs e)
        {
            lbProductName.Text = Application.ProductName;

            lbProductInfo.Text = string.Format("{0}\nVersion {1}\n{2}", 
                Application.ProductName,
                ProductUtil.GetProductVersionInfo().FileVersion, 
                Application.CompanyName);

            // Set release notes and icon
            tbReleaseNotes.Text = ProductUtil.GetReleaseNotes().Replace("*", "");
            this.pbIcon.Image = ProductUtil.GetProductIcon32x32();
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_BLOG_URL);
        }

        private void llTwitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_TWITTER_URL);
        }

        private void llCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CODEPLEX_PROJECT_URL);
        }
    }
}
