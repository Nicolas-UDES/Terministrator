#region Usings

using System;
using DiscordSharp;
using DiscordSharp.Events;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    #region Usings

    using TMessage = DiscordMessageEventArgs;
    using TPrivateMessage = DiscordPrivateMessageEventArgs;

    #endregion

    /// <summary>
    /// Implement the application interface for Discord.
    /// </summary>
    /// <seealso cref="IMessage" />
    class Message : IMessage
    {
        private readonly Channel _channel;
        private readonly User _from;
        private readonly string _messageId;
        private readonly DateTime _sentDate;
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Message(TMessage message)
        {
            _messageId = message.Message.ID;
            _from = new User(message.Author);
            _sentDate = DateTime.Parse(message.RawJson["d"]["timestamp"].ToString()).ToUniversalTime();
            _channel = new Channel(message.Channel);
            _text = message.MessageText;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public Message(TPrivateMessage message)
        {
            _messageId = message.RawJson["d"]["id"].ToString();
            _from = new User(message.Author);
            _sentDate = DateTime.Parse(message.RawJson["d"]["timestamp"].ToString()).ToUniversalTime();
            _channel = new Channel(message.Channel);
            _text = message.Message;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _messageId;

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
        /// Gets the message replied by this one.
        /// </summary>
        /// <returns>
        /// The message being replied to
        /// </returns>
        public IMessage GetRepliesTo() => null;

        /// <summary>
        /// Gets the joined user.
        /// </summary>
        /// <returns>
        /// The joinded user
        /// </returns>
        public IUser GetJoinedUser() => null;
    }
}