namespace Terministrator.Terministrator.DAL
{
    static class SimilarTexts
    {
        /// <summary>
        /// Creates the specified similar text.
        /// </summary>
        /// <param name="similarText">The similar text.</param>
        /// <returns>The similar text with an updated ID.</returns>
        public static Entites.SimilarTexts Create(Entites.SimilarTexts similarText)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                similarText.SimilarMessagesId = context.SimilarTexts.Add(similarText).SimilarMessagesId;
                context.SaveChanges();
            }
            return similarText;
        }

        /// <summary>
        /// Gets the specified similar texts.
        /// </summary>
        /// <param name="similarTextsId">The similar texts identifier.</param>
        /// <returns>The specified similar texts</returns>
        public static Entites.SimilarTexts Get(int similarTextsId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.SimilarTexts.Find(similarTextsId);
            }
        }

        /// <summary>
        /// Updates the specified similar text.
        /// </summary>
        /// <param name="similarText">The similar text.</param>
        /// <returns>The same similar text.</returns>
        public static Entites.SimilarTexts Update(Entites.SimilarTexts similarText)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.SimilarTexts old = context.SimilarTexts.Find(similarText.SimilarMessagesId);
                if (old != null)
                {
                    old.LastIncrement = similarText.LastIncrement;
                    old.NBSimilar = similarText.NBSimilar;
                    context.SaveChanges();
                }
            }
            return similarText;
        }
    }
}