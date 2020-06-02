using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Factories
{
    public class CarFactory
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CarFactory(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public Car Create(Guid carId, string registrationNumber, double currentDistance, double totalDistance,
            decimal pricePerMinute, Guid rentalAreaId)
        {
            return new Car(carId, _domainEventPublisher, registrationNumber, currentDistance, totalDistance,
                pricePerMinute, rentalAreaId);
        }

        public Car Create(Guid carId, string registrationNumber, double currentDistance, double totalDistance,
            double currentLatitude, double currentLongitude, decimal pricePerMinute, Guid rentalAreaId)
        {
            return new Car(carId, _domainEventPublisher, registrationNumber, currentDistance, totalDistance,
                currentLatitude, currentLongitude, pricePerMinute, rentalAreaId);
        }
    }
}