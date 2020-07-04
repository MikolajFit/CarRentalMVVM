using System.Collections.Generic;
using CarRental.Model.ApplicationLayer.DTOs;

namespace CarRental.Model.ApplicationLayer.Interfaces
{
    public interface IDriverService : IApplicationService
    {
        void CreateDriver(DriverDTO driverDto);
        void UpdateDriver(DriverDTO driverDto);
        List<DriverDTO> GetAllDrivers();
    }
}