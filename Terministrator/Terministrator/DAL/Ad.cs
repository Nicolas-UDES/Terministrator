#region Usings

using System;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Ad
    {
        /// <summary>
        /// Creates the specified ad.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns>The same ad with an updated ID.</returns>
        public static Entites.Ad Create(Entites.Ad ad)
        {
            Entites.Ad reference = ClearReferences(ad);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                ad.AdId = context.Ad.Add(ad).AdId;
                context.SaveChanges();
            }
            return AddReferences(ad, reference);
        }

        /// <summary>
        /// Gets the specified ad.
        /// </summary>
        /// <param name="adId">The ad identifier.</param>
        /// <returns>The requested ad.</returns>
        public static Entites.Ad Get(int adId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Ad ad = context.Ad.Find(adId);
                if (ad == null)
                {
                    return null;
                }
                context.Entry(ad).Reference(x => x.AdSystem).Load();
                context.Entry(ad).Reference(x => x.Message).Load();
                context.Entry(ad.Message).Reference(x => x.UserToChannel).Load();
                context.Entry(ad.Message).Collection(x => x.Texts).Load();
                context.Entry(ad.Message).Reference(x => x.Application).Load();
                context.Entry(ad.Message.UserToChannel).Reference(x => x.Channel).Load();
                return ad;
            }
        }

        /// <summary>
        /// Updates the specified ad.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns>The same ad.</returns>
        public static Entites.Ad Update(Entites.Ad ad)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Ad old = context.Ad.Find(ad.AdId);
                if (old != null)
                {
                    old.MaxShow = ad.MaxShow;
                    old.Name = ad.Name;
                    old.LastSent = ad.LastSent;
                    context.SaveChanges();
                }
            }
            return ad;
        }

        /// <summary>
        /// Loads the ad system associated to the ad.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns>The same ad with the ad system reference loaded.</returns>
        public static Entites.Ad LoadAdSystem(Entites.Ad ad)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Ad.Attach(ad);
                context.Entry(ad).Reference(p => p.AdSystem).Load();
            }
            return ad;
        }

        /// <summary>
        /// Clears the references of the ad.
        /// </summary>
        /// <param name="ad">The ad system type to ad.</param>
        /// <returns>A copy of the ad given in entry with only the references.</returns>
        private static Entites.Ad ClearReferences(Entites.Ad ad)
        {
            Entites.Ad reference = new Entites.Ad(0, null, DateTime.MinValue, ad.Message, ad.AdSystem);
            ad.Message = null;
            ad.AdSystem = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="ad">The ad to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Ad AddReferences(Entites.Ad ad, Entites.Ad reference)
        {
            ad.Message = reference.Message;
            ad.AdSystem = reference.AdSystem;
            return ad;
        }
    }
}