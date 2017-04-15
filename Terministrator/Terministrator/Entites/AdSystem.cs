#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class AdSystem
    {
        public AdSystem()
        {
        }

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