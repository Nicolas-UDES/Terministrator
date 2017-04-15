#region Usings

using System.Data.Entity;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Rules
    {
        public static bool Exists(Entites.Rules rules)
        {
            return Get(rules.RulesId) != null;
        }

        public static Entites.Rules Create(Entites.Rules rules)
        {
            Entites.Rules reference = ClearReferences(rules);
            using (TerministratorContext conrules = new TerministratorContext(true))
            {
                rules.RulesId = conrules.Rules.Add(rules).RulesId;
                conrules.SaveChanges();
            }
            return AddReferences(rules, reference);
        }

        public static Entites.Rules Get(int rulesId)
        {
            using (TerministratorContext conrules = new TerministratorContext(true))
            {
                return conrules.Rules.Find(rulesId);
            }
        }

        public static Entites.Rules Update(Entites.Rules rules)
        {
            using (TerministratorContext conrules = new TerministratorContext(true))
            {
                Entites.Rules old = conrules.Rules.Find(rules.RulesId);
                if (old != null)
                {
                    old.BlockedWordsEnabled = rules.BlockedWordsEnabled;
                    old.DomainBlocked = rules.DomainBlocked;
                    old.ExtensionBlocked = rules.ExtensionBlocked;
                    old.MessageTypeBlocked = rules.MessageTypeBlocked;
                    conrules.SaveChanges();
                }
            }
            return rules;
        }

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

        private static Entites.Rules ClearReferences(Entites.Rules rules)
        {
            return null;
        }

        private static Entites.Rules AddReferences(Entites.Rules rules, Entites.Rules reference)
        {
            return rules;
        }
    }
}