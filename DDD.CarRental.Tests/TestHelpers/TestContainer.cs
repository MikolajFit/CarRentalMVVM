using DDD.Base.DomainModelLayer.Events;
using DDD.Base.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Interfaces;
using DDD.CarRentalLib.ApplicationLayer.Mappers;
using DDD.CarRentalLib.ApplicationLayer.Services;
using DDD.CarRentalLib.DomainModelLayer.Factories;
using DDD.CarRentalLib.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Services;
using DDD.CarRentalLib.InfrastructureLayer;

namespace DDD.CarRental.Tests.TestHelpers
{
    public class TestContainer
    {
        public TestContainer()
        {
            var domainEventPublisher = new SimpleEventPublisher();
            UnitOfWork = new MemoryCarRentalUnitOfWork();

            var carFactory = new CarFactory(domainEventPublisher);
            var driverFactory = new DriverFactory(domainEventPublisher); 
            rentalFactory = new RentalFactory(domainEventPublisher);
            var rentalAreaFactory = new RentalAreaFactory(domainEventPublisher);

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