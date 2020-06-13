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
        private PositionViewModel _carStartingPosition;
        private decimal _outOfBondsPenaltyPerDistanceUnit;
        public Guid Id { get; set; }
        public string Name
        {
            get => _name;
            set { Set(() => Name, ref _name, value); }
        }
        public List<PositionViewModel> Area { get; set; }
        public decimal OutOfBondsPenaltyPerDistanceUnit
        {
            get => _outOfBondsPenaltyPerDistanceUnit;
            set { Set(() => OutOfBondsPenaltyPerDistanceUnit, ref _outOfBondsPenaltyPerDistanceUnit, value); }
        }
        public PositionViewModel CarStartingPosition
        {
            get => _carStartingPosition;
            set { Set(() => CarStartingPosition, ref _carStartingPosition, value); }
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
