#region Usings

using System;
using System.Collections.Generic;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the messages. Mainly process the switch between an IMessage and a message.
    /// </summary>
    static class Message
    {
        /// <summary>
        /// Gets or create a message.
        /// </summary>
        /// <param name="iMessage">The imessage.</param>
        /// <returns>The requested/created message.</returns>
        public static Entites.Message GetOrCreate(IMessage iMessage)
        {
            return Get(iMessage) ?? Create(iMessage);
        }

        /// <summary>
        /// Gets the related message.
        /// </summary>
        /// <param name="iMessage">The imessage.</param>
        /// <returns>The requested message.</returns>
        public static Entites.Message Get(IMessage iMessage)
        {
            return DAL.Message.Get(iMessage.ApplicationId,
                iMessage.Channel.Application.ApplicationName);
        }

        /// <summary>
        /// Creates the specified message.
        /// </summary>
        /// <param name="iMessage">The imessage.</param>
        /// <returns>The newly created message.</returns>
        public static Entites.Message Create(IMessage iMessage)
        {
            //TODO: Actually process the real message types.
            Entites.Message message =
                DAL.Message.Create(
                    new Entites.Message(Application.UpdateOrCreate(iMessage.Channel.Application),
                        iMessage.ApplicationId, iMessage.SentDate,
                        UserToChannel.UpdateOrCreate(iMessage.Channel.Application, iMessage.From,
                            iMessage.Channel),
                        MessageType.Get("Text"),
                        iMessage.RepliesTo != null ? GetOrCreate(iMessage.RepliesTo) : null,
                        false,
                        iMessage.JoinedUser == null
                            ? null
                            : UserToChannel.GetOrCreate(iMessage.Channel.Application, iMessage.JoinedUser,
                                iMessage.Channel)));

            if (!string.IsNullOrEmpty(iMessage.Text))
            {
                message.Texts = new List<Entites.Text>
                {
                    Text.Create(new Entites.Text(iMessage.Text, iMessage.SentDate, message,
                        string.IsNullOrEmpty(iMessage.Text) ? null : Rules.ToR9KText(iMessage.Text)))
                };
            }

            // If the user was invited back, we make sure to reboot their consequences.
            if (message.JoinedUser != null &&
                (message.JoinedUser.NbSilences != 0 || message.JoinedUser.SilencedTo != null))
            {
                message.JoinedUser.NbSilences = 0;
                message.JoinedUser.SilencedTo = null;
                UserToChannel.Update(message.JoinedUser);
            }

            return message;
        }

        /// <summary>
        /// Creates a new message for a user, in a channel, with a specified text (not in the database).
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The newly created message.</returns>
        public static Entites.Message Create(string text, Entites.UserToChannel userToChannel)
        {
            Entites.Message message = new Entites.Message(userToChannel.Application, null, DateTime.Now, userToChannel,
                null);
            message.Texts = new List<Entites.Text> {new Entites.Text(text, message.SentOn, message)};
            return message;
        }

        /// <summary>
        /// Given a message, make Terministrator answers to it.
        /// </summary>
        /// <param name="original">The original message.</param>
        /// <param name="text">The text.</param>
        /// <returns>The answering message.</returns>
        public static Entites.Message Answer(Entites.Message original, string text)
        {
            Entites.Message message = new Entites.Message(original.Application, null, DateTime.UtcNow,
                original.UserToChannel, MessageType.Get("Text"), original);
            message.Texts = new List<Entites.Text> {new Entites.Text(text, message.SentOn, message)};
            return message;
        }

        /// <summary>
        /// Counts the amount of messages between two dates.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns>The amount of messages.</returns>
        public static int NbMessagesBetween(Entites.Channel channel, DateTime from, DateTime to)
        {
            return DAL.Message.NbMessagesBetween(channel.NamableId, channel.Application.ApplicationName, from, to);
        }

        /// <summary>
        /// Counts the amount of messages between now and the specified date..
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="from">From.</param>
        /// <returns>The amount of messages.</returns>
        public static int NbMessagesSince(Entites.Channel channel, DateTime from)
        {
            return NbMessagesBetween(channel, from, DateTime.UtcNow);
        }
    }
}