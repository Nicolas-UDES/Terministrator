using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terministrator.Terministrator.Entites
{
    abstract class MessageContent
    {
        [Key]
        public int MessageContentId { get; set; }

        [Required]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public virtual Message Message { get; set; }

        [ForeignKey("SimilarContent")]
        public int? SimilarContentId { get; set; }

        public virtual SimilarContent SimilarContent { get; set; }

        public DateTime SetOn { get; set; }
    }
}
