#region Usings

using System;
using System.Collections.Generic;
using System.Threading;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.DAL;
using Terministrator.Terministrator.Entites;
using Terministrator.Terministrator.Types;
using Terministrator.Terministrator.View;
using Ad = Terministrator.Terministrator.BLL.Ad;
using AdSystem = Terministrator.Terministrator.BLL.AdSystem;
using Channel = Terministrator.Terministrator.BLL.Channel;
using Message = Terministrator.Terministrator.Entites.Message;
using MessageTypeToPointSystem = Terministrator.Terministrator.BLL.MessageTypeToPointSystem;
using Privileges = Terministrator.Terministrator.BLL.Privileges;
using Rules = Terministrator.Terministrator.BLL.Rules;
using User = Terministrator.Terministrator.DAL.User;
using UserToChannel = Terministrator.Terministrator.BLL.UserToChannel;

#endregion

namespace Terministrator.Terministrator
{
    #region Usings

    using FormApplication = System.Windows.Forms.Application;

    #endregion

    /// <summary>
    /// The core of the application. Distributes the commands and messages when receiving some.
    /// </summary>
    public class Core
    {
        private static readonly Dictionary<string, Action<Command, Core>> Commands = new Dictionary
            <string, Action<Command, Core>>
            {
                {"give", UserToChannel.GivePoints},
                {"points", UserToChannel.GetPoints},
                {"setamounts", MessageTypeToPointSystem.SetAmounts},
                {"topposters", Channel.GetTopPosters},
                {"addad", Ad.AddAd},
                {"setadsystem", AdSystem.SetAdSystem},
                {"privileges", UserToChannel.GetPrivileges},
                {"setprivileges", UserToChannel.SetPrivileges},
                {"addprivileges", Privileges.AddPrivileges},
                {"renameprivileges", Privileges.RenamePrivileges},
                {"rules", Rules.GetRules},
                {"setrules", Rules.SetRules},
                {"resetblockedwords", Rules.ResetBlockedWords},
                {"start", BLL.Terministrator.Start},
                {"help", BLL.Terministrator.Help}
            };

        private readonly List<IApplication> _applications;
        private readonly MainConsole _mainConsole;
        private readonly Timer _refreshPings;
        private readonly Timer _upTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Core"/> class.
        /// </summary>
        public Core()
        {
            _applications = new List<IApplication>();
            _mainConsole = new MainConsole(SendMessage);
            DateTime now = DateTime.Now;
            _mainConsole.UpdateUpSince(now);
            _upTime = new Timer(UpdateUpTime, now, 0, 1000);
            //_refreshPings = new Timer(RefreshPings, null, 500, 5000);

            Message.SendMessage = SendMessage;
            Logger.LoggerInstance.LoggingRequested += _mainConsole.Log;
            Logger.LoggerInstance.IsNoisy = true;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _applications.ForEach(x => x.Start());
            Logger.LoggerInstance.LogInformation("Starting main console.");
            FormApplication.Run(_mainConsole);
            Stop();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _upTime.Dispose();
            _refreshPings.Dispose();
            _applications.ForEach(x => x.Stop());
        }

        /// <summary>
        /// Updates the up time in the console.
        /// </summary>
        /// <param name="obj">The datetime to compare with.</param>
        private void UpdateUpTime(object obj)
        {
            _mainConsole.UpdateUpTime(DateTime.Now - (DateTime) obj);
        }

        /// <summary>
        /// Refreshes the pings in the console.
        /// </summary>
        /// <param name="obj">Unused.</param>
        private void RefreshPings(object obj)
        {
            _mainConsole.RefreshPing(0, TerministratorContext.Ping()?.Milliseconds);

            for (var i = 0; i < _applications.Count; i++)
            {
                _mainConsole.RefreshPing(i + 1, _applications[i].Ping()?.Milliseconds);
            }
        }

        /// <summary>
        /// Registers the specified application to start with the core.
        /// </summary>
        /// <param name="application">The application.</param>
        internal void Register(IApplication application)
        {
            application.SetMessageDestination(ReceiveMessage);
            _applications.Add(application);
            _mainConsole.AddClient(BLL.Application.GetOrCreate(application));
            _mainConsole.AddChannels(Channel.Get(application));
        }

