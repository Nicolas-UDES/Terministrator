#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the blocked words. Contains all the datas required for a blocked word.
    /// Pretty much only the word itself.
    /// </summary>
    class BlockedWord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedWord"/> class.
        /// </summary>
        public BlockedWord()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedWord"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        public BlockedWord(string word)
        {
            Word = word;
        }

        [Key]
        public int BlockedWordId { get; set; }

        public string Word { get; set; }
        public virtual List<Rules> Rules { get; set; }
    }
}