using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.Mappers
{
    public class CarMapper
    {
        public List<CarDTO> Map(IEnumerable<Car> cars)
        {
            return cars.Select(c => Map(c)).ToList();
        }

        public CarDTO Map(Car c)
        {
            return new CarDTO()
            {
                Id = c.Id,
                CurrentDistance = c.CurrentDistance.Value,
                TotalDistance = c.TotalDistance.Value,
                CurrentLatitude = Math.Round(c.CurrentPosition.Latitude,6),
                CurrentLongitude = Math.Round(c.CurrentPosition.Longitude,6),
                Status = c.Status,
                RegistrationNumber = c.RegistrationNumber,
                PricePerMinute = c.PricePerMinute.Amount,
                RentalAreaId = c.RentalAreaId,

            };
        }
    }
}
