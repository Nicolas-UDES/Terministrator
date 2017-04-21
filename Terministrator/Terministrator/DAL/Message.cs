#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the messages. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class Message
    {
        /// <summary>
        /// Creates the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The same message with an updated ID.</returns>
        public static Entites.Message Create(Entites.Message message)
        {
            Entites.Message reference = ClearReferences(message);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                message.MessageId = context.Message.Add(message).MessageId;
                context.SaveChanges();
            }
            return AddReferences(message, reference);
        }

        /// <summary>
        /// Gets the specified message.
        /// </summary>
        /// <param name="messageID">The message identifier (for the application).</param>
        /// <param name="application">The application.</param>
        /// <returns>The requested message. Null if none found.</returns>
        public static Entites.Message Get(string messageID, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Message
                    where c.IdForApplication == messageID &&
                          c.UserToChannel.Channel.Application.ApplicationName == application
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Count the messages between two dates.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="deb">The start date (inclusive).</param>
        /// <param name="fin">The end date (exclusive).</param>
        /// <returns>The number of messages.</returns>
        public static int NbMessagesBetween(int channelId, string applicationName, DateTime deb, DateTime fin)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Message
                    where c.UserToChannel.Channel.NamableId == channelId &&
                          c.ApplicationName == applicationName &&
                          c.SentOn >= deb &&
                          c.SentOn < fin
                    select c).Count();
            }
        }

        /// <summary>
        /// Loads the user to channel of a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The same message with the user to channel loaded.</returns>
        public static Entites.Message LoadUserToChannel(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (message.UserToChannel != null)
                {
                    message.UserToChannel = null;
                }
                context.Entry(context.Message.Find(message.MessageId)).State = EntityState.Detached;
                context.Message.Attach(message);
                context.Entry(message).Reference(p => p.UserToChannel).Load();
            }
            return message;
        }

        /// <summary>
        /// Loads the text collection of a message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The same message with the text collection loaded.</returns>
        public static Entites.Message LoadTexts(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Message.Attach(message);
                context.Entry(message).Collection(p => p.Texts).Load();
            }
            return message;
        }

        /// <summary>
        /// Loads the application of the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The same message with the application instance initialized.</returns>
        public static Entites.Message LoadApplication(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Message.Attach(message);
                context.Entry(message).Reference(p => p.Application).Load();
            }
            return message;
        }

        /// <summary>
        /// Loads the message being replied to.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The same message with the replied message loaded.</returns>
        public static Entites.Message LoadRepliesTo(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (context.Entry(message).State == EntityState.Detached)
                {
                    context.Message.Attach(message);
                }
                context.Entry(message).Reference(p => p.RepliesTo).Load();
            }
            return message;
        }

        /// <summary>
        /// Clears the references of the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>A copy of the message given in entry with only the references.</returns>
        private static Entites.Message ClearReferences(Entites.Message message)
        {
            Entites.Message reference = new Entites.Message(message.Application, null, DateTime.MinValue,
                message.UserToChannel, message.MessageType, message.RepliesTo, false, message.JoinedUser);
            message.Application = null;
            message.UserToChannel = null;
            message.RepliesTo = null;
            message.MessageType = null;
            message.JoinedUser = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="message">The message to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Message AddReferences(Entites.Message message, Entites.Message reference)
        {
            message.Application = reference.Application;
            message.UserToChannel = reference.UserToChannel;
            message.RepliesTo = reference.RepliesTo;
            message.MessageType = reference.MessageType;
            message.JoinedUser = reference.JoinedUser;
            return message;
        }
    }
}