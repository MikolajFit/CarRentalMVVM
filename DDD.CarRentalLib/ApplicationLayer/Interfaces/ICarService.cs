using System;
using System.Collections.Generic;
using System.Text;
using DDD.Base.ApplicationLayer.Services;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace DDD.CarRentalLib.ApplicationLayer.Interfaces
{
    public interface ICarService: IApplicationService
    {
        void CreateCar(CarDTO carDTO);
        void UpdateCar(CarDTO carDto0);
        void ReparkCar(Guid carId, PositionDTO newPosition);
        List<CarDTO> GetAllCars();
        List<CarDTO> GetFreeCars();

    }
}
