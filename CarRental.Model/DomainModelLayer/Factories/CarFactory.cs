using System;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Factories
{
    public class CarFactory
    {

    

        public Car Create(Guid carId, string registrationNumber, double currentDistance, double totalDistance,
            decimal pricePerMinute, Guid rentalAreaId)
        {
            return new Car(carId, registrationNumber, currentDistance, totalDistance,
                pricePerMinute, rentalAreaId);
        }

        public Car Create(Guid carId, string registrationNumber, double currentDistance, double totalDistance,
            double currentLatitude, double currentLongitude, decimal pricePerMinute, Guid rentalAreaId)
        {
            return new Car(carId, registrationNumber, currentDistance, totalDistance,
                currentLatitude, currentLongitude, pricePerMinute, rentalAreaId);
        }
    }
}