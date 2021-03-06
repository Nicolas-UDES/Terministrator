﻿#region Usings

using System;
using System.Diagnostics;
using System.Linq;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Entites;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the user to channel. Create and update upon requests. Also does all the working when a single user in a channel is aimed.
    /// </summary>
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
            return DAL.UserToChannel.Get(iUser.ApplicationId, iChannel.ApplicationId,
                iChannel.Application.ApplicationName);
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
            Entites.Message.SendMessage(Message.Answer(command.Message, GivePointsAnalyzes(command)));
        }

        /// <summary>
        /// Checks and extracts the GivePoints arguements.
        /// </summary>
        /// <param name="command">The command</param>
        /// <returns>An error message. Null if none.</returns>
        private static string GivePointsAnalyzes(Command command)
        {
            string[] arguements = command.SplitArguements(' ', 2);
            Entites.UserToChannel sender = command.Message.UserToChannel;

            if (!float.TryParse(arguements[1], out float amount))
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

            var receiver = DAL.User.LoadChannels(
                    (Entites.User)
                    UserName.GetFromUsername(arguements[0].Substring(1), sender.ApplicationName)?.OwnedBy)
                .Channels.FirstOrDefault(c => c.ChannelId == sender.ChannelId);
            if (receiver == null)
            {
                return "No one with the username " + arguements[0] + " known.";
            }

            command.Message.UserToChannel.Points -= amount;
            receiver.Points += amount;
            Update(command.Message.UserToChannel);
            Update(receiver);

            return "You gave @" + arguements[0].Substring(1) + " " + arguements[1] + " points.";
        }

        /// <summary>
        /// User command. Tells the user their privileges group's name.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GetPrivileges(Command command, Core core = null)
        {
            Entites.Message.SendMessage(Message.Answer(command.Message,
                $"Your privilege groups is: {command.Message.UserToChannel.Privileges.Name}. To know the related rules, write /rules {command.Message.UserToChannel.Privileges.Name}."));
        }

        /// <summary>
        /// Mod command. Sets the privileges group of a user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void SetPrivileges(Command command, Core core = null)
        {
            Entites.Message.SendMessage(Message.Answer(command.Message, SetPrivilegesAnalyzes(command)));
        }

        private static string SetPrivilegesAnalyzes(Command command)
        {
            if (!Tools.IsMod(command.Message.UserToChannel))
            {
                return "This command is reserved for moderators.";
            }

            string[] arguements = command.Arguement.Split(new[] { ' ' }, 2);
            string user;
            string privilegesName;
            if (arguements.Length > 1)
            {
                user = arguements[0].Substring(1);
                privilegesName = arguements[1];
            }
            else
            {
                user = command.Message.UserToChannel.User.CurrentUserName.Username;
                privilegesName = arguements[0];
            }

            Entites.UserToChannel userToChannel =
                Get(DAL.UserName.LoadOwnedBy(DAL.UserName.GetFromUsername(user, command.Message.ApplicationName))
                        .OwnedBy as Entites.User, command.Message.UserToChannel.Channel);
            if (userToChannel == null)
            {
                return $"No one nammed @{user} was found.";
            }

            Entites.Privileges privileges = Privileges.GetPrivileges(command.Message.UserToChannel.Channel, privilegesName);
            if (privileges == null)
            {
                return $"No privileges group nammed {privilegesName} was found.";
            }

            userToChannel.PrivilegesId = privileges.PrivilegesId;
            Update(userToChannel);
            return $"Your privileges group is now set to {privilegesName}.";
        }
    }
}