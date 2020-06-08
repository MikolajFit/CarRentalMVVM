using System;
using System.Collections.Generic;
using System.ComponentModel;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using GalaSoft.MvvmLight;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class RentalAreaViewModel:ObservableObject, IDataErrorInfo
    {
        private string _name;
        private PositionDTO _carStartingPositionDTO;
        private decimal _outOfBondsPenaltyPerDistanceUnit;
        public Guid Id { get; set; }
        public string Name
        {
            get => _name;
            set { Set(() => Name, ref _name, value); }
        }
        public List<PositionDTO> Area { get; set; }
        public decimal OutOfBondsPenaltyPerDistanceUnit
        {
            get => _outOfBondsPenaltyPerDistanceUnit;
            set { Set(() => OutOfBondsPenaltyPerDistanceUnit, ref _outOfBondsPenaltyPerDistanceUnit, value); }
        }
        public PositionDTO CarStartingPositionDTO
        {
            get => _carStartingPositionDTO;
            set { Set(() => CarStartingPositionDTO, ref _carStartingPositionDTO, value); }
        }
        public override string ToString()
        {
            return Name;
        }
        public string this[string columnName]
        {
            get { return null; }
        }

        public string Error { get; }
    }
}
