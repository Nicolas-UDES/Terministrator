#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Entites;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the channels. Processes every functions mainly dealing with channels.
    /// </summary>
    static class Channel
    {
        /// <summary>
        /// Updates or create a channel.
        /// </summary>
        /// <param name="iChannel">The ichannel.</param>
        /// <returns>The requested/created channel.</returns>
        public static Entites.Channel UpdateOrCreate(IChannel iChannel)
        {
            Entites.Channel channel = Get(iChannel);
            return channel == null ? Create(iChannel) : Update(iChannel, channel);
        }

        /// <summary>
        /// Gets the specified channel.
        /// </summary>
        /// <param name="iChannel">The ichannel.</param>
        /// <returns>The requested channel.</returns>
        public static Entites.Channel Get(IChannel iChannel)
        {
            return DAL.Channel.Get(iChannel.ApplicationId, iChannel.Application.ApplicationName);
        }

        /// <summary>
        /// Gets every channels followed for an application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The collection of channels.</returns>
        public static List<Entites.Channel> Get(IApplication application)
        {
            return DAL.Channel.Get(application.ApplicationName);
        }

        /// <summary>
        /// Gives the users of a channel in descending order of messages sent.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The collection of user to channel</returns>
        public static List<Entites.UserToChannel> TopPoster(Entites.Channel channel)
        {
            return
                DAL.Channel.LoadUsers(channel)
                    .Users.OrderByDescending(x => DAL.UserToChannel.CountMessage(x.UserToChannelId))
                    .ToList();
        }

        /// <summary>
        /// Creates the specified channel.
        /// </summary>
        /// <param name="iChannel">The ichannel.</param>
        /// <returns>The newly created channel.</returns>
        public static Entites.Channel Create(IChannel iChannel)
        {
            Entites.Channel channel =
                DAL.Channel.Create(new Entites.Channel(Application.GetOrCreate(iChannel.Application),
                    iChannel.ApplicationId, iChannel.IsSolo));

            channel.UserNames = new List<Entites.UserName>
            {
                DAL.UserName.Create(UserName.ExtractUserName(iChannel, channel))
            };

            if (!channel.Private)
            {
                channel.AdSystem = AdSystem.Create(channel);
                channel.PointSystem = PointSystem.Create(channel);
                channel.Privileges = Privileges.Create(channel);
            }

            return channel;
        }

        /// <summary>
        /// Updates the specified channel.
        /// </summary>
        /// <param name="iChannel">The ichannel to take the information from.</param>
        /// <param name="channel">The channel to update.</param>
        /// <returns>The second arguement, but updated.</returns>
        public static Entites.Channel Update(IChannel iChannel, Entites.Channel channel)
        {
            DAL.Channel.LoadUserNames(channel);
            Entites.UserName userName = UserName.ExtractUserName(iChannel, channel);
            if (channel.UserNames.All(x => x.ToString() != userName.ToString()))
            {
                UserName.UpdateUserName(userName);
            }
            return channel;
        }

        /// <summary>
        /// Gets the private channel with a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The requested channel.</returns>
        public static Entites.Channel GetPrivateChannel(Entites.User user)
        {
            return
                DAL.User.LoadChannels(user)
                    .Channels.FirstOrDefault(x => DAL.UserToChannel.LoadChannel(x).Channel.Private)?.Channel;
        }

        /// <summary>
        /// User command. Answers with every users in the channel ordered (descending) by messages sent.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="core">The core.</param>
        public static void GetTopPosters(Command command, Core core = null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Entites.UserToChannel userToChannel in TopPoster(command.Message.UserToChannel.Channel))
            {
                sb.AppendLine(
                    $"{DAL.User.LoadUserNames(DAL.UserToChannel.LoadUser(userToChannel).User)} - {DAL.UserToChannel.CountMessage(userToChannel.UserToChannelId)} messages");
            }
            Entites.Message.SendMessage(Message.Answer(command.Message, sb.ToString()));
        }
    }
}