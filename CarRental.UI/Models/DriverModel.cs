using System;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;

namespace CarRental.UI.Models
{
    public class DriverModel : ObservableObject, IDataErrorInfo
    {
        private static readonly string[] ValidatedProperties =
        {
            nameof(FirstName),
            nameof(LastName),
            nameof(LicenseNumber)
        };

        private string _firstName;
        private string _lastName;
        private string _licenseNumber;

        /// <summary>
        ///     Returns true if this object has no validation errors.
        /// </summary>
        public bool IsValid
        {
            get { return ValidatedProperties.All(property => GetValidationError(property) == null); }
        }

        public Guid Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set { Set(() => FirstName, ref _firstName, value); }
        }

        public string LastName
        {
            get => _lastName;
            set { Set(() => LastName, ref _lastName, value); }
        }

        public string LicenseNumber
        {
            get => _licenseNumber;
            set { Set(() => LicenseNumber, ref _licenseNumber, value); }
        }

        public string Error => throw new NotImplementedException();


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
                case nameof(FirstName):
                    if (string.IsNullOrEmpty(FirstName)) error = "First name cannot be empty!";
                    break;
                case nameof(LastName):
                    if (string.IsNullOrEmpty(LastName)) error = "Last name cannot be empty!";
                    break;
                case nameof(LicenseNumber):
                    if (string.IsNullOrEmpty(LicenseNumber)) error = "License number cannot be empty!";
                    break;
            }

            return error;
        }
    }
}