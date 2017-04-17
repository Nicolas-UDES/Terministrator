#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Privileges
    {
        public Privileges()
        {
        }

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