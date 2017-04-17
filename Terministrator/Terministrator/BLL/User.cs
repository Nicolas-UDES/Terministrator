#region Usings

using System.Collections.Generic;
using System.Linq;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class User
    {
        /// <summary>
        /// Gets or create an user.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <returns>The requested user.</returns>
        public static Entites.User GetOrCreate(IUser iUser)
        {
            return Get(iUser) ?? Create(iUser);
        }

        /// <summary>
        /// Updates or create an user.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <returns>The requested user.</returns>
        public static Entites.User UpdateOrCreate(IUser iUser)
        {
            Entites.User user = Get(iUser);
            return user == null ? Create(iUser) : Update(iUser, user);
        }

        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <returns>The requested user. Null if nothing found.</returns>
        public static Entites.User Get(IUser iUser)
        {
            return DAL.User.Get(iUser.GetApplicationId(), iUser.GetApplication().GetApplicationName());
        }

        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <returns>The newly created user.</returns>
        public static Entites.User Create(IUser iUser)
        {
            Entites.User user =
                DAL.User.Create(new Entites.User(Application.GetOrCreate(iUser.GetApplication()),
                    iUser.GetApplicationId()));
            user.UserNames = new List<Entites.UserName> {DAL.UserName.Create(UserName.ExtractUserName(iUser, user))};
            return user;
        }

        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="iUser">The iuser.</param>
        /// <param name="user">The user.</param>
        /// <returns>The same user reference, but updated.</returns>
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