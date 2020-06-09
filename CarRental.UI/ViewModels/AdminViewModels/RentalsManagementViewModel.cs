using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class RentalsManagementViewModel : CustomViewModelBase
    {
        private readonly IRentalService _rentalService;
        private readonly CollectionViewSource _rentalsCollection;

        public RentalsManagementViewModel(IRentalService rentalService)
        {
            _rentalService = rentalService;
            _rentalsCollection = new CollectionViewSource { Source = RentalsObservableCollection };
            _rentalsCollection.Filter += FilterActiveRentals;
            PopulateRentalsCollection();
        }

        private void FilterActiveRentals(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalDTO rental))
                e.Accepted = false;
            else if (IsActiveRentalsSelected && rental.StopDateTime.HasValue)
                e.Accepted = false;
        }

        private void PopulateRentalsCollection()
        {
            _rentalObservableCollection.Clear();
            var rentals = _rentalService.GetAllRentals();
            foreach (var rental in rentals)
            {
                _rentalObservableCollection.Add(rental);
            }
            _rentalsCollection.View.Refresh();
        }

        private ObservableCollection<RentalDTO> _rentalObservableCollection = new ObservableCollection<RentalDTO>();
        private bool _isActiveRentalsSelected;

        public ObservableCollection<RentalDTO> RentalsObservableCollection
        {
            get => _rentalObservableCollection;
            set { Set(() => RentalsObservableCollection, ref _rentalObservableCollection, value); }
        }
        public ICollectionView RentalsCollection => _rentalsCollection.View;

        public bool IsActiveRentalsSelected
        {
            get => _isActiveRentalsSelected;
            set
            {
                Set(() => IsActiveRentalsSelected, ref _isActiveRentalsSelected, value);
                _rentalsCollection.View.Refresh();
            }
        }
    }
}