using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.ApplicationLayer.Mappers
{
    public class RentalMapper
    {
        public List<RentalDTO> Map(IEnumerable<Rental> rentals)
        {
            return rentals.Select(r => Map(r)).ToList();
        }

        public RentalDTO Map(Rental rental)
        {
            return new RentalDTO()
            {
                Id = rental.Id,
                CarId = rental.CarId,
                DriverId = rental.DriverId,
                StartDateTime = rental.StartDateTime,
                StopDateTime = rental.StopDateTime,
                Total = rental.Total.Amount
            };
        }
    }
}
