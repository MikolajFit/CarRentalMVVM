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
            _rentalFactory = new RentalFactory();
            var rentalAreaFactory = new RentalAreaFactory();

            var positionService = new PositionService(UnitOfWork);

            var carMapper = new CarMapper();
            var driverMapper = new DriverMapper();
            _rentalMapper = new RentalMapper();
            var rentalAreaMapper = new RentalAreaMapper();

            CarService = new CarService(carFactory, UnitOfWork, carMapper);
            DriverService = new DriverService(driverFactory, UnitOfWork, driverMapper);
            RentalService = new RentalService(_rentalFactory, UnitOfWork, _rentalMapper, positionService);
            RentalAreaService = new RentalAreaService(rentalAreaFactory,UnitOfWork,rentalAreaMapper);

        }

        public IRentalService RentalService { get; set; }
        public IDriverService DriverService { get; set; }
        public ICarService CarService { get; set; }
        public IRentalAreaService RentalAreaService { get; set; }

        private readonly RentalMapper _rentalMapper;
        private readonly RentalFactory _rentalFactory;
        public readonly ICarRentalUnitOfWork UnitOfWork;
        public void ChangePositionService(PositionService positionService)
        {
            RentalService = new RentalService(_rentalFactory, UnitOfWork, _rentalMapper, positionService);
        }
    }
}