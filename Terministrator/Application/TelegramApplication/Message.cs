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

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
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

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _messageId.ToString();

        /// <summary>
        /// Gets the author of the message.
        /// </summary>
        /// <returns>
        /// The author
        /// </returns>
        public IUser GetFrom() => _from;

        /// <summary>
        /// Gets the channel.
        /// </summary>
        /// <returns>
        /// The channel
        /// </returns>
        public IChannel GetChannel() => _channel;

        /// <summary>
        /// Gets the sent date.
        /// </summary>
        /// <returns>
        /// The sent date
        /// </returns>
        public DateTime GetSentDate() => _sentDate;

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>
        /// The text
        /// </returns>
        public string GetText() => _text;

        /// <summary>
        /// Gets the joined user.
        /// </summary>
        /// <returns>
        /// The joinded user
        /// </returns>
        public IUser GetJoinedUser() => _joinedUser;

        /// <summary>
        /// Gets the message this instance replies to.
        /// </summary>
        /// <returns>The requested message.</returns>
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
    }
}