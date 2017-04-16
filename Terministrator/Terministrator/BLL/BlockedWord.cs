#region Usings

using System.Collections.Generic;
using System.Configuration;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class BlockedWord
    {
        // https://www.frontgatemedia.com/a-list-of-723-bad-words-to-blacklist-and-how-to-use-facebooks-moderation-tool/

        public static List<Entites.BlockedWord> GetDefaultBlockedWords()
        {
            List<Entites.BlockedWord> types = new List<Entites.BlockedWord>();
            foreach (string word in ConfigurationManager.AppSettings["DefaultBlockedWords"].Split(','))
            {
                types.Add(DAL.BlockedWord.Get(word) ?? DAL.BlockedWord.Create(new Entites.BlockedWord(word)));
            }
            return types;
        }
    }
}