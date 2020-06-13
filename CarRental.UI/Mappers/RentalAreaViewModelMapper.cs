using System.Collections.Generic;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;

namespace CarRental.UI.Mappers
{
    public class RentalAreaViewModelMapper : IRentalAreaViewModelMapper

    {
        private readonly IPositionVewModelMapper _positionVewModelMapper;

        public RentalAreaViewModelMapper(IPositionVewModelMapper positionVewModelMapper)
        {
            _positionVewModelMapper = positionVewModelMapper;
        }

        public RentalAreaViewModel Map(RentalAreaDTO rentalAreaDto)
        {
            var result = new RentalAreaViewModel
            {
                CarStartingPosition = _positionVewModelMapper.Map(rentalAreaDto.CarStartingPositionDTO),
                Area = new List<PositionViewModel>(),
                Id = rentalAreaDto.Id,
                OutOfBondsPenaltyPerDistanceUnit = rentalAreaDto.OutOfBondsPenaltyPerDistanceUnit,
                Name = rentalAreaDto.Name
            };
            foreach (var positionDto in rentalAreaDto.Area)
            {
                result.Area.Add(_positionVewModelMapper.Map(positionDto));
            }

            return result;
        }

        public RentalAreaDTO Map(RentalAreaViewModel rentalArea)
        {
            var result = new RentalAreaDTO
            {
                CarStartingPositionDTO = _positionVewModelMapper.Map(rentalArea.CarStartingPosition),
                Area = new List<PositionDTO>(),
                Id = rentalArea.Id,
                OutOfBondsPenaltyPerDistanceUnit = rentalArea.OutOfBondsPenaltyPerDistanceUnit,
                Name = rentalArea.Name
            };
            foreach (var position in rentalArea.Area)
            {
                result.Area.Add(_positionVewModelMapper.Map(position));
            }

            return result;
        }
    }
}