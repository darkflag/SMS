using SPBrowser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPBrowser
{
    /// <summary>
    /// Represents command-line arguments.
    /// </summary>
    /// <remarks>
    /// The command-line arguments are used for the <see cref="TaskBar"/> jump lists. This allows 
    /// to start the SPCB with a site or tenant to open during application launch.
    /// </remarks>
    public class CommandArguments
    {
        private const string COMMAND_KEY_ACTION = "-action";
        private const string COMMAND_KEY_URL = "-url";
        private const string COMMAND_KEY_HELP = "-help";

        public CommandAction Action { get; private set; }
        public Uri Url { get; private set; }
        public string HelpTopic { get; private set; }

        public CommandArguments(string[] args)
        {
            LogUtil.LogMessage("Program arguments are: {0}", string.Join(",", args));

            if (args.Length == 0)
                this.Action = CommandAction.None;

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                Validate(args[i], args[i + 1]);

                switch (args[i].ToLower())
                {
                    case COMMAND_KEY_ACTION:
                        CommandAction action = CommandAction.Help;
                        if (Enum.TryParse<CommandAction>(args[i + 1], true, out action))
                            this.Action = action;
                        else
                            throw new CommandArgumentException(string.Format("Invalid value for argument '{0}'", COMMAND_KEY_ACTION));
                        break;
                    case COMMAND_KEY_URL:
                        this.Url = new Uri(args[i + 1]);
                        break;
                    case COMMAND_KEY_HELP:
                        this.HelpTopic = args[i + 1];
                        break;
                    default:
                        break;
                }
            }
        }

        private void Validate(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new CommandArgumentException("Action parameter is empty. Please try again.");

            if (string.IsNullOrEmpty(value))
                throw new CommandArgumentException("Value parameter is empty. Please try again.");

            if (!key.StartsWith("-"))
                throw new CommandArgumentException("Incorrect action parameter. Please try again.");

            switch (value)
            {
                case COMMAND_KEY_ACTION:
                case COMMAND_KEY_URL:
                case COMMAND_KEY_HELP:
                    throw new CommandArgumentException("Value parameter contains action. Please try again.");
                default:
                    break;
            }
        }
    }

    public enum CommandAction
    {
        None,
        OpenSite,
        OpenTenant,
        Help
    }

    public class CommandArgumentException : ApplicationException
    {
        public CommandArgumentException()
            : base()
        { }

        public CommandArgumentException(string message)
            : base(message)
        { }
    }
}
