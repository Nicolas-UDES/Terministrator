#region Usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class MessageType
    {
        public static bool Exists(Entites.MessageType messageType)
        {
            return Get(messageType.Name) != null;
        }

        public static Entites.MessageType Create(Entites.MessageType messageType)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                messageType.MessageTypeId = context.MessageType.Add(messageType).MessageTypeId;
                context.SaveChanges();
                return messageType;
            }
        }

        public static Entites.MessageType Update(Entites.MessageType messageType)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.MessageType original = context.MessageType.Find(messageType.MessageTypeId);
                if (original != null)
                {
                    original.Name = messageType.Name;
                    context.SaveChanges();
                }
                return original;
            }
        }

        public static Entites.MessageType CreateOrUpdate(Entites.MessageType messageType)
        {
            if (Exists(messageType))
            {
                return Update(messageType);
            }
            return Create(messageType);
        }

        public static Entites.MessageType Get(string name)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.MessageType
                    where c.Name == name
                    select c).FirstOrDefault();
            }
        }

        public static List<Entites.MessageType> GetAll()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.MessageType
                    select c).ToList();
            }
        }

        public static int Count()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.MessageType.Count();
            }
        }

        public static void DeleteAll()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.MessageType.RemoveRange(context.MessageType);
            }
        }
    }
}