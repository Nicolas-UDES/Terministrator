#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class BlockedWord
    {
        public BlockedWord()
        {
            
        }

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