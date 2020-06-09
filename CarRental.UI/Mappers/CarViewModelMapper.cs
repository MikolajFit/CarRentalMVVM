using System.Globalization;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;

namespace CarRental.UI.Mappers
{
    public class CarViewModelMapper : ICarViewModelMapper
    {
 
        public CarViewModel Map(CarDTO car)
        {
            return new CarViewModel
            {
                Id = car.Id,
                CarStatus = car.Status,
                CurrentDistance = $"{car.CurrentDistance:0.00}",
                CurrentLatitude = car.CurrentLatitude.ToString(CultureInfo.InvariantCulture),
                CurrentLongitude = car.CurrentLongitude.ToString(CultureInfo.InvariantCulture),
                PricePerMinute = $"{car.PricePerMinute:0.00}",
                RegistrationNumber = car.RegistrationNumber,
                RentalAreaId = car.RentalAreaId,
                RentalAreaName = car.RentalAreaName,
                TotalDistance = $"{car.TotalDistance:0.00}"
            };
        }

        public CarDTO Map(RentalAreaDTO selectedRentalArea, CarViewModel carViewModel,
            bool provideCustomPosition = false)
        {
            var dto = new CarDTO
            {
                Id = carViewModel.Id,
                Status = carViewModel.CarStatus,
                CurrentDistance = double.Parse(carViewModel.CurrentDistance),
                PricePerMinute = decimal.Parse(carViewModel.PricePerMinute),
                RegistrationNumber = carViewModel.RegistrationNumber,
                RentalAreaId = selectedRentalArea.Id,
                RentalAreaName = selectedRentalArea.Name,
                TotalDistance = double.Parse(carViewModel.TotalDistance)
            };
            if (provideCustomPosition)
            {
                dto.CurrentLatitude = double.Parse(carViewModel.CurrentLatitude);
                dto.CurrentLongitude = double.Parse(carViewModel.CurrentLongitude);
            }
            else
            {
                dto.CurrentLatitude = selectedRentalArea.CarStartingPositionDTO.Latitude;
                dto.CurrentLongitude = selectedRentalArea.CarStartingPositionDTO.Longitude;
            }

            return dto;
        }
    }
}