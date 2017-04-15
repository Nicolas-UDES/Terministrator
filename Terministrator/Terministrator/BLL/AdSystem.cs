using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terministrator.Terministrator.Types;

namespace Terministrator.Terministrator.BLL
{
    static class AdSystem
    {
        public static bool Exists(int adSystemId)
        {
            return DAL.AdSystem.Exists(adSystemId);
        }

        public static bool Exists(Entites.AdSystem adSystem)
        {
            return Exists(adSystem.ChannelId);
        }

        public static Entites.AdSystem UpdateOrCreate(Entites.AdSystem adSystem)
        {
            return Exists(adSystem) ? DAL.AdSystem.Update(adSystem) : DAL.AdSystem.Create(adSystem);
        }

        public static Entites.AdSystem Create(Entites.Channel channel)
        {
            return DAL.AdSystem.Create(new Entites.AdSystem(channel, 20, TimeSpan.FromHours(1), true));
        }

        public static Entites.AdSystem Update(Entites.AdSystem adSystem)
        {
            return DAL.AdSystem.Update(adSystem);
        }

        public static void SetAdSystem(Command command, Core core)
        {
            string[] arguements = command.Arguement.Split(new[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
            bool and = "and".Equals(arguements[1], StringComparison.InvariantCultureIgnoreCase);
            int nbMessages = -1, temps = -1;

            if (!and && !"or".Equals(arguements[1], StringComparison.InvariantCultureIgnoreCase) || !int.TryParse(arguements[0], out nbMessages) || !int.TryParse(arguements[2], out temps))
            {
                Entites.Message.SendMessage(Message.Answer(command.Message,
                    "The format for this command is '\\setadsystem nbMessages (and|or) periode', where nbMessages is the number of messages between two ads and the period is the time (in minutes) between two messages."));
            }

            command.Message.UserToChannel.Channel.AdSystem = new Entites.AdSystem(command.Message.UserToChannel.Channel, nbMessages, TimeSpan.FromMinutes(temps), and);
            UpdateOrCreate(command.Message.UserToChannel.Channel.AdSystem);
        }
    }
}
