#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class BlockedWord
    {
        /// <summary>
        /// Creates the specified blocked word.
        /// </summary>
        /// <param name="blockedWord">The blocked word.</param>
        /// <returns>The same blocked word with an updated ID.</returns>
        public static Entites.BlockedWord Create(Entites.BlockedWord blockedWord)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                blockedWord.BlockedWordId = context.BlockedWord.Add(blockedWord).BlockedWordId;
                context.SaveChanges();
                return blockedWord;
            }
        }

        /// <summary>
        /// Gets the specified blocked word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The requested blocked word.</returns>
        public static Entites.BlockedWord Get(string word)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.BlockedWord
                    where c.Word == word
                    select c).FirstOrDefault();
            }
        }
    }
}