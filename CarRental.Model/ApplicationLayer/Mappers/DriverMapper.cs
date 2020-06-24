using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.Mappers
{
    public class DriverMapper
    {
        public List<DriverDTO> Map(IEnumerable<Driver> drivers)
        {
            return drivers.Select(c => Map(c)).ToList();
        }

        public DriverDTO Map(Driver d)
        {
            return new DriverDTO
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