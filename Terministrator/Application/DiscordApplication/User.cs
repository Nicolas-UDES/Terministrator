#region Usings

using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    #region Usings

    using TUser = DiscordMember;

    #endregion

    /// <summary>
    /// Implement the user interface for Discord.
    /// </summary>
    /// <seealso cref="IUser" />
    class User : IUser
    {
        private readonly string _applicationId;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _username;

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public User(TUser user)
        {
            _applicationId = user.ID;
            _firstName = user.Username;
            _lastName = "#" + user.Discriminator;
            _username = user.Email;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _applicationId;

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