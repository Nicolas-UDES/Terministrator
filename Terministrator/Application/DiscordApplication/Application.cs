#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Events;
using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    /// <summary>
    /// Implement the application interface for Discord. Used to discuss with Discord's API.
    /// </summary>
    /// <seealso cref="IApplication" />
    class Application : IApplication
    {
        private static readonly Lazy<Application> Lazy = new Lazy<Application>(() => new Application());

        private readonly List<IMessage> _waitingMessages;
        private DiscordClient _bot;
        private Action<IMessage> _receivedMessage;
        private Task<User> _terministratorTask;

        /// <summary>
        /// Prevents a default instance of the <see cref="Application"/> class from being created.
        /// </summary>
        private Application()
        {
            _waitingMessages = new List<IMessage>();
        }

        public static Application Instance => Lazy.Value;
        public DateTime StartTime { get; set; }
        public bool Running { get; private set; }

        public string Token { get; set; }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <returns>A <see cref="string"/> with the value "DISCORD"</returns>
        public string ApplicationName => "DISCORD";

        /// <summary>
        /// Sets the message destination.
        /// </summary>
        /// <param name="receivedMessage">The received message method.</param>
        public void SetMessageDestination(Action<IMessage> receivedMessage) => _receivedMessage = receivedMessage;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (Running)
            {
                return;
            }
            Running = true;

            StartTime = DateTime.UtcNow;
            _bot = new DiscordClient(Token, true);
            _bot.MessageReceived += MessageReceived;
            _bot.PrivateMessageReceived += PrivateMessageReceived;

            ManualResetEvent waitConnected = new ManualResetEvent(false);
            _bot.Connected += (sender, e) => { waitConnected.Set(); };
            _bot.SendLoginRequest();
            _bot.Connect();
            waitConnected.WaitOne();
            _terministratorTask = Task.Factory.StartNew(() => new User(_bot.Me));
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!Running)
            {
                return;
            }
            Running = false;

            _bot.Logout();
        }

        /// <summary>
        /// Gets the terministrator.
        /// </summary>
        /// <returns>
        /// Gets the user Terministrator on Discord.
        /// </returns>
        public IUser Terministrator => _terministratorTask.Result;

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// A task containing the ID of the message sent.
        /// </returns>
        public Task<string> SendMessage(IMessage message)
        {
            if (message != null)
            {
                if (message.Channel.IsSolo)
                {
                    DiscordPrivateChannel dpc =
                        _bot.GetPrivateChannels().FirstOrDefault(x => x.ID == message.Channel.ApplicationId);
                    return dpc != null
                        ? Task.Factory.StartNew(() => _bot.SendMessageToUser(message.Text, dpc.Recipient).ID)
                        : null;
                }

                return
                    Task.Factory.StartNew(
                        () =>
                            _bot.SendMessageToChannel(message.Text,
                                _bot.GetChannelByID(Convert.ToInt64(message.Channel.ApplicationId))).ID);
            }

            return null;
        }

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void EditMessage(IMessage message)
        {
            _bot.EditMessage(message.ApplicationId, message.Text,
                _bot.GetChannelByID(Convert.ToInt64(message.Channel.ApplicationId)));
        }

        /// <summary>
        /// Kicks the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="channel">The channel.</param>
        public void Kick(IUser user, IChannel channel)
        {
            _bot.KickMember(_bot.GetMemberFromChannel(_bot.GetChannelByID(Convert.ToInt64(channel.ApplicationId)),
                user.ApplicationId));
        }

        /// <summary>
        /// Determines whether this instance can kick the specified user.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///   <c>true</c> if this instance can kick the specified user; otherwise, <c>false</c>.
        /// </returns>
        public bool CanKick(IChannel channel)
        {
            return true;
        }

        /// <summary>
        /// Gets the mods of the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        /// A collection of user which are moderators.
        /// </returns>
        public List<IUser> GetMods(IChannel channel)
        {
            DiscordChannel dChannel = _bot.GetChannelByID(Convert.ToInt64(channel.ApplicationId));
            List<DiscordSpecialPermissions> perm = dChannel.Parent.Owner.Roles.SelectMany(x => x.Permissions.GetAllPermissions()).ToList();
            return
                dChannel.Parent.Members.Where(
                        x => x.Value.Roles.Exists(
                            y => y.Permissions.HasPermission(DiscordSpecialPermissions.KickMembers)))
                    .Select(x => (IUser) new User(x.Value))
                    .ToList();
        }

        /// <summary>
        /// Pings the application.
        /// </summary>
        /// <param name="max">The maximum time a ping can take. 5s by default.</param>
        /// <returns>
        /// The time necessary to ping. Null if no connection.
        /// </returns>
        public TimeSpan? Ping(TimeSpan? max = null)
        {
            if (!Running)
            {
                return null;
            }

            DateTime now = DateTime.UtcNow;
            try
            {
                return
                    Task.Run(() => { _bot.ChangeClientInformation(_bot.ClientPrivateInformation); })
                        .Wait(max?.Milliseconds ?? 5000)
                        ? (TimeSpan?) (DateTime.UtcNow - now)
                        : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the command symbol.
        /// </summary>
        /// <returns>
        /// The command symbol.
        /// </returns>
        public string CommandSymbols => "!";

        /// <summary>
        /// Gets the user symbol.
        /// </summary>
        /// <returns>
        /// The user symbol.
        /// </returns>
        public string UserSymbols => "@";

        /// <summary>
        /// Called when Discord sends us a message.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="args">The <see cref="DiscordMessageEventArgs"/> instance containing the event data.</param>
        private void MessageReceived(object o, DiscordMessageEventArgs args)
        {
            if (_receivedMessage != null)
            {
                foreach (IMessage message in _waitingMessages)
                {
                    _receivedMessage(message);
                }
                _waitingMessages.Clear();

                _receivedMessage(new Message(args));
            }
            else
            {
                _waitingMessages.Add(new Message(args));
            }
        }

        /// <summary>
        /// Called when Discord sends us a private message.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="args">The <see cref="DiscordPrivateMessageEventArgs"/> instance containing the event data.</param>
        private void PrivateMessageReceived(object o, DiscordPrivateMessageEventArgs args)
        {
            if (_receivedMessage != null)
            {
                foreach (IMessage message in _waitingMessages)
                {
                    _receivedMessage(message);
                }
                _waitingMessages.Clear();

                _receivedMessage(new Message(args));
            }
            else
            {
                _waitingMessages.Add(new Message(args));
            }
        }
    }
}