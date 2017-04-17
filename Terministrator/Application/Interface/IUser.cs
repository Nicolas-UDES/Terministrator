namespace Terministrator.Application.Interface
{
    interface IUser
    {
        /// <summary>
        /// Gets the application identifier.
        /// </summary>
        /// <returns>The application identifier</returns>
        string GetApplicationId();

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
        /// Gets the application.
        /// </summary>
        /// <returns>The application</returns>
        IApplication GetApplication();
    }
}