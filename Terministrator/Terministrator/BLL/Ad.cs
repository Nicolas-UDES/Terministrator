#region Usings

using System;
using System.Data.SqlTypes;
using System.Timers;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the ads. Implements the chat commands and the functions to send the ads following an interval.
    /// </summary>
    static class Ad
    {
        /// <summary>
        /// Gets the specified ad.
        /// </summary>
        /// <param name="adId">The ad identifier.</param>
        /// <returns>The requested ad.</returns>
        public static Entites.Ad Get(int adId)
        {
            return DAL.Ad.Get(adId);
        }

        /// <summary>
        /// Creates the specified ad.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns>The newly created ad.</returns>
        public static Entites.Ad Create(Entites.Ad ad)
        {
            return DAL.Ad.Create(ad);
        }

        /// <summary>
        /// Updates the specified ad.
        /// </summary>
        /// <param name="ad">The ad.</param>
        /// <returns>The first arguement.</returns>
        public static Entites.Ad Update(Entites.Ad ad)
        {
            return DAL.Ad.Update(ad);
        }

        /// <summary>
        /// Mod command. Add a new ad to repeat on the channel following the ad system's settings.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void AddAd(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

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

        /// <summary>
        /// Sends the requested ad.
        /// </summary>
        /// <param name="timer">The timer calling this function.</param>
        /// <param name="adId">The ad identifier.</param>
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