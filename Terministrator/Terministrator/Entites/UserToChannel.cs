#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the user to channels. Contains all the datas required for the information specific to a user on a single channel.
    /// </summary>
    class UserToChannel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserToChannel"/> class.
        /// </summary>
        public UserToChannel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserToChannel"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="user">The user.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="joinedOn">The joined on.</param>
        /// <param name="privileges">The privileges.</param>
        /// <param name="points">The points.</param>
        /// <param name="nbSilences">The nb silences.</param>
        /// <param name="silencedTo">The silenced to.</param>
        public UserToChannel(Application application, User user, Channel channel, DateTime joinedOn,
            Privileges privileges, float points = 0, int nbSilences = 0, DateTime? silencedTo = null)
        {
            ApplicationName = application?.ApplicationName;
            Application = application;
            UserId = user?.NamableId;
            User = user;
            ChannelId = channel?.NamableId;
            Channel = channel;
            JoinedOn = joinedOn;
            PrivilegesId = privileges?.PrivilegesId;
            Privileges = privileges;
            Points = points;
            SilencedTo = silencedTo;
            NbSilences = nbSilences;
        }

        [Key]
        public int UserToChannelId { get; set; }

        [ForeignKey("Application")]
        public string ApplicationName { get; set; }

        public virtual Application Application { get; set; }

        [ForeignKey("User")]
        [Index("IX_OneUserToChannel", 1, IsUnique = true)]
        public int? UserId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("Channel")]
        [Index("IX_OneUserToChannel", 2, IsUnique = true)]
        public int? ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        public DateTime JoinedOn { get; set; }

        public virtual List<Message> Messages { get; set; }

        [ForeignKey("Privileges")]
        public int? PrivilegesId { get; set; }

        public virtual Privileges Privileges { get; set; }

        public DateTime? SilencedTo { get; set; }

        public int NbSilences { get; set; }

        public float Points { get; set; }
    }
}