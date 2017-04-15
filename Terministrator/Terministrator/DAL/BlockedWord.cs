#region Usings

using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class BlockedWord
    {
        public static bool Exists(Entites.BlockedWord blockedWord)
        {
            return Get(blockedWord.Word) != null;
        }

        public static Entites.BlockedWord Create(Entites.BlockedWord blockedWord)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                blockedWord.BlockedWordId = context.BlockedWord.Add(blockedWord).BlockedWordId;
                context.SaveChanges();
                return blockedWord;
            }
        }

        public static Entites.BlockedWord Get(string word)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return (from c in context.BlockedWord
                    where c.Word == word
                    select c).FirstOrDefault();
            }
        }

        public static Entites.BlockedWord Get(int blockedWordId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.BlockedWord.Find(blockedWordId);
            }
        }
    }
}