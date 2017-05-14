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

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Channel(TChannel channel)
        {
            ApplicationId = channel.ID;
            FirstName = channel.Name;
            LastName = channel.Topic;
            Username = channel.Parent.Name;
            IsSolo = false;
            _application = DiscordApplication.Application.Instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Channel"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        public Channel(TPrivateChannel channel)
        {
            ApplicationId = channel.ID;
            FirstName = channel.Recipient.Username;
            LastName = null;
            Username = channel.Recipient.Email;
            IsSolo = channel.Private;
            _application = DiscordApplication.Application.Instance;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string ApplicationId { get; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>
        /// The application
        /// </returns>
        public IApplication Application => _application;

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
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSolo { get; }
    }
}