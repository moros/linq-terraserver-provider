using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace LinqToTerraServerProvider
{
    public class TerraServiceClient
    {
        public PlaceFact[] GetPlaceFacts(string location)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var jsonFile = Path.Combine(baseDirectory, "places.json");
            
            var serializer = new JsonSerializer();
            using (var file = File.OpenText(jsonFile))
            using (var reader = new JsonTextReader(file))
            {
                var places = serializer.Deserialize<List<PlaceFact>>(reader);

                var array = places
                    .Where(place => place.Value == location || place.State == location || place.Counties.Contains(location))
                    .ToArray();

                return array;
            }
        }
    }
}
