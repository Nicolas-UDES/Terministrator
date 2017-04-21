#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the message types. Contains all the datas required for a message type.
    /// Eg: Text, SystemMessage, File, Emote, etc.
    /// </summary>
    class MessageType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageType"/> class.
        /// </summary>
        public MessageType()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public MessageType(string name)
        {
            Name = name;
        }

        [Key]
        public int MessageTypeId { get; set; }

        [Index(IsUnique = true)]
        [StringLength(128)]
        public string Name { get; set; }

        public virtual List<Message> Messages { get; set; }
        public virtual List<MessageTypeToPointSystem> MessageTypeToPointSystem { get; set; }
        public virtual List<Rules> Rules { get; set; }
    }
}