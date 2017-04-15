#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Application
    {
        public static bool Exists(string applicationName)
        {
            return Get(applicationName) != null;
        }

        public static Entites.Application Get(string applicationName)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.Application
                    where c.ApplicationName == applicationName
                    select c).FirstOrDefault();
            }
        }

        public static Entites.Application Create(Entites.Application application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                application = context.Application.Add(application);
                context.SaveChanges();
                return application;
            }
        }

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