using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class RentalsManagementViewModel : CustomViewModelBase
    {
        private readonly IRentalService _rentalService;

        public RentalsManagementViewModel(IRentalService rentalService)
        {
            _rentalService = rentalService;
            PopulateRentalsCollection();
        }

        private void PopulateRentalsCollection()
        {
            _activeRentalCollection.Clear();
            var rentals = _rentalService.GetAllRentals();
            foreach (var rental in rentals)
            {
                _activeRentalCollection.Add(rental);
            }
        }

        private ObservableCollection<RentalDTO> _activeRentalCollection = new ObservableCollection<RentalDTO>();

        public ObservableCollection<RentalDTO> ActiveRentalsCollection
        {
            get => _activeRentalCollection;
            set { Set(() => ActiveRentalsCollection, ref _activeRentalCollection, value); }
        }
    }
}