using System;
using DDD.Base.DomainModelLayer.Interfaces;
using DDD.CarRentalLib.DomainModelLayer.Models;

namespace DDD.CarRentalLib.DomainModelLayer.Interfaces
{
    public interface ICarRentalUnitOfWork : IUnitOfWork, IDisposable
    {
        IRepository<Car> CarRepository { get; }
        IRepository<Driver> DriverRepository { get; }
        IRepository<Rental> RentalRepository { get; }
        IRepository<RentalArea> RentalAreaRepository { get; }
    }
}