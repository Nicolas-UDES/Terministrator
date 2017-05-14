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
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public User(TUser user)
        {
            ApplicationId = user.ID;
            FirstName = user.Username;
            LastName = "#" + user.Discriminator;
            Username = user.Email;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string ApplicationId { get; }

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
        public IApplication Application => DiscordApplication.Application.Instance;
    }
}