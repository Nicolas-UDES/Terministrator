#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DiscordSharp;
using DiscordSharp.Events;
using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    class Application : IApplication
    {
        private static readonly Lazy<Application> Lazy = new Lazy<Application>(() => new Application());
        public static Application Instance => Lazy.Value;

        private readonly List<IMessage> _waitingMessages;
        private DiscordClient _bot;
        private Task<User> _terministratorTask;       
        private Action<IMessage> _receivedMessage;

        public string Token { get; set; }
        public DateTime StartTime { get; set; }
        public Boolean Running { get; private set; }

        private Application()
        {
            _waitingMessages = new List<IMessage>();
        }

        public string GetApplicationName() => "DISCORD";

        public void SetMessageDestination(Action<IMessage> receivedMessage) => _receivedMessage = receivedMessage;

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

        public void Stop()
        {
            if (!Running)
            {
                return;
            }
            Running = false;

            _bot.Logout();
        }

        public IUser GetTerministrator()
        {
            return _terministratorTask.Result;
        }

        public Task<string> SendMessage(IMessage message)
        {
            if (message != null)
            {
                if (message.GetChannel().IsSolo())
                {
                    DiscordPrivateChannel dpc = _bot.GetPrivateChannels().FirstOrDefault(x => x.ID == message.GetChannel().GetApplicationId());
                    return dpc != null ? Task.Factory.StartNew(() => _bot.SendMessageToUser(message.GetText(), dpc.Recipient).ID) : null;
                }

                return Task.Factory.StartNew(() => _bot.SendMessageToChannel(message.GetText(), _bot.GetChannelByID(Convert.ToInt64(message.GetChannel().GetApplicationId()))).ID);
            }

            return null;
        }

        public void EditMessage(IMessage message)
        {
            _bot.EditMessage(message.GetApplicationId(), message.GetText(), _bot.GetChannelByID(Convert.ToInt64(message.GetChannel().GetApplicationId())));
        }

        public void Kick(IUser user, IChannel channel)
        {
            _bot.KickMember(_bot.GetMemberFromChannel(_bot.GetChannelByID(Convert.ToInt64(channel.GetApplicationId())), user.GetApplicationId()));
        }

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

        public bool CanKick(IChannel channel)
        {
            return true;
        }

        public List<IUser> Mods(IChannel channel)
        {
            return _bot.GetMessageHistory(_bot.GetChannelByID(Convert.ToInt64(channel.GetApplicationId())), Int32.MaxValue)
                .Where(x => x.Author.Roles.Any(y => y.Position > 0))
                .Select(x => (IUser) new User(x.Author)).ToList();
        }

        public TimeSpan? Ping(TimeSpan? max = null)
        {
            if (!Running)
            {
                return null;
            }

            DateTime now = DateTime.UtcNow;
            try
            {
                return Task.Run(()=> { _bot.ChangeClientInformation(_bot.ClientPrivateInformation);}).Wait(max?.Milliseconds ?? 5000) ? (TimeSpan?)(DateTime.UtcNow - now) : null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetCommandSymbol()
        {
            return "!";
        }

        public string GetUserSymbol()
        {
            return "@";
        }
    }
}