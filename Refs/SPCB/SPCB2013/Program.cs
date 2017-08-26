using SPBrowser.Managers;
using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SPBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SplashScreen.ShowSplashScreen();
#if DEBUG
                Configuration.Current.LogLevel = LogLevel.Verbose;
#endif
                LogUtil.LogMessage("Initialize application {2} v{1} ({0}) on {3} ({4}) {5}",
                    ProductUtil.GetProductVersionInfo().OriginalFilename,
                    ProductUtil.GetProductVersionInfo().ProductVersion,
                    Application.ProductName,
                    Environment.OSVersion,
                    Environment.OSVersion.Platform,
                    Environment.OSVersion.ServicePack);

                ConfigUtil.UpgradeAppSettingsAfterRename(Constants.APP_CONFIG_FILE_SPCB2013);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ApplicationExit += Application_ApplicationExit;

                Globals.Arguments = new CommandArguments(args);

                SplashScreen.UpdateForm("Loading recent tenants and sites...");
                Globals.Sites.Load();
                Globals.Tenants.Load();
                Globals.CustomFeatureDefinitions.Load();
                TaskBar.UpdateRecentItemsJumpList();

                LogUtil.TruncateLogFiles();

                if (Configuration.Current.CheckUpdatesOnStartup)
                {
                    SplashScreen.UpdateForm("Checking for new releases...");
                    Globals.LatestRelease = ReleasesManager.GetLatestRelease();
                }

                SplashScreen.UpdateForm("Launching application...");
                MainForm mainForm = new MainForm();
                SplashScreen.CloseForm();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                if (SplashScreen.IsActive())
                {
                    SplashScreen.ShowMessageBox(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LogUtil.LogException(ex);
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            TaskBar.UpdateRecentItemsJumpList();

            Configuration.Current.LastUsedVersion = new Version(Application.ProductVersion);

            Globals.Sites.Save();
            Globals.Tenants.Save();
            Globals.CustomFeatureDefinitions.Save();
        }
    }
}
