namespace SPBrowser
{
    partial class MainBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Site Collections");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainBrowser));
            this.mnContextRootSiteCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvSite = new System.Windows.Forms.TreeView();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.tabs = new System.Windows.Forms.TabControl();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.pgProperties = new System.Windows.Forms.PropertyGrid();
            this.tpRawData = new System.Windows.Forms.TabPage();
            this.gvRawData = new System.Windows.Forms.DataGridView();
            this.tpMsdn = new System.Windows.Forms.TabPage();
            this.wbMsdnHelp = new System.Windows.Forms.WebBrowser();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.mnContextSite = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSpecificWebToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.browseMSDNHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnContextItem = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.browseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.browseMSDNHelpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.recentSitesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCodeplexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.tsClassName = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnContextCollection = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnContextRootSiteCollection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.tpRawData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvRawData)).BeginInit();
            this.tpMsdn.SuspendLayout();
            this.mnContextSite.SuspendLayout();
            this.mnContextItem.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.mnContextCollection.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnContextRootSiteCollection
            // 
            this.mnContextRootSiteCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSiteToolStripMenuItem});
            this.mnContextRootSiteCollection.Name = "mnContextSiteCollection";
            this.mnContextRootSiteCollection.Size = new System.Drawing.Size(128, 26);
            // 
            // addSiteToolStripMenuItem
            // 
            this.addSiteToolStripMenuItem.Name = "addSiteToolStripMenuItem";
            this.addSiteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.addSiteToolStripMenuItem.Text = "Add Site...";
            this.addSiteToolStripMenuItem.Click += new System.EventHandler(this.addSiteToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvSite);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabs);
            this.splitContainer1.Size = new System.Drawing.Size(1435, 731);
            this.splitContainer1.SplitterDistance = 478;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvSite
            // 
            this.tvSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSite.HideSelection = false;
            this.tvSite.ImageIndex = 0;
            this.tvSite.ImageList = this.ilIcons;
            this.tvSite.Location = new System.Drawing.Point(0, 0);
            this.tvSite.Name = "tvSite";
            treeNode1.ContextMenuStrip = this.mnContextRootSiteCollection;
            treeNode1.Name = "Node0";
            treeNode1.Text = "Site Collections";
            this.tvSite.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvSite.SelectedImageIndex = 0;
            this.tvSite.Size = new System.Drawing.Size(478, 731);
            this.tvSite.TabIndex = 0;
            this.tvSite.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvSite_AfterExpand);
            this.tvSite.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSite_AfterSelect);
            this.tvSite.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvSite_KeyUp);
            this.tvSite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvSite_MouseDown);
            // 
            // ilIcons
            // 
            this.ilIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilIcons.ImageStream")));
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilIcons.Images.SetKeyName(0, "sharepointfoundation16.png");
            this.ilIcons.Images.SetKeyName(1, "16x16.png");
            this.ilIcons.Images.SetKeyName(2, "ContentType.png");
            this.ilIcons.Images.SetKeyName(3, "ERRSM.GIF");
            this.ilIcons.Images.SetKeyName(4, "EventReceiver.png");
            this.ilIcons.Images.SetKeyName(5, "gbwapset.gif");
            this.ilIcons.Images.SetKeyName(6, "GenericFeature.gif");
            this.ilIcons.Images.SetKeyName(7, "GenericFeatureCustom.gif");
            this.ilIcons.Images.SetKeyName(8, "ICXDDOC.GIF");
            this.ilIcons.Images.SetKeyName(9, "ITAL.PNG");
            this.ilIcons.Images.SetKeyName(10, "ITANN.PNG");
            this.ilIcons.Images.SetKeyName(11, "itapprequests.png");
            this.ilIcons.Images.SetKeyName(12, "ITCOMMCAT.PNG");
            this.ilIcons.Images.SetKeyName(13, "ITCOMMEM.PNG");
            this.ilIcons.Images.SetKeyName(14, "ITCONTCT.PNG");
            this.ilIcons.Images.SetKeyName(15, "ITDISC.PNG");
            this.ilIcons.Images.SetKeyName(16, "ITDL.PNG");
            this.ilIcons.Images.SetKeyName(17, "ITEVENT.PNG");
            this.ilIcons.Images.SetKeyName(18, "ITGEN.GIF");
            this.ilIcons.Images.SetKeyName(19, "ITGEN.PNG");
            this.ilIcons.Images.SetKeyName(20, "ITLINK.PNG");
            this.ilIcons.Images.SetKeyName(21, "ITSURVEY.PNG");
            this.ilIcons.Images.SetKeyName(22, "ITTASK.PNG");
            this.ilIcons.Images.SetKeyName(23, "ITWFH.PNG");
            this.ilIcons.Images.SetKeyName(24, "ITWP.PNG");
            this.ilIcons.Images.SetKeyName(25, "ListSettings.png");
            this.ilIcons.Images.SetKeyName(26, "Open.png");
            this.ilIcons.Images.SetKeyName(27, "personresult.gif");
            this.ilIcons.Images.SetKeyName(28, "Settings.png");
            this.ilIcons.Images.SetKeyName(29, "SharePoint.png");
            this.ilIcons.Images.SetKeyName(30, "sharepointfoundation16-warning.png");
            this.ilIcons.Images.SetKeyName(31, "sharethissite.png");
            this.ilIcons.Images.SetKeyName(32, "SiteColumn.png");
            this.ilIcons.Images.SetKeyName(33, "SiteGroup.png");
            this.ilIcons.Images.SetKeyName(34, "siteicon_16x16.png");
            this.ilIcons.Images.SetKeyName(35, "sitelaunchsharedanonymously.png");
            this.ilIcons.Images.SetKeyName(36, "SubSite.png");
            this.ilIcons.Images.SetKeyName(37, "User.png");
            this.ilIcons.Images.SetKeyName(38, "USERS.GIF");
            this.ilIcons.Images.SetKeyName(39, "workflows.png");
            this.ilIcons.Images.SetKeyName(40, "Property.png");
            this.ilIcons.Images.SetKeyName(41, "File.png");
            this.ilIcons.Images.SetKeyName(42, "RecycleBin.png");
            this.ilIcons.Images.SetKeyName(43, "Refresh.png");
            this.ilIcons.Images.SetKeyName(44, "SiteGroupDistributionList.png");
            this.ilIcons.Images.SetKeyName(45, "View.png");
            this.ilIcons.Images.SetKeyName(46, "ListTemplate.png");
            this.ilIcons.Images.SetKeyName(47, "WebTemplate.png");
            this.ilIcons.Images.SetKeyName(48, "Permission2.png");
            this.ilIcons.Images.SetKeyName(49, "Permission.png");
            this.ilIcons.Images.SetKeyName(50, "workflows2.png");
            this.ilIcons.Images.SetKeyName(51, "GroupCollection1.png");
            this.ilIcons.Images.SetKeyName(52, "GroupCollection.png");
            this.ilIcons.Images.SetKeyName(53, "SiteGroupSecurityGroup.png");
            this.ilIcons.Images.SetKeyName(54, "UserExclamation.png");
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tpProperties);
            this.tabs.Controls.Add(this.tpRawData);
            this.tabs.Controls.Add(this.tpMsdn);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(952, 731);
            this.tabs.TabIndex = 1;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // tpProperties
            // 
            this.tpProperties.Controls.Add(this.pgProperties);
            this.tpProperties.Location = new System.Drawing.Point(4, 24);
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tpProperties.Size = new System.Drawing.Size(944, 703);
            this.tpProperties.TabIndex = 0;
            this.tpProperties.Text = "Properties";
            this.tpProperties.UseVisualStyleBackColor = true;
            // 
            // pgProperties
            // 
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.Location = new System.Drawing.Point(3, 3);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(938, 697);
            this.pgProperties.TabIndex = 0;
            // 
            // tpRawData
            // 
            this.tpRawData.Controls.Add(this.gvRawData);
            this.tpRawData.Location = new System.Drawing.Point(4, 22);
            this.tpRawData.Name = "tpRawData";
            this.tpRawData.Padding = new System.Windows.Forms.Padding(3);
            this.tpRawData.Size = new System.Drawing.Size(944, 705);
            this.tpRawData.TabIndex = 1;
            this.tpRawData.Text = "Raw Data";
            this.tpRawData.UseVisualStyleBackColor = true;
            // 
            // gvRawData
            // 
            this.gvRawData.AllowUserToAddRows = false;
            this.gvRawData.AllowUserToDeleteRows = false;
            this.gvRawData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvRawData.Location = new System.Drawing.Point(3, 3);
            this.gvRawData.Name = "gvRawData";
            this.gvRawData.ReadOnly = true;
            this.gvRawData.Size = new System.Drawing.Size(938, 699);
            this.gvRawData.TabIndex = 0;
            // 
            // tpMsdn
            // 
            this.tpMsdn.Controls.Add(this.wbMsdnHelp);
            this.tpMsdn.Controls.Add(this.webBrowser1);
            this.tpMsdn.Location = new System.Drawing.Point(4, 22);
            this.tpMsdn.Name = "tpMsdn";
            this.tpMsdn.Padding = new System.Windows.Forms.Padding(3);
            this.tpMsdn.Size = new System.Drawing.Size(944, 705);
            this.tpMsdn.TabIndex = 2;
            this.tpMsdn.Text = "MSDN Help";
            this.tpMsdn.UseVisualStyleBackColor = true;
            // 
            // wbMsdnHelp
            // 
            this.wbMsdnHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbMsdnHelp.Location = new System.Drawing.Point(3, 3);
            this.wbMsdnHelp.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbMsdnHelp.Name = "wbMsdnHelp";
            this.wbMsdnHelp.Size = new System.Drawing.Size(938, 699);
            this.wbMsdnHelp.TabIndex = 1;
            this.wbMsdnHelp.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.wbMsdnHelp_ProgressChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(3, 3);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(938, 699);
            this.webBrowser1.TabIndex = 0;
            // 
            // mnContextSite
            // 
            this.mnContextSite.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSpecificWebToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripSeparator2,
            this.browseMSDNHelpToolStripMenuItem});
            this.mnContextSite.Name = "mnContextSite";
            this.mnContextSite.Size = new System.Drawing.Size(197, 76);
            // 
            // addSpecificWebToolStripMenuItem
            // 
            this.addSpecificWebToolStripMenuItem.Name = "addSpecificWebToolStripMenuItem";
            this.addSpecificWebToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.addSpecificWebToolStripMenuItem.Text = "Add specific web...";
            this.addSpecificWebToolStripMenuItem.Click += new System.EventHandler(this.addSpecificWebToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(193, 6);
            // 
            // browseMSDNHelpToolStripMenuItem
            // 
            this.browseMSDNHelpToolStripMenuItem.Image = global::SPBrowser.Properties.Resources.MSDN;
            this.browseMSDNHelpToolStripMenuItem.Name = "browseMSDNHelpToolStripMenuItem";
            this.browseMSDNHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.browseMSDNHelpToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.browseMSDNHelpToolStripMenuItem.Text = "Browse &MSDN Help";
            this.browseMSDNHelpToolStripMenuItem.Click += new System.EventHandler(this.browseMSDNHelpToolStripMenuItem_Click);
            // 
            // mnContextItem
            // 
            this.mnContextItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator3,
            this.browseMSDNHelpToolStripMenuItem1});
            this.mnContextItem.Name = "mnContextWeb";
            this.mnContextItem.Size = new System.Drawing.Size(197, 76);
            this.mnContextItem.Opened += new System.EventHandler(this.mnContextItem_Opened);
            // 
            // browseToolStripMenuItem
            // 
            this.browseToolStripMenuItem.Image = global::SPBrowser.Properties.Resources.sitelaunchsharedanonymously;
            this.browseToolStripMenuItem.Name = "browseToolStripMenuItem";
            this.browseToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.browseToolStripMenuItem.Text = "&Open in browser...";
            this.browseToolStripMenuItem.Click += new System.EventHandler(this.browseToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripMenuItem.Image")));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.settingsToolStripMenuItem.Text = "Browse &Settings...";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // browseMSDNHelpToolStripMenuItem1
            // 
            this.browseMSDNHelpToolStripMenuItem1.Image = global::SPBrowser.Properties.Resources.MSDN;
            this.browseMSDNHelpToolStripMenuItem1.Name = "browseMSDNHelpToolStripMenuItem1";
            this.browseMSDNHelpToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.browseMSDNHelpToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.browseMSDNHelpToolStripMenuItem1.Text = "Browse &MSDN Help";
            this.browseMSDNHelpToolStripMenuItem1.Click += new System.EventHandler(this.browseMSDNHelpToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1435, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSiteToolStripMenuItem1,
            this.recentSitesToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // addSiteToolStripMenuItem1
            // 
            this.addSiteToolStripMenuItem1.Name = "addSiteToolStripMenuItem1";
            this.addSiteToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.addSiteToolStripMenuItem1.Text = "Add &Site...";
            this.addSiteToolStripMenuItem1.Click += new System.EventHandler(this.addSiteToolStripMenuItem1_Click);
            // 
            // recentSitesToolStripMenuItem
            // 
            this.recentSitesToolStripMenuItem.Name = "recentSitesToolStripMenuItem";
            this.recentSitesToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.recentSitesToolStripMenuItem.Text = "Recent Sites";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(134, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateToolStripMenuItem,
            this.openCodeplexToolStripMenuItem,
            this.toolStripSeparator4,
            this.aboutToolStripMenuItem});
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.infoToolStripMenuItem.Text = "&Info";
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.updateToolStripMenuItem.Text = "Check for &updates...";
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // openCodeplexToolStripMenuItem
            // 
            this.openCodeplexToolStripMenuItem.Name = "openCodeplexToolStripMenuItem";
            this.openCodeplexToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.openCodeplexToolStripMenuItem.Text = "Go to &CodePlex project";
            this.openCodeplexToolStripMenuItem.Click += new System.EventHandler(this.openCodeplexToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgressBar,
            this.tsClassName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 758);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1435, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgressBar
            // 
            this.tsProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsProgressBar.Name = "tsProgressBar";
            this.tsProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // tsClassName
            // 
            this.tsClassName.IsLink = true;
            this.tsClassName.Name = "tsClassName";
            this.tsClassName.Size = new System.Drawing.Size(55, 17);
            this.tsClassName.Text = "Class: xxx";
            this.tsClassName.Click += new System.EventHandler(this.tsClassName_Click);
            // 
            // mnContextCollection
            // 
            this.mnContextCollection.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
            this.mnContextCollection.Name = "mnContextCollection";
            this.mnContextCollection.Size = new System.Drawing.Size(114, 26);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Image = global::SPBrowser.Properties.Resources.Refresh;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // MainBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1435, 780);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainBrowser";
            this.Text = "SharePoint 2010 Client Browser";
            this.mnContextRootSiteCollection.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabs.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.tpRawData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvRawData)).EndInit();
            this.tpMsdn.ResumeLayout(false);
            this.mnContextSite.ResumeLayout(false);
            this.mnContextItem.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mnContextCollection.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvSite;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.PropertyGrid pgProperties;
        private System.Windows.Forms.ToolStripMenuItem addSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSpecificWebToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSiteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentSitesToolStripMenuItem;
        internal System.Windows.Forms.ContextMenuStrip mnContextRootSiteCollection;
        internal System.Windows.Forms.ContextMenuStrip mnContextSite;
        internal System.Windows.Forms.ContextMenuStrip mnContextItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem browseMSDNHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem browseMSDNHelpToolStripMenuItem1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tpProperties;
        private System.Windows.Forms.TabPage tpRawData;
        private System.Windows.Forms.DataGridView gvRawData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsClassName;
        private System.Windows.Forms.TabPage tpMsdn;
        private System.Windows.Forms.WebBrowser wbMsdnHelp;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripProgressBar tsProgressBar;
        internal System.Windows.Forms.ContextMenuStrip mnContextCollection;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem openCodeplexToolStripMenuItem;
    }
}