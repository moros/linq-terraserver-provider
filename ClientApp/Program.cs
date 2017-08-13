using System;
using System.Linq;
using LinqToTerraServerProvider;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var terraPlaces = new QueryableTerraServerData<Place>();

            var places = (
                from place in terraPlaces
                where place.Name == "Dubuque" || place.Name == "Maquoketa" || place.Name == "Maquoketa River"
                select place
            ).ToList();

            foreach (var place in places)
                Console.WriteLine($"name: {place.Name}, state: {place.State}, type: {place.PlaceType}");

            Console.Read();
        }
    }
}
