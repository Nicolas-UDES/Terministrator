namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the similar texts. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class SimilarTexts
    {
        /// <summary>
        /// Creates the specified similar text.
        /// </summary>
        /// <param name="similarContent">The similar text.</param>
        /// <returns>The similar text with an updated ID.</returns>
        public static Entites.SimilarContent Create(Entites.SimilarContent similarContent)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                similarContent.SimilarMessagesId = context.SimilarTexts.Add(similarContent).SimilarMessagesId;
                context.SaveChanges();
            }
            return similarContent;
        }

        /// <summary>
        /// Gets the specified similar texts.
        /// </summary>
        /// <param name="similarTextsId">The similar texts identifier.</param>
        /// <returns>The specified similar texts</returns>
        public static Entites.SimilarContent Get(int similarTextsId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.SimilarTexts.Find(similarTextsId);
            }
        }

        /// <summary>
        /// Updates the specified similar text.
        /// </summary>
        /// <param name="similarContent">The similar text.</param>
        /// <returns>The same similar text.</returns>
        public static Entites.SimilarContent Update(Entites.SimilarContent similarContent)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.SimilarContent old = context.SimilarTexts.Find(similarContent.SimilarMessagesId);
                if (old != null)
                {
                    old.LastIncrement = similarContent.LastIncrement;
                    old.NBSimilar = similarContent.NBSimilar;
                    context.SaveChanges();
                }
            }
            return similarContent;
        }
    }
}