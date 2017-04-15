#region Usings

using System;
using System.Data.Entity;
using System.Linq;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.DAL
{
    internal static class UserToChannel
    {
        public static bool Exists(string userID, string channelID, string applicationName) =>
            Get(userID, channelID, applicationName) != null;

        public static bool Exists(Entites.UserToChannel userToChannel) =>
            Exists(userToChannel.User.IdForApplication, userToChannel.Channel.IdForApplication, userToChannel.Channel.Application.ApplicationName);

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

        public static Entites.UserToChannel CreateOrUpdate(Entites.UserToChannel userToChannel)
        {
            if (Exists(userToChannel))
            {
                return Update(userToChannel);
            }
            return Create(userToChannel);
        }

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

        public static int CountMessage(int userToChannelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.UserToChannel userToChannel = context.UserToChannel.Find(userToChannelId);
                return userToChannel == null ? 0 : LoadMessage(userToChannel).Messages.Count;
            }
        }
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

        public static Entites.UserToChannel LoadUser(Entites.UserToChannel userToChannel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.UserToChannel.Attach(userToChannel);
                context.Entry(userToChannel).Reference(p => p.User).Load();
            }
            return userToChannel;
        }

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

        private static Entites.UserToChannel AddReferences(Entites.UserToChannel userToChannel, Entites.UserToChannel reference)
        {
            userToChannel.Application = reference.Application;
            userToChannel.User = reference.User;
            userToChannel.Channel = reference.Channel;
            userToChannel.Privileges = reference.Privileges;
            return userToChannel;
        }
    }
}