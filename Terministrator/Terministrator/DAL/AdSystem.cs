#region Usings

using System;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class AdSystem
    {
        /// <summary>
        /// Creates the specified ad system.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns></returns>
        public static Entites.AdSystem Create(Entites.AdSystem adSystem)
        {
            Entites.AdSystem reference = ClearReferences(adSystem);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.AdSystem.Add(adSystem);
                context.SaveChanges();
            }
            return AddReferences(adSystem, reference);
        }

        /// <summary>
        /// Updates the specified ad system.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns>The same ad system.</returns>
        public static Entites.AdSystem Update(Entites.AdSystem adSystem)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.AdSystem original = context.AdSystem.Find(adSystem.ChannelId);
                if (original != null)
                {
                    original.BothConditions = adSystem.BothConditions;
                    original.MinNbOfMessage = adSystem.MinNbOfMessage;
                    original.MinTime = adSystem.MinTime;
                    context.SaveChanges();
                }
                return original;
            }
        }

        /// <summary>
        /// Gets the specified ad system associated to a channel.
        /// </summary>
        /// <param name="channelId">The channel identifier (Namable).</param>
        /// <returns>The requested ad system.</returns>
        public static Entites.AdSystem Get(int channelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.AdSystem
                    where c.ChannelId == channelId
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Clears the references of the ad system.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns>A copy of the ad system given in entry with only the references.</returns>
        private static Entites.AdSystem ClearReferences(Entites.AdSystem adSystem)
        {
            Entites.AdSystem reference = new Entites.AdSystem(adSystem.Channel, 0, TimeSpan.Zero, false);
            adSystem.Channel = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="adSystem">The ad system to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.AdSystem AddReferences(Entites.AdSystem adSystem, Entites.AdSystem reference)
        {
            adSystem.Channel = reference.Channel;
            return adSystem;
        }
    }
}