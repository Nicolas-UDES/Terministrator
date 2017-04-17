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

    class Channel : IChannel
    {
        private readonly Application _application;
        private readonly long _applicationId;
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
            _applicationId = channel.Id;
            _firstName = channel.FirstName;
            _lastName = channel.LastName;
            _username = channel.Title;
            _isSolo = channel.Type == ChatType.Private;
            _application = Application.Instance;
        }

        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>
        /// The application identifier
        /// </returns>
        public string GetApplicationId() => _applicationId.ToString();

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
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSolo() => _isSolo;

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>
        /// The username
        /// </returns>
        public string GetUsername() => _username;
    }
}