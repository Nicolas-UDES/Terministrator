#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terministrator.Application.Interface;
using Terministrator.Terministrator.Types;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class Channel
    {
        public static Entites.Channel UpdateOrCreate(IChannel iChannel)
        {
            Entites.Channel channel = Get(iChannel);
            return channel == null ? Create(iChannel) : Update(iChannel, channel);
        }

        public static Entites.Channel GetOrCreate(IChannel iChannel)
        {
            return Get(iChannel) ?? Create(iChannel);
        }

        public static Entites.Channel Get(IChannel iChannel)
        {
            return DAL.Channel.Get(iChannel.GetApplicationId(), iChannel.GetApplication().GetApplicationName());
        }

        public static List<Entites.Channel> Get(IApplication application)
        {
            return DAL.Channel.Get(application.GetApplicationName());
        }
        
        public static List<Entites.UserToChannel> TopPoster(Entites.Channel channel)
        {
            return DAL.Channel.LoadUsers(channel).Users.OrderByDescending(x => DAL.UserToChannel.CountMessage(x.UserToChannelId)).ToList();

        }

        public static Entites.Channel Create(IChannel iChannel)
        {
            Entites.Channel channel =
                DAL.Channel.Create(new Entites.Channel(Application.GetOrCreate(iChannel.GetApplication()),
                    iChannel.GetApplicationId(), iChannel.IsSolo()));

            channel.UserNames = new List<Entites.UserName> { DAL.UserName.Create(UserName.ExtractUserName(iChannel, channel)) };

            if (!channel.Private)
            {
                channel.AdSystem = AdSystem.Create(channel);
                channel.PointSystem = PointSystem.Create(channel);
                channel.Privileges = Privileges.Create(channel);
            }

            return channel;
        }

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

        public static Entites.Channel GetPrivateChannel(Entites.User user)
        {
            return DAL.User.LoadChannels(user).Channels.FirstOrDefault(x => DAL.UserToChannel.LoadChannel(x).Channel.Private)?.Channel;
        }

        public static void GetTopPosters(Command command, Core core = null)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Entites.UserToChannel userToChannel in TopPoster(command.Message.UserToChannel.Channel))
            {
                sb.AppendLine($"{DAL.User.LoadUserNames(DAL.UserToChannel.LoadUser(userToChannel).User)} - {DAL.UserToChannel.CountMessage(userToChannel.UserToChannelId)} messages");
            }
            command.Message.Application.SendMessage(Message.Answer(command.Message, sb.ToString()));
        }
    }
}