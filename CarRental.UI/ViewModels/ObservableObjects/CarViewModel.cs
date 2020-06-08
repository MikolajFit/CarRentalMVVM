using System;
using System.ComponentModel;
using System.Linq;
using DDD.CarRentalLib.DomainModelLayer.Models;
using GalaSoft.MvvmLight;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class CarViewModel : ObservableObject, IDataErrorInfo
    {
        private static readonly string[] ValidatedProperties =
        {
            nameof(RegistrationNumber),
            nameof(PricePerMinute),
            nameof(TotalDistance)
        };

        private CarStatus _carStatus;
        private string _currentDistance = "0";
        private string _currentLatitude;
        private string _currentLongitude;
        private string _pricePerMinute;
        private string _registrationNumber;
        private string _rentalAreaName;
        private string _totalDistance;

        public bool IsValid
        {
            get { return ValidatedProperties.All(property => GetValidationError(property) == null); }
        }

        public Guid Id { get; set; }
        public Guid RentalAreaId { get; set; }

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set { Set(() => RegistrationNumber, ref _registrationNumber, value); }
        }

        public string CurrentLatitude
        {
            get => _currentLatitude;
            set { Set(() => CurrentLatitude, ref _currentLatitude, value); }
        }

        public string CurrentLongitude
        {
            get => _currentLongitude;
            set { Set(() => CurrentLongitude, ref _currentLongitude, value); }
        }

        public string CurrentDistance
        {
            get => _currentDistance;
            set { Set(() => CurrentDistance, ref _currentDistance, value); }
        }

        public string TotalDistance
        {
            get => _totalDistance;
            set { Set(() => TotalDistance, ref _totalDistance, value); }
        }

        public CarStatus CarStatus
        {
            get => _carStatus;
            set { Set(() => CarStatus, ref _carStatus, value); }
        }

        public string PricePerMinute
        {
            get => _pricePerMinute;
            set { Set(() => PricePerMinute, ref _pricePerMinute, value); }
        }

        public string RentalAreaName
        {
            get => _rentalAreaName;
            set { Set(() => RentalAreaName, ref _rentalAreaName, value); }
        }


        public string this[string columnName]
        {
            get
            {
                var result = GetValidationError(columnName);
                return result;
            }
        }

        public string Error { get; }

        private string GetValidationError(string propertyName)
        {
            string error = null;

            switch (propertyName)
            {
                case nameof(RegistrationNumber):
                    if (string.IsNullOrEmpty(RegistrationNumber)) error = "Registration number cannot be empty!";
                    break;
                case nameof(PricePerMinute):
                    if (!double.TryParse(PricePerMinute, out _)) error = "Enter valid price";
                    break;
                case nameof(TotalDistance):
                    if (!double.TryParse(TotalDistance, out _)) error = "Enter valid number";
                    break;
            }

            return error;
        }
    }
}