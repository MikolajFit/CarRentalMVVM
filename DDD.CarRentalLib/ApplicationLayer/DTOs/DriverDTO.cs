using System;
using System.Collections.Generic;
using System.Text;
using DDD.CarRentalLib.DomainModelLayer.Models;
using DDD.CarRentalLib.DomainModelLayer.Policies;

namespace DDD.CarRentalLib.ApplicationLayer.DTOs
{
    public class DriverDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public PoliciesEnum FreeMinutesPolicy { get; set; }
    }
}
