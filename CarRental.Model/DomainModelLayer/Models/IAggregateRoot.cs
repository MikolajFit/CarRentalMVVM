using System;

namespace CarRental.Model.DomainModelLayer.Models
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}
