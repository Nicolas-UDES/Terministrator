#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the texts. Contains all the datas required for a text that was contained in a message.
    /// </summary>
    class Text : MessageContent
    {
        public Text()
        {
        }

        public Text(string text, DateTime setOn, Message message, string r9kText = null, SimilarContent similarContent = null)
        {
            ZeText = text;
            SetOn = setOn;
            MessageId = message.MessageId;
            Message = message;
            SimilarContentId = similarContent?.SimilarMessagesId;
            SimilarContent = similarContent;
            R9KText = r9kText;
        }

        public string ZeText { get; set; }

        public string R9KText { get; set; }
    }
}