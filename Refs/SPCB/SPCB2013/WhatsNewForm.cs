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
    public partial class WhatsNewForm : Form
    {
        public WhatsNewForm()
        {
            InitializeComponent();
        }

        private void WhatsNewForm_Load(object sender, EventArgs e)
        {
            lbProductName.Text = string.Format("{0} {1}",
                Application.ProductName,
                ProductUtil.GetProductVersionInfo().FileVersion);

            // Set release notes and icon
            string[] lines = ProductUtil.GetReleaseNotes().Split('\n');
            this.pbIcon.Image = ProductUtil.GetProductIcon32x32();

            // Only add the first lines related to the current release.
            foreach (string line in lines)
            {
                string ln = line;

                if (ln.Equals("\r"))
                {
                    break;
                }

                // Remove first space for readability on bulleted list and remove '*' to combine 2 releases in the What's New text
                if (ln.StartsWith(" -") || ln.StartsWith("*"))
                    ln = ln.Substring(1);

                if (!string.IsNullOrEmpty(ln))
                {
                    tbReleaseNotes.Text += ln + "\n";
                }
            }

            // Remove last line, which is empty
            tbReleaseNotes.Text = tbReleaseNotes.Text.Remove(tbReleaseNotes.Text.Length - 1);
        }

        private void llTwitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_TWITTER_URL);
        }

        private void llWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_BLOG_URL);
        }

        private void llCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.CODEPLEX_PROJECT_URL);
        }

        private void btnMoreInfo_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();
            form.ShowDialog();
        }
    }
}
