#region Usings

using System;
using System.Threading.Tasks;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.TelegramApplication
{
    #region Usings

    using TMessage = Telegram.Bot.Types.Message;

    #endregion

    class Message : IMessage
    {
        private readonly Channel _channel;
        private readonly User _from;
        private readonly User _joinedUser;
        private readonly int _messageId;
        private readonly Task<Message> _repliesToTask;
        private readonly DateTime _sentDate;
        private readonly string _text;
        private Message _repliesTo;

        public Message(TMessage message)
        {
            if (message.ReplyToMessage != null)
            {
                _repliesToTask = Task.Factory.StartNew(() => new Message(message.ReplyToMessage));
            }

            _messageId = message.MessageId;
            _from = new User(message.From);
            _sentDate = message.Date;
            _channel = new Channel(message.Chat);
            _text = message.Text;
            _joinedUser = message.NewChatMember == null ? null : new User(message.NewChatMember);
        }

        public string GetApplicationId() => _messageId.ToString();

        public IUser GetFrom() => _from;

        public IChannel GetChannel() => _channel;

        public DateTime GetSentDate() => _sentDate;

        public string GetText() => _text;

        public IMessage GetRepliesTo()
        {
            if (_repliesTo != null)
            {
                return _repliesTo;
            }
            if (_repliesToTask != null)
            {
                return _repliesTo = _repliesToTask.Result;
            }

            return null;
        }

        public IUser GetJoinedUser()
        {
            return _joinedUser;
        }

        public int GetOriginalApplicationId() => _messageId;
    }
}