
using System.Linq;

namespace Terministrator.Terministrator.Types
{
    internal static class Tools
    {
        public static bool IsAdmin(Entites.UserToChannel userToChannel)
        {
            return
                userToChannel.Application.Mods(userToChannel.Channel)
                    .Any(x => x.GetApplicationId() == userToChannel.User.GetApplicationId());
        }

        public static bool IsNotAdminThenSendWarning(Command command)
        {
            if (IsAdmin(command.Message.UserToChannel))
            {
                return false;
            }

            command.Message.Application.SendMessage(BLL.Message.Answer(command.Message, "This command is reserved for moderators."));
            return true;
        }
    }
}