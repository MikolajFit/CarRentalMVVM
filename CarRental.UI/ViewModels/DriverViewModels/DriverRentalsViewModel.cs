using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using CarRental.UI.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverRentalsViewModel : CustomViewModelBase
    {
        private readonly IRentalService _rentalService;
        private DriverDTO _currentDriver;
        private ObservableCollection<RentalDTO> _driverRentals;
        private readonly CollectionViewSource _driverRentalsCollection;
        private DateTime? _selectedStartDateFrom;
        private DateTime? _selectedStartDateTo;
        private DateTime? _selectedStopDateFrom;
        private DateTime? _selectedStopDateTo;

        public DriverRentalsViewModel(IRentalService rentalService)
        {
            _rentalService = rentalService;
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
            Messenger.Default.Register<RefreshRentalsMessage>(this, NewRentalAdded);
            DriverRentals = new ObservableCollection<RentalDTO>();
            _driverRentalsCollection = new CollectionViewSource {Source = DriverRentals};
            _driverRentalsCollection.Filter += FilterByStartDateTimeFrom;
            _driverRentalsCollection.Filter += FilterByStartDateTimeTo;
            _driverRentalsCollection.Filter += FilterByStopDateTimeFrom;
            _driverRentalsCollection.Filter += FilterByStopDateTimeTo;
        }

        public DriverDTO CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }


        public ObservableCollection<RentalDTO> DriverRentals
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

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = driver;
            RefreshRentalList();
        }

        private void NewRentalAdded(RefreshRentalsMessage message)
        {
            RefreshRentalList();
        }

        private void RefreshRentalList()
        {
            var rentals = _rentalService.GetRentalsForDriver(CurrentDriver.Id);
            DriverRentals.Clear();
            foreach (var rentalDto in rentals) DriverRentals.Add(rentalDto);
            _driverRentalsCollection.View.Refresh();
        }

        private void FilterByStartDateTimeFrom(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalDTO rental))
                e.Accepted = false;
            else if (SelectedStartDateFrom != null && rental.StartDateTime < SelectedStartDateFrom.Value)
                e.Accepted = false;
        }

        private void FilterByStartDateTimeTo(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalDTO rental))
                e.Accepted = false;
            else if (SelectedStartDateTo != null && rental.StartDateTime > SelectedStartDateTo.Value)
                e.Accepted = false;
        }

        private void FilterByStopDateTimeFrom(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalDTO rental))
                e.Accepted = false;
            else if (rental.StopDateTime != null && SelectedStopDateFrom != null &&
                     rental.StopDateTime.Value.Date < SelectedStopDateFrom.Value.Date) e.Accepted = false;
        }

        private void FilterByStopDateTimeTo(object sender, FilterEventArgs e)
        {
            if (!(e.Item is RentalDTO rental))
                e.Accepted = false;
            else if (rental.StopDateTime != null && SelectedStopDateTo != null &&
                     rental.StopDateTime.Value.Date > SelectedStopDateTo.Value.Date) e.Accepted = false;
        }
    }
}