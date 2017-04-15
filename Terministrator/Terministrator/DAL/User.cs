#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class User
    {
        public static bool Exists(Entites.User user)
        {
            return Get(user.IdForApplication, user.Application.ApplicationName) != null;
        }

        public static Entites.User Create(Entites.User user)
        {
            Entites.User reference = ClearReferences(user);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.User newUser = context.User.Add(user);
                user.NamableId = newUser.NamableId;
                //context.Entry(user.Application).State = EntityState.Unchanged;
                context.SaveChanges();
            }
            return AddReferences(user, reference);
        }

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

        public static Entites.User LoadUserNames(Entites.User user)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.User.Attach(user);
                context.Entry(user).Collection(p => p.UserNames).Load();
            }
            return user;
        }

        public static Entites.User LoadChannels(Entites.User user)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.User.Attach(user);
                context.Entry(user).Collection(p => p.Channels).Load();
            }
            return user;
        }

        private static Entites.User ClearReferences(Entites.User user)
        {
            Entites.User reference = new Entites.User(user.Application, null);
            user.Application = null;
            return reference;
        }

        private static Entites.User AddReferences(Entites.User user, Entites.User reference)
        {
            user.Application = reference.Application;
            return user;
        }
    }
}