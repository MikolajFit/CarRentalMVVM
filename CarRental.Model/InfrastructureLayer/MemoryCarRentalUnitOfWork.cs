using CarRental.Model.DomainModelLayer.Interfaces;
using CarRental.Model.DomainModelLayer.Models;
using DDD.Base.DomainModelLayer.Interfaces;
using GalaSoft.MvvmLight.Ioc;

namespace CarRental.Model.InfrastructureLayer
{
    public class MemoryCarRentalUnitOfWork : ICarRentalUnitOfWork
    {
        public MemoryCarRentalUnitOfWork(IRepository<Car> carRepository, IRepository<Driver> driverRepository,
            IRepository<Rental> rentalRepository, IRepository<RentalArea> rentalAreaRepository)
        {
            CarRepository = carRepository;
            DriverRepository = driverRepository;
            RentalRepository = rentalRepository;
            RentalAreaRepository = rentalAreaRepository;
        }

        [PreferredConstructor]
        public MemoryCarRentalUnitOfWork()
        {
            CarRepository = new MemoryRepository<Car>();
            DriverRepository = new MemoryRepository<Driver>();
            RentalRepository = new MemoryRepository<Rental>();
            RentalAreaRepository = new MemoryRepository<RentalArea>();
        }

        public IRepository<RentalArea> RentalAreaRepository { get; protected set; }
        public IRepository<Car> CarRepository { get; protected set; }
        public IRepository<Driver> DriverRepository { get; protected set; }
        public IRepository<Rental> RentalRepository { get; protected set; }

        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        public void RejectChanges()
        {
        }
    }
}