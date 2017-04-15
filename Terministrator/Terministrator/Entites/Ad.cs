#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Ad
    {
        public Ad()
        {
        }

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