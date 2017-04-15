namespace Terministrator.Terministrator.BLL
{
    static class PointSystem
    {
        public static Entites.PointSystem GetOrCreate(Entites.Channel channel)
        {
            return Get(channel.NamableId) ?? Create(channel);
        }

        public static Entites.PointSystem Get(int channelId)
        {
            return DAL.PointSystem.Get(channelId);
        }

        public static Entites.PointSystem Create(Entites.Channel channel)
        {
            return DAL.PointSystem.Create(new Entites.PointSystem(channel));
        }
    }
}
