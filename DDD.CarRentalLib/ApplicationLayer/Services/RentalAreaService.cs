using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Mappers;
using DDD.CarRentalLib.DomainModelLayer.Factories;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.ApplicationLayer.Services
{
    public class RentalAreaService:IRentalAreaService
    {
        private readonly RentalAreaFactory _rentalAreaFactory;
        private readonly RentalAreaMapper _rentalAreaMapper;
        private readonly ICarRentalUnitOfWork _unitOfWork;

        public RentalAreaService(RentalAreaFactory rentalAreaFactory, ICarRentalUnitOfWork unitOfWork, RentalAreaMapper rentalAreaMapper)
        {
            _rentalAreaFactory = rentalAreaFactory;
            _unitOfWork = unitOfWork;
            _rentalAreaMapper = rentalAreaMapper;
        }
        public void CreateRentalArea(RentalAreaDTO rentalAreaDto)
        {
            var rentalArea = _unitOfWork.RentalAreaRepository.Find(d => d.Id == rentalAreaDto.Id)
                .FirstOrDefault();
            if (rentalArea != null)
                throw new Exception($"Rental Area with {rentalArea.Id} Id already exists.");
            rentalArea = _rentalAreaFactory.Create(rentalAreaDto.Id, rentalAreaDto.OutOfBondsPenaltyPerDistanceUnit,
                rentalAreaDto.Area.ToList(), rentalAreaDto.Name);
            if (rentalAreaDto.CarStartingPositionDTO != null)
                rentalArea.CarStartingPosition = new Position(rentalAreaDto.CarStartingPositionDTO.Latitude,rentalAreaDto.CarStartingPositionDTO.Longitude);

            _unitOfWork.RentalAreaRepository.Insert(rentalArea);
            _unitOfWork.Commit();
        }

        public List<RentalAreaDTO> GetAllRentalAreas()
        {
            var rentalAreas = _unitOfWork.RentalAreaRepository.GetAll();
            var result = _rentalAreaMapper.Map(rentalAreas);
            return result;
        }

        public RentalAreaDTO GetRentalArea(Guid id)
        {
            var rentalArea = _unitOfWork.RentalAreaRepository.Get(id);
            var result = _rentalAreaMapper.Map(rentalArea);
            return result;
        }
    }
}
