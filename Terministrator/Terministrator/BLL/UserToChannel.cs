#region Usings

using System;
using System.Diagnostics;
using System.Linq;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class UserToChannel
    {
        /// <summary>
        /// Updates or create the user to channel.
        /// </summary>
        /// <param name="iApplication">The application.</param>
        /// <param name="iUser">The user.</param>
        /// <param name="iChannel">The channel.</param>
        /// <returns>The user to channel.</returns>
        public static Entites.UserToChannel UpdateOrCreate(IApplication iApplication, IUser iUser, IChannel iChannel)
        {
            Entites.UserToChannel userToChannel = Get(iUser, iChannel);
            return userToChannel == null
                ? Create(iApplication, iUser, iChannel)
                : Update(iUser, iChannel, userToChannel);
        }

        /// <summary>
        /// Gets or create the user to channel.
        /// </summary>
        /// <param name="iApplication">The application.</param>
        /// <param name="iUser">The user.</param>
        /// <param name="iChannel">The channel.</param>
        /// <returns>The user to channel.</returns>
        public static Entites.UserToChannel GetOrCreate(IApplication iApplication, IUser iUser, IChannel iChannel)
        {
            return Get(iUser, iChannel) ?? Create(iApplication, iUser, iChannel);
        }

        /// <summary>
        /// Updates the user to channel with the unique parameters.
        /// </summary>
        /// <param name="iUser">The user.</param>
        /// <param name="iChannel">The channel.</param>
        /// <param name="userToChannel">The user to channel to update.</param>
        /// <returns>The same user to channel updated with the two other arguements' properties.</returns>
        public static Entites.UserToChannel Update(IUser iUser, IChannel iChannel, Entites.UserToChannel userToChannel)
        {
            DAL.UserToChannel.LoadUser(DAL.UserToChannel.LoadChannel(userToChannel));
            userToChannel.User = User.Update(iUser, userToChannel.User);
            userToChannel.Channel = Channel.Update(iChannel, userToChannel.Channel);
            return userToChannel;
        }

        /// <summary>
        /// Gets the specified user to channel from the unique parameters.
        /// </summary>
        /// <param name="iUser">The user.</param>
        /// <param name="iChannel">The channel.</param>
        /// <returns>The requested user to channel. Null if none found.</returns>
        public static Entites.UserToChannel Get(IUser iUser, IChannel iChannel)
        {
            return DAL.UserToChannel.Get(iUser.GetApplicationId(), iChannel.GetApplicationId(),
                iChannel.GetApplication().GetApplicationName());
        }

        /// <summary>
        /// Creates the specified user to channel.
        /// </summary>
        /// <param name="iApplication">The application.</param>
        /// <param name="iUser">The user.</param>
        /// <param name="iChannel">The channel.</param>
        /// <returns>The newly created uesr to channel.</returns>
        public static Entites.UserToChannel Create(IApplication iApplication, IUser iUser, IChannel iChannel)
        {
            Entites.Channel channel = Channel.UpdateOrCreate(iChannel);
            return DAL.UserToChannel.Create(new Entites.UserToChannel(Application.GetOrCreate(iApplication),
                User.UpdateOrCreate(iUser),
                channel, DateTime.UtcNow, Privileges.GetDefaultUser(channel)));
        }

        /// <summary>
        /// Updates the specified user to channel.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <returns>The same user to channel.</returns>
        public static Entites.UserToChannel Update(Entites.UserToChannel userToChannel)
        {
            return DAL.UserToChannel.Update(userToChannel);
        }

        /// <summary>
        /// Gets the message sent by the user in that channel just before the requested date.
        /// </summary>
        /// <param name="userToChannel">The user to channel.</param>
        /// <param name="date">The date.</param>
        /// <returns>The requested message.</returns>
        public static Entites.Message GetMessageBefore(Entites.UserToChannel userToChannel, DateTime date)
        {
            return DAL.UserToChannel.GetMessageBefore(userToChannel.UserToChannelId, date);
        }

        /// <summary>
        /// Chat command. Answers with the user's points in that channel.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GetPoints(Command command, Core core = null)
        {
            Entites.Message.SendMessage(Message.Answer(command.Message,
                "You have " + command.Message.UserToChannel.Points + " points."));
        }

        /// <summary>
        /// Attributes the points to a user from the channel's policies. Called upon receiving a new message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="core">The core.</param>
        /// <returns>How many points were given to that user.</returns>
        public static float AttributePoints(Entites.Message message, Core core = null)
        {
            float points =
                DAL.PointSystem.LoadMessageTypes(DAL.Channel.LoadPointSystem(message.UserToChannel.Channel).PointSystem)
                    ?.MessageTypeToPointSystem.FirstOrDefault(x => x.MessageTypeId == message.MessageTypeId)?.Reward ?? 0;
            message.UserToChannel.Points += points;
            Update(message.UserToChannel);
            return points;
        }

        /// <summary>
        /// Chat command. Transfert points between a user to another one in the same channel.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GivePoints(Command command, Core core = null)
        {
            string[] arguements = command.SplitArguements(' ', 2);

            float amount;
            Entites.UserToChannel receiver;
            string errorMessage = CheckGivePointsArguements(command.SplitArguements(' ', 2), out amount,
                command.Message.UserToChannel, out receiver);
            if (errorMessage != null)
            {
                Entites.Message.SendMessage(Message.Answer(command.Message, errorMessage));
                return;
            }

            command.Message.UserToChannel.Points -= amount;
            receiver.Points += amount;
            Update(command.Message.UserToChannel);
            Update(receiver);

            Entites.Message.SendMessage(Message.Answer(command.Message,
                "You gave @" + arguements[0].Substring(1) + " " + arguements[1] + " points."));
        }

        /// <summary>
        /// Checks and extracts the GivePoints arguements.
        /// </summary>
        /// <param name="arguements">The arguements.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns>An error message. Null if none.</returns>
        private static string CheckGivePointsArguements(string[] arguements, out float amount,
            Entites.UserToChannel sender, out Entites.UserToChannel receiver)
        {
            receiver = null;

            if (!float.TryParse(arguements[1], out amount))
            {
                return "Is that supposed to be a number?";
            }
            if (sender.Points < amount)
            {
                return "You only have " + sender.Points + " points. You can't give " + amount + "!";
            }
            if (amount <= 0)
            {
                return "You can't bill people; positive numbers only!";
            }

            receiver =
                DAL.User.LoadChannels(
                        (Entites.User)
                        UserName.GetFromUsername(arguements[0].Substring(1), sender.ApplicationName)?.OwnedBy)
                    .Channels.FirstOrDefault(c => c.ChannelId == sender.ChannelId);
            if (receiver == null)
            {
                return "No one with the username " + arguements[0] + " known.";
            }

            return null;
        }

        /// <summary>
        /// User command. Tells the user their privileges group's name.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GetPrivileges(Command command, Core core = null)
        {
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                $"Your privilege groups is: {command.Message.UserToChannel.Privileges.Name}. To know the related rules, write /rules {command.Message.UserToChannel.Privileges.Name}."));
        }

        /// <summary>
        /// Mod command. Sets the privileges group of a user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void SetPrivileges(Command command, Core core = null)
        {
            if (Tools.IsNotModThenSendWarning(command))
            {
                return;
            }

            string[] arguements = command.Arguement.Split(new[] {' '}, 2);
            string user;
            string privilegesName;
            if (arguements.Length > 1)
            {
                user = arguements[0].Substring(1);
                privilegesName = arguements[1];
            }
            else
            {
                user = command.Message.UserToChannel.User.GetCurrentUserName().Username;
                privilegesName = arguements[0];
            }

            Debug.Assert(command.Message.UserToChannel.ChannelId != null,
                "command.Message.UserToChannel.ChannelId != null");
            Entites.UserToChannel userToChannel =
                Get(
                    DAL.UserName.LoadOwnedBy(DAL.UserName.GetFromUsername(user, command.Message.ApplicationName))
                        .OwnedBy as Entites.User, command.Message.UserToChannel.Channel);
            if (userToChannel == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No one nammed @{user} was found."));
                return;
            }

            Entites.Privileges privileges = Privileges.GetPrivileges(command.Message.UserToChannel.Channel,
                privilegesName);
            if (privileges == null)
            {
                command.Message.Application.SendMessage(Message.Answer(command.Message,
                    $"No privileges group nammed {privilegesName} was found."));
                return;
            }

            userToChannel.PrivilegesId = privileges.PrivilegesId;
            Update(userToChannel);
            command.Message.Application.SendMessage(Message.Answer(command.Message,
                $"Your privileges group is now set to {privilegesName}."));
        }
    }
}