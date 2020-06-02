using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Factories
{
    public class RentalFactory
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public RentalFactory(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public Rental Create(Guid rentalId, DateTime startDateTime, Car car, Guid driverId)
        {
            CheckIfCarIsFree(car);
            return new Rental(rentalId, _domainEventPublisher, startDateTime, car.Id, driverId);
        }

        private void CheckIfCarIsFree(Car car)
        {
            if (car.Status != CarStatus.Free) throw new Exception($"Car '{car.Id}' is not free");
        }
    }
}