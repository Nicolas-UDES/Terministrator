#region Usings

using System.Collections.Generic;
using System.Configuration;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the blocked words. Currently only gives the default words.
    /// </summary>
    static class BlockedWord
    {
        /// <summary>
        /// Gets the default blocked words from app.config.
        /// </summary>
        /// <returns>The requested blocked words collection.</returns>
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