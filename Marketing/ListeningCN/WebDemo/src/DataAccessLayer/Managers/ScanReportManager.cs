// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ScanReportManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataAccess;
    using DataAccessLayer.DataModels;

    /// <summary>
    /// Class ScanReportManager.
    /// </summary>
    public class ScanReportManager
    {
        /// <summary>
        /// The defaultwordcloudkeyword
        /// </summary>
        private const string DEFAULTWORDCLOUDKEYWORD = "all";

        /// <summary>
        /// The numberofnewsshowforsource
        /// </summary>
        private const int NUMBEROFNEWSSHOWFORSOURCE = 3;

        /// <summary>
        /// The current user
        /// </summary>
        private readonly ClientUser currentUser;

        /// <summary>
        /// The keyword manager
        /// </summary>
        private readonly CompanyKeywordManager keywordManager;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly SysConfig config;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly ScanReportRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanReportManager"/> class.
        /// </summary>
        /// <param name="clientUser">The client user.</param>
        public ScanReportManager(ClientUser clientUser)
        {
            this.currentUser = clientUser;
            this.repository = new ScanReportRepository(clientUser);
            this.config = new SysConfig();
            this.keywordManager = new CompanyKeywordManager(this.config, this.currentUser);
        }

        /// <summary>
        /// Gets the sentiment scan result.
        /// </summary>
        /// <returns>SentimentScanReport.</returns>
        public SentimentScanReport GetSentimentScanResult()
        {
            SentimentScanReport report = null;
            var result = repository.GetLatestSentimentResult(currentUser);
            var results = result as IList<SentimentScanResult> ?? result.ToList();
            if (result != null && results.Any())
            {
                report = ModelConverter.ToSentimentScanReport(results.First());
            }

            return report;
        }

        /// <summary>
        /// Gets the word dic.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="number">The number.</param>
        /// <returns>Dictionary&lt;System.String, WordStatistic&gt;.</returns>
        public Dictionary<string, WordStatistic> GetWordDic(string key, int number)
        {
            var file = this.repository.LoadWordCloudRecord(currentUser, key);
            if (file == null || file.Count <= 0) return null;
            var result = new Dictionary<string, WordStatistic>();
            var text = string.Empty;
            if (string.IsNullOrEmpty(key) || !file.Keys.Contains(key))
            {
                text = file[DEFAULTWORDCLOUDKEYWORD];
            }
            else
            {
                text = file[key];
            }

            if (!string.IsNullOrEmpty(text))
            {
                var textArray = text.Split(';');
                foreach (var item in textArray)
                {
                    var subTextArray = item.Split(',');
                    if (subTextArray.Length != 3) continue;
                    result.Add(
                        subTextArray[0],
                        new WordStatistic { Count = int.Parse(subTextArray[1]), Score = float.Parse(subTextArray[2]) });
                }
            }

            return result.OrderByDescending(i => i.Value.Count).Take(100).ToDictionary(p => p.Key, p => p.Value);
        }

        /// <summary>
        /// Gets the news list by key words.
        /// </summary>
        /// <param name="searchKeyWords">The search key words.</param>
        /// <param name="number">The number.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetNewsListByKeyWords(List<string> searchKeyWords, int number)
        {
            List<NewsBrief> result = null;
            var newsStream = this.repository.GetNewsStream(searchKeyWords, number, currentUser);
            var newsStreams = newsStream as IList<NewsStream> ?? newsStream.ToList();
            if (newsStream != null && newsStreams.Any())
            {
                result = ModelConverter.ToNewsBriefList(newsStreams.ToList());
            }
            return result;
        }

        /// <summary>
        /// Gets the word cloud result.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="wordDicnumber">The word dicnumber.</param>
        /// <param name="newsListNumber">The news list number.</param>
        /// <returns>WordCloudResult.</returns>
        public WordCloudResult GetWordCloudResult(string key, int wordDicnumber, int newsListNumber)
        {
            var result = new WordCloudResult();
            result.WordDic = this.GetWordDic(key, wordDicnumber);
            var keyWordsList = this.GetFilterKeyWords(result.WordDic, key);
            result.NewsList = this.GetNewsListByKeyWords(keyWordsList, newsListNumber);
            if (string.IsNullOrEmpty(key)
                || key.Equals(DEFAULTWORDCLOUDKEYWORD, StringComparison.InvariantCultureIgnoreCase))
            {
                result.KeyWord = currentUser.Name;
            }

            else
            {
                result.KeyWord = key;
            }
            return result;
        }

        /// <summary>
        /// Gets the news source scan report.
        /// </summary>
        /// <returns>NewsSourceScanReport.</returns>
        public NewsSourceScanReport GetNewsSourceScanReport()
        {
            const int top = 5;
            var endTime = this.GetCurrentDateTime();
            var startTime = endTime.AddHours(-24);
            var keywords = this.keywordManager.GetCompanyKeywords();
            var keyword = string.Join(" or ", keywords);
            var source = this.repository.GetNewsCountBySource(top, keyword, startTime, endTime);
            NewsSourceScanReport result = null;
            var sources = source as IList<NewsReportSourceScan> ?? source.ToList();
            if (source == null || !sources.Any()) return null;
            result = new NewsSourceScanReport();
            var sortedSource = sources.OrderBy(i => i.Count).ToList();
            foreach (var item in sortedSource)
            {
                result.NewsSource.Add(item.NewsSource);
                result.NewsCount.Add(item.Count);
            }

            return result;
        }

        /// <summary>
        /// Gets the media report count scan report.
        /// </summary>
        /// <returns>MediaReportCountScanReport.</returns>
        public MediaReportCountScanReport GetMediaReportCountScanReport()
        {
            var endTime = this.GetCurrentDateTime();
            var middleTime = endTime.AddHours(-24);
            var startTime = endTime.AddHours(-48);
            var filter = this.currentUser.UserFilter;
            var currentReportCount = this.repository.GetNewsReportCount(middleTime, endTime, filter);
            var previouseReportCount = this.repository.GetNewsReportCount(startTime, middleTime, filter);
            var report = new MediaReportCountScanReport();
            var currentCount = 0;
            var previouseCount = 0;
            var reportCount = currentReportCount as IList<int> ?? currentReportCount.ToList();
            if (currentReportCount != null && reportCount.Any())
            {
                currentCount = reportCount.First();
            }

            var enumerable = previouseReportCount as IList<int> ?? previouseReportCount.ToList();
            if (previouseReportCount != null && enumerable.Any())
            {
                previouseCount = enumerable.First();
            }

            var delta = currentCount - previouseCount;
            report.Count = currentCount;
            var compareRate = 0;
            if (previouseCount == 0)
            {
                if (delta != 0)
                {
                    compareRate = 100;
                }
            }

            else
            {
                compareRate = (int)(Math.Round(delta / (double)previouseCount, 2) * 100);
            }

            report.CompareRate = compareRate;
            return report;
        }

        /// <summary>
        /// Gets the sentiment news list.
        /// </summary>
        /// <param name="takeNumber">The take number.</param>
        /// <returns>SentimentNewsList.</returns>
        public SentimentNewsList GetSentimentNewsList(int takeNumber)
        {
            var keyword = this.keywordManager.GetCompany();
            var result = new SentimentNewsList();
            Parallel.Invoke(
                () => result.NegativeNewsList = this.GetSentimentNewsList(keyword, takeNumber, "negative"),
                () => result.PositiveNewsList = this.GetSentimentNewsList(keyword, takeNumber, "positive"));
            return result;
        }

        /// <summary>
        /// Gets the sentiment news list.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="takeCount">The take count.</param>
        /// <param name="sentimentType">Type of the sentiment.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetSentimentNewsList(string userName, int takeCount, string sentimentType)
        {
            List<NewsBrief> result = null;
            var newsStream = this.repository.GetNewsStreamForSentiment(userName, takeCount, sentimentType);
            var newsStreams = newsStream as IList<NewsStream> ?? newsStream.ToList();
            if (newsStream != null && newsStreams.Any())
            {
                result = ModelConverter.ToNewsBriefList(newsStreams.ToList());
            }
            return result;
        }

        /// <summary>
        /// Gets the news list based on source.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <returns>List&lt;NewsBrief&gt;.</returns>
        public List<NewsBrief> GetNewsListBasedOnSource(List<string> sourceList)
        {
            List<NewsBrief> result = null;
            var endTime = this.GetCurrentDateTime();
            var startTime = endTime.AddHours(-24);
            var filter = this.currentUser.UserFilter;
            var newsStream = this.repository.GetNewsStreamBasedOnSource(
                sourceList,
                NUMBEROFNEWSSHOWFORSOURCE,
                startTime,
                endTime,
                filter);
            var newsStreams = newsStream as IList<NewsStream> ?? newsStream.ToList();
            if (newsStream != null && newsStreams.Any())
            {
                result = ModelConverter.ToNewsBriefList(newsStreams.ToList());
            }
            return result;
        }

        /// <summary>
        /// Gets the current date time.
        /// </summary>
        /// <returns>DateTime.</returns>
        protected DateTime GetCurrentDateTime()
        {
            if (SysConfig.UseStaticDate)
            {
                return repository.Context.GetMaxDate().AddDays(1);
            }
            else
            {
                return DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Gets the filter key words.
        /// </summary>
        /// <param name="wordDic">The word dic.</param>
        /// <param name="key">The key.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        private List<string> GetFilterKeyWords(Dictionary<string, WordStatistic> wordDic, string key)
        {
            var result = new List<string>();
            if (wordDic != null && wordDic.Count >= 2
                && !key.Equals(DEFAULTWORDCLOUDKEYWORD, StringComparison.InvariantCultureIgnoreCase))
            {
                result.Add(key);
                var selectedPair =
                    wordDic.Take(1)
                        .OrderByDescending(i => i.Value.Count)
                        .Where(j => j.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase) == false);
                foreach (var pair in selectedPair)
                {
                    result.Add(pair.Key);
                }
            }
            else
            {
                var keywords = this.keywordManager.GetCompanyKeywords();
                result.AddRange(keywords);
            }

            return result;
        }
    }
}