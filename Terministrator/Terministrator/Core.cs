#region Usings

using System;
using System.Collections.Generic;
using System.Threading;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.DAL;
using Terministrator.Terministrator.Types;
using Terministrator.Terministrator.View;
using Ad = Terministrator.Terministrator.BLL.Ad;
using AdSystem = Terministrator.Terministrator.BLL.AdSystem;
using Channel = Terministrator.Terministrator.BLL.Channel;
using Message = Terministrator.Terministrator.Entites.Message;
using MessageTypeToPointSystem = Terministrator.Terministrator.BLL.MessageTypeToPointSystem;
using Privileges = Terministrator.Terministrator.BLL.Privileges;
using Rules = Terministrator.Terministrator.BLL.Rules;
using UserToChannel = Terministrator.Terministrator.BLL.UserToChannel;

#endregion

namespace Terministrator.Terministrator
{
    #region Usings

    using FormApplication = System.Windows.Forms.Application;

    #endregion

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

        public Core()
        {
            _applications = new List<IApplication>();
            _mainConsole = new MainConsole(SendMessage);
            DateTime now = DateTime.Now;
            _mainConsole.UpdateUpSince(now);
            _upTime = new Timer(UpdateUpTime, now, 0, 1000);
            _refreshPings = new Timer(RefreshPings, null, 500, 5000);

            Message.SendMessage = SendMessage;
            Logger.LoggerInstance.LoggingRequested += _mainConsole.Log;
            Logger.LoggerInstance.IsNoisy = true;
        }

        public void Start()
        {
            _applications.ForEach(x => x.Start());
            Logger.LoggerInstance.LogInformation("Starting main console.");
            FormApplication.Run(_mainConsole);
            Stop();
        }

        public void Stop()
        {
            _upTime.Dispose();
            _refreshPings.Dispose();
            _applications.ForEach(x => x.Stop());
        }

        private void UpdateUpTime(object obj)
        {
            _mainConsole.UpdateUpTime(DateTime.Now - (DateTime) obj);
        }

        private void RefreshPings(object obj)
        {
            _mainConsole.RefreshPing(0, TerministratorContext.Ping()?.Milliseconds);

            for (var i = 0; i < _applications.Count; i++)
            {
                _mainConsole.RefreshPing(i + 1, _applications[i].Ping()?.Milliseconds);
            }
        }

        internal void Register(IApplication application)
        {
            application.SetMessageDestination(ReceivedMessage);
            _applications.Add(application);
            _mainConsole.AddClient(BLL.Application.GetOrCreate(application));
            _mainConsole.AddChannels(Channel.Get(application));
        }

        private void SendMessage(Message message)
        {
            message.Application.SendMessage(message);
            Logger.LoggerInstance.LogInformation("Sent a message.");
            _mainConsole.MessagesSent++;
        }

        void ReceivedMessage(IMessage iMessage)
        {
            Logger.LoggerInstance.LogNoisy("Received a message.");
            try
            {
                Logger.LoggerInstance.LogNoisy("Creating message.");
                Message message = BLL.Message.GetOrCreate(iMessage);

                Logger.LoggerInstance.LogNoisy("Loading message's related datas.");
                LoadMessageChilds(message);

                Logger.LoggerInstance.LogInformation(
                    $"Message was sent by {message.UserToChannel.User} in {(message.UserToChannel.Channel.Private ? "private" : message.UserToChannel.Channel.ToString())} on {message.ApplicationName}.{(string.IsNullOrEmpty(message.GetText()) ? "" : $" \"{message.GetText()}\"")}");
                DispatchMessage(message);
            }
            catch (Exception e)
            {
                Logger.LoggerInstance.LogError(e, "An error occured while treating the message.");
            }
        }

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

        private void DispatchMessage(Message message)
        {
            _mainConsole.AddChannel(message.UserToChannel.Channel);
            _mainConsole.AddMessage(message);
            _mainConsole.MessagesReceived++;
            _mainConsole.Points += UserToChannel.AttributePoints(message);
            Rules.ReceivedMessage(message, CommandAnalyzer(message));
        }

        private bool CommandAnalyzer(Message message)
        {
            string command = message.GetText();
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
            string terministrator = message.Application.GetUserSymbol() +
                                    message.Application.GetTerministrator().GetUsername();
            if (command.Contains(terministrator))
            {
                command = command.Substring(0, command.Length - terministrator.Length);
            }

            if (Commands.ContainsKey(command))
            {
                Action<Command, Core> action = Commands[command];
                Logger.LoggerInstance.LogInformation(
                    $"Command recognized. Calling {action.Method.ReflectedType?.FullName}.{action.Method.Name}");
                action(new Command(message, command, message.GetText().Substring(index + 1)), this);
                return true;
            }
            return false;
        }
    }
}