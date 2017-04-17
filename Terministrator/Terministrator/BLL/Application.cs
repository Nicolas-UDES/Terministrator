#region Usings

using Terministrator.Application.Interface;

#endregion

namespace Terministrator.Terministrator.BLL
{
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
            return DAL.Application.Get(iApplication.GetApplicationName());
        }

        /// <summary>
        /// Creates the specified application.
        /// </summary>
        /// <param name="iApplication">The iapplication.</param>
        /// <returns>The newly created application.</returns>
        public static Entites.Application Create(IApplication iApplication)
        {
            return
                DAL.Application.Create(new Entites.Application(iApplication.GetApplicationName(),
                    iApplication.GetCommandSymbol(), iApplication.GetUserSymbol(), iApplication.Token));
        }

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="iApplication">The iapplication to read from.</param>
        /// <param name="application">The application to update.</param>
        /// <returns>The second arguement, updated.</returns>
        public static Entites.Application Update(IApplication iApplication, Entites.Application application)
        {
            application.CommandSymbols = iApplication.GetCommandSymbol();
            application.UserSymbols = iApplication.GetUserSymbol();
            application.Token = iApplication.Token;
            return DAL.Application.Update(application);
        }
    }
}