using System;
using System.Collections.Generic;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace DDD.CarRental.Tests.TestHelpers
{
    public static class Helper
    {
        public static RentalAreaDTO CreateRentalAreaDTO()
        {
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
                        Longitude =19.870131
                    }
                }
            };
            
            return rentalArea;
        }
    }
}