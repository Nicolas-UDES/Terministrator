#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the applications. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class Application
    {
        /// <summary>
        /// Gets the specified application name.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns>The requested application.</returns>
        public static Entites.Application Get(string applicationName)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Application
                    where c.ApplicationName == applicationName
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Creates the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The same application with an updated ID.</returns>
        public static Entites.Application Create(Entites.Application application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                application = context.Application.Add(application);
                context.SaveChanges();
                return application;
            }
        }

        /// <summary>
        /// Updates the specified application.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <returns>The same application</returns>
        public static Entites.Application Update(Entites.Application application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Application original = context.Application.Find(application.ApplicationName);
                if (original != null)
                {
                    original.CommandSymbols = application.CommandSymbols;
                    original.UserSymbols = application.UserSymbols;
                    original.Token = original.Token;
                    context.SaveChanges();
                }
            }
            return application;
        }
    }
}