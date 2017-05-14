#region Usings

using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.TelegramApplication
{
    #region Usings

    using TChannel = Chat;

    #endregion

    /// <summary>
    /// Implement the channel interface for Telegram.
    /// </summary>
    /// <seealso cref="IChannel" />
    class Channel : IChannel
    {
        private readonly long _applicationId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Channel(TChannel channel)
        {
            _applicationId = channel.Id;
            FirstName = channel.FirstName;
            LastName = channel.LastName;
            Username = channel.Title;
            IsSolo = channel.Type == ChatType.Private;
            Application = TelegramApplication.Application.Instance;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string ApplicationId => _applicationId.ToString();

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application
        /// </returns>
        public IApplication Application { get; }

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
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSolo { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>
        /// The username
        /// </returns>
        public string Username { get; }
    }
}