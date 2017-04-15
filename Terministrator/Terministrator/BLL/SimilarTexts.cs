using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terministrator.Terministrator.BLL
{
    static class SimilarTexts
    {
        public static Entites.SimilarTexts Create()
        {
            return DAL.SimilarTexts.Create(new Entites.SimilarTexts(1, DateTime.UtcNow));
        }

        public static Entites.SimilarTexts Increment(int similarTextsId)
        {
            Entites.SimilarTexts similarTexts = DAL.SimilarTexts.Get(similarTextsId);
            similarTexts.LastIncrement = DateTime.UtcNow;
            similarTexts.NBSimilar++;
            return DAL.SimilarTexts.Update(similarTexts);
        }
    }
}
