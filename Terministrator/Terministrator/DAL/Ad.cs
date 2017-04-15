using System;
using System.Linq;

namespace Terministrator.Terministrator.DAL
{
    static class Ad
    {
        public static bool Exists(Entites.Ad ad)
        {
            return Get(ad.AdId) != null;
        }

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

        public static Entites.Ad Get(int adId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Ad ad = context.Ad.Find(adId);
                context.Entry(ad).Reference(x => x.AdSystem).Load();
                context.Entry(ad).Reference(x => x.Message).Load();
                context.Entry(ad.Message).Reference(x => x.UserToChannel).Load();
                context.Entry(ad.Message).Collection(x => x.Texts).Load();
                context.Entry(ad.Message).Reference(x => x.Application).Load();
                context.Entry(ad.Message.UserToChannel).Reference(x => x.Channel).Load();
                return ad;
            }
        }

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

        public static Entites.Ad LoadAdSystem(Entites.Ad ad)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Ad.Attach(ad);
                context.Entry(ad).Reference(p => p.AdSystem).Load();
            }
            return ad;
        }

        private static Entites.Ad ClearReferences(Entites.Ad ad)
        {
            Entites.Ad reference = new Entites.Ad(0, null, DateTime.MinValue, ad.Message, ad.AdSystem);
            ad.Message = null;
            ad.AdSystem = null;
            return reference;
        }

        private static Entites.Ad AddReferences(Entites.Ad ad, Entites.Ad reference)
        {
            ad.Message = reference.Message;
            ad.AdSystem = reference.AdSystem;
            return ad;
        }
    }
}
