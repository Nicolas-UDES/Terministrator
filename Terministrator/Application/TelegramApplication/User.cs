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

    /// <summary>
    /// Implement the user interface for Telegram.
    /// </summary>
    /// <seealso cref="IUser" />
    class User : IUser
    {
        private readonly int _applicationId;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _username;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public User(TUser user)
        {
            _applicationId = user.Id;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _username = user.Username;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _applicationId.ToString();

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <returns>
        /// The first name
        /// </returns>
        public string GetFirstName() => _firstName;

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <returns>
        /// The last name
        /// </returns>
        public string GetLastName() => _lastName;

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>
        /// The username
        /// </returns>
        public string GetUsername() => _username;

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application
        /// </returns>
        public IApplication GetApplication() => Application.Instance;
    }
}