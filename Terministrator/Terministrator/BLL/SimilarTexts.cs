#region Usings

using System;

#endregion

namespace Terministrator.Terministrator.BLL
{
    /// <summary>
    /// Business logic layer of the similar texts. Simply create empty ones and let increment when a new similar message is found.
    /// </summary>
    static class SimilarTexts
    {
        /// <summary>
        /// Creates a default SimilarContent.
        /// </summary>
        /// <returns></returns>
        public static Entites.SimilarContent Create()
        {
            return DAL.SimilarTexts.Create(new Entites.SimilarContent(1, DateTime.UtcNow));
        }

        /// <summary>
        /// Increments the specified similar texts's amount number.
        /// </summary>
        /// <param name="similarTextsId">The similar texts identifier.</param>
        /// <returns>The incremented similar texts.</returns>
        public static Entites.SimilarContent Increment(int similarTextsId)
        {
            Entites.SimilarContent similarContent = DAL.SimilarTexts.Get(similarTextsId);
            similarContent.LastIncrement = DateTime.UtcNow;
            similarContent.NBSimilar++;
            return DAL.SimilarTexts.Update(similarContent);
        }
    }
}