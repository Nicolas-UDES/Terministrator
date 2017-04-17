#region Usings

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class MessageTypeToPointSystem
    {
        /// <summary>
        /// Creates the specified message type to point system.
        /// </summary>
        /// <param name="messageTypeToPointSystem">The message type to point system.</param>
        /// <returns>The same message type to point system.</returns>
        public static Entites.MessageTypeToPointSystem Create(Entites.MessageTypeToPointSystem messageTypeToPointSystem)
        {
            Entites.MessageTypeToPointSystem reference = ClearReferences(messageTypeToPointSystem);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.MessageTypeToPointSystem.Add(messageTypeToPointSystem);
                context.SaveChanges();
            }
            return AddReferences(messageTypeToPointSystem, reference);
        }

        /// <summary>
        /// Deletes all the message type to point systems from the point system.
        /// </summary>
        /// <param name="pointSystemId">The point system identifier.</param>
        public static void DeleteAllFrom(int pointSystemId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.MessageTypeToPointSystem.RemoveRange(
                    context.MessageTypeToPointSystem.Where(x => x.PointSystemId == pointSystemId));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gets all the message type to point systems associated to a point system.
        /// </summary>
        /// <param name="pointSystemId">The point system identifier.</param>
        /// <returns>The requested collection.</returns>
        public static List<Entites.MessageTypeToPointSystem> GetAll(int pointSystemId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return
                    context.MessageTypeToPointSystem.Where(x => x.PointSystemId == pointSystemId)
                        .Include(x => x.MessageType)
                        .Include(x => x.PointSystem)
                        .ToList();
            }
        }

        /// <summary>
        /// Clears the references of the message type to point system.
        /// </summary>
        /// <param name="messageTypeToPointSystem">The message type to point system.</param>
        /// <returns>A copy of the message type to point system given in entry with only the references.</returns>
        private static Entites.MessageTypeToPointSystem ClearReferences(
            Entites.MessageTypeToPointSystem messageTypeToPointSystem)
        {
            Entites.MessageTypeToPointSystem reference =
                new Entites.MessageTypeToPointSystem(messageTypeToPointSystem.PointSystem,
                    messageTypeToPointSystem.MessageType, 0);
            messageTypeToPointSystem.MessageType = null;
            messageTypeToPointSystem.PointSystem = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="messageTypeToPointSystem">The message type to point system to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.MessageTypeToPointSystem AddReferences(
            Entites.MessageTypeToPointSystem messageTypeToPointSystem, Entites.MessageTypeToPointSystem reference)
        {
            messageTypeToPointSystem.MessageType = reference.MessageType;
            messageTypeToPointSystem.PointSystem = reference.PointSystem;
            return messageTypeToPointSystem;
        }
    }
}