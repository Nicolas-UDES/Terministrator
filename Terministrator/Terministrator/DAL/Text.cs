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
                text.MessageContentId = context.Text.Add(text).MessageContentId;
                context.SaveChanges();
            }
            return AddReferences(text, reference);
        }

        /// <summary>
        /// Gets the specified text.
        /// </summary>
        /// <param name="messageContentId">The text identifier.</param>
        /// <returns>The text with the specified identifier.</returns>
        public static Entites.Text Get(int messageContentId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Text.Find(messageContentId);
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
                Entites.Text old = context.Text.Find(text.MessageContentId);
                if (old != null)
                {
                    old.SimilarContentId = text.SimilarContentId;
                    context.SaveChanges();
                    context.Entry(old).Reference(x => x.SimilarContent);
                }
                text.SimilarContent = old?.SimilarContent;
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
                    context.Text.FirstOrDefault(x => x.R9KText == text.R9KText && x.MessageContentId != text.MessageContentId);
                if (retour != null)
                {
                    context.Entry(retour).Reference(p => p.SimilarContent).Load();
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
                context.Entry(text).Reference(p => p.SimilarContent).Load();
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
            Entites.Text reference = new Entites.Text(null, DateTime.MinValue, text.Message, null, text.SimilarContent);
            text.Message = null;
            text.SimilarContent = null;
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
            text.SimilarContent = reference.SimilarContent;
            return text;
        }
    }
}