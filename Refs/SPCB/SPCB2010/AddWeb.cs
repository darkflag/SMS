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
    public partial class AddWeb : Form
    {
        private SiteAuth _site;

        public AddWeb()
        {
            InitializeComponent();
        }

        public AddWeb(SiteAuth site)
        {
            InitializeComponent();

            _site = site;

            tbSiteUrl.Text = site.UrlAsString;
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            string url = tbSiteUrl.Text.Trim() + tbWebUrl.Text.Trim();

            // Validate input
            Regex regex = new Regex(@"(((http|https):\/\/)|www\.)[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#!]*[\w\-\@?^=%&amp;/~\+#])?");
            if (!regex.IsMatch(url.Trim()))
            {
                MessageBox.Show("Please enter a valid URL...", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Uri webUrl = new Uri(url);

            _site.Webs.Add(webUrl.AbsolutePath);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
