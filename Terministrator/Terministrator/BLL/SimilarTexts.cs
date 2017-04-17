#region Usings

using System;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class SimilarTexts
    {
        /// <summary>
        /// Creates a default SimilarTexts.
        /// </summary>
        /// <returns></returns>
        public static Entites.SimilarTexts Create()
        {
            return DAL.SimilarTexts.Create(new Entites.SimilarTexts(1, DateTime.UtcNow));
        }

        /// <summary>
        /// Increments the specified similar texts's amount number.
        /// </summary>
        /// <param name="similarTextsId">The similar texts identifier.</param>
        /// <returns>The incremented similar texts.</returns>
        public static Entites.SimilarTexts Increment(int similarTextsId)
        {
            Entites.SimilarTexts similarTexts = DAL.SimilarTexts.Get(similarTextsId);
            similarTexts.LastIncrement = DateTime.UtcNow;
            similarTexts.NBSimilar++;
            return DAL.SimilarTexts.Update(similarTexts);
        }
    }
}