using SPBrowser.Properties;
using SPBrowser.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SPBrowser
{
    /// <summary>
    /// Represents the splash screen launched at the start of the application.
    /// </summary>
    public partial class SplashScreen : Form
    {
        // Delegates for cross thread calls
        private delegate bool IsActiveDelegate();
        private delegate void CloseDelegate();
        private delegate void UpdateDelegate(string message);
        private delegate DialogResult ShowMessageBoxDelegate(string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon);

        // The type of form to be displayed as the splash screen.
        private static SplashScreen _splashForm;

        /// <summary>
        /// Launch splash screen.
        /// </summary>
        static public void ShowSplashScreen()
        {
            // Make sure it is only launched once.
            if (_splashForm != null)
                return;

            Thread thread = new Thread(new ThreadStart(SplashScreen.ShowFormInternal));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /// <summary>
        /// Internal <see cref="ShowFormInternal"/> method to support launch in cross thread call.
        /// </summary>
        static private void ShowFormInternal()
        {
            _splashForm = new SplashScreen();
            _splashForm.MinimumSize = _splashForm.Size;
            _splashForm.MaximumSize = _splashForm.Size;

            Application.Run(_splashForm);

            _splashForm.Focus();
        }

        /// <summary>
        /// Determines whether the <see cref="SplashScreen"/> is active.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </returns>
        static public bool IsActive()
        {
            bool isActive = false;

            try
            {
                isActive = (bool)_splashForm.Invoke(new IsActiveDelegate(SplashScreen.IsActiveInternal));
            }
            catch (InvalidOperationException)
            { }

            return isActive;
        }

        /// <summary>
        /// Internal <see cref="IsActiveInternal"/> method to support close in cross thread call.
        /// </summary>
        static private bool IsActiveInternal()
        {
            return _splashForm != null;
        }

        /// <summary>
        /// Closes the splash screen.
        /// </summary>
        static public void CloseForm()
        {
            WaitForSplashScreen();

            _splashForm.Invoke(new CloseDelegate(SplashScreen.CloseFormInternal));
        }

        /// <summary>
        /// Internal <see cref="CloseFormInternal"/> method to support close in cross thread call.
        /// </summary>
        static private void CloseFormInternal()
        {
            _splashForm.Close();
        }

        /// <summary>
        /// Update the status message shown on the splash screen.
        /// </summary>
        /// <param name="message">Status message shown on the splash screen.</param>
        static public void UpdateForm(string message)
        {
            WaitForSplashScreen();

            _splashForm.Invoke(new UpdateDelegate(SplashScreen.UpdateFormInternal), message);
        }

        /// <summary>
        /// Internal <see cref="UpdateFormInternal"/> method to support update in cross thread call.
        /// </summary>
        static private void UpdateFormInternal(string message)
        {
            _splashForm.lbStatus.Text = message;
        }

        /// <summary>
        /// Shows the message box.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageBoxButtons">The message box buttons.</param>
        /// <param name="messageBoxIcon">The message box icon.</param>
        /// <returns></returns>
        static public DialogResult ShowMessageBox(string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
        {
            WaitForSplashScreen();

            DialogResult result = (DialogResult)_splashForm.Invoke(new ShowMessageBoxDelegate(SplashScreen.ShowMessageBoxInternal), message, messageBoxButtons, messageBoxIcon);

            return result;
        }

        /// <summary>
        /// Internal <see cref="ShowMessageBoxInternal"/> method to support update in cross thread call.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageBoxButtons">The message box buttons.</param>
        /// <param name="messageBoxIcon">The message box icon.</param>
        /// <returns></returns>
        static private DialogResult ShowMessageBoxInternal(string message, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon)
        {
            return MessageBox.Show(message, Application.ProductName, messageBoxButtons, messageBoxIcon);
        }

        /// <summary>
        /// Wait handle to ensure the splash screen is initialized before making changes to it. 
        /// </summary>
        /// <remarks>
        /// The wait handle will time out after 10 seconds.
        /// </remarks>
        static private void WaitForSplashScreen()
        {
            int step = 1000; // Step = 1 sec
            int timeout = 10000; // Timeout after 10 sec

            for (int i = step; i < timeout; i = i + step)
            {
                if (_splashForm != null && _splashForm.IsHandleCreated)
                {
                    break;
                }

                Console.WriteLine("Working on it... (Splashscreen launch - {0} ms)", i);
                Thread.Sleep(i);
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();

            this.Text = string.Format("{0} by Bram de Jager", Application.ProductName);
#if CLIENTSDKV150 || CLIENTSDKV160
            this.BackgroundImage = Resources.SplashScreenSharePoint2013;
            this.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
#elif CLIENTSDKV161
            this.BackgroundImage = Resources.SplashScreenOffice365;
            this.BackColor = System.Drawing.Color.FromArgb(235, 59, 0);
#endif
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            lbProductInfo.Text = string.Format("{0} ({1}) by Bram de Jager",
                Application.ProductName,
                ProductUtil.GetProductVersionInfo().FileVersion);
        }

        private void llWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_BLOG_URL);
        }

        private void llTwitter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Constants.PERSONAL_TWITTER_URL);
        }
    }
}
