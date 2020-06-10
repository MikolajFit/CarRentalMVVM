using System.Collections.ObjectModel;
using CarRental.UI.Mappers;
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
        private DriverViewModel _selectedDriver;

        public LoginViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper)
        {
            _driverService = driverService;
            _driverViewModelMapper = driverViewModelMapper;
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
            var drivers = _driverService.GetAllDrivers();
            Drivers.Clear();
            foreach (var driver in drivers)
            {
                Drivers.Add(_driverViewModelMapper.Map(driver));
            }
        }

        private static void NavigateToAdminMainView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToAdminMainView"));
        }

        private static void NavigateToRegisterView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToRegisterWindow"));
        }

        private void NavigateToDriverMainView()
        {
            Messenger.Default.Send(new NotificationMessage("GoToDriverMainWindow"));
            Messenger.Default.Send(SelectedDriver);
        }
    }
}