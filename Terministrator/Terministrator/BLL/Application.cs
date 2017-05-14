#region Usings

using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the applications. Is able to work with IApplication to allow the Application Interface to talk with the database.
    /// </summary>
    static class Application
    {
        /// <summary>
        /// Gets or create the application.
        /// </summary>
        /// <param name="iApplication">The iapplication.</param>
        /// <returns>The requested/created application.</returns>
        public static Entites.Application GetOrCreate(IApplication iApplication)
        {
            return Get(iApplication) ?? Create(iApplication);
        }

        /// <summary>
        /// Updates or create the application.
        /// </summary>
        /// <param name="iApplication">The iapplication.</param>
        /// <returns>The updated/created application.</returns>
        public static Entites.Application UpdateOrCreate(IApplication iApplication)
        {
            Entites.Application application = Get(iApplication);
            return application == null ? Create(iApplication) : Update(iApplication, application);
        }

        /// <summary>
        /// Gets the specified application.
        /// </summary>
        /// <param name="iApplication">The iapplication.</param>
        /// <returns>The requested application.</returns>
        public static Entites.Application Get(IApplication iApplication)
        {
            return DAL.Application.Get(iApplication.ApplicationName);
        }

        /// <summary>
        /// Creates the specified application.
        /// </summary>
        /// <param name="iApplication">The iapplication.</param>
        /// <returns>The newly created application.</returns>
        public static Entites.Application Create(IApplication iApplication)
        {
            return
                DAL.Application.Create(new Entites.Application(iApplication.ApplicationName,
                    iApplication.CommandSymbols, iApplication.UserSymbols, iApplication.Token));
        }

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="iApplication">The iapplication to read from.</param>
        /// <param name="application">The application to update.</param>
        /// <returns>The second arguement, updated.</returns>
        public static Entites.Application Update(IApplication iApplication, Entites.Application application)
        {
            application.CommandSymbols = iApplication.CommandSymbols;
            application.UserSymbols = iApplication.UserSymbols;
            application.Token = iApplication.Token;
            return DAL.Application.Update(application);
        }
    }
}