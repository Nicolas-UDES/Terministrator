#region Usings

using System;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Privileges
    {
        public static bool Exists(Entites.Privileges privileges)
        {
            return Get(privileges.PrivilegesId) != null;
        }

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

        public static Entites.Privileges Get(int privilegesId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Privileges.Find(privilegesId);
            }
        }

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

        private static Entites.Privileges ClearReferences(Entites.Privileges privileges)
        {
            Entites.Privileges references = new Entites.Privileges(null, false, privileges.Channel, privileges.Rules);
            privileges.Channel = null;
            privileges.Rules = null;
            return references;
        }

        private static Entites.Privileges AddReferences(Entites.Privileges privileges, Entites.Privileges reference)
        {
            privileges.Channel = reference.Channel;
            privileges.Rules = reference.Rules;
            return privileges;
        }
    }
}