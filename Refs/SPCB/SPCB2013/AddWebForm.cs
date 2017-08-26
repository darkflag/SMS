using SPBrowser.Entities;
using SPBrowser.Extentions;
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
    public partial class AddWebForm : Form
    {
        private SiteAuthentication _site;

        public AddWebForm()
        {
            InitializeComponent();
        }

        public AddWebForm(SiteAuthentication site)
        {
            try
            {
                InitializeComponent();

                _site = site;

                tbSiteUrl.Text = site.UrlAsString;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            try
            {
                string url = tbSiteUrl.Text.Trim();

                // Validate input
                Regex regex = new Regex(@"(((http|https):\/\/)|www\.)[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#!]*[\w\-\@?^=%&amp;/~\+#])?");
                if (!regex.IsMatch(url))
                {
                    MessageBox.Show("Please enter a valid URL...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Uri webUrl = new Uri(url);

                _site.Webs.Add(webUrl.AbsolutePath);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void tbWebUrl_TextChanged(object sender, EventArgs e)
        {
            tbSiteUrl.Text = _site.Url.Combine(tbWebUrl.Text.Trim());
        }
    }
}
