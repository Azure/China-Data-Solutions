using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NLPLib.Entitylinking
{
    [DataContract]
    public class EntityLinkingResponse
    {
        [DataMember]
        public IEnumerable<EntityLink> Entities;

        [DataMember(EmitDefaultValue = false)]
        public IEnumerable<MatchedEntityTypes> MatchedEntityTypes;

        [DataMember]
        public IEnumerable<EntityLink> FullEntitiesWithNonTop;

        [DataMember]
        public string BFPRQuery;

        [DataMember] 
        public bool PassQueryToBfpr;

        [DataMember]
        public IEnumerable<string> WikiExploreContextTerms;
    }
}
