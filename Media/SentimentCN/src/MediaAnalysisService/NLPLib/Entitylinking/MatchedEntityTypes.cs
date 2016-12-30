using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace NLPLib.Entitylinking
{
    [DataContract]
    public class MatchedEntityTypes
    {
        #region Public Properties
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public IList<string> Types
        {
            get
            {
                return new List<string>(_types);
            }
        }

        [DataMember]
        public IList<Match> Matches
        {
            get
            {
                return new List<Match>(_matches.Values);
            }
        }
        #endregion

        private Dictionary<string, Match> _matches;
        private HashSet<string> _types;


        public MatchedEntityTypes()
        {
            _matches = new Dictionary<string, Match>();
            _types = new HashSet<string>();
        }

        public void AddMatch(int offset, string text)
        {
            if (_matches.ContainsKey(text))
            {
                _matches[text].AddOffset(offset);
            }
            else
            {
                _matches.Add(text, new Match(text, offset));
            }
        }


        public void AddType(string type)
        {
            this._types.Add(type);
        }
    }
}