// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="WordCloudManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataAccess;
    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Helper;

   // using Iveonik.Stemmers;



    /// <summary>
    /// Class WordCloudManager.
    /// </summary>
    public class WordCloudManager
    {
        /// <summary>
        /// The index helper
        /// </summary>
        private readonly FulltextIndexHelper indexHelper = new FulltextIndexHelper();

        /// <summary>
        /// The stemword
        /// </summary>
        private readonly Dictionary<string, string> _stemword;

        /// <summary>
        /// The stopwords
        /// </summary>
       // private HashSet<string> _stopwords;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly WordCloudRepository repository = new WordCloudRepository();

        /// <summary>
        /// Initializes a new instance of the <see cref="WordCloudManager"/> class.
        /// </summary>
        public WordCloudManager()
        {
            this.WordDoc = new Dictionary<string, Dictionary<long, int>>();
            this.DocWord = new Dictionary<long, Dictionary<string, int>>();
            this.DocScore = new Dictionary<long, double>();

            this._stemword = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the word document.
        /// </summary>
        /// <value>The word document.</value>
        public Dictionary<string, Dictionary<long, int>> WordDoc { get; set; }

        /// <summary>
        /// Gets or sets the document word.
        /// </summary>
        /// <value>The document word.</value>
        public Dictionary<long, Dictionary<string, int>> DocWord { get; set; }

        /// <summary>
        /// Gets or sets the document score.
        /// </summary>
        /// <value>The document score.</value>
        public Dictionary<long, double> DocScore { get; set; }

        //  public List<WordClouds> indexedResults { get; set; }
        /// <summary>
        /// Gets or sets the new date.
        /// </summary>
        /// <value>The new date.</value>
        public DateTime newDate { get; set; }

        /// <summary>
        /// Generates the indexed result.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="userFilter">The user filter.</param>
        public void GenerateIndexedResult(ClientUser user, CustomerFilters userFilter)
        {
            this.newDate = this.repository.GetNewDate(user.Postfix);

            if (this.newDate != DateTime.MinValue)
            {
                this.BuildHashTable(user.Postfix, userFilter);

                var indexedResult = this.GenerateIndexedData();
                var sentiResult = this.GenerateSentimentResult(user.Name);

                this.repository.UpdateSentiTable(user.Postfix, sentiResult);
                this.repository.UpdateWordCloudsTable(user.Postfix, indexedResult);
                var sentiResultNews = this.GenerateSentimentResultNews(user.Name);
                this.repository.UpdateSentiNewsTable(user.Postfix, sentiResultNews);
            }
        }

        
        /// <summary>
        /// Builds the hash table.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="userFilter">The user filter.</param>
        public void BuildHashTable(string userid, CustomerFilters userFilter)
        {
            //this.ReadStopWords(@"..\..\DataFiles\english.stop");
            long docID = 0;
           // var stemer = new EnglishStemmer();

           // SentimentManager sentiManager = new SentimentManager();

            var filter = this.indexHelper.BuildPatternFromFilter(userFilter);

            var newslines = this.repository.GetKeyWords(userid, filter);
            foreach (var newsline in newslines)
            {
                docID = newsline.Id;
                DocScore.Add(docID, (double)newsline.Score);


                var subStrings = Regex.Split(newsline.KeyWords.Trim(), " ");
                foreach (var subString in subStrings)
                {
                    var word = subString.ToLower();

                        if (this.WordDoc.ContainsKey(word))
                        {
                            var value = this.WordDoc[word];
                            if (value.ContainsKey(docID))
                            {
                                value[docID]++;
                            }
                            else
                            {
                                value.Add(docID, 1);
                            }
                        }
                        else
                        {
                            var value = new Dictionary<long, int> { { docID, 1 } };
                            this.WordDoc.Add(word, value);
                        }

                        // DocWord
                        if (this.DocWord.ContainsKey(docID))
                        {
                            var value = this.DocWord[docID];
                            if (value.ContainsKey(word))
                            {
                                value[word]++;
                            }
                            else
                            {
                                value.Add(word, 1);
                            }
                        }
                        else
                        {
                            var value = new Dictionary<string, int> { { word, 1 } };
                            this.DocWord.Add(docID, value);
                        }
                   // }
                }
            }
        }

        /// <summary>
        /// Generates the indexed data.
        /// </summary>
        /// <param name="UserId">The user identifier.</param>
        /// <returns>List&lt;WordClouds&gt;.</returns>
        public List<WordClouds> GenerateIndexedData()
        {
            var indexedResults = new List<WordClouds>();
            var indexedResult = new WordClouds();
            var relatedWords = new List<RelatedWord>();
            indexedResult.Date = this.newDate.Date;
            indexedResult.Word = "ALL";
            double score;
            foreach (var d in this.WordDoc)
            {
                var relatedWord = new RelatedWord();

                if (d.Value.Count < 1) continue;

                relatedWord.Word = d.Key;
                relatedWord.Count = 0;
                relatedWord.Score = 0;
                foreach (var v in d.Value)
                {
                    this.DocScore.TryGetValue(v.Key, out score);
                    relatedWord.Score = (relatedWord.Count * relatedWord.Score + v.Value * score)
                                        / (relatedWord.Count + v.Value);

                    relatedWord.Count = relatedWord.Count + v.Value;
                }

                relatedWords.Add(relatedWord);
            }

            IEnumerable<RelatedWord> relatedWordssort1 = from relatedWord in relatedWords
                                                         orderby relatedWord.Count descending
                                                         select relatedWord;
            relatedWords = relatedWordssort1.ToList();
            var wordlist = "";

            foreach (var rword in relatedWords)
            {
                wordlist = wordlist + rword.Word + "," + rword.Count + "," + rword.Score + ";";
            }

            indexedResult.RelatedWords = wordlist;
            indexedResults.Add(indexedResult);

            foreach (var word in relatedWords)
            {
                var indexedResult1 = new WordClouds();
                indexedResult1.Date = this.newDate.Date;
                indexedResult1.Word = word.Word;

                var relatedWords2 = new List<RelatedWord>();
                foreach (var d in this.DocWord)
                {
                    //double score;
                    this.DocScore.TryGetValue(d.Key, out score);
                    if (d.Value.ContainsKey(word.Word))

                        foreach (var v in d.Value)
                        {
                            if (relatedWords2.Exists(x => x.Word == v.Key))
                            {
                                var rWord =
                                    relatedWords2.Find(
                                        delegate(RelatedWord rWord2) { return rWord2.Word.Equals(v.Key); });
                                rWord.Score = (rWord.Count * rWord.Score + v.Value * score) / (rWord.Count + v.Value);
                                rWord.Count = rWord.Count + v.Value;
                            }
                            else
                            {
                                var rWord3 = new RelatedWord();
                                rWord3.Word = v.Key;
                                rWord3.Score = score;
                                rWord3.Count = v.Value;
                                relatedWords2.Add(rWord3);
                            }
                        }
                }
                IEnumerable<RelatedWord> relatedWordssort2 = from relatedWord in relatedWords2
                                                             orderby relatedWord.Count descending
                                                             select relatedWord;
                relatedWords2 = relatedWordssort2.ToList();
                wordlist = "";

                foreach (var rword in relatedWords2)
                {
                    wordlist = wordlist + rword.Word + "," + rword.Count + "," + rword.Score + ";";
                }

                indexedResult1.RelatedWords = wordlist;

                indexedResults.Add(indexedResult1);
            }

            return indexedResults;
        }

        /// <summary>
        /// Generates the sentiment result.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>SentimentsResult.</returns>
        public SentimentsResult GenerateSentimentResult(string name)
        {
            var sentiResult = new SentimentsResult();
            sentiResult.Date = this.newDate.Date;
            sentiResult.Name = name;
            sentiResult.Positive = 0;
            sentiResult.Negative = 0;
            sentiResult.Score = 0;

            foreach (var d in this.DocScore)
            {
                if (d.Value > 0) sentiResult.Positive = sentiResult.Positive + 1;
                else sentiResult.Negative = sentiResult.Negative + 1;
                sentiResult.Score = sentiResult.Score + (decimal)d.Value;
            }
            var sum = (sentiResult.Positive + sentiResult.Negative)==0?1:(sentiResult.Positive + sentiResult.Negative);
            sentiResult.Score = sentiResult.Score /sum;

            return sentiResult;
        }

        /// <summary>
        /// Generates the sentiment result news.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>List&lt;SentimentsResultNews&gt;.</returns>
        public List<SentimentsResultNews> GenerateSentimentResultNews(string userName)
        {
            var sentimentsResultNews = new List<SentimentsResultNews>();

            foreach (var d in this.DocScore)
            {
                var sentiResult = new SentimentsResultNews();
                sentiResult.Date = this.newDate.Date;
                sentiResult.Name = userName;
                sentiResult.Id = d.Key;

                sentiResult.Score = (decimal)d.Value;
                sentimentsResultNews.Add(sentiResult);
            }

            return sentimentsResultNews;
        }
    }
}
