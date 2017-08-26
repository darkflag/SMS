using SPBrowser.Extentions;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser
{
    public partial class MainBrowser : Form
    {
        private TreeNode _selectedContextMenuNode;

        public MainBrowser()
        {
            InitializeComponent();

            this.Text = Application.ProductName;

            // Load recent sites
            this.LoadRecentSites();

            // Set sort
            tvSite.TreeViewNodeSorter = new TreeViewSorter();
            tvSite.Sort();

            // Update progressbar
            SPLoader.ItemLoaded += SPLoader_ItemLoaded;

            // Check on updates
            this.CheckOnUpdate();

            // Toggle menu's
            this.ToggleMenuOptions();
        }

        #region Events

        private void tvSite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Select the clicked node
            _selectedContextMenuNode = tvSite.SelectedNode;

            // Set Statusbar
            tsClassName.Text = string.Format("{0}", tvSite.SelectedNode.Tag != null && tvSite.SelectedNode.Tag.GetType() != typeof(SPBrowser.LoadType) ? tvSite.SelectedNode.Tag.GetType().ToString() : string.Empty);

            // Set tabs
            this.LoadingTabProperties();
            this.LoadingTabRawData();
            this.LoadingTabMsdnBrowser();

            // Toggle menu's
            this.ToggleMenuOptions();
        }

        private void tvSite_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null &&
                e.Node.FirstNode.Text == Constants.NODE_LOADING_TEXT &&
                (e.Node.Tag.GetType().Equals(typeof(LoadType)) || e.Node.FirstNode.Tag.GetType().Equals(typeof(LoadType))))
            {
                // Determine load type
                LoadType loadType = e.Node.Tag.GetType().Equals(typeof(LoadType)) ? (LoadType)e.Node.Tag : (LoadType)e.Node.FirstNode.Tag;

                // Load nodes
                LoadNodes(loadType, e.Node);
            }
        }

        private void tvSite_MouseDown(object sender, MouseEventArgs e)
        {
            // Select the clicked node
            _selectedContextMenuNode = tvSite.GetNodeAt(e.X, e.Y);
        }

        private void tvSite_KeyUp(object sender, KeyEventArgs e)
        {
            // Select the clicked node
            //_selectedContextMenuNode = tvSite.SelectedNode;
        }

        public void item_Click(object sender, EventArgs e)
        {
            SiteAuth site = Globals.SiteCollections.SingleOrDefault(s => s.UrlAsString.Equals(sender.ToString()));

            AddSite form = new AddSite(site);
            DialogResult result = form.ShowDialog(this);

            if (result == DialogResult.OK)
                this.LoadSiteCollections();
        }

        public void tsClassName_Click(object sender, EventArgs e)
        {
            Uri url = SPHelp.GetMSDNHelpLink(tvSite.SelectedNode);

            if (url != null)
                System.Diagnostics.Process.Start(url.OriginalString);
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadingTabProperties();
            this.LoadingTabRawData();
            this.LoadingTabMsdnBrowser();
        }

        private void wbMsdnHelp_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            try
            {
                tsProgressBar.Minimum = 0;
                tsProgressBar.Maximum = (int)e.MaximumProgress;

                if (e.CurrentProgress >= 0)
                    tsProgressBar.Value = (int)e.CurrentProgress;
            }
            catch { }
        }

        private void browseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenInBrowser();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BrowseSettings();
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.ShowDialog(this);
        }

        private void addSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSite();
        }

        private void addSiteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddSite();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSite();
        }

        private void browseMSDNHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.BrowseMSDNLink(_selectedContextMenuNode);
        }

        private void addSpecificWebToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiteAuth site = Globals.SiteCollections.SingleOrDefault(s => s.UrlAsString.Equals(_selectedContextMenuNode.Text, StringComparison.InvariantCultureIgnoreCase));

            AddWeb form = new AddWeb(site);
            DialogResult result = form.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
                LoadSiteCollections();
        }

        private void mnContextItem_Opened(object sender, EventArgs e)
        {
            ToggleMenuOptions();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CheckOnUpdate(false);
        }

        private void openCodeplexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Constants.CODEPLEX_PROJECT_URL);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RefreshCollection();
        }

        public void SPLoader_ItemLoaded(object sender, ItemLoadedEventArgs e)
        {
            tsProgressBar.Minimum = 0;
            tsProgressBar.Maximum = e.TotalItem;

            if (tsProgressBar.Maximum >= e.CurrentItem)
                tsProgressBar.Value = e.CurrentItem;
        }

        #endregion

        #region Methods

        private void CheckOnUpdate(bool ignoreNoUpdate = true)
        {
            Version newVersion;
            Uri downloadUrl;
            string updateTitle;

            if (ProductUtil.IsNewUpdateAvailable(out newVersion, out downloadUrl, out updateTitle))
            {
                if (MessageBox.Show(
                    string.Format("A new version '{0}' is available! \n\nDo you want to download the new version.",
                        updateTitle),
                    this.Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(downloadUrl.OriginalString);
                }
            }
            else if (!ignoreNoUpdate)
            {
                MessageBox.Show(
                    string.Format("No new version of '{0}' is available at the moment. Please check later...",
                        Application.ProductName),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void LoadRecentSites()
        {
            recentSitesToolStripMenuItem.DropDownItems.Clear();

            // Add history to menu item
            foreach (SiteAuth site in Globals.SiteCollections.OrderByDescending(s => s.LoadDate))
            {
                ToolStripItem item = recentSitesToolStripMenuItem.DropDownItems.Add(site.UrlAsString);
                item.Click += item_Click;
                item.ToolTipText = string.Format("Last opened: {0} {1}\nAuthentication: {2}\nUsername: {3}",
                    site.LoadDate.ToLongDateString(),
                    site.LoadDate.ToShortTimeString(),
                    site.Authentication == AuthN.Default ? "Default" : "SharePoint Online",
                    site.Username);
            }

            // Enable menu item
            if (recentSitesToolStripMenuItem.DropDownItems.Count == 0)
                recentSitesToolStripMenuItem.Enabled = false;
            else
                recentSitesToolStripMenuItem.Enabled = true;
        }

        private void LoadSiteCollections()
        {
            // Prepare root node
            TreeNode rootNode = tvSite.Nodes[0];
            rootNode.Nodes.Clear();

            // Add site collections from Global list
            foreach (SiteAuth sc in Globals.SiteCollections.Where(s => s.IsLoaded))
            {
                SPLoader.LoadSite(rootNode, sc, this);
            }

            // Ensure recent sites are in sync
            this.LoadRecentSites();

            // Expand root node
            rootNode.Expand();
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
            if (tabs.SelectedTab == tpMsdn)
            {
                Uri url = SPHelp.GetMSDNHelpLink(tvSite.SelectedNode);
                if (url != null)
                {
                    if (wbMsdnHelp.Url != url)
                        wbMsdnHelp.Url = url;
                }
                else if (wbMsdnHelp.Url != SPHelp.GetMSDNHelpLink())
                {
                    wbMsdnHelp.Url = SPHelp.GetMSDNHelpLink();
                }
            }
        }

        private void LoadingTabRawData()
        {
            gvRawData.Rows.Clear();

            if (tabs.SelectedTab == tpRawData)
                SPLoader.LoadRawData(tvSite.SelectedNode, gvRawData);

            tsProgressBar.Value = 0;
        }

        private void LoadNodes(LoadType loadType, TreeNode node)
        {
            // Initiate treeview load
            tvSite.BeginUpdate();

            // Clear Loading... node
            node.Nodes.Clear();

            // Load node contents
            switch (loadType)
            {
                case LoadType.SiteFields:
                    SPLoader.LoadSiteColumns(node, ((SPClient.Site)node.Parent.Tag).RootWeb.AvailableFields, this, loadType);
                    break;
                case LoadType.SiteContentTypes:
                    SPLoader.LoadContentTypes(node, ((SPClient.Site)node.Parent.Tag).RootWeb.AvailableContentTypes, this, loadType);
                    break;
                case LoadType.SiteFeatures:
                    SPLoader.LoadFeatures(node, ((SPClient.Site)node.Parent.Tag).Features, this, loadType);
                    break;
                case LoadType.SiteEventReceivers:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.SiteRecycleBin:
                    SPLoader.LoadRecycleBin(node, ((SPClient.Site)node.Parent.Tag).RecycleBin, this, loadType);
                    break;
                case LoadType.SiteWebTemplates:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.WebSubWebs:
                    SPLoader.LoadSubWebs(node, ((SPClient.Web)node.Parent.Tag).GetSubwebsForCurrentUser(null), this);
                    break;
                case LoadType.WebFields:
                    SPLoader.LoadSiteColumns(node, ((SPClient.Web)node.Parent.Tag).Fields, this, loadType);
                    break;
                case LoadType.WebContentTypes:
                    SPLoader.LoadContentTypes(node, ((SPClient.Web)node.Parent.Tag).ContentTypes, this, loadType);
                    break;
                case LoadType.WebLists:
                    SPLoader.LoadLists(node, ((SPClient.Web)node.Parent.Tag).Lists, this);
                    break;
                case LoadType.WebFeatures:
                    SPLoader.LoadFeatures(node, ((SPClient.Web)node.Parent.Tag).Features, this, loadType);
                    break;
                case LoadType.WebUsers:
                    SPLoader.LoadSiteUsers(node, ((SPClient.Web)node.Parent.Tag).SiteUserInfoList, this);
                    break;
                case LoadType.WebGroups:
                    SPLoader.LoadSiteGroups(node, ((SPClient.Web)node.Parent.Tag).SiteGroups, this);
                    break;
                case LoadType.WebRecycleBin:
					// Not supported in SP2010 CSOM
                    break;
                case LoadType.GroupUsers:
                    SPLoader.LoadUsers(node, ((SPClient.Group)node.Tag).Users, this, loadType);
                    break;
                case LoadType.WebWorkflowAssociations:
                    SPLoader.LoadWorkflowAssociations(node, ((SPClient.Web)node.Parent.Tag).WorkflowAssociations, this, loadType);
                    break;
                case LoadType.WebWorkflowTemplates:
                    SPLoader.LoadWorkflowTemplates(node, ((SPClient.Web)node.Parent.Tag).WorkflowTemplates, this, loadType);
                    break;
                case LoadType.WebEventReceivers:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.WebProperties:
                    SPLoader.LoadProperties(node, ((SPClient.Web)node.Parent.Tag).AllProperties, this, loadType);
                    break;
                case LoadType.WebListTemplates:
                    SPLoader.LoadListTemplates(node, ((SPClient.Web)node.Parent.Tag).ListTemplates, this, loadType);
                    break;
                case LoadType.WebPushNotificationSubscribers:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.WebRoleAssignments:
                    SPLoader.LoadRoleAssignments(node, ((SPClient.Web)node.Parent.Tag).RoleAssignments, this, loadType);
                    break;
                case LoadType.WebRoleDefinitions:
                    SPLoader.LoadRoleDefinitions(node, ((SPClient.Web)node.Parent.Tag).RoleDefinitions, this, loadType);
                    break;
                case LoadType.WebTemplates:
                    SPLoader.LoadWebTemplates(node, ((SPClient.Web)node.Parent.Tag).GetAvailableWebTemplates(((SPClient.Web)node.Parent.Tag).Language, true), this, loadType);
                    break;
                case LoadType.Folder:
                    SPLoader.LoadSubFolders(node, ((SPClient.Folder)node.Tag), this);
                    break;
                case LoadType.FolderProperties:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.ListFields:
                    SPLoader.LoadSiteColumns(node, ((SPClient.List)node.Parent.Tag).Fields, this, loadType);
                    break;
                case LoadType.ListContentTypes:
                    SPLoader.LoadContentTypes(node, ((SPClient.List)node.Parent.Tag).ContentTypes, this, loadType);
                    break;
                case LoadType.ListViews:
                    SPLoader.LoadViews(node, ((SPClient.List)node.Parent.Tag).Views, this);
                    break;
                case LoadType.ListWorkflowAssociations:
                    SPLoader.LoadWorkflowAssociations(node, ((SPClient.List)node.Parent.Tag).WorkflowAssociations, this, loadType);
                    break;
                case LoadType.ListEventReceivers:
                    // Not supported in SP2010 CSOM
                    break;
                case LoadType.ListRoleAssignments:
                    SPLoader.LoadRoleAssignments(node, ((SPClient.List)node.Parent.Tag).RoleAssignments, this, loadType);
                    break;
                default:
                    break;
            }

            // End treeview load
            tvSite.EndUpdate();
            tsProgressBar.Value = 0;
        }

        private void AddSite()
        {
            AddSite form = new AddSite();
            DialogResult result = form.ShowDialog(this);

            if (result == System.Windows.Forms.DialogResult.OK)
                this.LoadSiteCollections();
        }

        private void RemoveSite()
        {
            // Remove from global site collection list
            SiteAuth site = Globals.SiteCollections.SingleOrDefault(s => s.Url.OriginalString.Equals(((SPClient.Site)_selectedContextMenuNode.Tag).Url, StringComparison.InvariantCultureIgnoreCase));
            Globals.SiteCollections.Remove(site);

            // Remove node
            _selectedContextMenuNode.Remove();
        }

        private void BrowseMSDNLink(TreeNode node)
        {
            Uri link = SPHelp.GetMSDNHelpLink(node);

            if (link != null)
                System.Diagnostics.Process.Start(link.OriginalString);
        }

        private void ToggleMenuOptions()
        {
            // Context Menu
            browseToolStripMenuItem.Enabled = false;
            settingsToolStripMenuItem.Enabled = false;

            // Button menu
            //openInBrowserToolStripButton.Enabled = false;
            //browseSettingsToolStripButton.Enabled = false;
            //openPowerShellToolStripButton.Enabled = false;
            //browseMSDNtoolStripButton.Enabled = false;
            //refreshToolStripButton.Enabled = false;

            if (_selectedContextMenuNode != null && _selectedContextMenuNode.Tag != null)
            {
                //if (_selectedContextMenuNode.Tag.GetType() == typeof(SPBrowser.LoadType))
                //{
                //    // Button menu
                //    if (!_selectedContextMenuNode.Tag.Equals(LoadType.GeneralGroup))
                //        refreshToolStripButton.Enabled = true;
                //}
                //else
                //{
                //    // Button menu
                //    browseMSDNtoolStripButton.Enabled = true;
                //}

                // "Open in Browser...", "Browse Settings..." and "Open PowerShell with CSOM" buttons & menu's
                switch (_selectedContextMenuNode.Tag.GetType().ToString())
                {
                    case Constants.NS_SITE:
                        //openPowerShellToolStripButton.Enabled = true;
                        break;
                    case Constants.NS_WEB:
                    case Constants.NS_LIST:
                        // Context Menu
                        browseToolStripMenuItem.Enabled = true;
                        settingsToolStripMenuItem.Enabled = true;

                        // Button menu
                        //openInBrowserToolStripButton.Enabled = true;
                        //browseSettingsToolStripButton.Enabled = true;
                        break;
                    case Constants.NS_FOLDER:
                    case Constants.NS_FILE:
                    case Constants.NS_VIEW:
                        // Context Menu
                        browseToolStripMenuItem.Enabled = true;

                        // Button menu
                        //openInBrowserToolStripButton.Enabled = true;
                        break;
                    case Constants.NS_FEATURE:
                    case Constants.NS_CONTENT_TYPE:
                    case Constants.NS_SITE_USER:
                    case Constants.NS_SITE_GROUP:
                        // Context Menu
                        settingsToolStripMenuItem.Enabled = true;

                        // Button menu
                        //browseSettingsToolStripButton.Enabled = true;
                        break;
                    default:
                        break;
                }

                // Catch all field class types
                if (_selectedContextMenuNode.Tag.GetType().ToString().StartsWith(Constants.NS_FIELD))
                {
                    // Context Menu
                    settingsToolStripMenuItem.Enabled = true;

                    // Button menu
                    //browseSettingsToolStripButton.Enabled = true;
                }
            }
        }

        private void RefreshCollection()
        {
            if (_selectedContextMenuNode != null && _selectedContextMenuNode.Tag != null && _selectedContextMenuNode.Tag.GetType().Equals(typeof(LoadType)))
            {
                // Determine load type
                LoadType loadType = _selectedContextMenuNode.Tag.GetType().Equals(typeof(LoadType)) ? (LoadType)_selectedContextMenuNode.Tag : (LoadType)_selectedContextMenuNode.FirstNode.Tag;

                // Load nodes
                LoadNodes(loadType, _selectedContextMenuNode);
            }
        }

        private void OpenInBrowser()
        {
            string url = string.Empty;

            switch (_selectedContextMenuNode.Tag.GetType().ToString())
            {
                case Constants.NS_WEB:
                    url = ((SPClient.Web)_selectedContextMenuNode.Tag).GetWebUrl();
                    break;
                case Constants.NS_LIST:
                    url = ((SPClient.List)_selectedContextMenuNode.Tag).GetListUrl();
                    break;
                case Constants.NS_FOLDER:
                    url = ((SPClient.Folder)_selectedContextMenuNode.Tag).GetFolderUrl();
                    break;
                case Constants.NS_FILE:
                    url = ((SPClient.File)_selectedContextMenuNode.Tag).GetFileUrl();
                    break;
                case Constants.NS_VIEW:
                    url = ((SPClient.View)_selectedContextMenuNode.Tag).GetViewUrl();
                    break;
                default:
                    // No action. Not implemented.
                    break;
            }

            if (!string.IsNullOrEmpty(url))
                System.Diagnostics.Process.Start(url);
        }

        private void BrowseSettings()
        {
            string url = string.Empty;

            switch (_selectedContextMenuNode.Tag.GetType().ToString())
            {
                case Constants.NS_WEB:
                    url = ((SPClient.Web)_selectedContextMenuNode.Tag).GetSiteSettingsUrl();
                    break;
                case Constants.NS_LIST:
                    url = ((SPClient.List)_selectedContextMenuNode.Tag).GetSettingsUrl();
                    break;
                case Constants.NS_FEATURE:
                    url = ((SPClient.Feature)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);
                    break;
                case Constants.NS_CONTENT_TYPE:
                    url = ((SPClient.ContentType)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);
                    break;
                case Constants.NS_SITE_GROUP:
                    url = ((SPClient.Group)_selectedContextMenuNode.Tag).GetSettingsUrl();
                    break;
                case Constants.NS_SITE_USER:
                    url = ((SPClient.User)_selectedContextMenuNode.Tag).GetSettingsUrl();
                    break;
                default:
                    // No action. Not implemented.
                    break;
            }

            // Catch all field class types
            if (_selectedContextMenuNode.Tag.GetType().ToString().StartsWith(Constants.NS_FIELD))
                url = ((SPClient.Field)_selectedContextMenuNode.Tag).GetSettingsUrl(_selectedContextMenuNode);

            if (!string.IsNullOrEmpty(url))
                System.Diagnostics.Process.Start(url);
        }

        private void OpenPowerShellConsole()
        {
            SiteAuth site = Globals.SiteCollections.SingleOrDefault(s => s.Url.OriginalString.Equals(((SPClient.Site)_selectedContextMenuNode.Tag).Url, StringComparison.InvariantCultureIgnoreCase));
            string psFile = PowerShellUtil.CreatePowerShellScript(site);
            System.Diagnostics.Process.Start("powershell.exe", string.Format("-NoExit -File \"{0}\"", psFile));
        }

        #endregion
    }
}
