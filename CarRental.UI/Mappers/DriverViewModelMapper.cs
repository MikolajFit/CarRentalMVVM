using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.Mappers
{
    public class DriverViewModelMapper : IDriverViewModelMapper

    {
        public DriverViewModel Map(DriverDTO driver)
        {
            return new DriverViewModel
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber,
                DriverStatus = driver.DriverStatus,
                FreeMinutesPolicy = driver.FreeMinutesPolicy
            };
        }

        public DriverDTO Map(DriverViewModel driver)
        {
            return new DriverDTO
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber,
                DriverStatus = driver.DriverStatus,
                FreeMinutesPolicy = driver.FreeMinutesPolicy
            };
        }
    }
}