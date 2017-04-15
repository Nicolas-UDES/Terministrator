#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class MessageType
    {
        public MessageType()
        {
        }

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