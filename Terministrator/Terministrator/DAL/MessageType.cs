#region Usings

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class MessageType
    {
        /// <summary>
        /// Creates the specified message type.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>The same message type with an updated ID.</returns>
        public static Entites.MessageType Create(Entites.MessageType messageType)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                messageType.MessageTypeId = context.MessageType.Add(messageType).MessageTypeId;
                context.SaveChanges();
                return messageType;
            }
        }

        /// <summary>
        /// Updates the name of the specified message type.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>The same message type.</returns>
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

        /// <summary>
        /// Gets the specified message type.
        /// </summary>
        /// <param name="name">The name of the requested message type.</param>
        /// <returns>The requested message type.</returns>
        public static Entites.MessageType Get(string name)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.MessageType
                    where c.Name == name
                    select c).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets all message types.
        /// </summary>
        /// <returns>The collection of message types.</returns>
        public static List<Entites.MessageType> GetAll()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.MessageType
                    select c).ToList();
            }
        }

        /// <summary>
        /// Counts how many message types there are.
        /// </summary>
        /// <returns>The amount of message types.</returns>
        public static int Count()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.MessageType.Count();
            }
        }

        /// <summary>
        /// Deletes all message types from the database.
        /// </summary>
        public static void DeleteAll()
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.MessageType.RemoveRange(context.MessageType);
            }
        }
    }
}