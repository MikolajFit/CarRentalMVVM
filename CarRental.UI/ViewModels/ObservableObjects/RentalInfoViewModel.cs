using System;
using GalaSoft.MvvmLight;

namespace CarRental.UI.ViewModels.ObservableObjects
{
    public class RentalInfoViewModel
    {
        public Guid RentalId { get; set; }
        public string SelectedCar { get; set; }
        public decimal PricePerMinute { get; set; }
    }
}
