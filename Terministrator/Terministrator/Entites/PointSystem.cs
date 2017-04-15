#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class PointSystem
    {
        public PointSystem() { }

        public PointSystem(Channel channel)
        {
            ChannelId = channel.NamableId;
            Channel = channel;
            ExchangeEnabled = true;
            RewardsEnabled = true;
        }

        [Key]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        public float Total { get; set; }

        public bool ExchangeEnabled { get; set; }

        public bool RewardsEnabled { get; set; }

        public virtual List<Currencies> Currencies { get; set; }

        public virtual List<MessageTypeToPointSystem> MessageTypeToPointSystem { get; set; }
    }
}