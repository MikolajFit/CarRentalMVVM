using System;
using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;
using DDD.Base.ApplicationLayer.Services;

namespace CarRental.Model.ApplicationLayer.Interfaces
{
    public interface ICarService : IApplicationService
    {
        void CreateCar(CarDTO carDTO);
        void UpdateCar(CarDTO carDto0);
        void ReparkCar(Guid carId, PositionDTO newPosition);
        List<CarDTO> GetAllCars();
        List<CarDTO> GetFreeCars();
    }
}