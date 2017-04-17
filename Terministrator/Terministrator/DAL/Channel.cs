#region Usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Channel
    {
        /// <summary>
        /// Creates the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the updated ID.</returns>
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

        /// <summary>
        /// Gets the specified channel.
        /// </summary>
        /// <param name="channelID">The channel identifier (application).</param>
        /// <param name="application">The application.</param>
        /// <returns>The requested channel.</returns>
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

        /// <summary>
        /// Gets all the channels in a given application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The requested channel.</returns>
        public static List<Entites.Channel> Get(string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Channel
                    where c.Application.ApplicationName == application
                    select c).Include(x => x.UserNames).ToList();
            }
        }

        /// <summary>
        /// Loads the user names.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the user names collection loaded.</returns>
        public static Entites.Channel LoadUserNames(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Collection(p => p.UserNames).Load();
            }
            return channel;
        }

        /// <summary>
        /// Loads the application.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the application reference loaded.</returns>
        public static Entites.Channel LoadApplication(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.Application).Load();
            }
            return channel;
        }

        /// <summary>
        /// Loads the point system.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the point system reference loaded.</returns>
        public static Entites.Channel LoadPointSystem(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.PointSystem).Load();
            }
            return channel;
        }

        /// <summary>
        /// Loads the ad system.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the ad system reference loaded.</returns>
        public static Entites.Channel LoadAdSystem(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Reference(p => p.AdSystem).Load();
            }
            return channel;
        }

        /// <summary>
        /// Loads the user to channels.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The same channel with the user to channel collection loaded.</returns>
        public static Entites.Channel LoadUsers(Entites.Channel channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Channel.Attach(channel);
                context.Entry(channel).Collection(p => p.Users).Load();
            }
            return channel;
        }

        /// <summary>
        /// Clears the references of the channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>A copy of the channel given in entry with only the references.</returns>
        private static Entites.Channel ClearReferences(Entites.Channel channel)
        {
            Entites.Channel reference = new Entites.Channel(channel.Application, null, false, channel.AdSystem);
            channel.Application = null;
            channel.AdSystem = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="channel">The channel to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Channel AddReferences(Entites.Channel channel, Entites.Channel reference)
        {
            channel.Application = reference.Application;
            channel.AdSystem = reference.AdSystem;
            return channel;
        }
    }
}