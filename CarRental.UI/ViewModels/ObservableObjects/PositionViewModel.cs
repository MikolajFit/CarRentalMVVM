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

        public string this[string columnName] => throw new NotImplementedException();

        public string Error { get; }
    }
}
