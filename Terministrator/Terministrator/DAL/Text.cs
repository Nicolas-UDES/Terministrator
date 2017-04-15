#region Usings

using System;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.DAL
{
    static class Text
    {
        public static bool Exists(Entites.Text text)
        {
            return Get(text.TextId) != null;
        }

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

        public static Entites.Text Get(int textId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.Text.Find(textId);
            }
        }

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

        public static Entites.Text LoadSimilarTexts(Entites.Text text)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                context.Text.Attach(text);
                context.Entry(text).Reference(p => p.SimilarTexts).Load();
            }
            return text;
        }

        private static Entites.Text ClearReferences(Entites.Text text)
        {
            Entites.Text reference = new Entites.Text(null, DateTime.MinValue, text.Message, null, text.SimilarTexts);
            text.Message = null;
            text.SimilarTexts = null;
            return reference;
        }

        private static Entites.Text AddReferences(Entites.Text text, Entites.Text reference)
        {
            text.Message = reference.Message;
            text.SimilarTexts = reference.SimilarTexts;
            return text;
        }
    }
}