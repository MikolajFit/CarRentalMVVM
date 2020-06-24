using System;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Factories
{
    public class RentalFactory
    {
        public Rental Create(Guid rentalId, DateTime startDateTime, Car car, Guid driverId)
        {
            CheckIfCarIsFree(car);
            return new Rental(rentalId, startDateTime, car.Id, driverId);
        }

        private void CheckIfCarIsFree(Car car)
        {
            if (car.Status != CarStatus.Free) throw new Exception($"Car '{car.Id}' is not free");
        }
    }
}