using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class RentalsManagementViewModel : CustomViewModelBase
    {
        private readonly CollectionViewSource _rentalsCollection;
        private readonly IRentalService _rentalService;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private bool _isActiveRentalsSelected;

        private ObservableCollection<RentalViewModel> _rentalObservableCollection;

        public RentalsManagementViewModel(IRentalService rentalService, IRentalViewModelMapper rentalViewModelMapper)
        {
            _rentalService = rentalService ?? throw new ArgumentNullException();
            _rentalViewModelMapper = rentalViewModelMapper ?? throw new ArgumentNullException();
            RentalsObservableCollection = new ObservableCollection<RentalViewModel>();
            _rentalsCollection = new CollectionViewSource
            {
                Source = RentalsObservableCollection
            };
            _rentalsCollection.Filter += FilterActiveRentals;
            RefreshRentalsCollection();
        }

        public ObservableCollection<RentalViewModel> RentalsObservableCollection
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

        private void FilterActiveRentals(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
                e.Accepted = false;
            else if (IsActiveRentalsSelected && rental.StopDateTime.HasValue)
                e.Accepted = false;
        }

        private void RefreshRentalsCollection()
        {
            
            var rentals = _rentalService.GetAllRentals();
            if (rentals == null) return;
            _rentalObservableCollection.Clear();
            foreach (var rentalViewModel in rentals.Select(rental => _rentalViewModelMapper.Map(rental)))
            {
                RentalsObservableCollection.Add(rentalViewModel);
            }

            _rentalsCollection.View.Refresh();
        }
    }
}