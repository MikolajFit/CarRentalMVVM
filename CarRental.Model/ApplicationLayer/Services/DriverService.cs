using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Model.ApplicationLayer.DTOs;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.ApplicationLayer.Mappers;
using CarRental.Model.DomainModelLayer.Factories;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Models;
using CarRental.Model.DomainModelLayer.Policies;
using CarRental.Model.InfrastructureLayer;

namespace CarRental.Model.ApplicationLayer.Services
{
    public class DriverService : IDriverService
    {
        private readonly DriverFactory _driverFactory;
        private readonly DriverMapper _driverMapper;
        private readonly ICarRentalUnitOfWork _unitOfWork;

        public DriverService(DriverFactory driverFactory, ICarRentalUnitOfWork unitOfWork, DriverMapper driverMapper)
        {
            _driverFactory = driverFactory;
            _unitOfWork = unitOfWork;
            _driverMapper = driverMapper;
        }

        public void CreateDriver(DriverDTO driverDto)
        {
            var driver = _unitOfWork.DriverRepository.Find(d => d.LicenseNumber == driverDto.LicenseNumber)
                .FirstOrDefault();
            if (driver != null)
                throw new Exception($"Driver with {driverDto.LicenseNumber} license number already exists.");
            driver = _driverFactory.Create(driverDto.Id, driverDto.LicenseNumber, driverDto.FirstName,
                driverDto.LastName);
            _unitOfWork.DriverRepository.Insert(driver);
            _unitOfWork.Commit();
        }

        public void UpdateDriver(DriverDTO driverDto)
        {
            var driver = _unitOfWork.DriverRepository.Find(d => d.Id == driverDto.Id)
                .FirstOrDefault();
            if (driver == null)
                throw new Exception($"Driver with {driverDto.Id} was not found");
            MapDtoToDriver(driverDto, driver);
            _unitOfWork.Commit();
        }

        public List<DriverDTO> GetAllDrivers()
        {
            var drivers = _unitOfWork.DriverRepository.GetAll();
            var result = _driverMapper.Map(drivers);
            return result;
        }

        private static void MapDtoToDriver(DriverDTO driverDto, Driver driver)
        {
            driver.FirstName = driverDto.FirstName;
            driver.LastName = driverDto.LastName;
            driver.LicenseNumber = driverDto.LicenseNumber;
            driver.DriverStatus = driverDto.DriverStatus;
            if (driverDto.FreeMinutesPolicy == PoliciesEnum.Standard &&
                driver.FreeMinutesPolicy.PolicyType != PoliciesEnum.Standard)
            {
                driver.RegisterPolicy(new StandardFreeMinutesPolicy());
            }
            else if (driverDto.FreeMinutesPolicy == PoliciesEnum.Vip &&
                     driver.FreeMinutesPolicy.PolicyType != PoliciesEnum.Vip)
            {
                driver.RegisterPolicy(new VipFreeMinutesPolicy());
            }
        }
    }
}