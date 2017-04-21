#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the ad. Contains all the datas required for an ad.
    /// These are shown on the channels following said channel's <see cref="AdSystem"/> requirements.
    /// </summary>
    class Ad
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ad"/> class.
        /// </summary>
        public Ad()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ad"/> class.
        /// </summary>
        /// <param name="maxShow">The maximum show.</param>
        /// <param name="name">The name.</param>
        /// <param name="lastSent">The last sent.</param>
        /// <param name="message">The message.</param>
        /// <param name="adSystem">The ad system.</param>
        public Ad(int maxShow, string name, DateTime lastSent, Message message, AdSystem adSystem)
        {
            MaxShow = maxShow;
            Name = name;
            LastSent = lastSent;
            MessageId = message.MessageId;
            Message = message;
            AdSystemId = adSystem.ChannelId;
            AdSystem = adSystem;
        }

        [Key]
        public int AdId { get; set; }

        public int MaxShow { get; set; }

        public string Name { get; set; }

        public DateTime LastSent { get; set; }


        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public virtual Message Message { get; set; }


        [ForeignKey("AdSystem")]
        public int AdSystemId { get; set; }

        public virtual AdSystem AdSystem { get; set; }
    }
}