using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class RentalAreaViewModel:ObservableObject, IDataErrorInfo
    {
        public RentalAreaViewModel()
        {
            Area = new ObservableCollection<PositionViewModel>();
        }
        private static readonly string[] ValidatedProperties =
        {
            nameof(Name),
            nameof(OutOfBondsPenaltyPerDistanceUnit)
        };
        private string _name;
        private PositionViewModel _carStartingPosition;
        private string _outOfBondsPenaltyPerDistanceUnit;
        private ObservableCollection<PositionViewModel> _area;
        public Guid Id { get; set; }
        public string Name
        {
            get => _name;
            set { Set(() => Name, ref _name, value); }
        }

        public ObservableCollection<PositionViewModel> Area
        {
            get => _area;
            set { Set(() => Area, ref _area, value); }
        }
        public string OutOfBondsPenaltyPerDistanceUnit
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
        public bool IsValid => ValidatedProperties.All(property => GetValidationError(property) == null) && Area.ToList().TrueForAll((position => position.IsValid)) && Area.Count>=3;

        public string this[string columnName]
        {
            get
            {
                var result = GetValidationError(columnName);
                return result;
            }
        }

        private string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case nameof(Name):
                    if (string.IsNullOrEmpty(Name)) error = "Name cannot be empty!";
                    break;
                case nameof(OutOfBondsPenaltyPerDistanceUnit):
                    if (!double.TryParse(OutOfBondsPenaltyPerDistanceUnit, out _)) error = "Enter valid number";
                    break;
            }

            return error;
        }

        public string Error { get; }
    }
}
