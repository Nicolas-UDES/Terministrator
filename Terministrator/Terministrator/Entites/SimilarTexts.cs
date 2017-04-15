#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class SimilarTexts
    {
        public SimilarTexts()
        {
        }

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