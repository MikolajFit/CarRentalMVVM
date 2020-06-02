using System;
using System.Collections.Generic;
using System.Text;

namespace DDD.CarRentalLib.ApplicationLayer.DTOs
{
    public class DriverDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
    }
}
