using System;
using System.Collections.ObjectModel;
using CarRental.UI.Mappers;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace CarRental.UI.ViewModels.AdminViewModels
{
    public class DriversManagementViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;
        private readonly IDriverViewModelMapper _driverViewModelMapper;
        private ObservableCollection<DriverViewModel> _driversCollection;
        private string _saveErrorContent;
        private DriverViewModel _selectedDriver;

        public DriversManagementViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper)
        {
            _driverService = driverService ?? throw new ArgumentNullException();
            _driverViewModelMapper = driverViewModelMapper ?? throw new ArgumentNullException();
            DriversCollection = new ObservableCollection<DriverViewModel>();
            RefreshDriverListView();
            SaveDriverCommand = new RelayCommand(SaveDriver, IsDriverValid);
        }

        public ObservableCollection<DriverViewModel> DriversCollection
        {
            get => _driversCollection;
            set { Set(() => DriversCollection, ref _driversCollection, value); }
        }

        public DriverViewModel SelectedDriver
        {
            get => _selectedDriver;
            set { Set(() => SelectedDriver, ref _selectedDriver, value); }
        }

        public string SaveErrorContent
        {
            get => _saveErrorContent;
            set { Set(() => SaveErrorContent, ref _saveErrorContent, value); }
        }

        public RelayCommand SaveDriverCommand { get; }

        private void RefreshDriverListView()
        {
            var drivers = _driverService.GetAllDrivers();
            if (drivers == null) return;
            DriversCollection.Clear();
            foreach (var driver in drivers) DriversCollection.Add(_driverViewModelMapper.Map(driver));
        }

        private bool IsDriverValid()
        {
            return SelectedDriver != null && SelectedDriver.IsValid;
        }

        private void SaveDriver()
        {
            try
            {
                var driver = _driverViewModelMapper.Map(SelectedDriver);
                _driverService.UpdateDriver(driver);
                RefreshDriverListView();
                SaveErrorContent = null;
            }
            catch (Exception e)
            {
                SaveErrorContent = e.Message;
            }
        }
    }
}