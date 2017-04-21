namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the point systems. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class PointSystem
    {
        /// <summary>
        /// Creates the specified point system.
        /// </summary>
        /// <param name="pointSystem">The point system.</param>
        /// <returns>The same point system with an updated ID.</returns>
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

        /// <summary>
        /// Gets the point system linked to the specified channel.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <returns>The point system requested.</returns>
        public static Entites.PointSystem Get(int channelId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.PointSystem.Find(channelId);
            }
        }

        /// <summary>
        /// Loads the message type collection of the point system..
        /// </summary>
        /// <param name="pointSystem">The point system.</param>
        /// <returns>The same point system with an initialized message type to point system collection.</returns>
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

        /// <summary>
        /// Clears the references of the point system.
        /// </summary>
        /// <param name="pointSystem">The point system.</param>
        /// <returns>A copy of the point system given in entry with only the references.</returns>
        private static Entites.PointSystem ClearReferences(Entites.PointSystem pointSystem)
        {
            Entites.PointSystem reference = new Entites.PointSystem(pointSystem.Channel);
            pointSystem.Channel = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="pointSystem">The point system to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.PointSystem AddReferences(Entites.PointSystem pointSystem, Entites.PointSystem reference)
        {
            pointSystem.Channel = reference.Channel;
            return pointSystem;
        }
    }
}