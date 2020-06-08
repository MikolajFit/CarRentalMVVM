using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Models;
using DDD.CarRentalLib.DomainModelLayer.Policies;

namespace DDD.CarRentalLib.ApplicationLayer.Mappers
{
    public class DriverMapper
    {
        public List<DriverDTO> Map(IEnumerable<Driver> drivers)
        {
            return drivers.Select(c => Map(c)).ToList();
        }

        public DriverDTO Map(Driver d)
        {
            return new DriverDTO()
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                LicenseNumber = d.LicenseNumber,
                DriverStatus = d.DriverStatus,
                FreeMinutesPolicy = d.FreeMinutesPolicy.PolicyType
            };
        }

       
    }
}
