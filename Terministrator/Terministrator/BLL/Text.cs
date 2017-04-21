#region Usings

using System.Diagnostics;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the texts. Helps searching for r9k texts.
    /// </summary>
    static class Text
    {
        /// <summary>
        /// Gets another text with the same r9k.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A similar text. Null if nothing found.</returns>
        public static Entites.Text GetR9K(Entites.Text text)
        {
            return DAL.Text.GetR9K(text);
        }

        /// <summary>
        /// Creates the specified text in the database.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text with an updated ID.</returns>
        public static Entites.Text Create(Entites.Text text)
        {
            return DAL.Text.Create(text);
        }

        /// <summary>
        /// Updates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text.</returns>
        public static Entites.Text Update(Entites.Text text)
        {
            return DAL.Text.Update(text);
        }

        /// <summary>
        /// Searches another text with the same R9K. If one is found, link this one to their Similar Text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text with a possibly updated Similar Texts reference.</returns>
        public static Entites.Text SearchAndLink(Entites.Text text)
        {
            Entites.Text otherText = GetR9K(text);
            if (otherText != null)
            {
                if (otherText.SimilarTexts == null)
                {
                    otherText.SimilarTextsId = SimilarTexts.Create().SimilarMessagesId;
                }
                text.SimilarTextsId = otherText.SimilarTextsId;
                DAL.Text.LoadSimilarTexts(Update(text));
                Debug.Assert(text.SimilarTextsId != null, "text.SimilarTextsId != null");
                SimilarTexts.Increment(text.SimilarTextsId.Value);
            }
            return text;
        }
    }
}