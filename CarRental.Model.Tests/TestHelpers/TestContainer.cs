using CarRental.Model.ApplicationLayer.Interfaces;
using CarRental.Model.ApplicationLayer.Mappers;
using CarRental.Model.ApplicationLayer.Services;
using CarRental.Model.DomainModelLayer.Factories;
using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Services;
using CarRental.Model.InfrastructureLayer;

namespace CarRental.Model.Tests.TestHelpers
{
    public class TestContainer
    {
        public TestContainer()
        {
            UnitOfWork = new MemoryCarRentalUnitOfWork();

            var carFactory = new CarFactory();
            var driverFactory = new DriverFactory(); 
            rentalFactory = new RentalFactory();
            var rentalAreaFactory = new RentalAreaFactory();

            PositionService = new PositionService(UnitOfWork);

            var carMapper = new CarMapper();
            var driverMapper = new DriverMapper();
            rentalMapper = new RentalMapper();
            var rentalAreaMapper = new RentalAreaMapper();

            CarService = new CarService(carFactory, UnitOfWork, carMapper);
            DriverService = new DriverService(driverFactory, UnitOfWork, driverMapper);
            RentalService = new RentalService(rentalFactory, UnitOfWork, rentalMapper, PositionService);
            RentalAreaService = new RentalAreaService(rentalAreaFactory,UnitOfWork,rentalAreaMapper);

        }

        public void ChangePositionService(PositionService positionService)
        {
            RentalService = new RentalService(rentalFactory,UnitOfWork,rentalMapper,positionService);
        }

        private RentalMapper rentalMapper;
        private RentalFactory rentalFactory;
        public ICarRentalUnitOfWork UnitOfWork { get; set; }
        public PositionService PositionService { get; set; }
        public IRentalService RentalService { get; set; }
        public IDriverService DriverService { get; set; }
        public ICarService CarService { get; set; }
        public IRentalAreaService RentalAreaService { get; set; }
    }
}