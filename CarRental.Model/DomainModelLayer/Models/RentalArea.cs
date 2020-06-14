using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Models
{
    public class RentalArea : AggregateRoot
    {
        public RentalArea(Guid id, decimal outOfBondsPenalty,
            List<PositionDTO> points, string name) : base(id)
        {
            Name = name;
            OutOfBondsPenaltyPerDistanceUnit = new Money(outOfBondsPenalty);
            Area = new Area(points);
            CarStartingPosition = new Position((points[0].Latitude+points[1].Latitude)/2,(points[0].Longitude+points[1].Longitude)/2);
            if (!IsInArea(CarStartingPosition)) throw new Exception("Car starting position cannot be outside of bonds");
        }
        public RentalArea(Guid id, decimal outOfBondsPenalty,
            List<PositionDTO> points, string name, Position carStartingPosition) : base(id)
        {
            Name = name;
            OutOfBondsPenaltyPerDistanceUnit = new Money(outOfBondsPenalty);
            Area = new Area(points);
            if(!IsInArea(carStartingPosition)) throw new Exception("Car starting position cannot be outside of bonds");
            CarStartingPosition = carStartingPosition;
        }

        public Position CarStartingPosition { get; set; }
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