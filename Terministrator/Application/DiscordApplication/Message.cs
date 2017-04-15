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

    class Message : IMessage
    {
        private readonly Channel _channel;
        private readonly User _from;
        private readonly string _messageId;
        private readonly DateTime _sentDate;
        private readonly string _text;

        public Message(TMessage message)
        {
            _messageId = message.Message.ID;
            _from = new User(message.Author);
            _sentDate = DateTime.Parse(message.RawJson["d"]["timestamp"].ToString()).ToUniversalTime();
            _channel = new Channel(message.Channel);
            _text = message.MessageText;
        }

        public Message(TPrivateMessage message)
        {
            _messageId = message.RawJson["d"]["id"].ToString();
            _from = new User(message.Author);
            _sentDate = DateTime.Parse(message.RawJson["d"]["timestamp"].ToString()).ToUniversalTime();
            _channel = new Channel(message.Channel);
            _text = message.Message;
        }

        public string GetApplicationId() => _messageId;

        public IUser GetFrom() => _from;

        public IChannel GetChannel() => _channel;

        public DateTime GetSentDate() => _sentDate;

        public string GetText() => _text;

        public IMessage GetRepliesTo() => null;

        public IUser GetJoinedUser()
        {
            return null;
        }
    }
}