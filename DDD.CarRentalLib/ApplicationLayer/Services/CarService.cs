﻿using System;
using System.Collections.Generic;
using System.Linq;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Mappers;
using DDD.CarRentalLib.DomainModelLayer.Factories;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.ApplicationLayer.Services
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
            if(!rentalArea.IsInArea(car.CurrentPosition)) throw new Exception("Starting position of car cannot be outside of bonds!");
            _unitOfWork.CarRepository.Insert(car);
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
            if(!rentalArea.IsInArea(new Position(newPosition.Latitude,newPosition.Longitude))) throw new Exception("Position of car cannot be outside of bonds!");
            car.UpdatePosition(new Position(newPosition.Latitude,newPosition.Longitude));
            _unitOfWork.Commit();
        }

        public List<CarDTO> GetAllCars()
        {
            var cars = _unitOfWork.CarRepository.GetAll();
            var result = _carMapper.Map(cars);
            return result;
        }

        public List<CarDTO> GetFreeCars()
        {
            var cars = _unitOfWork.CarRepository.Find(car => car.Status == CarStatus.Free);
            var result = _carMapper.Map(cars);
            return result;
        }
    }
}