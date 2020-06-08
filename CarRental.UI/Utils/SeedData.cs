using System;
using System.Collections.Generic;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;

namespace CarRental.UI.Utils
{
    public class SeedData
    {
        public static void SeedInitialData(IDriverService driverService, ICarService carService, IRentalService rentalService,
            IRentalAreaService rentalAreaService)
        {
            var driver = new DriverDTO
            {
                Id =Guid.NewGuid(),
                FirstName = "Mikołaj",
                LastName = "Fitowski",
                LicenseNumber = "CDJ813"
            };

            var driver1 = new DriverDTO
            {
                Id = Guid.NewGuid(),
                FirstName = "Paweł",
                LastName = "Fitowski",
                LicenseNumber = "ABBCDEFD"
            };

            var driver2 = new DriverDTO
            {
                Id = new Guid(),
                FirstName = "Michał",
                LastName = "Fitowski",
                LicenseNumber = "CD1813"
            };
            driverService.CreateDriver(driver);
            driverService.CreateDriver(driver1);
            driverService.CreateDriver(driver2);

            var rentalArea = new RentalAreaDTO()
            {
                Id = new Guid(),
                Name = "Kraków",
                OutOfBondsPenaltyPerDistanceUnit = 2.5m,
                Area = new List<PositionDTO>()
                {
                    new PositionDTO()
                    {
                        Latitude = 50.010231,
                        Longitude = 19.899144
                    },
                    new PositionDTO()
                    {
                        Latitude = 50.011054,
                        Longitude = 20.042123
                    },
                    new PositionDTO()
                    {
                        Latitude = 50.093429,
                        Longitude = 20.057025
                    },
                    new PositionDTO()
                    {
                        Latitude = 50.094973,
                        Longitude = 19.870131
                    }
                },
                CarStartingPositionDTO = new PositionDTO()
                {
                    Latitude = 50.057236,
                    Longitude = 19.945147,
                }
            };

            rentalAreaService.CreateRentalArea(rentalArea);

            var car = new CarDTO
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "KR12245",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 1
            };
            var car1 = new CarDTO
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "KR16345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 1
            };
            var car2 = new CarDTO
            {
                Id = Guid.NewGuid(),
                RegistrationNumber = "KR02345",
                RentalAreaId = rentalArea.Id,
                CurrentLatitude = 50.057236,
                CurrentLongitude = 19.945147,
                PricePerMinute = 1
            };
            carService.CreateCar(car);
            carService.CreateCar(car1);
            carService.CreateCar(car2);


            var rentalId = Guid.NewGuid();
            var startTime = new DateTime(2020, 04, 19);

            rentalService.TakeCar(rentalId, car.Id, driver.Id, startTime);
            rentalService.ReturnCar(rentalId,DateTime.Now);

            var rentalId1 = Guid.NewGuid();
            var startTime1 = new DateTime(2020, 05, 19);

            rentalService.TakeCar(rentalId1, car1.Id, driver.Id, startTime1);
            rentalService.ReturnCar(rentalId1, DateTime.Now);

            var rentalId2 = Guid.NewGuid();
            var startTime2 = new DateTime(2020, 05, 17);

            rentalService.TakeCar(rentalId2, car2.Id, driver.Id, startTime2);
            rentalService.ReturnCar(rentalId2, DateTime.Now);
        }
    }
}
