using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terministrator.Terministrator.DAL
{
    static class MessageTypeToPointSystem
    {
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

        public static void DeleteAllFrom(int pointSystemId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.MessageTypeToPointSystem.RemoveRange(context.MessageTypeToPointSystem.Where(x => x.PointSystemId == pointSystemId));
                context.SaveChanges();
            }
        }

        public static List<Entites.MessageTypeToPointSystem> GetAll(int pointSystemId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.MessageTypeToPointSystem.Where(x => x.PointSystemId == pointSystemId).Include(x => x.MessageType).Include(x => x.PointSystem).ToList();
            }
        }

        private static Entites.MessageTypeToPointSystem ClearReferences(Entites.MessageTypeToPointSystem messageTypeToPointSystem)
        {
            Entites.MessageTypeToPointSystem reference = new Entites.MessageTypeToPointSystem(messageTypeToPointSystem.PointSystem, messageTypeToPointSystem.MessageType, 0);
            messageTypeToPointSystem.MessageType = null;
            messageTypeToPointSystem.PointSystem = null;
            return reference;
        }

        private static Entites.MessageTypeToPointSystem AddReferences(Entites.MessageTypeToPointSystem messageTypeToPointSystem, Entites.MessageTypeToPointSystem reference)
        {
            messageTypeToPointSystem.MessageType = reference.MessageType;
            messageTypeToPointSystem.PointSystem = reference.PointSystem;
            return messageTypeToPointSystem;
        }
    }
}
