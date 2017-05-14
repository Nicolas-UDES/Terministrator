namespace Terministrator.Application.Interface
{
    /// <summary>
    /// Interface of what a channel should implement for the Core.
    /// </summary>
    interface IChannel
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>The application identifier</returns>
        string ApplicationId { get; }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>The application</returns>
        IApplication Application { get; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <returns>The first name</returns>
        string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <returns>The last name</returns>
        string LastName { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>The username</returns>
        string Username { get; }

        /// <summary>
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        bool IsSolo { get; }
    }
}