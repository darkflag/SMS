using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client.UserProfiles;
using SPBrowser.Controls;
using SPBrowser.Entities;
using SPBrowser.Extentions;
using SPBrowser.Managers;
using SPBrowser.Properties;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;
using SPTenantAdmin = Microsoft.Online.SharePoint.TenantAdministration;
using SPTenantMngt = Microsoft.Online.SharePoint.TenantManagement;

namespace SPBrowser
{
    public partial class MainForm : Form
    {
        private TreeNode _selectedContextMenuNode;
        private bool _skipUserConfirmation = false;

        public MainForm()
        {
            try
            {
                InitializeComponent();
                InitializeForm();

                this.Text = Application.ProductName;
                this.Icon = ProductUtil.GetProductIcon();

                // Load recent sites
                this.LoadRecentSites();
                this.LoadRecentTenants();

                // Set sort
                tvSite.TreeViewNodeSorter = new TreeViewSorter();
                tvSite.Sort();

                // Update progressbar
                NodeLoader.ItemLoaded += NodeLoader_ItemLoaded;
                NodeLoader.UserProfilesBatchCompleted += NodeLoader_UserProfilesBatchCompleted;

                // Toggle menu's
                this.ToggleMenuOptions();
                this.ShowAllPropertiesWarning();

                // Initialize the XMLViewerSettings.
                this.InitializeXmlViewer();

                this.LoadContextMenuBrowsers();
            }
            catch (FileNotFoundException ex)
            {
                // Handle the file not found exception, likely caused by not having installed the SharePoint Client Components. 
                string message = string.Format(Resources.ErrorMissingAssemblySDK, ex.Message, HelpUtil.GetSDKDownloadTitle(), Application.ProductName);

                if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(HelpUtil.GetSDKDownloadUrl());
                }

                LogUtil.LogException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void InitializeForm()
        {
            // TODO: Support disabling tabs (https://web.archive.org/web/20131102065816/http://tutorials.csharp-online.net/Disabling_Tab_Pages)
            //tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
            //tabs.DrawItem += Tabs_DrawItem;
            //tabs.Selecting += Tabs_Selecting;
        }

        private void Tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage.Enabled == false)
            {
                e.Cancel = true;
            }
        }

        private void Tabs_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = sender as TabControl;
            TabPage tabPage = tabControl.TabPages[e.Index];

