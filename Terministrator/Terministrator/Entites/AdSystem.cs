#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the ad system. Contains all the datas required for an ad system.
    /// These set the rules for a channel's <see cref="Ad"/>.
    /// </summary>
    class AdSystem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdSystem"/> class.
        /// </summary>
        public AdSystem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdSystem"/> class.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="minNbOfMesage">The minimum nb of mesage.</param>
        /// <param name="minTime">The minimum time.</param>
        /// <param name="bothConditions">if set to <c>true</c> [both conditions].</param>
        public AdSystem(Channel channel, int minNbOfMesage, TimeSpan minTime, bool bothConditions)
        {
            ChannelId = channel.NamableId;
            Channel = channel;
            MinNbOfMessage = minNbOfMesage;
            MinTime = minTime;
            BothConditions = bothConditions;
        }

        [Key]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
        public int MinNbOfMessage { get; set; }

        [Required]
        public TimeSpan MinTime { get; set; }

        public bool BothConditions { get; set; }
        public virtual List<Ad> Ad { get; set; }
    }
}