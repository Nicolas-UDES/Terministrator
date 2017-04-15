#region Usings

using System.Diagnostics;

#endregion

namespace Terministrator.Terministrator.BLL
{
    static class Text
    {
        public static Entites.Text GetOrCreate(Entites.Text text)
        {
            return Get(text) ?? Create(text);
        }

        public static Entites.Text Get(Entites.Text text)
        {
            return DAL.Text.Get(text.TextId);
        }

        public static Entites.Text GetR9K(Entites.Text text)
        {
            return DAL.Text.GetR9K(text);
        }

        public static Entites.Text Create(Entites.Text text)
        {
            return DAL.Text.Create(text);
        }

        public static Entites.Text Update(Entites.Text text)
        {
            return DAL.Text.Update(text);
        }

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