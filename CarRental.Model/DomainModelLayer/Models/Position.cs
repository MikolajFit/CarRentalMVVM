using System;
using System.Device.Location;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class Position : GeoCoordinate

    {
        private const double MaxLatitude = 90;
        private const double MinLatitude = -90;
        private const double MaxLongitude = 180;
        private const double MinLongitude = -180;


        public Position()
        {
            GetStartingPosition();
        }

        public Position(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        private void GetStartingPosition()
        {
            Latitude = new Random().NextDouble() * (MaxLatitude - MinLatitude) + MinLatitude;
            Longitude = new Random().NextDouble() * (MaxLongitude - MinLongitude + MinLongitude);
        }

        public Distance GetDistanceTo(Position position, DistanceUnit unitToReturn)
        {
            var distanceInKm = GetDistanceTo(position) / 1000;

            return unitToReturn == DistanceUnit.KM
                ? new Distance(distanceInKm, unitToReturn)
                : new Distance(Units.SwitchUnits(distanceInKm, unitToReturn), unitToReturn);
        }
    }
}