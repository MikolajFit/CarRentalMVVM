using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class RentalsManagementViewModel : CustomViewModelBase
    {
        private readonly IRentalService _rentalService;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private readonly CollectionViewSource _rentalsCollection;

        public RentalsManagementViewModel(IRentalService rentalService, IRentalViewModelMapper rentalViewModelMapper)
        {
            _rentalService = rentalService;
            _rentalViewModelMapper = rentalViewModelMapper;
            _rentalsCollection = new CollectionViewSource { Source = RentalsObservableCollection };
            _rentalsCollection.Filter += FilterActiveRentals;
            PopulateRentalsCollection();
        }

        private void FilterActiveRentals(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
                e.Accepted = false;
            else if (IsActiveRentalsSelected && rental.StopDateTime.HasValue)
                e.Accepted = false;
        }

        private void PopulateRentalsCollection()
        {
            _rentalObservableCollection.Clear();
            var rentals = _rentalService.GetAllRentals();
            foreach (var rentalViewModel in rentals.Select(rental => _rentalViewModelMapper.Map(rental)))
            {
                RentalsObservableCollection.Add(rentalViewModel);
            }
            _rentalsCollection.View.Refresh();
        }

        private ObservableCollection<RentalViewModel> _rentalObservableCollection = new ObservableCollection<RentalViewModel>();
        private bool _isActiveRentalsSelected;

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
    }
}