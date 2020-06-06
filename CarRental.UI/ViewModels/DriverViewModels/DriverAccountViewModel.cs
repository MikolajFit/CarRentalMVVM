using System.Windows.Automation;
using CarRental.UI.Models;
using DDD.CarRentalLib.ApplicationLayer.DTOs;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverAccountViewModel : CustomViewModelBase
    {
        private readonly IDriverService _driverService;
        private DriverModel _currentDriver;
        private bool _isEditable;

        public DriverAccountViewModel(IDriverService driverService)
        {
            _driverService = driverService;
            Messenger.Default.Register<DriverDTO>(this, AssignLoggedInDriver);
            ChangeToEditModeCommand = new RelayCommand(ChangeToEditMode);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            LogoutCommand = new RelayCommand(Logout);
        }

        public bool CanSaveChanges()
        {
            return IsEditable && CurrentDriver.IsValid;
        }

        public DriverModel CurrentDriver
        {
            get => _currentDriver;
            set
            {
                Set(() => CurrentDriver, ref _currentDriver, value);
            }
        }

        public bool IsEditable
        {
            get => _isEditable;
            set { Set(() => IsEditable, ref _isEditable, value); }
        }

        public RelayCommand ChangeToEditModeCommand { get; }
        public RelayCommand SaveChangesCommand { get; }

        public RelayCommand LogoutCommand { get; }

        private void Logout()
        {
            Messenger.Default.Send(new NotificationMessage("Logout"));
        }

        private void SaveChanges()
        {
            _driverService.UpdateDriver(
                new DriverDTO()
                {
                    Id = CurrentDriver.Id,
                    FirstName = CurrentDriver.FirstName,
                    LastName = CurrentDriver.LastName,
                    LicenseNumber = CurrentDriver.LicenseNumber
                });
            IsEditable = false;
        }

        private void ChangeToEditMode()
        {
            IsEditable = true;
        }

        private void AssignLoggedInDriver(DriverDTO driver)
        {
            CurrentDriver = new DriverModel()
            {
                Id=driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                LicenseNumber = driver.LicenseNumber
            };
        }

        public override void Cleanup()
        {
            ViewModelLocator.Cleanup();
            base.Cleanup();
        }
    }
}