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

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public User(TUser user)
        {
            _applicationId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string ApplicationId => _applicationId.ToString();

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <returns>
        /// The first name
        /// </returns>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <returns>
        /// The last name
        /// </returns>
        public string LastName { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>
        /// The username
        /// </returns>
        public string Username { get; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application
        /// </returns>
        public IApplication Application => TelegramApplication.Application.Instance;
    }
}