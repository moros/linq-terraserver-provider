using System;
using System.Runtime.Serialization;

namespace LinqToTerraServerProvider
{
    [Serializable]
    [DataContract]
    public class PlaceFact
    {
        [DataMember(Name = "city")]
        public string City { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "placetype_id")]
        public int PlaceTypeId { get; set; }
    }
}
