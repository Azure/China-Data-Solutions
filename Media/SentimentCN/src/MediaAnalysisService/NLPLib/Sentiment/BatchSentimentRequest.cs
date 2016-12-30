using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPLib.Sentiment
{
    public class BatchSentimentRequest
    {
        public bool IgnoreInputWhenOutput { get; set; } = true;
        public List<KVPair<string, string>> Body { get; set; }
    }
}