            if (tabPage.Enabled == false)
            {
                using (SolidBrush brush = new SolidBrush(SystemColors.GrayText))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(tabPage.ForeColor))
                {
                    e.Graphics.DrawString(tabPage.Text, tabPage.Font, brush, e.Bounds.X + 3, e.Bounds.Y + 3);
                }
            }
        }

        #region Events

        private void MainBrowser_Shown(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                // Load site from arguments
                switch (Globals.Arguments.Action)
                {
                    case CommandAction.OpenSite:
                        this.AddSite(Globals.Sites[Globals.Arguments.Url.ToString(), null]);
                        break;
                    case CommandAction.OpenTenant:
                        this.AddTenant(Globals.Tenants[Globals.Arguments.Url.ToString(), null]);
                        break;
                    case CommandAction.Help:
                    case CommandAction.None:
                    default:
                        // TODO: Implement HELP function.
                        break;
                }

                // Show What's New form
                if (Configuration.Current.LastUsedVersion == null || new Version(Application.ProductVersion) > Configuration.Current.LastUsedVersion)
                {
                    WhatsNewForm form = new WhatsNewForm();
                    form.ShowDialog(this);
                }

                // Show New Release available message box
                this.CheckForNewRelease();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }

        }

        private void tvSite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                // Select the clicked node
                _selectedContextMenuNode = tvSite.SelectedNode;

                // Set Statusbar
                tsClassName.Text = string.Format("{0}", tvSite.SelectedNode.Tag != null && tvSite.SelectedNode.Tag.GetType() != typeof(SPBrowser.NodeLoadType) ? tvSite.SelectedNode.Tag.GetType().ToString() : string.Empty);
                tsDescription.Text = tvSite.SelectedNode.ToolTipText.Length == 0 ? string.Empty : tvSite.SelectedNode.ToolTipText;

                // Set tabs
                this.LoadingTabProperties();
                this.LoadingTabRawData();
                this.LoadingTabChanges();
                this.LoadingTabMsdnBrowser();
                this.LoadingTabSchemaXml();
                this.LoadingTabRest();

                // Toggle menu's
                this.ToggleMenuOptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void tvSite_AfterExpand(object sender, TreeViewEventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                if (e.Node.Tag != null &&
                    e.Node.FirstNode.Text == Constants.NODE_LOADING_TEXT &&
                    (e.Node.Tag.GetType().Equals(typeof(NodeLoadType)) || e.Node.FirstNode.Tag.GetType().Equals(typeof(NodeLoadType))))
                {
                    // Determine load type
                    NodeLoadType loadType = e.Node.Tag.GetType().Equals(typeof(NodeLoadType)) ? (NodeLoadType)e.Node.Tag : (NodeLoadType)e.Node.FirstNode.Tag;

                    // Load nodes
                    LoadNodes(loadType, e.Node);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void tvSite_MouseDown(object sender, MouseEventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                // Select the clicked node
                _selectedContextMenuNode = tvSite.GetNodeAt(e.X, e.Y);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void tvSite_KeyUp(object sender, KeyEventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                // Select the clicked node
                //_selectedContextMenuNode = tvSite.SelectedNode;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        public void recentSiteCollectionItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                SiteAuthentication site = Globals.Sites[sender.ToString(), null];

                AddSiteForm form = new AddSiteForm(site);
                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK)
                    this.LoadSiteCollections();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        public void recentTenantItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                TenantAuthentication tenant = Globals.Tenants[sender.ToString(), null];

                AddTenantForm form = new AddTenantForm(tenant);
                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK)
                    this.LoadTenants();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        public void tsClassName_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                Uri url = HelpUtil.GetMSDNHelpLink(tvSite.SelectedNode);

                if (url != null)
                    System.Diagnostics.Process.Start(url.OriginalString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.LoadingTabProperties();
            this.LoadingTabRawData();
            this.LoadingTabChanges();
            this.LoadingTabMsdnBrowser();
            this.LoadingTabSchemaXml();
            this.LoadingTabRest();
        }

        private void wbMsdnHelp_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                tsProgressBar.Minimum = 0;
                tsProgressBar.Maximum = (int)e.MaximumProgress;

                if (e.CurrentProgress >= 0)
                    tsProgressBar.Value = (int)e.CurrentProgress;
            }
            catch { }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            AboutForm form = new AboutForm();
            form.ShowDialog(this);
        }

        private void whatsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            WhatsNewForm form = new WhatsNewForm();
            form.ShowDialog(this);
        }

        private void addSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            AddSite();
        }

        private void addSiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            AddSite();
        }

        private void addTenantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            AddTenant();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            RemoveSite();
        }

        private void removeTenantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            RemoveTenant();
        }

        private void browseMSDNHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.BrowseMSDNLink(_selectedContextMenuNode);
        }

        private void browseMSDNtoolStripButton_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.BrowseMSDNLink(_selectedContextMenuNode);
        }

        private void addSpecificWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            try
            {
                SiteAuthentication site = Globals.Sites[_selectedContextMenuNode.Text];

                AddWebForm form = new AddWebForm(site);
                DialogResult result = form.ShowDialog(this);

                if (result == System.Windows.Forms.DialogResult.OK)
                    LoadSiteCollections();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void mnContextItem_Opened(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            ToggleMenuOptions();
        }

        private void openPowerShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.OpenPowerShellConsole();
        }

        private void openPowerShellToolStripButton_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.OpenPowerShellConsole();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            CheckForNewRelease(true);
        }

        private void openCodeplexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            System.Diagnostics.Process.Start(Constants.CODEPLEX_PROJECT_URL);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.RefreshCollection();
        }

        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.RefreshCollection();
        }

        private void removeSubWebsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.RemoveSubWebs();
        }

        private void NodeLoader_ItemLoaded(object sender, ItemLoadedEventArgs e)
        {
            // No new correlation identifier!

            tsProgressBar.Minimum = 0;
            tsProgressBar.Maximum = e.TotalItems;

            if (tsProgressBar.Maximum >= e.CurrentItem)
                tsProgressBar.Value = e.CurrentItem;
        }

        private void NodeLoader_UserProfilesBatchCompleted(object sender, BatchCompletedEventArgs e)
        {
            bool hasMore = e.TotalItems > e.CurrentItem;

            // Calculate estimated total duration
            int batchCount = e.TotalItems / e.BatchSize;
            int currentBatch = e.CurrentItem / e.BatchSize;
            double expectedDurationInMinutes = (batchCount - currentBatch) * e.Duration.TotalMinutes;

            if (!_skipUserConfirmation)
            {
                string message = string.Format("We have loaded {0} user profiles out of the {2}, would you like to continue loading the next batch of {1} user profiles or load all {2} user profiles?\n\nClick 'Yes' for loading the next batch of {1} user profiles\nClick 'No' for loading all {2} user profiles (approximately {3} minutes)\nOr 'Cancel' to exit",
                    e.CurrentItem.ToString("N0"),
                    e.BatchSize.ToString("N0"),
                    e.TotalItems.ToString("N0"),
                    expectedDurationInMinutes.ToString("N0"));

                switch (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes: // Load next batch of user profiles
                        _skipUserConfirmation = false;
                        hasMore = true;
                        break;
                    case DialogResult.No: // Load all user profiles, do not ask user confirmation again
                        _skipUserConfirmation = true;
                        hasMore = true;
                        break;
                    case DialogResult.Cancel: // Cancel loading user profiles
                        _skipUserConfirmation = false;
                        hasMore = false;
                        break;
                    default:
                        break;
                }
            }

            if (hasMore)
            {
                NodeLoader.LoadUserProfileOtherUsers(_selectedContextMenuNode, this, NodeLoadType.UserProfileOtherUsers, e.CurrentItem, e.BatchSize);
            }
        }

        private void MainBrowser_VisibleChanged(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.Activate();
        }

        private void openLogsMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            LogUtil.OpenLogsFolderLocation();
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.ReportABug();
        }

        private void btRestExecuteQuery_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.ExecuteRestQuery();
        }

        private void size860x715ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetWindowSize(860, 715);
        }

        private void size800x600ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetWindowSize(800, 600);
        }

        private void size1024x768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetWindowSize(1024, 768);
        }

        private void size1280x1024ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetWindowSize(1280, 1024);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            OptionsForm form = new OptionsForm();
            form.ShowDialog();

            // Enable warning message in status bar
            this.ShowAllPropertiesWarning();
        }

        private void openWebDavToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenInWebDav();
        }

        private void openItemWithBrowser_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            Browser browser = ((ToolStripMenuItem)sender).Tag as Browser;
            bool openInPrivate = ((ToolStripMenuItem)sender).Text.Contains(Constants.BROWSER_IN_PRIVATE_MODE_LABEL);
            string url = GetOpenItemUrl();

            BrowserUtil.StartBrowserWithUrl(browser, url, openInPrivate);
        }

        private void browseSettingsWithBrowser_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            Browser browser = ((ToolStripMenuItem)sender).Tag as Browser;
            bool openInPrivate = ((ToolStripMenuItem)sender).Text.Contains(Constants.BROWSER_IN_PRIVATE_MODE_LABEL);
            string url = GetBrowseSettingsUrl();

            BrowserUtil.StartBrowserWithUrl(browser, url, openInPrivate);
        }

        private void browseOpenUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            BrowserUtil.StartBrowserWithUrl(BrowserUtil.GetSystemDefaultBrowser(), GetOpenItemUrl());
        }

        private void browseCopyUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.CopyToClipboard(GetOpenItemUrl());
        }

        private void settingsOpenUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            BrowserUtil.StartBrowserWithUrl(BrowserUtil.GetSystemDefaultBrowser(), GetBrowseSettingsUrl());
        }

        private void settingsCopyUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            this.CopyToClipboard(GetBrowseSettingsUrl());
        }

        private void openInBrowserToolStripButton_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            BrowserUtil.StartBrowserWithUrl(BrowserUtil.GetSystemDefaultBrowser(), GetOpenItemUrl());
        }

        private void browseSettingsToolStripButton_Click(object sender, EventArgs e)
        {
            LogUtil.NewCorrelation();

            BrowserUtil.StartBrowserWithUrl(BrowserUtil.GetSystemDefaultBrowser(), GetBrowseSettingsUrl());
        }

        #endregion

        #region Methods

        public void CheckForNewRelease(bool forceReleaseAvailabilityCheck = false)
        {
            try
            {
                //force check on new release, when no release data is available
                if (Globals.LatestRelease == null || forceReleaseAvailabilityCheck)
                {
                    Globals.LatestRelease = ReleasesManager.GetLatestRelease();

                    if (forceReleaseAvailabilityCheck && Globals.LatestRelease == null)
                    {
                        MessageBox.Show($"No new release of '{Application.ProductName}' available at the moment. Please check later...", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                //check for update and take action
                if (Globals.LatestRelease != null && Globals.LatestRelease.Version > ProductUtil.GetCurrentProductVersion())
                {
                    // New version of SPCB is avialable, show message box.
                    LogUtil.LogMessage($"New release available: {Globals.LatestRelease.Version} ({Globals.LatestRelease.Title}), download at {Globals.LatestRelease.DownloadUrl}", LogLevel.Information);

                    string description = Globals.LatestRelease.Description.Length == 0 ? string.Empty : $"{Globals.LatestRelease.Description}\n\n";
                    if (MessageBox.Show($"New release '{Globals.LatestRelease.Title}' is available!\n\n{description}Do you want to download the new version.", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Globals.LatestRelease.DownloadUrl.OriginalString);
                    }
                }
                else if(forceReleaseAvailabilityCheck)
                {
                    // Show message box indicating NO new version is available.
                    LogUtil.LogMessage($"No new release available. Current downloadable release: {Globals.LatestRelease.Version} ({Globals.LatestRelease.Title}), download at {Globals.LatestRelease.DownloadUrl}", LogLevel.Information);

                    MessageBox.Show($"No new release of '{Application.ProductName}' available at the moment. Please check later...\n\nCurrent release is '{Globals.LatestRelease.Title}'.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadRecentSites()
        {
            try
            {
                recentSitesToolStripMenuItem.DropDownItems.Clear();
                recentSitesContextToolStripMenuItem.DropDownItems.Clear();

                // Add history to menu item
                foreach (SiteAuthentication site in Globals.Sites.OrderByDescending(s => s.LoadDate))
                {
                    string toolTip = string.Format("Last opened: {0} {1}\nAuthentication: {2}\nUsername: {3}",
                        site.LoadDate.ToLongDateString(),
                        site.LoadDate.ToShortTimeString(),
                        site.Authentication,
                        site.UserName);

                    ToolStripItem item = recentSitesToolStripMenuItem.DropDownItems.Add(site.UrlAsString);
                    item.Click += recentSiteCollectionItem_Click;
                    item.ToolTipText = toolTip;

                    ToolStripItem itemContext = recentSitesContextToolStripMenuItem.DropDownItems.Add(site.UrlAsString);
                    itemContext.Click += recentSiteCollectionItem_Click;
                    itemContext.ToolTipText = toolTip;
                }

                // Enable menu item
                if (recentSitesToolStripMenuItem.DropDownItems.Count == 0)
                {
                    recentSitesToolStripMenuItem.Enabled = false;
                    recentSitesContextToolStripMenuItem.Enabled = false;
                }
                else
                {
                    recentSitesToolStripMenuItem.Enabled = true;
                    recentSitesContextToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadRecentTenants()
        {
            try
            {
                recentTenantsToolStripMenuItem.DropDownItems.Clear();
                recentTenantsContextToolStripMenuItem.DropDownItems.Clear();

                // Add history to menu item
                foreach (TenantAuthentication tenant in Globals.Tenants.OrderByDescending(s => s.LoadDate))
                {
                    string toolTip = string.Format("Last opened: {0} {1}\nUsername: {2}",
                        tenant.LoadDate.ToLongDateString(),
                        tenant.LoadDate.ToShortTimeString(),
                        tenant.UserName);

                    ToolStripItem item = recentTenantsToolStripMenuItem.DropDownItems.Add(tenant.AdminUrlAsString);
                    item.Click += recentTenantItem_Click;
                    item.ToolTipText = toolTip;

                    ToolStripItem itemContext = recentTenantsContextToolStripMenuItem.DropDownItems.Add(tenant.AdminUrlAsString);
                    itemContext.Click += recentTenantItem_Click;
                    itemContext.ToolTipText = toolTip;
                }

                // Enable menu item
                if (recentTenantsToolStripMenuItem.DropDownItems.Count == 0)
                {
                    recentTenantsToolStripMenuItem.Enabled = false;
                    recentTenantsContextToolStripMenuItem.Enabled = false;
                }
                else
                {
                    recentTenantsToolStripMenuItem.Enabled = true;
                    recentTenantsContextToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadTenants()
        {
            try
            {
                // Prepare root node
                TreeNode rootNode = tvSite.Nodes["Tenants"];

                // Add tenants from Global list
                foreach (TenantAuthentication tenant in Globals.Tenants.Where(t => t.IsLoaded))
                {
                    // Only add site, when it does not exist in treeview
                    if (!rootNode.Nodes.ContainsKey(tenant.ClientContext.Url))
                    {
                        NodeLoader.LoadTenant(rootNode, tenant.ClientContext, this);
                    }
                }

                // Ensure recent sites are in sync
                this.LoadRecentTenants();

                // Expand root node
                rootNode.Expand();

                // Reset progress bar
                tsProgressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadSiteCollections()
        {
            this.LoadTenants();

            try
            {
                // Prepare root node
                TreeNode rootNode = tvSite.Nodes["SiteCollections"];

                // Add site collections from Global list
                foreach (SiteAuthentication sc in Globals.Sites.Where(s => s.IsLoaded))
                {
                    // Only add site, when it does not exist in treeview
                    if (!rootNode.Nodes.ContainsKey(sc.UrlAsString))
                    {
                        NodeLoader.LoadSite(rootNode, sc.ClientContext, sc.ClientContext.Site, this, sc.Webs);
                    }
                }

                // Ensure recent sites are in sync
                this.LoadRecentSites();

                // Expand root node
                rootNode.Expand();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadingTabProperties()
        {
            if (tabs.SelectedTab == tpProperties)
            {
                pgProperties.SelectedObject = tvSite.SelectedNode.Tag;
            }
        }

        private void LoadingTabMsdnBrowser()
        {
            try
            {
                if (tabs.SelectedTab == tpMsdn)
                {
                    Uri url = HelpUtil.GetMSDNHelpLink(tvSite.SelectedNode);
                    if (url != null)
                    {
                        if (wbMsdnHelp.Url != url)
                            wbMsdnHelp.Url = url;
                    }
                    else if (wbMsdnHelp.Url != HelpUtil.GetMSDNHelpLink())
                    {
                        wbMsdnHelp.Url = HelpUtil.GetMSDNHelpLink();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadingTabRawData()
        {
            try
            {
                gvRawData.Columns.Clear();
                gvRawData.Rows.Clear();

                if (tabs.SelectedTab == tpRawData)
                    NodeLoader.LoadRawData(tvSite.SelectedNode, gvRawData);

                // Show help info label
                if (gvRawData.Rows.Count + gvRawData.Columns.Count > 0)
                    lbInfo.Visible = false;
                else
                    lbInfo.Visible = true;

                tsProgressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadingTabChanges()
        {
            try
            {
                if (tabs.SelectedTab == tpChanges)
                    NodeLoader.LoadChanges(tvSite.SelectedNode, gvChanges);

                // Show help info label
                if (gvChanges.Rows.Count + gvChanges.Columns.Count > 0)
                    lbInfoChangeLog.Visible = false;
                else
                    lbInfoChangeLog.Visible = true;

                tsProgressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadingTabSchemaXml()
        {
            try
            {
                xvSchemaXML.Text = string.Empty;

                if (tabs.SelectedTab == tpSchemaXml)
                {
                    string schemaXml = NodeLoader.LoadSchemaXML(_selectedContextMenuNode);
                    if (schemaXml.Length > 0)
                    {
                        xvSchemaXML.Text = schemaXml;
                        xvSchemaXML.Process(true);
                    }
                }

                tsProgressBar.Value = 0;
            }
            catch (XmlViewerNamespaceException ex)
            {
                // Known exception is raised on Content Type and Workflow Definition, which contains a XML namespace
                LogUtil.LogException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadingTabRest()
        {
            try
            {
                tbRestQuery.Text = string.Empty;

                if (tabs.SelectedTab == tpRest)
                {
                    Uri restUrl = NodeLoader.LoadRestQuery(_selectedContextMenuNode);
                    if (restUrl != null)
                    {
                        tbRestQuery.Text = restUrl.ToString();
                        wbRest.Url = restUrl;
                    }
                }

                tsProgressBar.Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void ExecuteRestQuery()
        {
            try
            {
                Uri restUrl = new Uri(tbRestQuery.Text);
                wbRest.Url = restUrl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadNodes(NodeLoadType loadType, TreeNode node)
        {
            try
            {
                // Initiate treeview load
                tvSite.BeginUpdate();

                // Clear Loading... node
                node.Nodes.Clear();

                // Load node contents
                switch (loadType)
                {
                    case NodeLoadType.Sites:
                        NodeLoader.LoadSites(node, (SPTenantAdmin.Tenant)node.Parent.Tag, this);
                        break;
                    case NodeLoadType.TenantWebTemplates:
                        NodeLoader.LoadSPOWebTemplates(node, ((SPTenantAdmin.Tenant)node.Parent.Tag).GetSPOTenantWebTemplates(1033, 15), this, NodeLoadType.TenantWebTemplates);
                        break;
                    case NodeLoadType.SiteProperties:
                        NodeLoader.LoadSiteProperties(node, (SPTenantAdmin.Tenant)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.DeletedSiteProperties:
                        NodeLoader.LoadDeletedSiteProperties(node, (SPTenantAdmin.Tenant)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.ExternalUsers:
                        NodeLoader.LoadExternalUsers(node, (SPTenantMngt.Office365Tenant)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.Apps:
                        NodeLoader.LoadApps(node, (SPTenantAdmin.Tenant)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.AppErrors:
                        // TODO: Refactor!!
                        Guid productId = ((SPTenantAdmin.AppInfo)node.Parent.Tag).ProductId;
                        NodeLoader.LoadAppErrors(node, ((SPTenantAdmin.Tenant)node.Parent.Parent.Parent.Tag).GetAppErrors(productId, DateTime.UtcNow.AddMonths(-Constants.ERROR_LOGS_ROW_TIME_FRAME_IN_MONTHS), DateTime.UtcNow), this, loadType);
                        break;
                    case NodeLoadType.LogEntries:
                        // TODO: Refactor!!
                        NodeLoader.LoadTenantLogs(node, this, loadType);
                        break;
                    case NodeLoadType.Site:
                        NodeLoader.LoadSite(node, (SPTenantAdmin.Tenant)node.Parent.Parent.Tag, node.Text, this);
                        break;
                    case NodeLoadType.SiteFields:
                        NodeLoader.LoadSiteColumns(node, ((SPClient.Site)node.Parent.Tag).RootWeb.AvailableFields, this, loadType);
                        break;
                    case NodeLoadType.SiteContentTypes:
                        NodeLoader.LoadContentTypes(node, ((SPClient.Site)node.Parent.Tag).RootWeb.AvailableContentTypes, this, loadType);
                        break;
                    case NodeLoadType.SiteFeatures:
                        NodeLoader.LoadFeatures(node, ((SPClient.Site)node.Parent.Tag).Features, this, loadType);
                        break;
                    case NodeLoadType.SiteEventReceivers:
                        NodeLoader.LoadEventReceivers(node, ((SPClient.Site)node.Parent.Tag).EventReceivers, this, loadType);
                        break;
                    case NodeLoadType.SiteRecycleBin:
                        NodeLoader.LoadRecycleBin(node, ((SPClient.Site)node.Parent.Tag).RecycleBin, this, loadType);
                        break;
                    case NodeLoadType.SiteWebTemplates:
                        NodeLoader.LoadWebTemplates(node, ((SPClient.Site)node.Parent.Tag).GetWebTemplates(((SPClient.Site)node.Parent.Tag).RootWeb.Language, 0), this, loadType);
                        break;
                    case NodeLoadType.SiteUserCustomActions:
                        NodeLoader.LoadUserCustomActions(node, ((SPClient.Site)node.Parent.Tag).UserCustomActions, this, loadType);
                        break;
                    case NodeLoadType.WebSubWebs:
                        NodeLoader.LoadSubWebs(node, ((SPClient.Web)node.Parent.Tag).GetSubwebsForCurrentUser(null), this);
                        break;
                    case NodeLoadType.WebFields:
                        NodeLoader.LoadSiteColumns(node, ((SPClient.Web)node.Parent.Tag).Fields, this, loadType);
                        break;
                    case NodeLoadType.WebContentTypes:
                        NodeLoader.LoadContentTypes(node, ((SPClient.Web)node.Parent.Tag).ContentTypes, this, loadType);
                        break;
                    case NodeLoadType.WebLists:
                        NodeLoader.LoadLists(node, ((SPClient.Web)node.Parent.Tag).Lists, this);
                        break;
                    case NodeLoadType.WebFeatures:
                        NodeLoader.LoadFeatures(node, ((SPClient.Web)node.Parent.Tag).Features, this, loadType);
                        break;
                    case NodeLoadType.WebUsers:
                        NodeLoader.LoadUsers(node, ((SPClient.Web)node.Parent.Tag).SiteUsers, this, loadType);
                        break;
                    case NodeLoadType.GroupsForUser:
                        NodeLoader.LoadSiteGroups(node, ((SPClient.User)node.Parent.Tag).Groups, this, loadType);
                        break;
                    case NodeLoadType.WebGroups:
                        NodeLoader.LoadSiteGroups(node, ((SPClient.Web)node.Parent.Tag).SiteGroups, this, loadType);
                        break;
                    case NodeLoadType.WebAssociatedGroups:
                        NodeLoader.LoadAssociatedGroups(node, (SPClient.Web)node.Parent.Tag, this);
                        break;
                    case NodeLoadType.WebRecycleBin:
                        NodeLoader.LoadRecycleBin(node, ((SPClient.Web)node.Parent.Tag).RecycleBin, this, loadType);
                        break;
                    case NodeLoadType.WebAppInstances:
                        NodeLoader.LoadAppInstances(node, (SPClient.Web)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.GroupUsers:
                        NodeLoader.LoadUsers(node, ((SPClient.Group)node.Tag).Users, this, loadType, false);
                        break;
                    case NodeLoadType.WebWorkflowAssociations:
                        NodeLoader.LoadWorkflowAssociations(node, ((SPClient.Web)node.Parent.Tag).WorkflowAssociations, this, loadType);
                        break;
                    case NodeLoadType.WebWorkflowTemplates:
                        NodeLoader.LoadWorkflowTemplates(node, ((SPClient.Web)node.Parent.Tag).WorkflowTemplates, this, loadType);
                        break;
                    case NodeLoadType.WebWorkflowSubscriptions:
                        NodeLoader.LoadWorkflowSubscriptions(node, (SPClient.Web)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.WebWorkflowDefinitions:
                        NodeLoader.LoadWorkflowDefinitions(node, (SPClient.Web)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.WebWorkflowInstances:
                        NodeLoader.LoadWorkflowInstances(node, (SPClient.Web)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.WebEventReceivers:
                        NodeLoader.LoadEventReceivers(node, ((SPClient.Web)node.Parent.Tag).EventReceivers, this, loadType);
                        break;
                    case NodeLoadType.WebProperties:
                        NodeLoader.LoadProperties(node, ((SPClient.Web)node.Parent.Tag).AllProperties, this, loadType);
                        break;
                    case NodeLoadType.WebListTemplates:
                        NodeLoader.LoadListTemplates(node, ((SPClient.Web)node.Parent.Tag).ListTemplates, this, loadType);
                        break;
                    case NodeLoadType.WebPushNotificationSubscribers:
                        NodeLoader.LoadPushNotificationSubscribers(node, ((SPClient.Web)node.Parent.Tag).PushNotificationSubscribers, this, loadType);
                        break;
                    case NodeLoadType.WebRoleAssignments:
                        NodeLoader.LoadRoleAssignments(node, ((SPClient.Web)node.Parent.Tag).RoleAssignments, this, loadType);
                        break;
                    case NodeLoadType.WebRoleDefinitions:
                        NodeLoader.LoadRoleDefinitions(node, ((SPClient.Web)node.Parent.Tag).RoleDefinitions, this, loadType);
                        break;
                    case NodeLoadType.WebTemplates:
                        NodeLoader.LoadWebTemplates(node, ((SPClient.Web)node.Parent.Tag).GetAvailableWebTemplates(((SPClient.Web)node.Parent.Tag).Language, true), this, loadType);
                        break;
                    case NodeLoadType.WebUserCustomActions:
                        NodeLoader.LoadUserCustomActions(node, ((SPClient.Web)node.Parent.Tag).UserCustomActions, this, loadType);
                        break;
                    case NodeLoadType.Folder:
                        NodeLoader.LoadSubFolders(node, ((SPClient.Folder)node.Tag), this);
                        break;
                    case NodeLoadType.FolderProperties:
                        NodeLoader.LoadProperties(node, ((SPClient.Folder)node.Parent.Tag).Properties, this, loadType);
                        break;
                    case NodeLoadType.WebParts:
                        NodeLoader.LoadWebParts(node, (SPClient.File)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.WebPartProperties:
                        NodeLoader.LoadProperties(node, ((SPClient.WebParts.WebPart)node.Parent.Tag).Properties, this, loadType);
                        break;
                    case NodeLoadType.FileVersions:
                        NodeLoader.LoadFileVersions(node, ((SPClient.File)node.Parent.Tag).Versions, this, loadType);
                        break;
                    case NodeLoadType.ListFields:
                        NodeLoader.LoadSiteColumns(node, ((SPClient.List)node.Parent.Tag).Fields, this, loadType);
                        break;
                    case NodeLoadType.ListContentTypes:
                        NodeLoader.LoadContentTypes(node, ((SPClient.List)node.Parent.Tag).ContentTypes, this, loadType);
                        break;
                    case NodeLoadType.ListItems:
                        NodeLoader.LoadItems(node, (SPClient.List)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.ListViews:
                        NodeLoader.LoadViews(node, ((SPClient.List)node.Parent.Tag).Views, this);
                        break;
                    case NodeLoadType.ListWorkflowAssociations:
                        NodeLoader.LoadWorkflowAssociations(node, ((SPClient.List)node.Parent.Tag).WorkflowAssociations, this, loadType);
                        break;
                    case NodeLoadType.ListWorkflowSubscriptions:
                        NodeLoader.LoadWorkflowSubscriptions(node, (SPClient.Web)node.Parent.Parent.Parent.Tag, this, loadType, (SPClient.List)node.Parent.Tag);
                        break;
                    case NodeLoadType.ListEventReceivers:
                        NodeLoader.LoadEventReceivers(node, ((SPClient.List)node.Parent.Tag).EventReceivers, this, loadType);
                        break;
                    case NodeLoadType.ListRoleAssignments:
                        NodeLoader.LoadRoleAssignments(node, ((SPClient.List)node.Parent.Tag).RoleAssignments, this, loadType);
                        break;
                    case NodeLoadType.ListUserCustomActions:
                        NodeLoader.LoadUserCustomActions(node, ((SPClient.List)node.Parent.Tag).UserCustomActions, this, loadType);
                        break;
                    case NodeLoadType.ItemAttachmentFiles:
                        NodeLoader.LoadAttachmentCollection(node, ((SPClient.ListItem)node.Parent.Tag).AttachmentFiles, this, loadType);
                        break;
                    case NodeLoadType.ItemFieldValues:
                        NodeLoader.LoadDictionaryValues(node, ((SPClient.ListItem)node.Parent.Tag).FieldValues, this, loadType);
                        break;
                    case NodeLoadType.ItemFieldValuesAsHtml:
                        NodeLoader.LoadFieldStringValues(node, ((SPClient.ListItem)node.Parent.Tag).FieldValuesAsHtml, this, loadType);
                        break;
                    case NodeLoadType.ItemFieldValuesAsText:
                        NodeLoader.LoadFieldStringValues(node, ((SPClient.ListItem)node.Parent.Tag).FieldValuesAsText, this, loadType);
                        break;
                    case NodeLoadType.ItemFieldValuesForEdit:
                        NodeLoader.LoadFieldStringValues(node, ((SPClient.ListItem)node.Parent.Tag).FieldValuesForEdit, this, loadType);
                        break;
                    case NodeLoadType.ItemRoleAssignments:
                        NodeLoader.LoadRoleAssignments(node, ((SPClient.ListItem)node.Parent.Tag).RoleAssignments, this, loadType);
                        break;
                    case NodeLoadType.ItemWorkflowInstances:
                        SPClient.ListItem item = (SPClient.ListItem)node.Parent.Tag;
                        NodeLoader.LoadWorkflowInstances(node, item.ParentList.ParentWeb, this, loadType, item);
                        break;
                    case NodeLoadType.ContentTypeFields:
                        NodeLoader.LoadSiteColumns(node, ((SPClient.ContentType)node.Parent.Tag).Fields, this, loadType);
                        break;
                    case NodeLoadType.ContentTypeWorkflowAssociations:
                        NodeLoader.LoadWorkflowAssociations(node, ((SPClient.ContentType)node.Parent.Tag).WorkflowAssociations, this, loadType);
                        break;
                    case NodeLoadType.FieldLinks:
                        NodeLoader.LoadFieldLinks(node, ((SPClient.ContentType)node.Parent.Tag).FieldLinks, this, loadType);
                        break;
                    case NodeLoadType.NavigationTopNavigationBar:
                        NodeLoader.LoadNavigationNodes(node, ((SPClient.Navigation)node.Parent.Tag).TopNavigationBar, this, loadType);
                        break;
                    case NodeLoadType.NavigationQuickLaunch:
                        NodeLoader.LoadNavigationNodes(node, ((SPClient.Navigation)node.Parent.Tag).QuickLaunch, this, loadType);
                        break;
                    case NodeLoadType.NavigationNodes:
                        NodeLoader.LoadNavigationNodes(node, ((SPClient.NavigationNode)node.Tag).Children, this, loadType);
                        break;
                    case NodeLoadType.ProjectPolicy:
                        NodeLoader.LoadProjectPolicy(node, (SPClient.Web)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.ProjectPolicies:
                        NodeLoader.LoadProjectPolicies(node, (SPClient.Web)node.Parent.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.TermStores:
                        NodeLoader.LoadTermStores(node, this, loadType);
                        break;
                    case NodeLoadType.TermGroups:
                        NodeLoader.LoadTermGroups(node, ((TermStore)node.Tag).Groups, this, loadType);
                        break;
                    case NodeLoadType.TermSets:
                        NodeLoader.LoadTermSets(node, ((TermGroup)node.Tag).TermSets, this, loadType);
                        break;
                    case NodeLoadType.Terms:
                        NodeLoader.LoadTerms(node, ((TermSet)node.Tag).Terms, this, loadType);
                        break;
                    case NodeLoadType.TermChilds:
                        NodeLoader.LoadTerms(node, ((Term)node.Tag).Terms, this, loadType);
                        break;
                    case NodeLoadType.UserProfiles:
                        NodeLoader.LoadUserProfiles(node, this, loadType);
                        break;
                    case NodeLoadType.UserProfileCurrentUser:
                        NodeLoader.LoadUserProfileCurrentUser(node, this, loadType);
                        break;
                    case NodeLoadType.UserProfileOtherUsers:
                        NodeLoader.LoadUserProfileOtherUsers(node, this, loadType);
                        break;
                    case NodeLoadType.UserProfileProperties:
                        NodeLoader.LoadUserProfileProperties(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfilePeers:
                        NodeLoader.LoadUserProfilePeers(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileDirectReports:
                        NodeLoader.LoadUserProfileDirectReports(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileExtendedManagers:
                        NodeLoader.LoadUserProfileExtendedManagers(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileExtendedReports:
                        NodeLoader.LoadUserProfileExtendedReports(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileFollowedTags:
                        NodeLoader.LoadUserProfileFollowedTags(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileFollowers:
                        NodeLoader.LoadUserProfileFollowers(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileSuggestions:
                        NodeLoader.LoadUserProfileSuggestions(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfilePeopleFollowedByUser:
                        NodeLoader.LoadUserProfilePeopleFollowedByUser(node, (PersonProperties)node.Parent.Tag, this, loadType);
                        break;
                    case NodeLoadType.UserProfileTrendingTags:
                        NodeLoader.LoadUserProfileTrendingTags(node, this, loadType);
                        break;
                    case NodeLoadType.TimeZones:
                        NodeLoader.LoadTimeZones(node, ((SPClient.RegionalSettings)node.Parent.Tag).TimeZones, this, loadType);
                        break;
                    case NodeLoadType.SiteCollectionAdmins:
                        NodeLoader.LoadSiteCollectionAdmins(node, (SPClient.Site)node.Parent.Tag, this, loadType);
                        break;
#if CLIENTSDKV160UP
                    case NodeLoadType.FileProperties:
                        NodeLoader.LoadProperties(node, ((SPClient.File)node.Parent.Tag).Properties, this, loadType);
                        break;
                    case NodeLoadType.AppTiles:
                        NodeLoader.LoadAppTiles(node, ((SPClient.Web)node.Parent.Tag).AppTiles, this, NodeLoadType.AppTiles);
                        break;
#endif
#if CLIENTSDKV161UP
                    case NodeLoadType.ItemProperties:
                        NodeLoader.LoadProperties(node, ((SPClient.ListItem)node.Parent.Tag).Properties, this, loadType);
                        break;
                    case NodeLoadType.WebAlerts:
                        NodeLoader.LoadAlerts(node, ((SPClient.Web)node.Parent.Tag).Alerts, this, loadType);
                        break;
                    case NodeLoadType.UserAlerts:
                        NodeLoader.LoadAlerts(node, ((SPClient.User)node.Parent.Tag).Alerts, this, loadType);
                        break;
                    case NodeLoadType.FileVersionEvents:
                        NodeLoader.LoadFileVersionEvents(node, ((SPClient.File)node.Parent.Tag).VersionEvents, this, loadType);
                        break;
#endif
                    default:
                        throw new NotImplementedException("Working on it... (not implemented yet!)");
                }
            }
            catch (NotImplementedException ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                LogUtil.LogException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
            finally
            {
                // End treeview load
                tvSite.EndUpdate();
                tsProgressBar.Value = 0;
            }
        }

        private void AddSite()
        {
            AddSite(null);
        }

        private void AddSite(SiteAuthentication site)
        {
            try
            {
                AddSiteForm form = new AddSiteForm(site);
                DialogResult result = form.ShowDialog(this);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.LoadSiteCollections();
                    TaskBar.UpdateRecentItemsJumpList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void RemoveTenant()
        {
            try
            {
                // Remove from global tenants list
                TenantAuthentication tenant = Globals.Tenants[((SPTenantAdmin.Tenant)_selectedContextMenuNode.Tag).RootSiteUrl];
                Globals.Tenants.Remove(tenant);

                // Remove node
                _selectedContextMenuNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void RemoveSite()
        {
            try
            {
                // Remove from global site collection list
                SiteAuthentication site = Globals.Sites[((SPClient.Site)_selectedContextMenuNode.Tag).Url];
                Globals.Sites.Remove(site);

                // Remove node
                _selectedContextMenuNode.Remove();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void RemoveSubWebs()
        {
            try
            {
                SiteAuthentication site = Globals.Sites[((SPClient.Site)_selectedContextMenuNode.Tag).Url]; //.SingleOrDefault(s => s.Url.OriginalString.Equals(((SPClient.Site)_selectedContextMenuNode.Tag).Url, StringComparison.InvariantCultureIgnoreCase));

                // Check if any subwebs exist?
                if (site.Webs.Count == 0)
                {
                    MessageBox.Show("No subwebs found to remove.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Ask for delete confirmation and delete subwebs
                string question = string.Format("Do you want to delete these subweb(s)? \n\n- {0}", string.Join("\n- ", site.Webs.ToArray()));
                if (MessageBox.Show(question, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    foreach (string weburl in site.Webs)
                    {
                        TreeNode node = _selectedContextMenuNode.Nodes.OfType<TreeNode>().FirstOrDefault(t => t.Text.ToLower().Contains(weburl.ToLower()));
                        if (node != null)
                            _selectedContextMenuNode.Nodes.Remove(node);
                    }

                    site.Webs.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void AddTenant()
        {
            AddTenant(null);
        }

        private void AddTenant(TenantAuthentication tenant)
        {
            try
            {
                AddTenantForm form = new AddTenantForm(tenant);
                DialogResult result = form.ShowDialog(this);

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    this.LoadTenants();
                    TaskBar.UpdateRecentItemsJumpList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void BrowseMSDNLink(TreeNode node)
        {
            try
            {
                Uri link = HelpUtil.GetMSDNHelpLink(node);

                if (link != null)
                    System.Diagnostics.Process.Start(link.OriginalString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void LoadContextMenuBrowsers()
        {
            foreach (Browser browser in BrowserUtil.GetBrowsers().OrderBy(b => b.Name))
            {
                AddBrowserToContextMenu(browseToolStripMenuItem, browser, false, openItemWithBrowser_Click);
                AddBrowserToContextMenu(settingsToolStripMenuItem, browser, false, browseSettingsWithBrowser_Click);

                if (browser.SupportsInPrivate)
                {
                    AddBrowserToContextMenu(browseToolStripMenuItem, browser, true, openItemWithBrowser_Click);
                    AddBrowserToContextMenu(settingsToolStripMenuItem, browser, true, browseSettingsWithBrowser_Click);
                }
            }
        }

        private void AddBrowserToContextMenu(ToolStripMenuItem parentMenuItem, Browser browser, bool isPrivateMode, EventHandler eventHandler)
        {
            ToolStripMenuItem menu = new ToolStripMenuItem(string.Format("Open with {1}{0}", isPrivateMode ? Constants.BROWSER_IN_PRIVATE_MODE_LABEL : string.Empty, browser.Name));
            menu.Click += eventHandler;
            menu.Tag = browser;

            Icon browserIcon = BrowserUtil.GetBrowserIcon(browser);
            menu.Image = browserIcon == null ? null : browserIcon.ToBitmap();

            parentMenuItem.DropDownItems.Add(menu);
        }

        private void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }

        private void ToggleMenuOptions()
        {
            try
            {
                // Context Menu
                browseToolStripMenuItem.Enabled = false;
                settingsToolStripMenuItem.Enabled = false;
                openWebDavToolStripMenuItem.Enabled = false;
                openPowerShellToolStripMenuItem.Enabled = false;

                // Button menu
                openInBrowserToolStripButton.Enabled = false;
                browseSettingsToolStripButton.Enabled = false;
                openPowerShellToolStripButton.Enabled = false;
                browseMSDNtoolStripButton.Enabled = false;
                refreshToolStripButton.Enabled = false;

                // Tabs
                ((Control)tpRest).Enabled = false;

                if (_selectedContextMenuNode != null && _selectedContextMenuNode.Tag != null)
                {
                    if (_selectedContextMenuNode.Tag.GetType() == typeof(SPBrowser.NodeLoadType))
                    {
                        // Button menu
                        if (!_selectedContextMenuNode.Tag.Equals(NodeLoadType.GeneralGroup))
                            refreshToolStripButton.Enabled = true;
                    }
                    else
                    {
                        // Button menu
                        browseMSDNtoolStripButton.Enabled = true;
                    }

                    // "Open in Browser...", "Browse Settings...", "Open PowerShell with CSOM" and "Open in WebDav" buttons & menu's
                    switch (_selectedContextMenuNode.Tag.GetClientType())
                    {
                        case ClientType.Tenant:
                            // Button menu and Context Menu for PowerShell
                            openPowerShellToolStripButton.Enabled = true;
                            openPowerShellToolStripMenuItem.Enabled = true;
                            break;
                        case ClientType.Site:
                            // Button menu and Context Menu for PowerShell, only enable for Site Collection directly added, not via Office 365 Tenant (not supported, see https://spcb.codeplex.com/workitem/63889)
                            // TODO: Add support for PowerShell on site collections below Office 365 Tenant, currently not supported
                            if (_selectedContextMenuNode.Parent.Parent == null)
                            {
                                openPowerShellToolStripButton.Enabled = true;
                                openPowerShellToolStripMenuItem.Enabled = true;
                            }
                            break;
                        case ClientType.Web:
                        case ClientType.List:
                            // Context Menu for both
                            browseToolStripMenuItem.Enabled = true;
                            settingsToolStripMenuItem.Enabled = true;
                            openWebDavToolStripMenuItem.Enabled = true;

                            // Button menu for both
                            openInBrowserToolStripButton.Enabled = true;
                            browseSettingsToolStripButton.Enabled = true;
                            break;
                        case ClientType.ListItem:
                            // Context Menu for both
                            browseToolStripMenuItem.Enabled = true;
                            settingsToolStripMenuItem.Enabled = true;

                            // Button menu for both
                            openInBrowserToolStripButton.Enabled = true;
                            browseSettingsToolStripButton.Enabled = true;
                            break;
                        case ClientType.Folder:
                            refreshToolStripButton.Enabled = true;
                            refreshToolStripMenuItem.Enabled = true;
                            break;
                        case ClientType.File:
                        case ClientType.View:
                        case ClientType.PersonProperties:
                        case ClientType.AppInstance:
                            // Context Menu Open in browser...
                            browseToolStripMenuItem.Enabled = true;

                            // Button menu Open in browser...
                            openInBrowserToolStripButton.Enabled = true;
                            break;
                        case ClientType.Feature:
                        case ClientType.ContentType:
                        case ClientType.Field:
                        case ClientType.FieldCalculated:
                        case ClientType.FieldChoice:
                        case ClientType.FieldComputed:
                        case ClientType.FieldDateTime:
                        case ClientType.FieldGuid:
                        case ClientType.FieldLookup:
                        case ClientType.FieldMultiChoice:
                        case ClientType.FieldMultiLineText:
                        case ClientType.FieldNumber:
                        case ClientType.FieldRatingScale:
                        case ClientType.FieldText:
                        case ClientType.FieldUrl:
                        case ClientType.FieldUser:
                        case ClientType.TaxonomyField:
                        case ClientType.User:
                        case ClientType.Group:
                            // Button menu Settings
                            browseSettingsToolStripButton.Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void RefreshCollection()
        {
            try
            {
                if (_selectedContextMenuNode != null && _selectedContextMenuNode.Tag != null && _selectedContextMenuNode.Tag.GetType().Equals(typeof(NodeLoadType)))
                {
                    // Determine load type
                    NodeLoadType loadType = _selectedContextMenuNode.Tag.GetType().Equals(typeof(NodeLoadType)) ? (NodeLoadType)_selectedContextMenuNode.Tag : (NodeLoadType)_selectedContextMenuNode.FirstNode.Tag;

                    // Load nodes
                    LoadNodes(loadType, _selectedContextMenuNode);
                }

                // Refreshing folders
                if (_selectedContextMenuNode != null && _selectedContextMenuNode.Tag != null && _selectedContextMenuNode.Tag.GetType().Equals(typeof(SPClient.Folder)))
                {
                    LoadNodes(NodeLoadType.Folder, _selectedContextMenuNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private string GetOpenItemUrl()
        {
            string url = string.Empty;

            try
            {

                switch (_selectedContextMenuNode.Tag.GetClientType())
                {
                    case ClientType.Web:
                        url = ((SPClient.Web)_selectedContextMenuNode.Tag).GetUrl();
                        break;
                    case ClientType.List:
                        url = ((SPClient.List)_selectedContextMenuNode.Tag).GetListUrl();
                        break;
                    case ClientType.ListItem:
                        url = ((SPClient.ListItem)_selectedContextMenuNode.Tag).GetDisplayItemUrl();
                        break;
                    case ClientType.Folder:
                        url = ((SPClient.Folder)_selectedContextMenuNode.Tag).GetFolderUrl();
                        break;
                    case ClientType.File:
                        url = ((SPClient.File)_selectedContextMenuNode.Tag).GetUrl();
                        break;
                    case ClientType.View:
                        url = ((SPClient.View)_selectedContextMenuNode.Tag).GetViewUrl();
                        break;
                    case ClientType.PersonProperties:
                        url = ((PersonProperties)_selectedContextMenuNode.Tag).GetUserProfileUrl();
                        break;
                    case ClientType.AppInstance:
                        url = ((SPClient.AppInstance)_selectedContextMenuNode.Tag).GetAppRedirectUrl(_selectedContextMenuNode);
                        break;
                    default:
                        // No action. Not implemented.
                        break;
                }

                LogUtil.LogMessage("Retrieved URL to open in browser: " + url, LogLevel.Information);

            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return url;
        }

        private string GetBrowseSettingsUrl()
        {
            string url = string.Empty;

            try
            {

                switch (_selectedContextMenuNode.Tag.GetClientType())
                {
                    case ClientType.Web:
                        url = ((SPClient.Web)_selectedContextMenuNode.Tag).GetSettingsUrl();
                        break;
                    case ClientType.List:
                        url = ((SPClient.List)_selectedContextMenuNode.Tag).GetSettingsUrl();
                        break;
                    case ClientType.ListItem:
                        url = ((SPClient.ListItem)_selectedContextMenuNode.Tag).GetEditItemUrl();
                        break;
                    case ClientType.Feature:
                        url = ((SPClient.Feature)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);
                        break;
                    case ClientType.ContentType:
                        url = ((SPClient.ContentType)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);
                        break;
                    case ClientType.Group:
                        url = ((SPClient.Group)_selectedContextMenuNode.Tag).GetSettingsUrl();
                        break;
                    case ClientType.User:
                        url = ((SPClient.User)_selectedContextMenuNode.Tag).GetSettingsUrl();
                        break;
                    case ClientType.Field:
                    case ClientType.FieldCalculated:
                    case ClientType.FieldChoice:
                    case ClientType.FieldComputed:
                    case ClientType.FieldDateTime:
                    case ClientType.FieldGuid:
                    case ClientType.FieldLookup:
                    case ClientType.FieldMultiChoice:
                    case ClientType.FieldMultiLineText:
                    case ClientType.FieldNumber:
                    case ClientType.FieldRatingScale:
                    case ClientType.FieldText:
                    case ClientType.FieldUrl:
                    case ClientType.FieldUser:
                    case ClientType.TaxonomyField:
                        url = ((SPClient.Field)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);
                        break;
                    default:
                        // No action. Not implemented.
                        break;
                }
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return url;
        }

        private void OpenInWebDav(bool useAlternativeBrowser = false)
        {
            // TODO: Fix opening in default browser (also interesting...) but requires IE!
            try
            {
                string url = string.Empty;

                switch (_selectedContextMenuNode.Tag.GetClientType())
                {
                    case ClientType.Web:
                        url = ((SPClient.Web)_selectedContextMenuNode.Tag).GetSiteWebDavUrl();
                        break;
                    case ClientType.List:
                        url = ((SPClient.List)_selectedContextMenuNode.Tag).GetListWebDavUrl();
                        break;
                    default:
                        // No action. Not implemented.
                        break;
                }

                BrowserUtil.StartBrowserWithUrl(BrowserUtil.GetSystemDefaultBrowser(), url);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReportABug()
        {
            try
            {
                System.Diagnostics.Process.Start(Constants.CODEPLEX_ISSUE_LIST_URL);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUtil.LogException(ex);
            }
        }

        private void OpenPowerShellConsole()
        {
            if (_selectedContextMenuNode.Tag.GetType() == typeof(SPClient.Site))
                this.OpenSitePowerShellConsole();
            else
                this.OpenTenantPowerShellConsole();
        }

        private void OpenTenantPowerShellConsole()
        {
            try
            {
                TenantAuthentication tenant = Globals.Tenants[((SPTenantAdmin.Tenant)_selectedContextMenuNode.Tag).RootSiteUrl];

                string psFile = PowerShellUtil.CreatePowerShellScript(tenant);
                string cmd = string.Format("{0}\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows));

                if (!File.Exists(cmd))
                {
                    LogUtil.LogMessage(string.Format("Can't find file '{0}'.", cmd), LogLevel.Exception);
                    throw new ApplicationException("Can't locate Windows PowerShell.");
                }

                System.Diagnostics.Process.Start(cmd, string.Format("-NoExit -File \"{0}\"", psFile));
            }
            catch (FileNotFoundException ex)
            {
                LogUtil.LogException(string.Format("File '{0}' not found, check log file {1} for detailed information.", ex.FileName, ex.FusionLog), ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenSitePowerShellConsole()
        {
            try
            {
                SiteAuthentication site = Globals.Sites[((SPClient.Site)_selectedContextMenuNode.Tag).Url];

                string psFile = PowerShellUtil.CreatePowerShellScript(site);
                string cmd = string.Format("{0}\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows));

                if (!File.Exists(cmd))
                {
                    LogUtil.LogMessage(string.Format("Can't find file '{0}'.", cmd), LogLevel.Exception);
                    throw new ApplicationException("Can't locate Windows PowerShell.");
                }

                System.Diagnostics.Process.Start(cmd, string.Format("-NoExit -File \"{0}\"", psFile));
            }
            catch (FileNotFoundException ex)
            {
                LogUtil.LogException(string.Format("File '{0}' not found, check log file {1} for detailed information.", ex.FileName, ex.FusionLog), ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeXmlViewer()
        {
            // Initialize the XMLViewerSettings.
            XMLViewerSettings viewerSetting = new XMLViewerSettings
            {
                AttributeKey = Color.Red,
                AttributeValue = Color.Blue,
                Tag = Color.Blue,
                Element = Color.DarkRed,
                Value = Color.Black,
            };

            xvSchemaXML.Settings = viewerSetting;
        }

        private void SetWindowSize(int width, int height)
        {
            if (this.WindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Normal;

            this.Width = width;
            this.Height = height;
        }

        private void ShowAllPropertiesWarning()
        {
            // Enable warning message in status bar
            tsAllPropertiesWarning.Visible = Configuration.Current.LoadAllProperties;
        }

        #endregion
    }
}
