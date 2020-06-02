using System;
using DDD.Base.DomainModelLayer.Events;
using DDD.Base.DomainModelLayer.Services;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Services
{
    public class PositionService : IDomainService
    {
        private readonly ICarRentalUnitOfWork _unitOfWork;

        public PositionService(ICarRentalUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual void UpdateCarPosition(Guid carId)
        {
            var car = _unitOfWork.CarRepository.Get(carId) ??
                      throw new Exception($"Could not find car '{carId}'.");
            var random = new Random();
            var currentPositionLatitude = car.CurrentPosition.Latitude + (random.NextDouble() - 0.5)/10;
            var currentPositionLongitude = car.CurrentPosition.Longitude + (random.NextDouble() - 0.5)/10;
            var newPosition = new Position(currentPositionLatitude,
                currentPositionLongitude);
            var newDistance = car.CurrentPosition.GetDistanceTo(newPosition, car.TotalDistance.Unit);
            car.AddDistance(newDistance);
            car.UpdatePosition(newPosition);
            _unitOfWork.Commit();
        }
    }
}