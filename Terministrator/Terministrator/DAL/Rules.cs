#region Usings

using System.Data.Entity;
using System.Security.Cryptography;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Rules
    {
        /// <summary>
        /// Creates the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with the updated ID.</returns>
        public static Entites.Rules Create(Entites.Rules rules)
        {
            Entites.Rules reference = ClearReferences(rules);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                rules.RulesId = context.Rules.Add(rules).RulesId;
                context.SaveChanges();
            }
            return AddReferences(rules, reference);
        }

        /// <summary>
        /// Gets the specified rules.
        /// </summary>
        /// <param name="rulesId">The rules identifier.</param>
        /// <returns>The rules requested.</returns>
        public static Entites.Rules Get(int rulesId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Rules.Find(rulesId);
            }
        }

        /// <summary>
        /// Updates the specified rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules.</returns>
        public static Entites.Rules Update(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Rules old = context.Rules.Find(rules.RulesId);
                if (old != null)
                {
                    old.SpamDelay = rules.SpamDelay;
                    old.BlockedWordsEnabled = rules.BlockedWordsEnabled;
                    old.DomainBlocked = rules.DomainBlocked;
                    old.ExtensionBlocked = rules.ExtensionBlocked;
                    old.MessageTypeBlocked = rules.MessageTypeBlocked;
                    old.R9KEnabled = rules.R9KEnabled;
                    context.SaveChanges();
                }
            }
            return rules;
        }

        /// <summary>
        /// Updates the blocked words.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public static Entites.Rules UpdateBlockedWords(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Rules old = context.Rules.Find(rules.RulesId);
                if (old != null)
                {
                    old.BlockedWords = rules.BlockedWords;
                    context.SaveChanges();
                }
            }
            return rules;
        }

        /// <summary>
        /// Loads the blocked extensions associated to rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with the blocked extensions collection initialized.</returns>
        public static Entites.Rules LoadExtensions(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (rules.Extensions != null)
                {
                    rules.Extensions = null;
                }
                if (context.Entry(rules).State == EntityState.Detached)
                {
                    context.Rules.Attach(rules);
                }
                context.Entry(rules).Collection(p => p.Extensions).Load();
            }
            return rules;
        }

        /// <summary>
        /// Loads the blocked message types associated to rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with the blocked message types collection initialized.</returns>
        public static Entites.Rules LoadMessageTypes(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (rules.MessageTypes != null)
                {
                    rules.MessageTypes = null;
                }
                if (context.Entry(rules).State == EntityState.Detached)
                {
                    context.Rules.Attach(rules);
                }
                context.Entry(rules).Collection(p => p.MessageTypes).Load();
            }
            return rules;
        }

        /// <summary>
        /// Loads the blocked words associated to rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with the blocked words collection initialized.</returns>
        public static Entites.Rules LoadBlockedWords(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (rules.BlockedWords != null)
                {
                    rules.BlockedWords = null;
                }
                if (context.Entry(rules).State == EntityState.Detached)
                {
                    context.Rules.Attach(rules);
                }
                context.Entry(rules).Collection(p => p.BlockedWords).Load();
            }
            return rules;
        }

        /// <summary>
        /// Loads the blocked domains associated to rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>The same rules with the blocked domains collection initialized.</returns>
        public static Entites.Rules LoadBlockedDomains(Entites.Rules rules)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                if (rules.Domains != null)
                {
                    rules.Domains = null;
                }
                if (context.Entry(rules).State == EntityState.Detached)
                {
                    context.Rules.Attach(rules);
                }
                context.Entry(rules).Collection(p => p.Domains).Load();
            }
            return rules;
        }

        /// <summary>
        /// Clears the references of the rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns>A copy of the rules given in entry with only the references.</returns>
        private static Entites.Rules ClearReferences(Entites.Rules rules)
        {
            return null;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="rules">The rules to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Rules AddReferences(Entites.Rules rules, Entites.Rules reference)
        {
            return rules;
        }
    }
}