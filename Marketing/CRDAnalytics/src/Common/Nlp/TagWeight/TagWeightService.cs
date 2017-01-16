namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.Nlp.TagWeight
{
    using System.Collections.Generic;
    using System.Linq;

    using JiebaNet.Analyser;

    internal static class TagWeightService
    {
        private static readonly TextRankExtractor TagExtractor = new TextRankExtractor();

        public static IEnumerable<TagWeightResult> Extract(string text) =>
            TagExtractor.ExtractTagsWithWeight(text)
                .Select(t => new TagWeightResult { Word = t.Word, Weight = t.Weight });
    }
}
