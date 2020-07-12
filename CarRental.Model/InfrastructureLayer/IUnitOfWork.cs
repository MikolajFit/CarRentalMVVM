using System;

namespace CarRental.Model.InfrastructureLayer
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void RejectChanges();
    }
}
