#region Usings

using System;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class AdSystem
    {
        public static bool Exists(int adSystemId)
        {
            return Get(adSystemId) != null;
        }

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

        public static Entites.AdSystem Get(int channelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.AdSystem
                    where c.ChannelId == channelId
                    select c).FirstOrDefault();
            }
        }

        private static Entites.AdSystem ClearReferences(Entites.AdSystem adSystem)
        {
            Entites.AdSystem reference = new Entites.AdSystem(adSystem.Channel, 0, TimeSpan.Zero, false);
            adSystem.Channel = null;
            return reference;
        }

        private static Entites.AdSystem AddReferences(Entites.AdSystem adSystem, Entites.AdSystem reference)
        {
            adSystem.Channel = reference.Channel;
            return adSystem;
        }
    }
}