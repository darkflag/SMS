using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SPBrowser.Extentions
{
    public static class TextBoxExtentions
    {
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);

        /// <summary>
        /// Set the cue banner (background) text displayed when textbox is empty.
        /// </summary>
        /// <param name="textbox">Extended control.</param>
        /// <param name="cueBannerText">Text to display in textbox when textbox is empty.</param>
        public static void SetCueBanner(this TextBox textbox, string cueBannerText)
        {
            try
            {
                SendMessage(textbox.Handle, EM_SETCUEBANNER, 0, cueBannerText);
            }
            catch (Exception ex)
            {
                LogUtil.LogException(string.Format("Error on setting cue banner on TextBox control '{0}'", textbox.Name), ex);
            }
        }
    }
}