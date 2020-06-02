using System;
using System.Collections.Generic;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Factories
{
    public class RentalAreaFactory
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public RentalAreaFactory(IDomainEventPublisher domainEventPublisher)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public RentalArea Create(Guid id, decimal outOfBondsPenalty, List<PositionDTO> points, string name)
        {
            return new RentalArea(id, _domainEventPublisher, outOfBondsPenalty, points, name);
        }
    }
}