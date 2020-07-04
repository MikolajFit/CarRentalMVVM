using System;

namespace CarRental.Model.DomainModelLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RejectChanges();
    }
}
