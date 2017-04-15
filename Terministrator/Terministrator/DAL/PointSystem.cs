namespace Terministrator.Terministrator.DAL
{
    static class PointSystem
    {
        public static Entites.PointSystem Create(Entites.PointSystem pointSystem)
        {
            Entites.PointSystem reference = ClearReferences(pointSystem);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.PointSystem.Add(pointSystem);
                context.SaveChanges();
            }
            return AddReferences(pointSystem, reference);
        }

        public static Entites.PointSystem Get(int channel)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.PointSystem.Find(channel);
            }
        }

        public static Entites.PointSystem LoadMessageTypes(Entites.PointSystem pointSystem)
        {
            if (pointSystem == null)
            {
                return null;
            }

            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.PointSystem.Attach(pointSystem);
                context.Entry(pointSystem).Collection(p => p.MessageTypeToPointSystem).Load();
            }
            return pointSystem;
        }

        private static Entites.PointSystem ClearReferences(Entites.PointSystem pointSystem)
        {
            Entites.PointSystem reference = new Entites.PointSystem(pointSystem.Channel);
            pointSystem.Channel = null;
            return reference;
        }

        private static Entites.PointSystem AddReferences(Entites.PointSystem pointSystem, Entites.PointSystem reference)
        {
            pointSystem.Channel = reference.Channel;
            return pointSystem;
        }
    }
}