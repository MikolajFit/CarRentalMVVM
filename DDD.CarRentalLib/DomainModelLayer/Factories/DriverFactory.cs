using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Factories
{
    public class DriverFactory
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public DriverFactory(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public Driver Create(Guid id, string licenseNumber, string firstName, string lastName)
        {
            return new Driver(id, _domainEventPublisher, licenseNumber, firstName, lastName);
        }
    }
}