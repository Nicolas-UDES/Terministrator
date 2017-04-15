#region Usings

using Telegram.Bot.Types;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.TelegramApplication
{
    #region Usings

    using TUser = Telegram.Bot.Types.User;
    using CUser = ChatMember;

    #endregion

    class User : IUser
    {
        private readonly int _applicationId;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _username;

        public User(TUser user)
        {
            _applicationId = user.Id;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _username = user.Username;
        }

        public string GetApplicationId() => _applicationId.ToString();

        public string GetFirstName() => _firstName;

        public string GetLastName() => _lastName;

        public string GetUsername() => _username;

        public IApplication GetApplication() => Application.Instance;

        public int GetOriginalApplicationId() => _applicationId;
    }
}