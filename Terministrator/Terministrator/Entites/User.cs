#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class User : Namable, IUser
    {
        public User()
        {
        }

        public User(Application application, string idUserForApplication) : base(application, idUserForApplication) { }

        public virtual List<UserToChannel> Channels { get; set; }

        public string GetApplicationId()
        {
            return IdForApplication;
        }

        public IApplication GetApplication()
        {
            return Application;
        }
    }
}