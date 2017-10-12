using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NLPLib.Entitylinking
{
    [DataContract]
    public class EntityLink : IComparable<EntityLink>
    {
        #region Public Properties

        [DataMember(EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember]
        public bool IsTopEntity { get; set; }

        [DataMember]
        public bool IsTitleEntity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IList<string> Types
        {
            get
            {
                return new List<string>(_types);
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public IDictionary<string, string> Metadata
        {
            get
            {
                return _metadata;
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public double? Score { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public IList<Match> Matches
        {
            get
            {
                return new List<Match>(_matches.Values);
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public IList<Match> TitleMatches
        {
            get
            {
                return new List<Match>(_titleMatches.Values);
            }
        }

        [DataMember(EmitDefaultValue = false)]
        public double? IERScore { get; set; }
        #endregion

        private readonly Dictionary<string, Match> _matches;

        private readonly Dictionary<string, Match> _titleMatches;

        private readonly HashSet<string> _types;

        private readonly Dictionary<string, string> _metadata;

        public EntityLink()
        {
            _matches = new Dictionary<string, Match>();
            _titleMatches = new Dictionary<string, Match>();
            _types = new HashSet<string>();
            _metadata = new Dictionary<string, string>();
        }

        public void AddType(string type)
        {
            _types.Add(type);
        }

        public bool HasType(string type)
        {
            return _types.Contains(type);
        }

        public void AddMetadata(string key, string value)
        {
            _metadata[key] = value;
        }

        public bool IsValid()
        {
            return (!String.IsNullOrEmpty(Id) && _matches.Count > 0);
        }

        /// <summary>
        /// This enables sorting EntityLinks in descending order of their scores.
        /// </summary>
        /// <param name="other">The EntityLinks object to compare to.</param>
        /// <returns>1, 0 or -1</returns>
        public int CompareTo(EntityLink other)
        {
            if (other == null)
            {
                return -1;
            }
            if (Score == other.Score)
            {
                int order = IsTopEntity.CompareTo(other.IsTopEntity);
                if (0 != order)
                {
                    return -order;
                }
                order = Matches.Count.CompareTo(other.Matches.Count);
                return -order;
            }
            return Score > other.Score ? -1 : 1;
        }
    }
}
