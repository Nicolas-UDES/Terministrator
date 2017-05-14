
using System.Linq;
using Terministrator.Terministrator.Entites;

namespace Terministrator.Terministrator.Types
{
    /// <summary>
    /// Some handy tools that don't really find their place elsewhere in the code.
    /// </summary>
    internal static class Tools
    {
        /// <summary>
        /// Determines whether the specified user to channel is mod.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>
        ///   <c>true</c> if the specified user to channel is mod; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMod(Entites.UserToChannel userToChannel)
        {
            return
                userToChannel.Application.GetMods(userToChannel.Channel)
                    .Any(x => x.GetApplicationId() == userToChannel.User.GetApplicationId());
        }

        /// <summary>
        /// Determines whether the user sending the command is not an mod. If so, warn them.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///   <c>true</c> if is not mod; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotModThenSendWarning(Command command)
        {
            if (IsMod(command.Message.UserToChannel))
            {
                return false;
            }

            Entites.Message.SendMessage(BLL.Message.Answer(command.Message, "This command is reserved for moderators."));
            return true;
        }
    }
}