using System;
using System.Collections.ObjectModel;
using CarRental.UI.Mappers;
using CarRental.UI.Services;
using CarRental.UI.ViewModels.ObservableObjects;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class LoginViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;
        private readonly IDriverViewModelMapper _driverViewModelMapper;
        private readonly IMessengerService _messengerService;
        private DriverViewModel _selectedDriver;

        public LoginViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper,IMessengerService messengerService)
        {
            _driverService = driverService ?? throw new ArgumentNullException();
            _driverViewModelMapper = driverViewModelMapper ?? throw new ArgumentNullException(); ;
            _messengerService = messengerService ?? throw new ArgumentNullException();
            PopulateDriversListView();
            LoginCommand = new RelayCommand(
                NavigateToDriverMainView, IsDriverSelected);
            RegisterCommand = new RelayCommand(NavigateToRegisterView);
            AdminLoginCommand = new RelayCommand(NavigateToAdminMainView);
        }

        public ObservableCollection<DriverViewModel> Drivers { get; set; } =
            new ObservableCollection<DriverViewModel>();

        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand AdminLoginCommand { get; }

        public DriverViewModel SelectedDriver
        {
            get => _selectedDriver;
            set { Set(() => SelectedDriver, ref _selectedDriver, value); }
        }

        private bool IsDriverSelected()
        {
            return SelectedDriver != null;
        }

        private void PopulateDriversListView()
        {
            Drivers.Clear();
            var drivers = _driverService.GetAllDrivers();
            if (drivers == null) return;
            foreach (var driver in drivers)
            {
                Drivers.Add(_driverViewModelMapper.Map(driver));
            }
        }

        private void NavigateToAdminMainView()
        {
            _messengerService.Send(new NotificationMessage("GoToAdminMainView"));
        }

        private void NavigateToRegisterView()
        {
            _messengerService.Send(new NotificationMessage("GoToRegisterWindow"));
        }

        private void NavigateToDriverMainView()
        {
            _messengerService.Send(new NotificationMessage("GoToDriverMainWindow"));
            _messengerService.Send(SelectedDriver);
        }
    }
}