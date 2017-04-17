namespace Terministrator.Terministrator.BLL
{
    static class PointSystem
    {
        /// <summary>
        /// Creates a default point system for the specified channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>The created point system.</returns>
        public static Entites.PointSystem Create(Entites.Channel channel)
        {
            return DAL.PointSystem.Create(new Entites.PointSystem(channel));
        }
    }
}