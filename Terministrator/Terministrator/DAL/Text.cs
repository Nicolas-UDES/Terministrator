#region Usings

using System;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    /// <summary>
    /// Data access layer of the texts. Process every exchanges with Entity-Framework (AKA the database).
    /// </summary>
    static class Text
    {
        /// <summary>
        /// Creates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text with an updated id.</returns>
        public static Entites.Text Create(Entites.Text text)
        {
            Entites.Text reference = ClearReferences(text);
            using (TerministratorContext context = new TerministratorContext(true))
            {
                text.TextId = context.Text.Add(text).TextId;
                context.SaveChanges();
            }
            return AddReferences(text, reference);
        }

        /// <summary>
        /// Gets the specified text.
        /// </summary>
        /// <param name="textId">The text identifier.</param>
        /// <returns>The text with the specified identifier.</returns>
        public static Entites.Text Get(int textId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Text.Find(textId);
            }
        }

        /// <summary>
        /// Updates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text given in arguement.</returns>
        public static Entites.Text Update(Entites.Text text)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Text old = context.Text.Find(text.TextId);
                if (old != null)
                {
                    old.SimilarTextsId = text.SimilarTextsId;
                    context.SaveChanges();
                    context.Entry(old).Reference(x => x.SimilarTexts);
                }
                text.SimilarTexts = old?.SimilarTexts;
            }
            return text;
        }

        /// <summary>
        /// Gets a text with the same r9k as the given text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>A text with the same r9k. Null if none found.</returns>
        public static Entites.Text GetR9K(Entites.Text text)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                Entites.Text retour =
                    context.Text.FirstOrDefault(x => x.R9KText == text.R9KText && x.TextId != text.TextId);
                if (retour != null)
                {
                    context.Entry(retour).Reference(p => p.SimilarTexts).Load();
                }
                return retour;
            }
        }

        /// <summary>
        /// Loads the similar texts reference.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The same text with the reference updated.</returns>
        public static Entites.Text LoadSimilarTexts(Entites.Text text)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Text.Attach(text);
                context.Entry(text).Reference(p => p.SimilarTexts).Load();
            }
            return text;
        }

        /// <summary>
        /// Clears the references of the text.
        /// </summary>
        /// <param name="text">The user.</param>
        /// <returns>A copy of the text given in entry with only the references.</returns>
        private static Entites.Text ClearReferences(Entites.Text text)
        {
            Entites.Text reference = new Entites.Text(null, DateTime.MinValue, text.Message, null, text.SimilarTexts);
            text.Message = null;
            text.SimilarTexts = null;
            return reference;
        }

        /// <summary>
        /// Adds the references of the second arguement in the first one.
        /// </summary>
        /// <param name="text">The text to add the references in.</param>
        /// <param name="reference">The references.</param>
        /// <returns>The first arguement.</returns>
        private static Entites.Text AddReferences(Entites.Text text, Entites.Text reference)
        {
            text.Message = reference.Message;
            text.SimilarTexts = reference.SimilarTexts;
            return text;
        }
    }
}