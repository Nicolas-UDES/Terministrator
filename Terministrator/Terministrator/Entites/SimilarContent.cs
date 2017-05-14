#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the similar texts. Contains all the datas required for a texts that are the same to point on.
    /// </summary>
    class SimilarContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarContent"/> class.
        /// </summary>
        public SimilarContent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarContent"/> class.
        /// </summary>
        /// <param name="nbSimilar">The nb similar.</param>
        /// <param name="lastIncrement">The last increment.</param>
        public SimilarContent(int nbSimilar, DateTime lastIncrement)
        {
            NBSimilar = nbSimilar;
            LastIncrement = lastIncrement;
        }

        [Key]
        public int SimilarMessagesId { get; set; }

        public int NBSimilar { get; set; }
        public DateTime LastIncrement { get; set; }
        public virtual List<MessageContent> Contents { get; set; }
    }
}