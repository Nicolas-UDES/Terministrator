#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Text
    {
        public Text()
        {
        }

        public Text(string text, DateTime setOn, Message message, string r9kText = null,
            SimilarTexts similarTexts = null)
        {
            ZeText = text;
            SetOn = setOn;
            MessageId = message.MessageId;
            Message = message;
            SimilarTextsId = similarTexts?.SimilarMessagesId;
            SimilarTexts = similarTexts;
            R9KText = r9kText;
        }

        [Key]
        public int TextId { get; set; }

        [Required]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public virtual Message Message { get; set; }

        [ForeignKey("SimilarTexts")]
        public int? SimilarTextsId { get; set; }

        public virtual SimilarTexts SimilarTexts { get; set; }

        public string ZeText { get; set; }

        public DateTime SetOn { get; set; }

        public string R9KText { get; set; }
    }
}