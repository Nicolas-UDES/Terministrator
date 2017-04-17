#region Usings

using System;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    internal static class UserToChannel
    {
        /// <summary>
        /// Tells if the user to channel with these unique values exists in the database.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns><c>true</c> if the UserToChannel exist; otherwise <c>false</c>.</returns>
        public static bool Exists(string userID, string channelID, string applicationName) =>
            Get(userID, channelID, applicationName) != null;

        /// <summary>
        /// Tells if the user to channel exists in the database.
        /// </summary>
        /// <param name="userToChannel">The user identifier.</param>
        /// <returns><c>true</c> if the UserToChannel exist; otherwise <c>false</c>.</returns>
        public static bool Exists(Entites.UserToChannel userToChannel) =>
            Exists(userToChannel.User.IdForApplication, userToChannel.Channel.IdForApplication,
                userToChannel.Channel.Application.ApplicationName);

        /// <summary>
        /// Creates the specified user to channel in the database.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The user to channel witn an updated ID.</returns>
        public static Entites.UserToChannel Create(Entites.UserToChannel userToChannel)
        {
            Entites.UserToChannel reference = ClearReferences(userToChannel);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                userToChannel.UserToChannelId = context.UserToChannel.Add(userToChannel).UserToChannelId;
                context.SaveChanges();
            }
            return AddReferences(userToChannel, reference);
        }

        /// <summary>
        /// Updates the specified user to channel in the database.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The same user to channel that was given in entry.</returns>
        public static Entites.UserToChannel Update(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.UserToChannel original = context.UserToChannel.Find(userToChannel.UserToChannelId);
                if (original != null)
                {
                    original.Points = userToChannel.Points;
                    original.PrivilegesId = userToChannel.PrivilegesId;
                    original.NbSilences = userToChannel.NbSilences;
                    original.SilencedTo = userToChannel.SilencedTo;
                    context.SaveChanges();
                }
            }
            return userToChannel;
        }

        /// <summary>
        /// Gets the specified user to channel from database.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="channelID">The channel identifier.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns></returns>
        public static Entites.UserToChannel Get(string userID, string channelID, string applicationName)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.UserToChannel
                    where c.User.IdForApplication == userID &&
                          c.Channel.IdForApplication == channelID &&
                          c.Channel.Application.ApplicationName == applicationName
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the first message sent before the specified date in the specified channel.
        /// </summary>
        /// <param name="userToChannelId">The user to channel identifier.</param>
        /// <param name="sent">The date to compare with.</param>
        /// <returns>The first message found. Null if none was found.</returns>
        public static Entites.Message GetMessageBefore(int userToChannelId, DateTime sent)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Message
                    where c.UserToChannelId == userToChannelId &&
                          c.SentOn < sent
                    orderby c.SentOn descending 
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Counts the number of messages a user sent in a channel.
        /// </summary>
        /// <param name="userToChannelId">The user to channel identifier.</param>
        /// <returns>The number of messages a user sent in a channel</returns>
        public static int CountMessage(int userToChannelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.UserToChannel userToChannel = context.UserToChannel.Find(userToChannelId);
                return userToChannel == null ? 0 : LoadMessage(userToChannel).Messages.Count;
            }
        }

        /// <summary>
        /// Loads the message collection.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The user to channel with the initialized message collection.</returns>
        public static Entites.UserToChannel LoadMessage(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                userToChannel.Messages = (from c in context.Message
                    where c.UserToChannelId == userToChannel.UserToChannelId
                    select c).ToList();
            }
            return userToChannel;
        }

        /// <summary>
        /// Loads the user reference.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The user to channel with the initialized user reference.</returns>
        public static Entites.UserToChannel LoadUser(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.UserToChannel.Attach(userToChannel);
                context.Entry(userToChannel).Reference(p => p.User).Load();
            }
            return userToChannel;
        }

        /// <summary>
        /// Loads the channel reference.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The user to channel with the initialized channel reference.</returns>
        public static Entites.UserToChannel LoadChannel(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (userToChannel.Channel != null)
                {
                    userToChannel.Channel = null;
                }
                if (context.Entry(userToChannel).State == EntityState.Detached)
                {
                    context.UserToChannel.Attach(userToChannel);
                }
                context.Entry(userToChannel).Reference(p => p.Channel).Load();
            }
            return userToChannel;
        }

        /// <summary>
        /// Loads the privileges reference.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The user to channel with the initialized privileges reference.</returns>
        public static Entites.UserToChannel LoadPrivileges(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (userToChannel.Privileges != null)
                {
                    userToChannel.Privileges = null;
                }
                if (context.Entry(userToChannel).State == EntityState.Detached)
                {
                    context.UserToChannel.Attach(userToChannel);
                }
                context.Entry(userToChannel).Reference(p => p.Privileges).Load();
            }
            return userToChannel;
        }

        /// <summary>
        /// Clears the references of the user to channel.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>A copy of the user to channel given in entry with only the references.</returns>
        private static Entites.UserToChannel ClearReferences(Entites.UserToChannel userToChannel)
        {
            Entites.UserToChannel reference = new Entites.UserToChannel(userToChannel.Application, userToChannel.User,
                userToChannel.Channel, DateTime.MinValue, userToChannel.Privileges);
            userToChannel.Application = null;
            userToChannel.User = null;
            userToChannel.Channel = null;
            userToChannel.Privileges = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="userToChannel">The user to channel to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.UserToChannel AddReferences(Entites.UserToChannel userToChannel,
            Entites.UserToChannel reference)
        {
            userToChannel.Application = reference.Application;
            userToChannel.User = reference.User;
            userToChannel.Channel = reference.Channel;
            userToChannel.Privileges = reference.Privileges;
            return userToChannel;
        }
    }
}