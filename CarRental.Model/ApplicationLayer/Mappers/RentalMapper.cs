using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.Mappers
{
    public class RentalMapper
    {
        public List<RentalDTO> Map(IEnumerable<Rental> rentals)
        {
            return rentals.Select(r => Map(r)).ToList();
        }

        public RentalDTO Map(Rental rental)
        {
            return new RentalDTO
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