        /// <summary>
        /// Sends the message before treating it if needed.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendMessage(Message message)
        {
            message.Application.SendMessage(message);
            Logger.LoggerInstance.LogInformation($"Sent a message to {message.UserToChannel.Channel}: {message.Text}");
            _mainConsole.MessagesSent++;
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        /// <param name="iMessage">The message.</param>
        void ReceiveMessage(IMessage iMessage)
        {
            Logger.LoggerInstance.LogNoisy("Received a message.");
            try
            {
                Logger.LoggerInstance.LogNoisy("Creating message.");
                Message message = BLL.Message.GetOrCreate(iMessage);

                Logger.LoggerInstance.LogNoisy("Loading message's related datas.");
                LoadMessageChilds(message);

                Logger.LoggerInstance.LogInformation(
                    $"Message was sent by {message.UserToChannel.User} in {(message.UserToChannel.Channel.Private ? "private" : message.UserToChannel.Channel.ToString())} on {message.ApplicationName}.{(string.IsNullOrEmpty(message.Text) ? "" : $" \"{message.Text}\"")}");
                DispatchMessage(message);
            }
            catch (Exception e)
            {
                Logger.LoggerInstance.LogError(e, "An error occured while treating the message.");
            }
        }

        /// <summary>
        /// Loads the subclasses from the message.
        /// </summary>
        /// <remarks>
        /// This is a time eater; should ultimately all be moved in a single transaction
        /// </remarks>
        /// <param name="message">The message.</param>
        private static void LoadMessageChilds(Message message)
        {
            DAL.Channel.LoadPointSystem(
                DAL.Channel.LoadUserNames(
                    DAL.UserToChannel.LoadPrivileges(
                        DAL.UserToChannel.LoadChannel(DAL.Message.LoadUserToChannel(message).UserToChannel)).Channel));
            User.LoadUserNames(
                DAL.UserToChannel.LoadUser(DAL.Message.LoadTexts(DAL.Message.LoadApplication(message)).UserToChannel)
                    .User);
            if (!message.UserToChannel.Channel.Private)
            {
                DAL.Rules.LoadExtensions(
                    DAL.Rules.LoadBlockedWords(
                        DAL.Rules.LoadBlockedDomains(
                            DAL.Rules.LoadMessageTypes(DAL.Privileges.LoadRules(message.UserToChannel.Privileges).Rules))));
            }
        }

        /// <summary>
        /// Dispatches the message to different methods waiting new messages.
        /// </summary>
        /// <param name="message">The message.</param>
        private void DispatchMessage(Message message)
        {
            _mainConsole.AddChannel(message.UserToChannel.Channel);
            _mainConsole.AddMessage(message);
            _mainConsole.MessagesReceived++;
            _mainConsole.Points += UserToChannel.AttributePoints(message);
            Rules.ReceivedMessage(message, CommandAnalyzer(message));
        }

        /// <summary>
        /// Parses the text and see if it's a command; if so calls the linked method.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns><c>true</c> if it was a command; otherwise, <c>false</c>.</returns>
        private bool CommandAnalyzer(Message message)
        {
            string command = message.Text;
            string commandSymbol = message.Application.GetCommandSymbol();
            if (command == null || command.Length <= commandSymbol.Length ||
                !commandSymbol.Equals(command.Substring(0, commandSymbol.Length),
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            int index = command.IndexOf(' ');
            command = index < 0
                ? command.Substring(commandSymbol.Length)
                : command.Substring(commandSymbol.Length, index - commandSymbol.Length);
            string terministrator = message.Application.UserSymbols +
                                    message.Application.Terministrator.Username;
            if (command.Contains(terministrator))
            {
                command = command.Substring(0, command.Length - terministrator.Length);
            }

            if (Commands.ContainsKey(command))
            {
                Action<Command, Core> action = Commands[command];
                Logger.LoggerInstance.LogInformation(
                    $"Command recognized. Calling {action.Method.ReflectedType?.FullName}.{action.Method.Name}");
                action(new Command(message, command, message.Text.Substring(index + 1)), this);
                return true;
            }
            return false;
        }
    }
}