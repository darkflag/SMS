using System;
using System.Runtime;
using System.Runtime.InteropServices;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Represents utility class for Network related methods.
    /// </summary>
    public class NetworkUtil
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        /// <summary>
        /// Checks if the Internet connection is available.
        /// </summary>
        /// <returns>Returns TRUE when Internet is available, else FALSE.</returns>
        public static bool IsConnectedToInternet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}
