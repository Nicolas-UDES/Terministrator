#region Usings

using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    #region Usings

    using TUser = DiscordMember;

    #endregion

    class User : IUser
    {
        private readonly string _applicationId;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _username;

        public User(TUser user)
        {
            _applicationId = user.ID;
            _firstName = user.Username;
            _lastName = "#" + user.Discriminator;
            _username = user.Email;
        }

        public string GetApplicationId() => _applicationId;

        public string GetFirstName() => _firstName;

        public string GetLastName() => _lastName;

        public string GetUsername() => _username;

        public IApplication GetApplication() => Application.Instance;
    }
}