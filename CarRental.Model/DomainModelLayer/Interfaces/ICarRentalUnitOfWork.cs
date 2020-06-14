using System;
using CarRental.Model.DomainModelLayer.Models;
using DDD.Base.DomainModelLayer.Interfaces;

namespace CarRental.Model.DomainModelLayer.Interfaces
{
    public interface ICarRentalUnitOfWork : IUnitOfWork, IDisposable
    {
        IRepository<Car> CarRepository { get; }
        IRepository<Driver> DriverRepository { get; }
        IRepository<Rental> RentalRepository { get; }
        IRepository<RentalArea> RentalAreaRepository { get; }
    }
}