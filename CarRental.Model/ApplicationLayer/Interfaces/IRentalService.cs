using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;
using DDD.Base.ApplicationLayer.Services;

namespace CarRental.Model.ApplicationLayer.Interfaces
{
    public interface IRentalService : IApplicationService
    {
        void TakeCar(Guid rentalId, Guid carId, Guid driverId, DateTime startDateTime);
        void ReturnCar(Guid rentalId, DateTime stopDateTime);
        RentalDTO GetRental(Guid rentalId);
        List<RentalDTO> GetAllRentals();
        List<RentalDTO> GetRentalsForDriver(Guid driverId);
        RentalDTO GetActiveRentalForDriver(Guid driverId);
    }
}