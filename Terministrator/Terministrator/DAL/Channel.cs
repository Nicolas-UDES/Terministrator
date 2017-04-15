#region Usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Channel
    {
        public static bool Exists(Entites.Channel channel)
        {
            return Get(channel.IdForApplication, channel.Application.ApplicationName) != null;
        }

        public static Entites.Channel Create(Entites.Channel channel)
        {
            Entites.Channel reference = ClearReferences(channel);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Channel newChannel = context.Channel.Add(channel);
                channel.NamableId = newChannel.NamableId;
                context.SaveChanges();
            }
            return AddReferences(channel, reference);
        }

        public static Entites.Channel Get(string channelID, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Channel
                    where c.IdForApplication == channelID &&
                          c.Application.ApplicationName == application
                    select c).Include(x => x.PointSystem).Include(x => x.AdSystem).FirstOrDefault();
            }
        }

        public static List<Entites.Channel> Get(string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Channel
                    where c.Application.ApplicationName == application
                    select c).Include(x => x.UserNames).ToList();
            }
        }

        public static Entites.Channel LoadUserNames(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Collection(p => p.UserNames).Load();
            }
            return channel;
        }

        public static Entites.Channel LoadApplication(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.Application).Load();
            }
            return channel;
        }

        public static Entites.Channel LoadPointSystem(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.PointSystem).Load();
            }
            return channel;
        }

        public static Entites.Channel LoadAdSystem(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.AdSystem).Load();
            }
            return channel;
        }

        public static Entites.Channel LoadUsers(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Collection(p => p.Users).Load();
            }
            return channel;
        }

        private static Entites.Channel ClearReferences(Entites.Channel channel)
        {
            Entites.Channel reference = new Entites.Channel(channel.Application, null, false, channel.AdSystem);
            channel.Application = null;
            channel.AdSystem = null;
            return reference;
        }

        private static Entites.Channel AddReferences(Entites.Channel channel, Entites.Channel reference)
        {
            channel.Application = reference.Application;
            channel.AdSystem = reference.AdSystem;
            return channel;
        }
    }
}