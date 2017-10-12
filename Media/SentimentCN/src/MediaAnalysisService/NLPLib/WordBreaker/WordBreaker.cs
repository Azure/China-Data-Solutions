using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPLib.WordBreaker
{
    public class WordBreaker
    {
        private static JiebaNet.Segmenter.JiebaSegmenter breaker = new JiebaNet.Segmenter.JiebaSegmenter();
        private  static HashSet<string> stopWords = new HashSet<string>();

        public WordBreaker()
        {
            lock (stopWords)
            {
                if (stopWords.Count == 0)
                {
                    var lines = File.ReadAllLines("stopword.txt");
                    foreach (var line in lines)
                    {
                        stopWords.Add(line);
                    }
                }
            }
        }

        public IEnumerable<string> Break(string text, bool removeStopWords = false)
        {
            var result = breaker.Cut((text));
            if (removeStopWords)
            {
                var nonStopwordsResults = new List<string>();
                foreach (var item in result)
                {
                    if (!stopWords.Contains(item))
                    {
                        nonStopwordsResults.Add(item);
                    }
                }

                return nonStopwordsResults;
            }

            return result;
        }
    }
}
