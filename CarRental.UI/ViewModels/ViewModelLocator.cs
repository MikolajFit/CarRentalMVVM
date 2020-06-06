using CarRental.UI.Models;
using CarRental.UI.Services;
using CarRental.UI.Utils;
using CarRental.UI.Utils.Interfaces;
using CarRental.UI.ViewModels.DriverViewModels;
using DDD.Base.DomainModelLayer.Events;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Mappers;
using DDD.CarRentalLib.ApplicationLayer.Services;
using DDD.CarRentalLib.DomainModelLayer.Factories;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Services;
using DDD.CarRentalLib.InfrastructureLayer;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;

namespace CarRental.UI.ViewModels
{
    public class ViewModelLocator
    {
        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            RegisterViewModels();
            RegisterServices();
            SeedInitialData();
        }

        public DriverMainViewModel DriverMainViewModel => SimpleIoc.Default.GetInstance<DriverMainViewModel>();

        public ActiveRentalSessionViewModel ActiveRentalSessionViewModel =>
            SimpleIoc.Default.GetInstance<ActiveRentalSessionViewModel>();

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        public DriverRentalsViewModel DriverRentalsViewModel => SimpleIoc.Default.GetInstance<DriverRentalsViewModel>();
        public RentCarViewModel RentCarViewModel => SimpleIoc.Default.GetInstance<RentCarViewModel>();
        public DriverAccountViewModel DriverAccountViewModel => SimpleIoc.Default.GetInstance<DriverAccountViewModel>();
        public RegisterDriverViewModel RegisterDriverViewModel => SimpleIoc.Default.GetInstance<RegisterDriverViewModel>();

        public static void RegisterViewModels()
        { 
            SimpleIoc.Default.Register<DriverRentalsViewModel>();
            SimpleIoc.Default.Register<DriverMainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<RentCarViewModel>();
            SimpleIoc.Default.Register<ActiveRentalSessionViewModel>();
            SimpleIoc.Default.Register<DriverAccountViewModel>();
            SimpleIoc.Default.Register<RegisterDriverViewModel>();
        }

        public static void RegisterServices()
        {
            SimpleIoc.Default.Register<ITimerFactory, TimerFactory>();
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
            SimpleIoc.Default.Register<IMessengerService, MessengerService>();
        }

        private static void SeedInitialData()
        {
            var carService = SimpleIoc.Default.GetInstance<ICarService>();
            var driverService = SimpleIoc.Default.GetInstance<IDriverService>();
            var rentalService = SimpleIoc.Default.GetInstance<IRentalService>();
            var rentalAreaService = SimpleIoc.Default.GetInstance<IRentalAreaService>();
            SeedData.SeedInitialData(driverService, carService, rentalService, rentalAreaService);
        }


        public static void Cleanup()
        {
            Messenger.Default.Send(new CleanupMessage("Cleanup"));
            UnregisterViewModels();
            RegisterViewModels();
        }

        private static void UnregisterViewModels()
        {
            SimpleIoc.Default.Unregister<DriverRentalsViewModel>();
            SimpleIoc.Default.Unregister<DriverMainViewModel>();
            SimpleIoc.Default.Unregister<LoginViewModel>();
            SimpleIoc.Default.Unregister<RentCarViewModel>();
            SimpleIoc.Default.Unregister<ActiveRentalSessionViewModel>();
            SimpleIoc.Default.Unregister<DriverAccountViewModel>();
            SimpleIoc.Default.Unregister<RegisterDriverViewModel>();
        }
    }
}