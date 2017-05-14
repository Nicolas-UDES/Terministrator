#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the user. Contains all the datas required for a user.
    /// </summary>
    /// <seealso cref="Namable" />
    /// <seealso cref="IUser" />
    class User : Namable, IUser
    {
        public User()
        {
        }

        public User(Application application, string idUserForApplication) : base(application, idUserForApplication)
        {
        }

        public virtual List<UserToChannel> Channels { get; set; }

        [NotMapped]
        public string ApplicationId => IdForApplication;

        [NotMapped]
        IApplication IUser.Application => Application;
    }
}