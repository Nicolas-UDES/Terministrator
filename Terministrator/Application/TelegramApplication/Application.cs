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
        public static Application Instance => Lazy.Value;

        private readonly List<IMessage> _waitingMessages;
        private Task<User> _terministratorTask;
        private TelegramBotClient _bot;
        private Action<IMessage> _receivedMessage;

        public DateTime StartTime { get; private set; }
        public Boolean Running { get; private set; }
        public string Token { get; set; }

        private Application()
        {
            _waitingMessages = new List<IMessage>();
        }

        

        public string GetApplicationName() => "TELEGRAM 1.0.6";

        public void SetMessageDestination(Action<IMessage> receivedMessage) => _receivedMessage = receivedMessage;

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

        public void Stop()
        {
            if (!Running)
            {
                return;
            }
            Running = false;

            _bot.StopReceiving();
        }

        public IUser GetTerministrator()
        {
            return _terministratorTask.Result;
        }

        public Task<string> SendMessage(IMessage message)
        {
            if (message != null)
            {
                return Task.Factory.StartNew(() => _bot.SendTextMessageAsync(Convert.ToInt64(message.GetChannel().GetApplicationId()), message.GetText(),
                    disableNotification: true,
                    replyToMessageId: Convert.ToInt32(message.GetRepliesTo()?.GetApplicationId() ?? "0")).Result.MessageId.ToString());
            }
            return null;
        }

        public void EditMessage(IMessage message)
        {
            _bot.EditMessageTextAsync(Convert.ToInt64(message.GetChannel().GetApplicationId()),
                Convert.ToInt32(message.GetChannel().GetApplicationId()), message.GetText());
        }

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

        public bool CanKick(IChannel channel)
        {
            return
                _bot.GetChatAdministratorsAsync(channel.GetApplicationId())
                    .Result.Any(x =>
                        x.User.Id.ToString() == _terministratorTask.Result.GetApplicationId() &&
                        x.Status == ChatMemberStatus.Administrator);
        }

        public void Kick(IUser user, IChannel channel)
        {
            _bot.KickChatMemberAsync(channel.GetApplicationId(), Convert.ToInt32(user.GetApplicationId()));
        }

        public List<IUser> Mods(IChannel channel)
        {
            return _bot.GetChatAdministratorsAsync(channel.GetApplicationId()).Result.Select(x => (IUser) new User(x.User)).ToList();
        }

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

        public string GetCommandSymbol()
        {
            return "/";
        }

        public string GetUserSymbol()
        {
            return "@";
        }
    }
}