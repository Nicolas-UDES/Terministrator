#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the user names. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class UserName
    {
        /// <summary>
        /// Find whom in the application has that username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="application">The application.</param>
        /// <returns>The user name. Null if none found.</returns>
        public static Entites.UserName GetFromUsername(string username, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.UserName
                    where c.Username == username &&
                          c.Current &&
                          c.OwnedBy.Application.ApplicationName == application
                    select c).Include(x => x.OwnedBy).FirstOrDefault();
            }
        }

        /// <summary>
        /// For an applicationReferencableId and the application's id, find all the user names associated to a user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="application">The application.</param>
        /// <returns></returns>
        public static List<Entites.UserName> GetAll(string userId, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.UserName
                    where c.OwnedBy.IdForApplication == userId &&
                          c.OwnedBy.Application.ApplicationName == application
                    select c).ToList();
            }
        }

        /// <summary>
        /// Creates the specified user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The same user name with the id updated.</returns>
        public static Entites.UserName Create(Entites.UserName userName)
        {
            Entites.UserName reference = ClearReferences(userName);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                userName.UserNameId = context.UserName.Add(userName).UserNameId;
                context.SaveChanges();
            }
            return AddReferences(userName, reference);
        }

        /// <summary>
        /// Switch the user name to a non-current state.
        /// </summary>
        /// <param name="ownedById">The user identifier.</param>
        public static void UpdateUserName(int ownedById)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                (from c in context.UserName
                    where c.OwnedById == ownedById &&
                          c.Current
                    select c).First().Current = false;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Loads the owner of the user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>The same user name but with the owner referenced.</returns>
        public static Entites.UserName LoadOwnedBy(Entites.UserName userName)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (userName.OwnedBy != null)
                {
                    userName.OwnedBy = null;
                }
                if (context.Entry(userName).State == EntityState.Detached)
                {
                    context.UserName.Attach(userName);
                }
                context.Entry(userName).Reference(p => p.OwnedBy).Load();
            }
            return userName;
        }

        /// <summary>
        /// Clears the references of the user name.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A copy of the user name given in entry with only the references.</returns>
        private static Entites.UserName ClearReferences(Entites.UserName userName)
        {
            Entites.UserName reference = new Entites.UserName(null, null, null, true, DateTime.MinValue,
                userName.OwnedBy);
            userName.OwnedBy = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="userName">The user to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.UserName AddReferences(Entites.UserName userName, Entites.UserName reference)
        {
            userName.OwnedBy = reference.OwnedBy;
            return userName;
        }
    }
}