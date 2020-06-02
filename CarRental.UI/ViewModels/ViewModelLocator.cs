using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Navigation;
using CarRental.UI.Utils;
using CarRental.UI.Utils.Interfaces;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Mappers;
using DDD.CarRentalLib.ApplicationLayer.Services;
using DDD.CarRentalLib.DomainModelLayer.Factories;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Services;
using DDD.CarRentalLib.InfrastructureLayer;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            RegisterViewModels();
            RegisterServices();
            SeedInitialData();

            Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
            // Messenger.Default.Register<NotificationMessage>(this, NotifyUserMethod);
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<DriverRentalsViewModel>();
            SimpleIoc.Default.Register<DriverMainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<RentCarViewModel>();
            SimpleIoc.Default.Register<ActiveRentalSessionViewModel>();
        }

        private static void RegisterServices()
        {
            SimpleIoc.Default.Register<ITimerFactory,TimerFactory>();
            SimpleIoc.Default.Register<IDomainEventPublisher, SimpleEventPublisher>();
            SimpleIoc.Default.Register<ICarRentalUnitOfWork, MemoryCarRentalUnitOfWork>();
            SimpleIoc.Default.Register<PositionService>();
            SimpleIoc.Default.Register<DriverFactory>();
            SimpleIoc.Default.Register<CarFactory>();
            SimpleIoc.Default.Register<RentalAreaFactory>();
            SimpleIoc.Default.Register<RentalFactory>();
            SimpleIoc.Default.Register<CarMapper>();
            SimpleIoc.Default.Register<DriverMapper>();
            SimpleIoc.Default.Register<RentalAreaMapper>();
            SimpleIoc.Default.Register<RentalMapper>();
            SimpleIoc.Default.Register<IDriverService, DriverService>();
            SimpleIoc.Default.Register<ICarService, CarService>();
            SimpleIoc.Default.Register<IRentalService, RentalService>();
            SimpleIoc.Default.Register<IRentalAreaService, RentalAreaService>();
        }
        private static void SeedInitialData()
        {
            var carService = SimpleIoc.Default.GetInstance<ICarService>();
            var driverService = SimpleIoc.Default.GetInstance<IDriverService>();
            var rentalService = SimpleIoc.Default.GetInstance<IRentalService>();
            var rentalAreaService = SimpleIoc.Default.GetInstance<IRentalAreaService>();
            SeedData.SeedInitialData(driverService,carService,rentalService,rentalAreaService);
        }

        public DriverMainViewModel DriverMainViewModel => SimpleIoc.Default.GetInstance<DriverMainViewModel>();
        public ActiveRentalSessionViewModel ActiveRentalSessionViewModel => SimpleIoc.Default.GetInstance<ActiveRentalSessionViewModel>();
        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public DriverRentalsViewModel DriverRentalsViewModel => SimpleIoc.Default.GetInstance<DriverRentalsViewModel>();
        public RentCarViewModel RentCarViewModel => SimpleIoc.Default.GetInstance<RentCarViewModel>();


        private void NotifyUserMethod(NotificationMessage message)
        {
            switch (message.Notification)
            {
                case "Driver logged in!":
                    MessageBox.Show(message.Notification);
                    break;
                case "Stop Car Rental":
                    ResetActiveRentalSessionViewModel();
                    break;
            }
        }

        private void ResetActiveRentalSessionViewModel()
        {
            SimpleIoc.Default.Unregister<ActiveRentalSessionViewModel>();
            SimpleIoc.Default.Register<ActiveRentalSessionViewModel>();
        }

        public static void Cleanup()
        {
           
        }
    }
}
