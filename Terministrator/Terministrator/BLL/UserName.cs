#region Usings

using System;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    class UserName
    {
        /// <summary>
        /// Gets the user name from a string.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns>The requested user name. Null if nothing found.</returns>
        public static Entites.UserName GetFromUsername(string userName, string applicationName)
        {
            return DAL.UserName.GetFromUsername(userName, applicationName);
        }

        /// <summary>
        /// Creates an user name from a iUser's infos and a user's reference.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <param name="user">The user.</param>
        /// <returns>The newly created user name.</returns>
        public static Entites.UserName ExtractUserName(IUser iUser, Entites.User user)
        {
            return new Entites.UserName(iUser.GetFirstName(), iUser.GetLastName(), iUser.GetUsername(), true,
                DateTime.UtcNow, user);
        }

        /// <summary>
        /// Creates an user name from a ichannel's infos and a channel's reference.
        /// </summary>
        /// <param name="iChannel">The ichannel.</param>
        /// <param name="channel">The channel.</param>
        /// <returns>The newly created user name.</returns>
        public static Entites.UserName ExtractUserName(IChannel iChannel, Entites.Channel channel)
        {
            return new Entites.UserName(iChannel.GetFirstName(), iChannel.GetLastName(), iChannel.GetUsername(), true,
                DateTime.UtcNow, channel);
        }

        /// <summary>
        /// Switch the old user name to a non-default state and create this one.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The same user name with an updated ID.</returns>
        public static Entites.UserName UpdateUserName(Entites.UserName userName)
        {
            DAL.UserName.UpdateUserName(userName.OwnedById);
            return DAL.UserName.Create(userName);
        }
    }
}