using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SPClient = Microsoft.SharePoint.Client;

namespace SPBrowser.Utils
{
    /// <summary>
    /// Represents logging utility.
    /// </summary>
    public static class LogUtil
    {
        private static Guid _correlation = Guid.NewGuid();

        /// <summary>
        /// The folder which contains all log files.
        /// </summary>
        public static string LogLocation { get { return Environment.CurrentDirectory; } }

        /// <summary>
        /// The full path for the current log file.
        /// </summary>
        public static string LogFile { get { return GetOrCreateLogFile(); } }

        /// <summary>
        /// Constructor.
        /// </summary>
        static LogUtil()
        {}

        /// <summary>
        /// Generates a new correlation identifier to consolidate log entries.
        /// </summary>
        public static void NewCorrelation()
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            System.Reflection.MethodBase methodBase = stackTrace.GetFrame(1).GetMethod();
            LogMessage(string.Format("New correlation, calling method: {0}", methodBase.Name), LogLevel.Verbose);

            _correlation = Guid.NewGuid();
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void LogMessage(string message)
        {
            LogMessage(message, LogLevel.Information);
        }

        /// <summary>
        /// Logs a informational message.
        /// </summary>
        /// <param name="format">A composite format string (see Remarks).</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <remarks>A copy of format in which the format items have been replaced by the string representation of the corresponding objects in args.</remarks>
        public static void LogMessage(string format, params object[] args)
        {
            LogMessage(string.Format(format, args), LogLevel.Information);
        }

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="level">Severity level of the message</param>
        /// <param name="eventId">Event ID to identify related logging entries</param>
        /// <param name="category">Category to log</param>
        public static void LogMessage(string message, LogLevel level = LogLevel.Verbose, int eventId = 0, LogCategory category = LogCategory.General)
        {
            Log(category, eventId, level, message);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="ex">Exception to log</param>
        public static void LogException(Exception ex)
        {
            LogException(ex.Message, ex);
        }

        /// <summary>
        /// Logs an exception.
        /// </summary>
        /// <param name="message">Additional message to log with exception</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="eventId">Event ID to identify related logging entries</param>
        /// <param name="category">Category to log</param>
        public static void LogException(string message, Exception ex, int eventId = 0, LogCategory category = LogCategory.General)
        {
            // General information
            if (!string.IsNullOrEmpty(message))
                Log(category, eventId, LogLevel.Exception, message);

            // Detailed information
            foreach (var line in ex.ToString().Split('\n'))
            {
                Log(category, eventId, LogLevel.Exception, line.Replace("\r", ""));
            }

            // Inner exception
            if (ex.InnerException != null)
            {
                // General information
                Log(category, eventId, LogLevel.Exception, "InnerException: " + ex.InnerException.Message);

                // Detailed information
                foreach (var line in ex.InnerException.ToString().Split('\n'))
                {
                    Log(category, eventId, LogLevel.Exception, line.Replace("\r", ""));
                }
            }

            //// SharePoint Server Exception
            //SPClient.ServerException spEx = ex as SPClient.ServerException;
            //if (spEx != null)
            //{
            //    Log(LogCategory.SharePointServer, 00, LogLevel.Exception, correlation, string.Format("Error value: {0}", spEx.ServerErrorValue));
            //}
        }

        private static void Log(LogCategory category, int eventId, LogLevel level, string message)
        {
            // Do not use the Configuration.Current object, because this will cause a loop when an exception is raised during initializing the Configuration.Current object.
            LogLevel customLogLevel = ConfigUtil.GetOrCreateAppSetting(Configuration.LOG_LEVEL_KEY, LogLevel.Exception);

            if (level >= customLogLevel)
            {
                string content = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}" + Environment.NewLine,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff"),
                    Environment.MachineName,
                    ProductUtil.GetProductVersionInfo().OriginalFilename,
                    ProductUtil.GetProductVersionInfo().ProductVersion,
                    category,
                    eventId,
                    level,
                    _correlation,
                    message);

                File.AppendAllText(LogFile, content);
            }
        }

        private static string GetOrCreateLogFile()
        {
            string logFileName = string.Format("SPCB-{0}-{1}.log",
                Environment.MachineName,
                DateTime.Now.ToString("yyyyMMdd"));

            string logFilePath = Path.Combine(LogLocation, logFileName);

            if (!File.Exists(logFilePath))
            {
                File.AppendAllText(logFilePath, "Time;Machine;Process;Version;Category;EventId;Level;Correlation;Message" + Environment.NewLine);
            }

            return logFilePath;
        }

        /// <summary>
        /// Truncating the log files, keep last 10 log files.
        /// </summary>
        /// <returns></returns>
        public static bool TruncateLogFiles()
        {
            bool isSuccess = false;

            try
            {
                LogMessage("Truncating log files, keep last {0} log files.", Configuration.Current.LogTruncateAfterNumberOfFiles);

                string[] files = Directory.GetFiles(LogLocation, "*.log");

                if (files.Length > Configuration.Current.LogTruncateAfterNumberOfFiles)
                {
                    for (int i = 0; i < files.Length - Configuration.Current.LogTruncateAfterNumberOfFiles; i++)
                    {
                        string filePath = files.GetValue(i).ToString();

                        File.Delete(filePath);
                        LogMessage(string.Format("Deleted log file: {0}", filePath), LogLevel.Information, 0, LogCategory.Logs);
                    }
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                LogUtil.LogException(ex);
            }

            return isSuccess;
        }
       
        /// <summary>
        /// Opens the Windows Explorer with the location of the log files.
        /// </summary>
        public static void OpenLogsFolderLocation()
        {
            WindowsExplorerUtil.OpenInExplorerAndSelect(LogFile);
        }
    }

    /// <summary>
    /// Severity level for log entry.
    /// </summary>
    public enum LogLevel
    {
        Verbose = 1,
        Information = 3,
        Exception = 5
    }

    /// <summary>
    /// Category for logging.
    /// </summary>
    public enum LogCategory
    {
        General,
        SharePointServer,
        Logs,
        TreeViewLoader,
        Releases
    }
}
