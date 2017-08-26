using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Represents utility class for Windows Explorer (EXPLORER.EXE)
    /// </summary>
    public class WindowsExplorerUtil
    {
        /// <summary>
        /// Opens Windows Explorer based on the specified folder.
        /// </summary>
        /// <param name="path">Path to base folder opening in Windows Explorer.</param>
        /// <seealso cref="https://support.microsoft.com/en-us/kb/130510"/>
        public static void OpenInExplorer(string path)
        {
            Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// Opens Windows Explorer and specifies the folder or file to receive the initial focus. The parent folder is opened and the specified object is selected.
        /// </summary>
        /// <remarks>
        /// If "/select" is used, the parent folder is opened and the specified object is selected.
        /// To view the C:\WINDOWS folder and select CALC.EXE, use the following syntax:
        /// explorer.exe /select,c:\windows\calc.exe
        /// </remarks>
        /// <param name="path">Path to selected folder or file opening in Windows Explorer.</param>
        /// <seealso cref="https://support.microsoft.com/en-us/kb/130510"/>
        public static void OpenInExplorerAndSelect(string path)
        {
            Process.Start("explorer.exe", string.Format("/select,{0}", path));
        }
    }
}
