using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terministrator.Application.Interface;

namespace Terministrator.Terministrator.BLL
{
    class UserName
    {
        public static Entites.UserName GetFromUsername(string userName, string applicationName)
        {
            return DAL.UserName.GetFromUsername(userName, applicationName);
        }

        public static Entites.UserName ExtractUserName(IUser iUser, Entites.User user)
        {
            return new Entites.UserName(iUser.GetFirstName(), iUser.GetLastName(), iUser.GetUsername(), true, DateTime.UtcNow, user);
        }

        public static Entites.UserName ExtractUserName(IChannel iChannel, Entites.Channel channel)
        {
            return new Entites.UserName(iChannel.GetFirstName(), iChannel.GetLastName(), iChannel.GetUsername(), true, DateTime.UtcNow, channel);
        }

        public static Entites.UserName UpdateUserName(Entites.UserName userName)
        {
            DAL.UserName.UpdateUserName(userName.OwnedById);
            return DAL.UserName.Create(userName);
        }
    }
}
