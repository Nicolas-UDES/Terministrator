namespace Terministrator.Terministrator.DAL
{
    static class SimilarTexts
    {
        public static Entites.SimilarTexts Create(Entites.SimilarTexts similarText)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                similarText.SimilarMessagesId = context.SimilarTexts.Add(similarText).SimilarMessagesId;
                context.SaveChanges();
            }
            return similarText;
        }

        public static Entites.SimilarTexts Get(int similarTextsId)
        {
            using (TerministratorContext context = new TerministratorContext(true))
            {
                return context.SimilarTexts.Find(similarTextsId);
            }
        }

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