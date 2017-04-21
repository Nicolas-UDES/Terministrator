#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the message type to point systems.
    /// Contains all the datas required for a point reward between a message type and a point system (so a channel).
    /// </summary>
    class MessageTypeToPointSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTypeToPointSystem"/> class.
        /// </summary>
        public MessageTypeToPointSystem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTypeToPointSystem"/> class.
        /// </summary>
        /// <param name="pointSystem">The point system.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="reward">The reward.</param>
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