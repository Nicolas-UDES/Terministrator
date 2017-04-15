#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Message
    {
        public static bool Exists(Entites.Message message)
        {
            return Get(message.IdForApplication, message.UserToChannel.Channel.Application.ApplicationName) != null;
        }

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

        public static List<Entites.Message> GetAll(string channelID, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Message
                    where c.UserToChannel.Channel.IdForApplication == channelID &&
                          c.ApplicationName == application
                    select c).ToList();
            }
        }

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

        public static Entites.Message LoadTexts(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Message.Attach(message);
                context.Entry(message).Collection(p => p.Texts).Load();
            }
            return message;
        }

        public static Entites.Message LoadApplication(Entites.Message message)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Message.Attach(message);
                context.Entry(message).Reference(p => p.Application).Load();
            }
            return message;
        }

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