#region Usings

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class UserName
    {
        public static bool Exists(string userId, string application)
        {
            return GetAll(userId, application).Count > 0;
        }

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

        public static Entites.UserName GetCurrent(string userId, string application)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.UserName
                    where c.OwnedBy.IdForApplication == userId &&
                          c.Current &&
                          c.OwnedBy.Application.ApplicationName == application
                    select c).FirstOrDefault();
            }
        }

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

        private static Entites.UserName ClearReferences(Entites.UserName userName)
        {
            Entites.UserName reference = new Entites.UserName(null, null, null, true, DateTime.MinValue,
                userName.OwnedBy);
            userName.OwnedBy = null;
            return reference;
        }

        private static Entites.UserName AddReferences(Entites.UserName user, Entites.UserName reference)
        {
            user.OwnedBy = reference.OwnedBy;
            return user;
        }
    }
}