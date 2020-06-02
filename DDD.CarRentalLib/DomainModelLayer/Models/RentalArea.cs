using System;
using System.Collections.Generic;
using DDD.Base.DomainModelLayer.Events;
using DDD.Base.DomainModelLayer.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace DDD.CarRentalLib.DomainModelLayer.Models
{
    public class RentalArea : AggregateRoot
    {
        public RentalArea(Guid id, IDomainEventPublisher domainEventPublisher, decimal outOfBondsPenalty,
            List<PositionDTO> points, string name) : base(id, domainEventPublisher)
        {
            Name = name;
            OutOfBondsPenaltyPerDistanceUnit = new Money(outOfBondsPenalty);
            Area = new Area(points);
        }

        public string Name { get; set; }
        public Area Area { get; set; }
        public Money OutOfBondsPenaltyPerDistanceUnit { get; set; }

        public Money CalculateTotalPenalty(Car car)
        {
            if (IsInArea(car.CurrentPosition)) return Money.Zero;
            var distanceFromArea = Area.GetDistanceFromNearestPoint(car.CurrentPosition, car.TotalDistance.Unit);
            return OutOfBondsPenaltyPerDistanceUnit.MultiplyBy(Math.Round(distanceFromArea.Value,2));
        }

        public bool IsInArea(Position position)
        {
            return Area.IsPointInPolygon(position.Latitude, position.Longitude);
        }

        public void BroadenArea(List<PositionDTO> points)
        {
            Area.AddPointsToPolygon(points);
        }
    }
}