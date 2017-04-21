#region Usings

using DiscordSharp.Objects;
using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Application.DiscordApplication
{
    #region Usings

    using TChannel = DiscordChannel;
    using TPrivateChannel = DiscordPrivateChannel;

    #endregion

    /// <summary>
    /// Implement the channel interface for Discord.
    /// </summary>
    /// <seealso cref="IChannel" />
    class Channel : IChannel
    {
        private readonly Application _application;
        private readonly string _applicationId;
        private readonly string _firstName;
        private readonly bool _isSolo;
        private readonly string _lastName;
        private readonly string _username;

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Channel(TChannel channel)
        {
            _applicationId = channel.ID;
            _firstName = channel.Name;
            _lastName = channel.Topic;
            _username = channel.Parent.Name;
            _isSolo = false;
            _application = Application.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Channel(TPrivateChannel channel)
        {
            _applicationId = channel.ID;
            _firstName = channel.Recipient.Username;
            _lastName = null;
            _username = channel.Recipient.Email;
            _isSolo = channel.Private;
            _application = Application.Instance;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _applicationId;

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application
        /// </returns>
        public IApplication GetApplication() => _application;

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
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSolo() => _isSolo;
    }
}