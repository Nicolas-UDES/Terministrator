#region Usings

using System;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the privileges. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class Privileges
    {
        /// <summary>
        /// Creates the specified privileges.
        /// </summary>
        /// <param name="privileges">The privileges.</param>
        /// <returns>The same privileges with an updated id.</returns>
        public static Entites.Privileges Create(Entites.Privileges privileges)
        {
            Entites.Privileges reference = ClearReferences(privileges);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                privileges.PrivilegesId = context.Privileges.Add(privileges).PrivilegesId;
                context.SaveChanges();
            }
            return AddReferences(privileges, reference);
        }

        /// <summary>
        /// Gets the specified privileges.
        /// </summary>
        /// <param name="privilegesId">The privileges identifier.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges Get(int privilegesId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Privileges.Find(privilegesId);
            }
        }

        /// <summary>
        /// Updates the specified privileges.
        /// </summary>
        /// <param name="privileges">The privileges.</param>
        /// <returns>The same privileges.</returns>
        public static Entites.Privileges Update(Entites.Privileges privileges)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Privileges old = context.Privileges.Find(privileges.PrivilegesId);
                if (old != null)
                {
                    old.Name = privileges.Name;
                    old.RulesId = privileges.RulesId;
                    old.Default = privileges.Default;
                    context.SaveChanges();
                }
            }
            return privileges;
        }

        /// <summary>
        /// Gets the default privileges of a channel.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges GetDefaultUser(int channelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Privileges
                    where c.ChannelId == channelId &&
                          c.Default
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the privileges associated to a channel.
        /// </summary>
        /// <param name="channelId">The channel identifier (namable).</param>
        /// <param name="name">The name.</param>
        /// <returns>The requested privileges.</returns>
        public static Entites.Privileges GetPrivileges(int channelId, string name)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Privileges
                    where c.ChannelId == channelId &&
                          c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Loads the rules reference.
        /// </summary>
        /// <param name="privileges">The privileges.</param>
        /// <returns>The same privileges with the initialized privileges reference.</returns>
        public static Entites.Privileges LoadRules(Entites.Privileges privileges)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (privileges.Rules != null)
                {
                    privileges.Rules = null;
                }
                if (context.Entry(privileges).State == EntityState.Detached)
                {
                    context.Privileges.Attach(privileges);
                }
                context.Entry(privileges).Reference(p => p.Rules).Load();
            }
            return privileges;
        }

        /// <summary>
        /// Clears the references of the privileges.
        /// </summary>
        /// <param name="privileges">The privileges.</param>
        /// <returns>A copy of the privileges given in entry with only the references.</returns>
        private static Entites.Privileges ClearReferences(Entites.Privileges privileges)
        {
            Entites.Privileges references = new Entites.Privileges(null, false, privileges.Channel, privileges.Rules);
            privileges.Channel = null;
            privileges.Rules = null;
            return references;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="privileges">The privileges to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Privileges AddReferences(Entites.Privileges privileges, Entites.Privileges reference)
        {
            privileges.Channel = reference.Channel;
            privileges.Rules = reference.Rules;
            return privileges;
        }
    }
}