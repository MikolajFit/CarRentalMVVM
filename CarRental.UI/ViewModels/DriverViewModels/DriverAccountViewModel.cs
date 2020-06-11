using System;
using CarRental.UI.Mappers;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public class DriverAccountViewModel : AssignedDriverViewModelBase
    {
        private readonly IDriverService _driverService;
        private readonly IDriverViewModelMapper _driverViewModelMapper;
        private bool _isEditable;
        private string _errorString;

        public DriverAccountViewModel(IDriverService driverService, IDriverViewModelMapper driverViewModelMapper)
        {
            _driverService = driverService ??throw new ArgumentNullException();
            _driverViewModelMapper = driverViewModelMapper ?? throw new ArgumentNullException();
            ChangeToEditModeCommand = new RelayCommand(ChangeToEditMode);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
        }


        public bool IsEditable
        {
            get => _isEditable;
            set { Set(() => IsEditable, ref _isEditable, value); }
        }

        public string ErrorString
        {
            get => _errorString;
            set { Set(() => ErrorString, ref _errorString, value); }
        }

        public RelayCommand ChangeToEditModeCommand { get; }
        public RelayCommand SaveChangesCommand { get; }

        public bool CanSaveChanges()
        {
            return IsEditable && CurrentDriver.IsValid;
        }


        private void SaveChanges()
        {
            try
            {
                var driverDto = _driverViewModelMapper.Map(CurrentDriver);
                _driverService.UpdateDriver(driverDto);
                IsEditable = false;
                ErrorString = null;
            }
            catch (Exception)
            {
                ErrorString = "Could not save changes.";
            }
        }

        private void ChangeToEditMode()
        {
            IsEditable = true;
        }
    }
}