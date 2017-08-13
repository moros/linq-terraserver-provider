namespace LinqToTerraServerProvider
{
    public class Place
    {
        public string Name { get; }
        public string State { get; }
        public PlaceType PlaceType { get; }

        internal Place(string name, string state, int placeType)
        {
            Name = name;
            State = state;
            PlaceType = (PlaceType)placeType;
        }
    }

    public enum PlaceType
    {
        Unknown = 0,
        AirRailStation = 1,
        BayGulf = 2,
        CapePeninsula = 3,
        CityTown = 4,
        HillMountain = 5,
        Island = 6,
        Lake = 7,
        OtherLandFeature = 8,
        OtherWaterFeature = 9,
        ParkBeach = 10,
        PointOfInterest = 11,
        River = 12
    }
}
