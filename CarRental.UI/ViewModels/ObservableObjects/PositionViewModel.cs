using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class PositionViewModel:ObservableObject, IDataErrorInfo
    {
        private static readonly string[] ValidatedProperties =
        {
            nameof(Latitude),
            nameof(Longitude)
        };

        public bool IsValid
        {
            get { return ValidatedProperties.All(property => GetValidationError(property) == null); }
        }

        private string _longitude;
        private string _latitude;

        public string Longitude
        {
            get => _longitude;
            set { Set(() => Longitude, ref _longitude, value); }
        }

        public string Latitude
        {
            get => _latitude;
            set { Set(() => Latitude, ref _latitude, value); }
        }

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
            return GetPositionError(propertyName);
        }

        private string GetPositionError(string position)
        {
            if(position==nameof(Latitude)){
                if (double.TryParse(Latitude, out _))
                {
                    var decimalPlacesItems = Latitude.Split(',', '.');
                    if (decimalPlacesItems.Length==2 && decimalPlacesItems[1].Length == 6) return null;
                }

            }
            else
            {
                if (double.TryParse(Longitude, out _))
                {
                    var decimalPlacesItems = Longitude.Split(',', '.');
                    if (decimalPlacesItems.Length == 2 && decimalPlacesItems[1].Length == 6) return null;
                }
            }

            return "Provide value with 6 decimal places.";

        }

        public string Error { get; }
    }
}
