using SPBrowser.Entities;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shell;

namespace SPBrowser
{
    /// <summary>
    /// Allows modifications to task bar related tasks, like adding jump list.
    /// </summary>
    /// <seealso cref="https://msdn.microsoft.com/en-us/library/system.windows.shell(v=vs.100).aspx"/>
    public class TaskBar
    {
        const string CATEGORY_SITES = "Recent Sites";
        const string CATEGORY_TENANTS = "Recent Tenants";

        /// <summary>
        /// Creates and/or updates jump list items in the Windows Taskbar for recent sites and tenants.
        /// </summary>
        public static void UpdateRecentItemsJumpList()
        {
            JumpList list = new JumpList();
            list.JumpItemsRemovedByUser += list_JumpItemsRemovedByUser;

            // Add recent site collections
            foreach (SiteAuthentication site in Globals.Sites.OrderByDescending(s => s.LoadDate))
            {
                // Define task general parameters
                JumpTask task = new JumpTask();
                task.Title = site.Url.ToString();
                task.Description = string.Format("Last opened: {0} {1}\nAuthentication: {2}\nUsername: {3}",
                    site.LoadDate.ToLongDateString(),
                    site.LoadDate.ToShortTimeString(),
                    site.Authentication,
                    site.UserName);
                task.CustomCategory = CATEGORY_SITES;
                // TODO: Add icons to TaskBar jump lists
                //task.IconResourcePath = "SPCB2013.exe";
                //task.IconResourceIndex = 1;

                // Set launch actions
                task.Arguments = string.Format("-action opensite -url {0}", site.Url);
                task.WorkingDirectory = Environment.CurrentDirectory;

                // Add task to jump list
                list.JumpItems.Add(task);
            }

            // Add recent Office 365 Tenants
            foreach (TenantAuthentication tenant in Globals.Tenants.OrderByDescending(t => t.LoadDate))
            {
                // Define task general parameters
                JumpTask task = new JumpTask();
                task.Title = tenant.AdminUrl.ToString();
                task.Description = string.Format("Last opened: {0} {1}\nUsername: {2}",
                    tenant.LoadDate.ToLongDateString(),
                    tenant.LoadDate.ToShortTimeString(),
                    tenant.UserName);
                task.CustomCategory = CATEGORY_TENANTS;
                // TODO: Add icons to TaskBar jump lists
                //task.IconResourcePath = "SPCB2013.exe";
                //task.IconResourceIndex = 0;

                // Set launch actions
                task.Arguments = string.Format("-action opentenant -url {0}", tenant.AdminUrlAsString);
                task.WorkingDirectory = Environment.CurrentDirectory;

                // Add task to jump list
                list.JumpItems.Add(task);
            }

            list.Apply();
        }

        static void list_JumpItemsRemovedByUser(object sender, JumpItemsRemovedEventArgs e)
        {
            foreach (JumpTask task in e.RemovedItems)
            {
                CommandArguments args = new CommandArguments(task.Arguments.Split(' '));

                if (Globals.Sites.Count(s => s.UrlAsString == args.Url.OriginalString) > 0)
                {
                    Globals.Sites.RemoveAll(s => s.UrlAsString == args.Url.OriginalString);
                    LogUtil.LogMessage($"Removed site '{args.Url}' from Recent Sites via Taskbar Jumplist", LogLevel.Information);
                }

                if (Globals.Tenants.Count(t => t.AdminUrlAsString == args.Url.OriginalString) > 0)
                {
                    Globals.Tenants.RemoveAll(t => t.AdminUrlAsString == args.Url.OriginalString);
                    LogUtil.LogMessage($"Removed tenant '{args.Url}' from Recent Tenants via Taskbar Jumplist", LogLevel.Information);
                }
            }
        }
    }
}
