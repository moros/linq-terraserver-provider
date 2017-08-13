using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqToTerraServerProvider
{
    public static class WebServiceHelper
    {
        public static Place[] GetPlacesFromTerraServer(List<string> locations)
        {
            // limit the total number of web service calls.
            if (locations.Count > 5)
                throw new Exception("This query requires more than five separate calls to the service.");

            var allPlaces = new List<Place>();
            foreach (var location in locations)
            {
                var places = GetPlace(location);
                allPlaces.AddRange(places);
            }

            return allPlaces.ToArray();
        }

        private static IEnumerable<Place> GetPlace(string location)
        {
            var client = new TerraServiceClient();

            // this call, simulates the call to getting place facts, since
            // the terraprovider service no longer exists, damn you Microsoft! lol.
            var placeFacts = client.GetPlaceFacts(location);

            var places = new Place[placeFacts.Length];
            for (var i = 0; i < placeFacts.Length; i++)
                places[i] = new Place(placeFacts[i].Value, placeFacts[i].State, placeFacts[i].PlaceTypeId);

            return places;
        }
    }
}
