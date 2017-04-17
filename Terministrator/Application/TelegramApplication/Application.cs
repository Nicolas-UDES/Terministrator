#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.TelegramApplication
{
    class Application : IApplication
    {
        private static readonly Lazy<Application> Lazy = new Lazy<Application>(() => new Application());

        private readonly List<IMessage> _waitingMessages;
        private TelegramBotClient _bot;
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

        public DateTime StartTime { get; private set; }
        public bool Running { get; private set; }
        public string Token { get; set; }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <returns>A <see cref="string"/> with the value "TELEGRAM 1.0.6"</returns>
        public string GetApplicationName() => "TELEGRAM 1.0.6";

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
            _bot = new TelegramBotClient(Token);
            _bot.OnMessage += MessageReceived;
            _terministratorTask = Task.Factory.StartNew(() => new User(_bot.GetMeAsync().Result));
            _bot.StartReceiving();
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

            _bot.StopReceiving();
        }

        /// <summary>
        /// Gets the user Terministrator on Telegram.
        /// </summary>
        /// <returns>Terministrator</returns>
        public IUser GetTerministrator()
        {
            return _terministratorTask.Result;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A task containing the ID of the message sent for Telegram.</returns>
        public Task<string> SendMessage(IMessage message)
        {
            if (message != null)
            {
                return
                    Task.Factory.StartNew(
                        () =>
                            _bot.SendTextMessageAsync(Convert.ToInt64(message.GetChannel().GetApplicationId()),
                                    message.GetText(),
                                    disableNotification: true,
                                    replyToMessageId: Convert.ToInt32(message.GetRepliesTo()?.GetApplicationId() ?? "0"))
                                .Result.MessageId.ToString());
            }
            return null;
        }

        /// <summary>
        /// Edits the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void EditMessage(IMessage message)
        {
            _bot.EditMessageTextAsync(Convert.ToInt64(message.GetChannel().GetApplicationId()),
                Convert.ToInt32(message.GetChannel().GetApplicationId()), message.GetText());
        }

        /// <summary>
        /// Determines whether this instance can kick in the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>
        ///   <c>true</c> if this instance can kick in the specified channel; otherwise, <c>false</c>.
        /// </returns>
        public bool CanKick(IChannel channel)
        {
            return
                _bot.GetChatAdministratorsAsync(channel.GetApplicationId())
                    .Result.Any(x =>
                        x.User.Id.ToString() == _terministratorTask.Result.GetApplicationId() &&
                        x.Status == ChatMemberStatus.Administrator);
        }

        /// <summary>
        /// Kicks the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="channel">The channel.</param>
        public void Kick(IUser user, IChannel channel)
        {
            _bot.KickChatMemberAsync(channel.GetApplicationId(), Convert.ToInt32(user.GetApplicationId()));
        }

        /// <summary>
        /// Gets the mods of the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>A collection of user which are moderators.</returns>
        public List<IUser> GetMods(IChannel channel)
        {
            return
                _bot.GetChatAdministratorsAsync(channel.GetApplicationId())
                    .Result.Select(x => (IUser) new User(x.User))
                    .ToList();
        }

        /// <summary>
        /// Pings Telegram.
        /// </summary>
        /// <param name="max">The maximum time a ping can take. 5s by default.</param>
        /// <returns>The time necessary to ping. Null if no connection.</returns>
        public TimeSpan? Ping(TimeSpan? max = null)
        {
            DateTime now = DateTime.UtcNow;
            try
            {
                return _bot.TestApiAsync().Wait(max?.Milliseconds ?? 5000) ? (TimeSpan?) (DateTime.UtcNow - now) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the command symbol. Example: /help.
        /// </summary>
        /// <returns>The command symbol.</returns>
        public string GetCommandSymbol()
        {
            return "/";
        }

        /// <summary>
        /// Gets the user symbol. Example: @Terministrator.
        /// </summary>
        /// <returns>The user symbol.</returns>
        public string GetUserSymbol()
        {
            return "@";
        }

        /// <summary>
        /// Called when Telegram sends us a message.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="args">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        private void MessageReceived(object o, MessageEventArgs args)
        {
            if (_receivedMessage != null)
            {
                foreach (IMessage message in _waitingMessages)
                {
                    _receivedMessage(message);
                }
                _waitingMessages.Clear();

                _receivedMessage(new Message(args.Message));
            }
            else
            {
                _waitingMessages.Add(new Message(args.Message));
            }
        }
    }
}