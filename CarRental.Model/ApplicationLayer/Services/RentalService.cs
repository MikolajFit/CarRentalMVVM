using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.ApplicationLayer.Mappers;
using CarRental.Model.DomainModelLayer.Factories;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Models;
using CarRental.Model.DomainModelLayer.Services;

namespace CarRental.Model.ApplicationLayer.Services
{
    public class RentalService : IRentalService
    {
        private readonly PositionService _positionService;
        private readonly RentalFactory _rentalFactory;
        private readonly RentalMapper _rentalMapper;
        private readonly ICarRentalUnitOfWork _unitOfWork;


        public RentalService(RentalFactory rentalFactory, ICarRentalUnitOfWork unitOfWork, RentalMapper rentalMapper,
            PositionService positionService)
        {
            _rentalFactory = rentalFactory;
            _unitOfWork = unitOfWork;
            _rentalMapper = rentalMapper;
            _positionService = positionService;
        }

        public void TakeCar(Guid rentalId, Guid carId, Guid driverId, DateTime startDateTime)
        {
            var car = _unitOfWork.CarRepository.Get(carId) ??
                      throw new Exception($"Could not find car '{carId}'.");
            var rentalArea = _unitOfWork.RentalAreaRepository.Find(r => r.Id == car.RentalAreaId).FirstOrDefault();
            if (rentalArea == null)
                throw new Exception($"There is no rental area with {car.RentalAreaId} id.");
            if (!rentalArea.IsInArea(car.CurrentPosition))
                throw new Exception("Starting position of car cannot be outside of bonds!");
            var driver = _unitOfWork.DriverRepository.Get(driverId) ??
                         throw new Exception($"Could not find driver '{driverId}'.");
            if (driver.DriverStatus == DriverStatus.Banned) throw new Exception("Driver is banned!");
            var rental = _rentalFactory.Create(rentalId, startDateTime, car, driver.Id);

            car.TakeCar();
            _unitOfWork.RentalRepository.Insert(rental);
            _unitOfWork.Commit();
        }

        public void ReturnCar(Guid rentalId, DateTime stopDateTime)
        {
            var rental = _unitOfWork.RentalRepository.Get(rentalId) ??
                         throw new Exception($"Could not find rental '{rentalId}'.");
            var car = _unitOfWork.CarRepository.Get(rental.CarId) ??
                      throw new Exception($"Could not find car '{rental.CarId}'.");
            var driver = _unitOfWork.DriverRepository.Get(rental.DriverId) ??
                         throw new Exception($"Could not find driver '{rental.DriverId}'.");
            var rentalArea = _unitOfWork.RentalAreaRepository.Get(car.RentalAreaId) ??
                             throw new Exception($"Could not find Rental Area '{car.RentalAreaId}'.");

            car.ExitCar();
            _positionService.UpdateCarPosition(car.Id);
            var outOfBondsPenalty = rentalArea.CalculateTotalPenalty(car);
            var totalMinutes = CalculateTotalMinutes(rental.StartDateTime, stopDateTime);
            var freeMinutes = driver.CalculateFreeMinutes(totalMinutes);
            rental.StopRental(stopDateTime, car.PricePerMinute, totalMinutes, freeMinutes, outOfBondsPenalty);
            _unitOfWork.Commit();
        }

        public RentalDTO GetRental(Guid rentalId)
        {
            var rental = _unitOfWork.RentalRepository.Find(r => r.Id == rentalId).FirstOrDefault();
            var result = _rentalMapper.Map(rental);
            AssignAdditionalValues(result);
            return result;
        }

        public List<RentalDTO> GetAllRentals()
        {
            var rentals = _unitOfWork.RentalRepository.GetAll();
            var result = MapRentalsToDtos(rentals);
            return result;
        }

        public RentalDTO GetActiveRentalForDriver(Guid driverId)
        {
            var rental = _unitOfWork.RentalRepository
                .Find(r => r.DriverId == driverId && r.StopDateTime.HasValue == false).FirstOrDefault();
            if (rental == null) return null;
            var result = _rentalMapper.Map(rental);
            AssignAdditionalValues(result);
            return result;
        }

        public List<RentalDTO> GetRentalsForDriver(Guid driverId)
        {
            var rentals = _unitOfWork.RentalRepository.Find(rental => rental.DriverId == driverId);
            var result = MapRentalsToDtos(rentals);
            return result;
        }


        private List<RentalDTO> MapRentalsToDtos(IList<Rental> rentals)
        {
            var result = _rentalMapper.Map(rentals);
            foreach (var r in result)
            {
                AssignAdditionalValues(r);
            }

            return result;
        }

        private void AssignAdditionalValues(RentalDTO rentalDto)
        {
            var driver = _unitOfWork.DriverRepository.Get(rentalDto.DriverId);
            var car = _unitOfWork.CarRepository.Get(rentalDto.CarId);
            rentalDto.DriverName = driver.FirstName + " " + driver.LastName;
            rentalDto.RegistrationNumber = car.RegistrationNumber;
            rentalDto.PricePerMinute = car.PricePerMinute.Amount;
        }

        private double CalculateTotalMinutes(in DateTime startDateTime, in DateTime stopDateTime)
        {
            return (stopDateTime - startDateTime).TotalMinutes;
        }
    }
}