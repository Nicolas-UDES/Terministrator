#region Usings

using System.Collections.Generic;
using System.Linq;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class User
    {
        public static Entites.User GetOrCreate(IUser iUser)
        {
            return Get(iUser) ?? Create(iUser);
        }

        public static Entites.User UpdateOrCreate(IUser iUser)
        {
            Entites.User user = Get(iUser);
            return user == null ? Create(iUser) : Update(iUser, user);
        }

        public static Entites.User Get(IUser iUser)
        {
            return DAL.User.Get(iUser.GetApplicationId(), iUser.GetApplication().GetApplicationName());
        }

        public static Entites.User Create(IUser iUser)
        {
            Entites.User user =
                DAL.User.Create(new Entites.User(Application.GetOrCreate(iUser.GetApplication()),
                    iUser.GetApplicationId()));
            user.UserNames = new List<Entites.UserName> {DAL.UserName.Create(UserName.ExtractUserName(iUser, user))};
            return user;
        }

        public static Entites.User Update(IUser iUser, Entites.User user)
        {
            DAL.User.LoadUserNames(user);
            Entites.UserName userName = UserName.ExtractUserName(iUser, user);
            if (user.UserNames.All(x => x.ToString() != userName.ToString()))
            {
                UserName.UpdateUserName(userName);
            }
            return user;
        }
    }
}