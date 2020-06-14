using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.ApplicationLayer.Mappers;
using CarRental.Model.DomainModelLayer.Factories;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Models;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.Model.ApplicationLayer.Services
{
    public class CarService : ICarService
    {
        private readonly CarFactory _carFactory;
        private readonly CarMapper _carMapper;
        private readonly ICarRentalUnitOfWork _unitOfWork;

        public CarService(CarFactory carFactory,
            ICarRentalUnitOfWork unitOfWork, CarMapper carMapper)
        {
            _carMapper = carMapper;
            _unitOfWork = unitOfWork;
            _carFactory = carFactory;
        }

        public void CreateCar(CarDTO carDTO)
        {
            var car = _unitOfWork.CarRepository.Find(c => c.RegistrationNumber == carDTO.RegistrationNumber)
                .FirstOrDefault();
            if (car != null)
                throw new Exception($"Car with {carDTO.RegistrationNumber} registration number already exists.");
            var rentalArea = _unitOfWork.RentalAreaRepository.Find(r => r.Id == carDTO.RentalAreaId).FirstOrDefault();
            if (rentalArea == null)
                throw new Exception($"There is no rental area with {carDTO.RentalAreaId} id.");
            car = _carFactory.Create(carDTO.Id, carDTO.RegistrationNumber, carDTO.CurrentDistance, carDTO.TotalDistance,
                carDTO.CurrentLatitude, carDTO.CurrentLongitude, carDTO.PricePerMinute, carDTO.RentalAreaId);
            if (!rentalArea.IsInArea(car.CurrentPosition))
                throw new Exception("Starting position of car cannot be outside of bonds!");
            _unitOfWork.CarRepository.Insert(car);
            _unitOfWork.Commit();
        }

        public void UpdateCar(CarDTO carDTO)
        {
            var car = _unitOfWork.CarRepository.Find(c => c.Id == carDTO.Id)
                .FirstOrDefault();
            if (car == null)
                throw new Exception($"Car with {carDTO.Id} Id doesn't exists.");
            var rentalArea = _unitOfWork.RentalAreaRepository.Find(r => r.Id == carDTO.RentalAreaId).FirstOrDefault();
            if (rentalArea == null)
                throw new Exception($"There is no rental area with {carDTO.RentalAreaId} id.");
            MapDtoToCar(carDTO, car);
            if (!rentalArea.IsInArea(car.CurrentPosition))
                throw new Exception("Position of car cannot be outside of bonds!");
            _unitOfWork.Commit();
        }

        public void ReparkCar(Guid CarId, PositionDTO newPosition)
        {
            var car = _unitOfWork.CarRepository.Find(c => c.Id == CarId)
                .FirstOrDefault();
            if (car == null)
                throw new Exception($"Could not find car '{CarId}'.");
            var rentalArea = _unitOfWork.RentalAreaRepository.Find(r => r.Id == car.RentalAreaId).FirstOrDefault();
            if (rentalArea == null)
                throw new Exception($"There is no rental area with {car.RentalAreaId} id.");
            if (!rentalArea.IsInArea(new Position(newPosition.Latitude, newPosition.Longitude)))
                throw new Exception("Position of car cannot be outside of bonds!");
            car.UpdatePosition(new Position(newPosition.Latitude, newPosition.Longitude));
            _unitOfWork.Commit();
        }

        public List<CarDTO> GetAllCars()
        {
            var cars = _unitOfWork.CarRepository.GetAll();
            var result = _carMapper.Map(cars);
            foreach (var carDto in result)
            {
                var rentalArea = _unitOfWork.RentalAreaRepository.Get(carDto.RentalAreaId);
                carDto.RentalAreaName = rentalArea.Name;
            }
            return result;
        }

        public List<CarDTO> GetFreeCars()
        {
            var cars = _unitOfWork.CarRepository.Find(car => car.Status == CarStatus.Free);
            var result = _carMapper.Map(cars);
            foreach (var carDto in result)
            {
                var rentalArea = _unitOfWork.RentalAreaRepository.Get(carDto.RentalAreaId);
                carDto.RentalAreaName = rentalArea.Name;
            }
            return result;
        }

        private static void MapDtoToCar(CarDTO carDTO, Car car)
        {
            car.RentalAreaId = carDTO.RentalAreaId;
            car.RegistrationNumber = carDTO.RegistrationNumber;
            car.PricePerMinute = new Money(carDTO.PricePerMinute);
            car.TotalDistance = new Distance(carDTO.TotalDistance);
            car.CurrentDistance = new Distance(carDTO.CurrentDistance);
            car.CurrentPosition = new Position(carDTO.CurrentLatitude,carDTO.CurrentLongitude);
        }
    }
}