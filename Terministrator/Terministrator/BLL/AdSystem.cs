#region Usings

using System;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the adsystems. Implements the chat command to set them.
    /// </summary>
    static class AdSystem
    {
        /// <summary>
        /// Tells if the specified ad system exists in the database.
        /// </summary>
        /// <param name="adSystemId">The ad system identifier.</param>
        /// <returns><c>true</c> if it exists; otherwise <c>false</c>.</returns>
        public static bool Exists(int adSystemId)
        {
            return DAL.AdSystem.Get(adSystemId) != null;
        }

        /// <summary>
        /// Tells if the specified ad system exists in the database.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns><c>true</c> if it exists; otherwise <c>false</c>.</returns>
        public static bool Exists(Entites.AdSystem adSystem)
        {
            return Exists(adSystem.ChannelId);
        }

        /// <summary>
        /// Updates or create the ad system.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns>The updated/created ad system.</returns>
        public static Entites.AdSystem UpdateOrCreate(Entites.AdSystem adSystem)
        {
            return Exists(adSystem) ? DAL.AdSystem.Update(adSystem) : DAL.AdSystem.Create(adSystem);
        }

        /// <summary>
        /// Creates the specified ad system.
        /// </summary>
        /// <param name="channel">The channel to create an ad system for.</param>
        /// <returns>The newly created ad system.</returns>
        public static Entites.AdSystem Create(Entites.Channel channel)
        {
            return DAL.AdSystem.Create(new Entites.AdSystem(channel, 20, TimeSpan.FromHours(1), true));
        }

        /// <summary>
        /// Updates the specified ad system.
        /// </summary>
        /// <param name="adSystem">The ad system.</param>
        /// <returns>The same ad system.</returns>
        public static Entites.AdSystem Update(Entites.AdSystem adSystem)
        {
            return DAL.AdSystem.Update(adSystem);
        }

        /// <summary>
        /// Mod command. Sets the ad system for this channel.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void SetAdSystem(Command command, Core core)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

            string[] arguements = command.Arguement.Split(new[] {' '}, 3, StringSplitOptions.RemoveEmptyEntries);
            bool and = "and".Equals(arguements[1], StringComparison.InvariantCultureIgnoreCase);
            int nbMessages = -1, temps = -1;

            if (!and && !"or".Equals(arguements[1], StringComparison.InvariantCultureIgnoreCase) ||
                !int.TryParse(arguements[0], out nbMessages) || !int.TryParse(arguements[2], out temps))
            {
                Entites.Message.SendMessage(Message.Answer(command.Message,
                    "The format for this command is '\\setadsystem nbMessages (and|or) periode', where nbMessages is the number of messages between two ads and the period is the time (in minutes) between two messages."));
            }

            command.Message.UserToChannel.Channel.AdSystem = new Entites.AdSystem(
                command.Message.UserToChannel.Channel, nbMessages, TimeSpan.FromMinutes(temps), and);
            UpdateOrCreate(command.Message.UserToChannel.Channel.AdSystem);
        }
    }
}