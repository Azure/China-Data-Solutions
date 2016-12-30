using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace NLPLib.Entitylinking
{
    [DataContract]
    public class Match
    {
        # region Public Properties

        [DataMember]
        public string Text { get; private set; }

        [DataMember]
        public IList<int> Offsets { get; private set; }


        #endregion

        public Match(string text)
        {
            Text = text;
            Offsets = new List<int>();
        }

        public Match(string text, int offset)
        {
            Text = text;
            Offsets = new List<int>();
            AddOffset(offset);
        }

        public void AddOffset(int offset)
        {
            if (!Offsets.Contains(offset))
            {
                Offsets.Add(offset);
            }
        }
    }
}