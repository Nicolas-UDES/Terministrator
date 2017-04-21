#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the privileges. Contains all the datas required for the different privileges on a channel.
    /// </summary>
    class Privileges
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Privileges"/> class.
        /// </summary>
        public Privileges()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Privileges"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaultUser">if set to <c>true</c> [default user].</param>
        /// <param name="channel">The channel.</param>
        /// <param name="rules">The rules.</param>
        public Privileges(string name, bool defaultUser, Channel channel, Rules rules)
        {
            Name = name;
            Default = defaultUser;
            ChannelId = channel.NamableId;
            Channel = channel;
            RulesId = rules.RulesId;
            Rules = rules;
        }

        [Key]
        public int PrivilegesId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual List<UserToChannel> UserToChannels { get; set; }

        [Required]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }

        public virtual Channel Channel { get; set; }

        [Required]
        [ForeignKey("Rules")]
        public int RulesId { get; set; }

        public virtual Rules Rules { get; set; }

        public bool Default { get; set; }
    }
}