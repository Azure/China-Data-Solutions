using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiebaNet.Analyser;

namespace NLPLib.KeywordExtract
{
    public class KeywordExtractor
    {

        private static readonly TfidfExtractor tfidfExtractor = new TfidfExtractor();
        private static readonly TextRankExtractor textrankExtrator = new TextRankExtractor();

        public IEnumerable<string> ExtractKeywordWithTfidf(string text)
        {
            try
            {
                var result = tfidfExtractor.ExtractTags(text);
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return null;
        }

        public IEnumerable<string> ExtractKeywordsWithTextRank(string text)
        {
            var result = textrankExtrator.ExtractTags(text);
            return result;
        }
    }
}
