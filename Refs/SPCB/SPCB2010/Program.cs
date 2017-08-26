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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;

            try
            {
                Globals.SiteCollections.Load();
                Globals.CustomFeatureDefinitions.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Run(new MainBrowser());
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Globals.SiteCollections.Save();
            Globals.CustomFeatureDefinitions.Save();
        }
    }
}
