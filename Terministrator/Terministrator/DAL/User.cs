#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class User
    {
        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The same user with an updated id.</returns>
        public static Entites.User Create(Entites.User user)
        {
            Entites.User reference = ClearReferences(user);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                user.NamableId = context.User.Add(user).NamableId;
                context.SaveChanges();
            }
            return AddReferences(user, reference);
        }

        /// <summary>
        /// Gets the specified user identifier.
        /// </summary>
        /// <param name="userID">The user identifier (for the application).</param>
        /// <param name="application">The application name.</param>
        /// <returns></returns>
        public static Entites.User Get(string userID, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.User
                    where c.IdForApplication == userID &&
                          c.Application.ApplicationName == application
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Loads the user names of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The same user with an initialized user name collection.</returns>
        public static Entites.User LoadUserNames(Entites.User user)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.User.Attach(user);
                context.Entry(user).Collection(p => p.UserNames).Load();
            }
            return user;
        }

        /// <summary>
        /// Loads the channels collection of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The same user with an initialized channel collection.</returns>
        public static Entites.User LoadChannels(Entites.User user)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.User.Attach(user);
                context.Entry(user).Collection(p => p.Channels).Load();
            }
            return user;
        }

        /// <summary>
        /// Clears the references of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A copy of the user given in entry with only the references.</returns>
        private static Entites.User ClearReferences(Entites.User user)
        {
            Entites.User reference = new Entites.User(user.Application, null);
            user.Application = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="user">The user to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.User AddReferences(Entites.User user, Entites.User reference)
        {
            user.Application = reference.Application;
            return user;
        }
    }
}