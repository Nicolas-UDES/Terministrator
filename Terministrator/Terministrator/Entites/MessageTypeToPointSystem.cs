#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class MessageTypeToPointSystem
    {
        public MessageTypeToPointSystem()
        {
        }

        public MessageTypeToPointSystem(PointSystem pointSystem, MessageType messageType, float reward)
        {
            MessageTypeId = messageType.MessageTypeId;
            MessageType = messageType;
            PointSystemId = pointSystem.ChannelId;
            PointSystem = pointSystem;
            Reward = reward;
        }

        [Key]
        [ForeignKey("MessageType")]
        [Column(Order = 0)]
        public int MessageTypeId { get; set; }

        [Key]
        [ForeignKey("PointSystem")]
        [Column(Order = 1)]
        public int PointSystemId { get; set; }

        public virtual MessageType MessageType { get; set; }
        public virtual PointSystem PointSystem { get; set; }
        public float Reward { get; set; }
    }
}