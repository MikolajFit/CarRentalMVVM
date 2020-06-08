using CarRental.UI.ViewModels.ObservableObjects;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels.DriverViewModels
{
    public abstract class AssignedDriverViewModelBase : CustomViewModelBase
    {
        private DriverViewModel _currentDriver;

        protected AssignedDriverViewModelBase()
        {
            Messenger.Default.Register<DriverViewModel>(this, AssignLoggedInDriver);
        }

        public DriverViewModel CurrentDriver
        {
            get => _currentDriver;
            set { Set(() => CurrentDriver, ref _currentDriver, value); }
        }

        public virtual void AssignLoggedInDriver(DriverViewModel driverViewModel)
        {
            CurrentDriver = driverViewModel;
        }
    }
}