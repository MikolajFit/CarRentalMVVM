using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.UI.Mappers;
using CarRental.UI.Messages;
using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverRentalsViewModel : AssignedDriverViewModelBase
    {
        private readonly CollectionViewSource _driverRentalsCollection;
        private readonly IRentalService _rentalService;
        private readonly IRentalViewModelMapper _rentalViewModelMapper;
        private ObservableCollection<RentalViewModel> _driverRentals;
        private DateTime? _selectedStartDateFrom;
        private DateTime? _selectedStartDateTo;
        private DateTime? _selectedStopDateFrom;
        private DateTime? _selectedStopDateTo;

        public DriverRentalsViewModel(IRentalService rentalService, IRentalViewModelMapper rentalViewModelMapper)
        {
            _rentalService = rentalService ?? throw new ArgumentNullException();
            _rentalViewModelMapper = rentalViewModelMapper ?? throw new ArgumentNullException();
            DriverRentals = new ObservableCollection<RentalViewModel>();
            _driverRentalsCollection = new CollectionViewSource {Source = DriverRentals};
            Messenger.Default.Register<RefreshRentalsMessage>(this, NewRentalAdded);
            ApplyFilters();
        }

        public ObservableCollection<RentalViewModel> DriverRentals
        {
            get => _driverRentals;
            set { Set(() => DriverRentals, ref _driverRentals, value); }
        }

        public ICollectionView DriverRentalsCollection => _driverRentalsCollection.View;

        public DateTime? SelectedStartDateFrom
        {
            get => _selectedStartDateFrom;
            set
            {
                Set(() => SelectedStartDateFrom, ref _selectedStartDateFrom, value);
                _driverRentalsCollection.View.Refresh();
            }
        }

        public DateTime? SelectedStartDateTo
        {
            get => _selectedStartDateTo;
            set
            {
                Set(() => SelectedStartDateTo, ref _selectedStartDateTo, value);
                _driverRentalsCollection.View.Refresh();
            }
        }

        public DateTime? SelectedStopDateFrom
        {
            get => _selectedStopDateFrom;
            set
            {
                Set(() => SelectedStopDateFrom, ref _selectedStopDateFrom, value);
                _driverRentalsCollection.View.Refresh();
            }
        }

        public DateTime? SelectedStopDateTo
        {
            get => _selectedStopDateTo;
            set
            {
                Set(() => SelectedStopDateTo, ref _selectedStopDateTo, value);
                _driverRentalsCollection.View.Refresh();
            }
        }

        private void ApplyFilters()
        {
            _driverRentalsCollection.Filter += FilterByStartDateTimeFrom;
            _driverRentalsCollection.Filter += FilterByStartDateTimeTo;
            _driverRentalsCollection.Filter += FilterByStopDateTimeFrom;
            _driverRentalsCollection.Filter += FilterByStopDateTimeTo;
        }

        public override void AssignLoggedInDriver(DriverViewModel driver)
        {
            base.AssignLoggedInDriver(driver);
            RefreshRentalList();
        }

        private void NewRentalAdded(RefreshRentalsMessage message)
        {
            RefreshRentalList();
        }

        private void RefreshRentalList()
        {
            var rentals = _rentalService.GetRentalsForDriver(CurrentDriver.Id);
            if (rentals == null) return;
            DriverRentals.Clear();
            foreach (var rentalViewModel in rentals.Select(rental => _rentalViewModelMapper.Map(rental)))
            {
                DriverRentals.Add(rentalViewModel);
            }
            _driverRentalsCollection.View.Refresh();
        }

        private void FilterByStartDateTimeFrom(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
            {
                e.Accepted = false;
            }
            else if (SelectedStartDateFrom != null && rental.StartDateTime < SelectedStartDateFrom.Value)
            {
                e.Accepted = false;
            }
        }

        private void FilterByStartDateTimeTo(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
            {
                e.Accepted = false;
            }
            else if (SelectedStartDateTo != null && rental.StartDateTime >SelectedStartDateTo.Value)
            {
                e.Accepted = false;
            }
        }

        private void FilterByStopDateTimeFrom(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
            {
                e.Accepted = false;
            }
            else if (rental.StopDateTime != null && SelectedStopDateFrom != null &&
                     rental.StopDateTime.Value.Date < SelectedStopDateFrom.Value.Date)
            {
                e.Accepted = false;
            }
        }

        private void FilterByStopDateTimeTo(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalViewModel rental))
            {
                e.Accepted = false;
            }
            else if (rental.StopDateTime != null && SelectedStopDateTo != null &&
                     rental.StopDateTime.Value.Date > SelectedStopDateTo.Value.Date)
            {
                e.Accepted = false;
            }
        }
    }
}