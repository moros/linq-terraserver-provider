using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LinqToTerraServerProvider
{
    [Serializable]
    [DataContract]
    public class PlaceFact
    {
        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "counties")]
        public List<string> Counties { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "placetype_id")]
        public int PlaceTypeId { get; set; }
    }
}
