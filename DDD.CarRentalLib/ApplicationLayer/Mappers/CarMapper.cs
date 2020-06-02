﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.ApplicationLayer.Mappers
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
                CurrentLatitude = c.CurrentPosition.Latitude,
                CurrentLongitude = c.CurrentPosition.Longitude,
                Status = c.Status,
                RegistrationNumber = c.RegistrationNumber,
                PricePerMinute = c.PricePerMinute.Amount,
                RentalAreaId = c.RentalAreaId
            };
        }
    }
}
