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
    class SimilarTexts
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarTexts"/> class.
        /// </summary>
        public SimilarTexts()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimilarTexts"/> class.
        /// </summary>
        /// <param name="nbSimilar">The nb similar.</param>
        /// <param name="lastIncrement">The last increment.</param>
        public SimilarTexts(int nbSimilar, DateTime lastIncrement)
        {
            NBSimilar = nbSimilar;
            LastIncrement = lastIncrement;
        }

        [Key]
        public int SimilarMessagesId { get; set; }

        public int NBSimilar { get; set; }
        public DateTime LastIncrement { get; set; }
        public virtual List<Text> Texts { get; set; }
    }
}