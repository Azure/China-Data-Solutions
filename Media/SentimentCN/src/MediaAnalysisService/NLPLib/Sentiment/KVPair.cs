using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPLib.Sentiment
{
    public class KVPair<K,V>
    {
        public K Key { get; set; }

        public V Value { get; set; }
    }
}
