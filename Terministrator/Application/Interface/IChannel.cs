namespace Terministrator.Application.Interface
{
    interface IChannel
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>The application identifier</returns>
        string GetApplicationId();

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns>The application</returns>
        IApplication GetApplication();

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <returns>The first name</returns>
        string GetFirstName();

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <returns>The last name</returns>
        string GetLastName();

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns>The username</returns>
        string GetUsername();

        /// <summary>
        /// Determines whether this instance is a private channel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is a private channel; otherwise, <c>false</c>.
        /// </returns>
        bool IsSolo();
    }
}