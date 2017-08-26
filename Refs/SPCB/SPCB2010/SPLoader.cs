using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPBrowser.Extentions;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser
{
    public static class SPLoader
    {
        public delegate void ItemLoadedEventHandler(object sender, ItemLoadedEventArgs e);
        public static event ItemLoadedEventHandler ItemLoaded;

        public static void LoadSite(TreeNode parentNode, SiteAuth siteAuth, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = siteAuth.ClientContext;
                SPClient.Site site = ctx.Site;
                ctx.Load(site);
                ctx.ExecuteQuery();

                TreeNode siteNode = parentNode.Nodes.Add(site.Url);
                siteNode.ImageKey = Constants.IMAGE_SITE;
                siteNode.SelectedImageKey = Constants.IMAGE_SITE;
                siteNode.Tag = site;
                siteNode.ContextMenuStrip = form.mnContextSite;
                siteNode.Expand();

                SPClient.Web rootWeb = site.RootWeb;

                TreeNode rootWebNode = LoadWeb(siteNode, rootWeb, form);
                //rootWebNode.Expand();

                foreach (string webUrl in siteAuth.Webs)
                {
                    LoadWeb(siteNode, site.OpenWeb(webUrl), form);
                }

                AddLoadingNode(siteNode, "Site Columns", Constants.IMAGE_SITE_COLUMN, LoadType.SiteFields);
                AddLoadingNode(siteNode, "Content Types", Constants.IMAGE_CONTENT_TYPE, LoadType.SiteContentTypes);
                AddLoadingNode(siteNode, "Site Features", "Gets a value that specifies the collection of the site collection features for the site collection that contains the site.", Constants.IMAGE_FEATURE, LoadType.SiteFeatures);
                AddLoadingNode(siteNode, "Recycle Bin", Constants.IMAGE_RECYCLE_BIN, LoadType.SiteRecycleBin);

                if (!site.Context.IsMinimalServerVersion(ServerVersion.SharePoint2010))
                {
                    siteNode.ImageKey = Constants.IMAGE_SITE_WARNING;
                    siteNode.SelectedImageKey = Constants.IMAGE_SITE_WARNING;

                    MessageBox.Show(string.Format("You are NOT connecting to a SharePoint 2010 site ({0}), this could result in errors. Please be aware! The application will continue loading the site as normal.", rootWeb.GetWebUrl()), form.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static TreeNode LoadWeb(TreeNode parentNode, SPClient.Web web, MainBrowser form)
        {
            if (!web.IsPropertyAvailable("Title"))
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(web);
                ctx.Load(web, w => w.AllowDesignerForCurrentUser,
                    w => w.AllowMasterPageEditingForCurrentUser,
                    w => w.AllowRevertFromTemplateForCurrentUser,
                    w => w.HasUniqueRoleAssignments,
                    w => w.ShowUrlStructureForCurrentUser);
                ctx.ExecuteQuery();
            }

            TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", web.Title, web.ServerRelativeUrl));
            node.ImageKey = Constants.IMAGE_SITE;
            node.SelectedImageKey = Constants.IMAGE_SITE;
            node.ContextMenuStrip = form.mnContextItem;
            node.Tag = web;

            AddLoadingNode(node, "Webs", "Returns the collection of child sites of the current site based on the specified query.", Constants.IMAGE_WEB, LoadType.WebSubWebs);
            AddLoadingNode(node, "Site Columns", "Gets the collection of field objects that represents all the fields in the Web site.", Constants.IMAGE_SITE_COLUMN, LoadType.WebFields);
            AddLoadingNode(node, "Content Types", "Gets the collection of content types for the Web site.", Constants.IMAGE_CONTENT_TYPE, LoadType.WebContentTypes);
            AddLoadingNode(node, "Lists", "Gets the collection of all lists that are contained in the Web site available to the current user based on the current user’s permissions.", Constants.IMAGE_LIST, LoadType.WebLists);
            AddLoadingNode(node, "Features", "Gets a value that specifies the collection of features that are currently activated in the site.", Constants.IMAGE_FEATURE, LoadType.WebFeatures);
            AddLoadingNode(node, "Site Users", "Gets the UserInfo list of the site collection that contains the Web site.", Constants.IMAGE_SITE_USER, LoadType.WebUsers);
            AddLoadingNode(node, "Site Groups", "Gets the collection of groups for the site collection.", Constants.IMAGE_SITE_GROUP, LoadType.WebGroups);
            AddLoadingNode(node, "Workflow Associations", "Gets a value that specifies the collection of all workflow associations for the site.", Constants.IMAGE_WORKFLOW_ASSOCIATION, LoadType.WebWorkflowAssociations);
            AddLoadingNode(node, "Workflow Templates", "Gets a value that specifies the collection of all workflow associations for the site.", Constants.IMAGE_WORKFLOW_TEMPLATE, LoadType.WebWorkflowTemplates);
            AddLoadingNode(node, "Properties", Constants.IMAGE_PROPERTY, LoadType.WebProperties);
            AddLoadingNode(node, "List Templates", Constants.IMAGE_LIST_TEMPLATES, LoadType.WebListTemplates);
            AddLoadingNode(node, "Role Assignments", Constants.IMAGE_ROLE_ASSIGNMENT, LoadType.WebRoleAssignments);
            AddLoadingNode(node, "Role Definitions", Constants.IMAGE_ROLE_DEFINITIONS, LoadType.WebRoleDefinitions);
            AddLoadingNode(node, "Web Templates", Constants.IMAGE_WEB_TEMPLATES, LoadType.WebTemplates);

            // Add root folder
            LoadFolder(node, web.RootFolder, form, true);

            return node;
        }

        public static void LoadSubWebs(TreeNode parentNode, SPClient.WebCollection webs, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(webs);
                ctx.ExecuteQuery();

                int total = webs.Count;
                int current = 0;

                foreach (SPClient.Web subweb in webs)
                {
                    LoadWeb(parentNode, subweb, form);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.WebSubWebs);
            }
        }

        public static void LoadFeatures(TreeNode parentNode, SPClient.FeatureCollection features, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(features);
                ctx.ExecuteQuery();

                int total = features.Count;
                int current = 0;

                foreach (SPClient.Feature feature in features) //.OrderBy(f => f.GetFeatureName()))
                {
                    TreeNode node = parentNode.Nodes.Add(feature.GetFeatureName());
                    node.Tag = feature;
                    node.ContextMenuStrip = form.mnContextItem;

                    if (feature.IsCustom())
                    {
                        node.ImageKey = Constants.IMAGE_FEATURE_CUSTOM;
                        node.SelectedImageKey = Constants.IMAGE_FEATURE_CUSTOM;
                        node.ToolTipText = "Custom user defined feature definition.";
                    }
                    else
                    {
                        node.ImageKey = Constants.IMAGE_FEATURE;
                        node.SelectedImageKey = Constants.IMAGE_FEATURE;
                        node.ToolTipText = "Out-of-the-box feature definition.";
                    }

                    if (feature.IsHidden())
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSiteColumns(TreeNode parentNode, SPClient.FieldCollection fields, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(fields);
                ctx.ExecuteQuery();

                int total = fields.Count;
                int current = 0;

                foreach (SPClient.Field field in fields)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", field.Title, field.Group));
                    node.ImageKey = Constants.IMAGE_SITE_COLUMN;
                    node.SelectedImageKey = Constants.IMAGE_SITE_COLUMN;
                    node.Tag = field;
                    node.ContextMenuStrip = form.mnContextItem;

                    if (field.Hidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Add group node
                    TreeNode groupNode = parentNode.Nodes.OfType<TreeNode>().SingleOrDefault(n => n.Text.Equals(field.Group));
                    if (groupNode == null)
                    {
                        groupNode = new TreeNode(field.Group);
                        groupNode.ImageKey = Constants.IMAGE_SITE_COLUMN_GROUP;
                        groupNode.SelectedImageKey = Constants.IMAGE_SITE_COLUMN_GROUP;
                        groupNode.Tag = LoadType.GeneralGroup;

                        parentNode.Nodes.Add(groupNode);
                    }
                    groupNode.Nodes.Add((TreeNode)node.Clone());

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadContentTypes(TreeNode parentNode, SPClient.ContentTypeCollection contentTypes, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(contentTypes);
                ctx.ExecuteQuery();

                int total = contentTypes.Count;
                int current = 0;

                foreach (SPClient.ContentType ct in contentTypes)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", ct.Name, ct.Group));
                    node.ImageKey = Constants.IMAGE_CONTENT_TYPE;
                    node.SelectedImageKey = Constants.IMAGE_CONTENT_TYPE;
                    node.Tag = ct;
                    node.ContextMenuStrip = form.mnContextItem;

                    if (ct.Hidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Add group node
                    TreeNode groupNode = parentNode.Nodes.OfType<TreeNode>().SingleOrDefault(n => n.Text.Equals(ct.Group));
                    if (groupNode == null)
                    {
                        groupNode = new TreeNode(ct.Group);
                        groupNode.ImageKey = Constants.IMAGE_CONTENT_TYPE_GROUP;
                        groupNode.SelectedImageKey = Constants.IMAGE_CONTENT_TYPE_GROUP;
                        groupNode.Tag = LoadType.GeneralGroup;

                        parentNode.Nodes.Add(groupNode);
                    }
                    groupNode.Nodes.Add((TreeNode)node.Clone());

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadLists(TreeNode parentNode, SPClient.ListCollection lists, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(lists);
                ctx.ExecuteQuery();

                int total = lists.Count;
                int current = 0;

                foreach (SPClient.List list in lists)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", list.Title, list.ItemCount));
                    node.ImageKey = SPHelp.GetFileName(list.ImageUrl);
                    node.SelectedImageKey = SPHelp.GetFileName(list.ImageUrl);
                    node.ContextMenuStrip = form.mnContextItem;
                    node.Tag = list;

                    if (list.Hidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    AddLoadingNode(node, "Fields", Constants.IMAGE_SITE_COLUMN, LoadType.ListFields);
                    AddLoadingNode(node, "Content Types", Constants.IMAGE_CONTENT_TYPE, LoadType.ListContentTypes);
                    AddLoadingNode(node, "Views", Constants.IMAGE_VIEW, LoadType.ListViews);
                    AddLoadingNode(node, "Role Assignments", Constants.IMAGE_ROLE_ASSIGNMENT, LoadType.ListRoleAssignments);
                    AddLoadingNode(node, "Workflow Associations", Constants.IMAGE_WORKFLOW_ASSOCIATION, LoadType.ListWorkflowAssociations);

                    // Add root folder
                    LoadFolder(node, list.RootFolder, form, true);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.WebLists);
            }
        }

        public static void LoadSiteGroups(TreeNode parentNode, SPClient.GroupCollection groups, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(groups);
                ctx.ExecuteQuery();

                int total = groups.Count;
                int current = 0;

                foreach (SPClient.Group group in groups)
                {
                    TreeNode node = parentNode.Nodes.Add(group.Title);
                    node.ImageKey = Constants.IMAGE_SITE_GROUP;
                    node.SelectedImageKey = Constants.IMAGE_SITE_GROUP;
                    node.Tag = group;
                    node.ContextMenuStrip = form.mnContextItem;

                    AddLoadingNode(node, LoadType.GroupUsers);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.WebGroups);
            }
        }

        public static void LoadUsers(TreeNode parentNode, SPClient.UserCollection users, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(users);
                ctx.ExecuteQuery();

                int total = users.Count;
                int current = 0;

                foreach (SPClient.User user in users)
                {
                    TreeNode node = parentNode.Nodes.Add(user.Title);
                    node.Tag = user;
                    node.ContextMenuStrip = form.mnContextItem;

                    switch (user.PrincipalType)
                    {
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.All:
                            node.ImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                            node.SelectedImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                            break;
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.DistributionList:
                            node.ImageKey = Constants.IMAGE_SITE_GROUP_DISTRIBUTION;
                            node.SelectedImageKey = Constants.IMAGE_SITE_GROUP_DISTRIBUTION;
                            break;
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.None:
                            node.ImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                            node.SelectedImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                            break;
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.SecurityGroup:
                            node.ImageKey = Constants.IMAGE_SITE_GROUP_SECURITY;
                            node.SelectedImageKey = Constants.IMAGE_SITE_GROUP_SECURITY;
                            break;
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.SharePointGroup:
                            node.ImageKey = Constants.IMAGE_SITE_GROUP;
                            node.SelectedImageKey = Constants.IMAGE_SITE_GROUP;
                            break;
                        case Microsoft.SharePoint.Client.Utilities.PrincipalType.User:
                            node.ImageKey = Constants.IMAGE_SITE_USER;
                            node.SelectedImageKey = Constants.IMAGE_SITE_USER;
                            break;
                        default:
                            break;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadSiteUsers(TreeNode parentNode, SPClient.List siteUserInfoList, MainBrowser form)
        {
            try
            {
                SPClient.CamlQuery query = new SPClient.CamlQuery();
                query.ViewXml = "<View><RowLimit>1000</RowLimit></View>";

                SPClient.ClientContext ctx = GetClientContext(parentNode);
                SPClient.ListItemCollection userItems = siteUserInfoList.GetItems(query);
                ctx.Load(userItems);
                ctx.ExecuteQuery();

                int total = userItems.Count;
                int current = 0;

                foreach (SPClient.ListItem userListItem in userItems)
                {
                    SPClient.User principle = null;

                    if (userListItem.FieldValues.ContainsKey("Name") && siteUserInfoList.ParentWeb.ParseUser(userListItem["Name"].ToString(), out principle))
                    {
                        TreeNode node = parentNode.Nodes.Add(principle.LoginName);
                        node.Tag = principle;
                        node.ContextMenuStrip = form.mnContextItem;

                        switch (principle.PrincipalType)
                        {
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.All:
                                node.ImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                                node.SelectedImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                                break;
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.DistributionList:
                                node.ImageKey = Constants.IMAGE_SITE_GROUP_DISTRIBUTION;
                                node.SelectedImageKey = Constants.IMAGE_SITE_GROUP_DISTRIBUTION;
                                break;
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.None:
                                node.ImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                                node.SelectedImageKey = Constants.IMAGE_SITE_USER_EXCLAMATION;
                                break;
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.SecurityGroup:
                                node.ImageKey = Constants.IMAGE_SITE_GROUP_SECURITY;
                                node.SelectedImageKey = Constants.IMAGE_SITE_GROUP_SECURITY;
                                break;
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.SharePointGroup:
                                node.ImageKey = Constants.IMAGE_SITE_GROUP;
                                node.SelectedImageKey = Constants.IMAGE_SITE_GROUP;
                                break;
                            case Microsoft.SharePoint.Client.Utilities.PrincipalType.User:
                                node.ImageKey = Constants.IMAGE_SITE_USER;
                                node.SelectedImageKey = Constants.IMAGE_SITE_USER;
                                break;
                            default:
                                break;
                        }
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.WebUsers);
            }
        }

        public static void LoadWorkflowAssociations(TreeNode parentNode, SPClient.Workflow.WorkflowAssociationCollection workflows, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(workflows);
                ctx.ExecuteQuery();

                int total = workflows.Count;
                int current = 0;

                foreach (SPClient.Workflow.WorkflowAssociation workflow in workflows)
                {
                    TreeNode node = parentNode.Nodes.Add(workflow.Name);
                    node.ImageKey = Constants.IMAGE_WORKFLOW_ASSOCIATION;
                    node.SelectedImageKey = Constants.IMAGE_WORKFLOW_ASSOCIATION;
                    node.Tag = workflow;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadWorkflowTemplates(TreeNode parentNode, SPClient.Workflow.WorkflowTemplateCollection workflows, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(workflows);
                ctx.ExecuteQuery();

                int total = workflows.Count;
                int current = 0;

                foreach (var workflow in workflows)
                {
                    TreeNode node = parentNode.Nodes.Add(workflow.Name);
                    node.ImageKey = Constants.IMAGE_WORKFLOW_ASSOCIATION;
                    node.SelectedImageKey = Constants.IMAGE_WORKFLOW_ASSOCIATION;
                    node.Tag = workflow;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadViews(TreeNode parentNode, SPClient.ViewCollection views, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(views);
                ctx.ExecuteQuery();

                int total = views.Count;
                int current = 0;

                foreach (SPClient.View view in views)
                {
                    TreeNode node = parentNode.Nodes.Add(view.Title);
                    node.ImageKey = Constants.IMAGE_VIEW;
                    node.SelectedImageKey = Constants.IMAGE_VIEW;
                    node.Tag = view;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.ListViews);
            }
        }

        public static void LoadFolder(TreeNode parentNode, SPClient.Folder folder, MainBrowser form, bool isRootFolder = false)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(folder);
                ctx.ExecuteQuery();

                // Add folder node
                TreeNode node = parentNode.Nodes.Add(isRootFolder ? "Root Folder" : folder.Name);
                node.ImageKey = Constants.IMAGE_FOLDER;
                node.SelectedImageKey = Constants.IMAGE_FOLDER;
                node.Tag = folder;
                node.ContextMenuStrip = form.mnContextItem;

                AddLoadingNode(node, LoadType.Folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (isRootFolder)
                {
                    TreeNode node = parentNode.Nodes.Add("Root Folder (Error)");
                    node.ImageKey = Constants.IMAGE_FOLDER;
                    node.SelectedImageKey = Constants.IMAGE_FOLDER;
                }
                else
                    AddLoadingNode(parentNode, LoadType.Folder);
            }
        }

        public static void LoadSubFolders(TreeNode parentNode, SPClient.Folder folder, MainBrowser form)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(folder.Folders);
                ctx.Load(folder.Files);
                ctx.ExecuteQuery();

                int total = folder.Folders.Count + folder.Files.Count;
                int current = 0;

                // Add folder properties
                AddLoadingNode(parentNode, "Properties", Constants.IMAGE_PROPERTY, LoadType.FolderProperties);

                // Add folders
                foreach (SPClient.Folder subFolder in folder.Folders)
                {
                    LoadFolder(parentNode, subFolder, form);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }

                // Add files
                foreach (SPClient.File file in folder.Files)
                {
                    // Add file node
                    TreeNode node = parentNode.Nodes.Add(file.Name);
                    node.ImageKey = Constants.IMAGE_FILE;
                    node.SelectedImageKey = Constants.IMAGE_FILE;
                    node.Tag = file;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, LoadType.Folder);
            }
        }

        public static void LoadProperties(TreeNode parentNode, SPClient.PropertyValues properties, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(properties);
                ctx.ExecuteQuery();

                int total = properties.FieldValues.Count;
                int current = 0;

                foreach (var property in properties.FieldValues)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0}", property.Key));
                    node.ImageKey = Constants.IMAGE_PROPERTY;
                    node.SelectedImageKey = Constants.IMAGE_PROPERTY;
                    node.Tag = property;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRecycleBin(TreeNode parentNode, SPClient.RecycleBinItemCollection recycleBinItems, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(recycleBinItems);
                ctx.ExecuteQuery();

                int total = recycleBinItems.Count;
                int current = 0;

                foreach (var recycleBinItem in recycleBinItems)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0}", recycleBinItem.Title));
                    node.ImageKey = Constants.IMAGE_RECYCLE_BIN;
                    node.SelectedImageKey = Constants.IMAGE_RECYCLE_BIN;
                    node.Tag = recycleBinItem;
                    node.ContextMenuStrip = form.mnContextItem;

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadListTemplates(TreeNode parentNode, SPClient.ListTemplateCollection listTemplates, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(listTemplates);
                ctx.ExecuteQuery();

                int total = listTemplates.Count;
                int current = 0;

                foreach (var template in listTemplates)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", template.Name, template.InternalName));
                    node.ImageKey = Constants.IMAGE_LIST_TEMPLATES;
                    node.SelectedImageKey = Constants.IMAGE_LIST_TEMPLATES;
                    node.Tag = template;
                    node.ContextMenuStrip = form.mnContextItem;

                    if (template.Hidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRoleAssignments(TreeNode parentNode, SPClient.RoleAssignmentCollection roleAssignments, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(roleAssignments);
                ctx.ExecuteQuery();

                int total = roleAssignments.Count;
                int current = 0;

                foreach (var roleAssignment in roleAssignments)
                {
                    ctx.Load(roleAssignment.Member);
                    ctx.Load(roleAssignment.RoleDefinitionBindings);
                    ctx.ExecuteQuery();

                    TreeNode node = parentNode.Nodes.Add(string.Format("{0}", roleAssignment.Member.LoginName));
                    node.ImageKey = Constants.IMAGE_ROLE_ASSIGNMENT;
                    node.SelectedImageKey = Constants.IMAGE_ROLE_ASSIGNMENT;
                    node.Tag = roleAssignment;
                    node.ContextMenuStrip = form.mnContextItem;

                    foreach (var binding in roleAssignment.RoleDefinitionBindings)
                    {
                        LoadRoleDefinition(node, form, binding);
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static void LoadRoleDefinitions(TreeNode parentNode, SPClient.RoleDefinitionCollection roleDefinitions, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(roleDefinitions);
                ctx.ExecuteQuery();

                int total = roleDefinitions.Count;
                int current = 0;

                foreach (var roleDefinition in roleDefinitions)
                {
                    LoadRoleDefinition(parentNode, form, roleDefinition);

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        private static void LoadRoleDefinition(TreeNode parentNode, MainBrowser form, SPClient.RoleDefinition roleDefinition)
        {
            TreeNode node = parentNode.Nodes.Add(string.Format("{0}", roleDefinition.Name));
            node.ImageKey = Constants.IMAGE_ROLE_DEFINITIONS;
            node.SelectedImageKey = Constants.IMAGE_ROLE_DEFINITIONS;
            node.Tag = roleDefinition;
            node.ContextMenuStrip = form.mnContextItem;

            string[] keys = Enum.GetNames(typeof(SPClient.PermissionKind));
            foreach (string key in keys.OrderBy(k => k))
            {
                if (roleDefinition.BasePermissions.Has((SPClient.PermissionKind)Enum.Parse(typeof(SPClient.PermissionKind), key)))
                {
                    SPClient.PermissionKind permission = (SPClient.PermissionKind)Enum.Parse(typeof(SPClient.PermissionKind), key, true);

                    TreeNode sNode = node.Nodes.Add(string.Format("{0} ({1})", permission.ToString(), (int)permission));
                    sNode.ImageKey = Constants.IMAGE_ROLE_DEFINITIONS;
                    sNode.SelectedImageKey = Constants.IMAGE_ROLE_DEFINITIONS;
                    sNode.Tag = permission;
                }
            }
        }

        public static void LoadWebTemplates(TreeNode parentNode, SPClient.WebTemplateCollection webTemplates, MainBrowser form, LoadType loadType)
        {
            try
            {
                SPClient.ClientContext ctx = GetClientContext(parentNode);
                ctx.Load(webTemplates);
                ctx.ExecuteQuery();

                int total = webTemplates.Count;
                int current = 0;

                foreach (var template in webTemplates)
                {
                    TreeNode node = parentNode.Nodes.Add(string.Format("{0} ({1})", template.Title, template.Name));
                    node.ImageKey = Constants.IMAGE_WEB_TEMPLATES;
                    node.SelectedImageKey = Constants.IMAGE_WEB_TEMPLATES;
                    node.Tag = template;
                    node.ContextMenuStrip = form.mnContextItem;

                    if (template.IsHidden)
                    {
                        node.ForeColor = Color.Gray;
                    }

                    // Update progress
                    current++;
                    ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                AddLoadingNode(parentNode, loadType);
            }
        }

        public static SPClient.ClientContext GetClientContext(TreeNode node)
        {
            if (node != null)
            {
                if (node.Tag != null &&
                    node.Tag.GetType().ToString().Equals(Constants.NS_SITE, StringComparison.InvariantCultureIgnoreCase))
                {
                    string siteUrl = ((SPClient.Site)node.Tag).Url;
                    return Globals.SiteCollections.SingleOrDefault(s => s.Url.OriginalString.Equals(siteUrl, StringComparison.InvariantCultureIgnoreCase)).ClientContext;
                }

                return GetClientContext(node.Parent);
            }

            return null;
        }

        private static void AddLoadingNode(TreeNode parentNode, LoadType loadType)
        {
            TreeNode node = parentNode.Nodes.Add(Constants.NODE_LOADING_TEXT);
            node.Tag = loadType;

            parentNode.Collapse();
        }

        private static void AddLoadingNode(TreeNode parentNode, string nodeText, string imageKey, LoadType tag)
        {
            AddLoadingNode(parentNode, nodeText, string.Empty, imageKey, tag);
        }

        private static void AddLoadingNode(TreeNode parentNode, string nodeText, string toolTipText, string imageKey, LoadType tag)
        {
            TreeNode node = parentNode.Nodes.Add(nodeText);
            node.ImageKey = imageKey;
            node.SelectedImageKey = imageKey;
            node.Tag = tag;
            node.ToolTipText = toolTipText;
            node.ContextMenuStrip = ((MainBrowser)parentNode.TreeView.FindForm()).mnContextCollection;

            node.Nodes.Add(Constants.NODE_LOADING_TEXT);

            node.Collapse();
        }

        public static void LoadRawData(TreeNode selectedNode, DataGridView gvRawData)
        {
            try
            {
                SPClient.List list = selectedNode.Tag as SPClient.List;
                if (list != null)
                {
                    // Reset the grid
                    gvRawData.Columns.Clear();
                    gvRawData.Rows.Clear();

                    // Get list data
                    SPClient.CamlQuery query = new SPClient.CamlQuery();
                    query.ViewXml = "<View Scope='RecursiveAll'><RowLimit>5000</RowLimit></View>";

                    SPClient.ClientContext ctx = SPLoader.GetClientContext(selectedNode);
                    SPClient.ListItemCollection items = list.GetItems(query);
                    ctx.Load(items);
                    ctx.Load(list.Fields);
                    ctx.ExecuteQuery();

                    // Add columns
                    foreach (SPClient.Field field in list.Fields)
                    {
                        DataGridViewColumn column = new DataGridViewTextBoxColumn();
                        column.Name = field.InternalName;
                        column.HeaderText = field.Title;
                        column.DataPropertyName = field.InternalName;
                        column.ReadOnly = true;
                        column.Tag = field;
                        column.ToolTipText = string.Format("InternalName: {0}\nStaticName: {1}\nTypeAsString: {2}\nType: {3}", field.InternalName, field.StaticName, field.TypeAsString, field.GetType());

                        if (field.Hidden)
                        {
                            DataGridViewCellStyle style = new DataGridViewCellStyle();
                            style.ForeColor = Color.Gray;

                            column.HeaderCell.Style = style;
                        }

                        gvRawData.Columns.Add(column);
                    }

                    int total = items.Count;
                    int current = 0;

                    // Add rows
                    foreach (SPClient.ListItem item in items)
                    {
                        DataGridViewRow row = gvRawData.Rows[gvRawData.Rows.Add()];

                        foreach (DataGridViewColumn column in gvRawData.Columns)
                        {
                            if (item.FieldValues.ContainsKey(column.Name) && item.FieldValues[column.Name] != null)
                            {
                                SPClient.Field field = (SPClient.Field)column.Tag;
                                switch (field.TypeAsString)
                                {
                                    case "Lookup":
                                        if (string.IsNullOrEmpty(((SPClient.FieldLookup)field).LookupList))
                                            row.Cells[column.Name].Value = item.FieldValues[column.Name].ToString();
                                        else
                                            row.Cells[column.Name].Value = string.Format("{0};#{1}", ((SPClient.FieldLookupValue)item.FieldValues[column.Name]).LookupId, ((SPClient.FieldLookupValue)item.FieldValues[column.Name]).LookupValue);
                                        break;
                                    case "URL":
                                        row.Cells[column.Name].Value = ((SPClient.FieldUrlValue)item.FieldValues[column.Name]).Url;
                                        break;
                                    case "User":
                                        row.Cells[column.Name].Value = string.Format("{0};#{1}", ((SPClient.FieldUserValue)item.FieldValues[column.Name]).LookupId, ((SPClient.FieldUserValue)item.FieldValues[column.Name]).LookupValue);
                                        break;
                                    default:
                                        row.Cells[column.Name].Value = item.FieldValues[column.Name].ToString();
                                        break;
                                }
                            }
                        }

                        // Update progress
                        current++;
                        ItemLoaded(null, new ItemLoadedEventArgs() { TotalItem = total, CurrentItem = current });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public class ItemLoadedEventArgs : EventArgs
    {
        public int TotalItem { get; set; }
        public int CurrentItem { get; set; }
    }
}
