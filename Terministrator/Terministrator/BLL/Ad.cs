#region Usings

using System;
using System.Data.SqlTypes;
using System.Timers;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class Ad
    {
        public static Entites.Ad GetOrCreate(Entites.Ad ad)
        {
            return Get(ad) ?? Create(ad);
        }

        public static Entites.Ad Get(Entites.Ad ad)
        {
            return Get(ad.AdId);
        }

        public static Entites.Ad Get(int adId)
        {
            return DAL.Ad.Get(adId);
        }

        public static Entites.Ad Create(Entites.Ad ad)
        {
            return DAL.Ad.Create(ad);
        }

        public static Entites.Ad Update(Entites.Ad ad)
        {
            return DAL.Ad.Update(ad);
        }

        public static void AddAd(Command command, Core core = null)
        {
            DAL.Channel.LoadAdSystem(command.Message.UserToChannel.Channel);
            DAL.Message.LoadRepliesTo(command.Message);
            if (command.Arguement == null || command.Message.UserToChannel.Channel.AdSystem == null ||
                command.Message.RepliesTo == null)
            {
                Entites.Message.SendMessage(Message.Answer(command.Message,
                    "Your add is not valid; you must reply to the message you want to set as an ad and give it a name like so: \"/addad name\"."));
                return;
            }
            string[] arguements = command.Arguement.Split(new[] {' '}, 1, StringSplitOptions.RemoveEmptyEntries);

            Entites.Ad ad =
                Create(new Entites.Ad(-1, arguements[0], SqlDateTime.MinValue.Value, command.Message.RepliesTo,
                    command.Message.UserToChannel.Channel.AdSystem));
            Timer t = new Timer {AutoReset = true};
            t.Elapsed += delegate { SendAd(t, ad.AdId); };
            t.Start();
        }

        private static void SendAd(Timer timer, int adId)
        {
            Entites.Ad ad = Get(adId);

            if (ad.AdSystem.BothConditions &&
                ad.AdSystem.MinNbOfMessage > Message.NbMessagesSince(ad.AdSystem.Channel, ad.LastSent))
            {
                timer.Interval = 60 * 1000;
                return;
            }
            if (ad.MaxShow > 0)
            {
                ad.MaxShow--;
            }

            ad.Message.RepliesTo = null;
            Entites.Message.SendMessage(ad.Message);
            ad.LastSent = DateTime.UtcNow;
            Update(ad);

            if (ad.MaxShow == 0)
            {
                timer.Stop();
                timer.Dispose();
            }
            else
            {
                timer.Interval = ad.AdSystem.MinTime.TotalMilliseconds;
            }
        }
    }
}