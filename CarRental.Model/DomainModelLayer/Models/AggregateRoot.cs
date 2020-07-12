using System;

namespace CarRental.Model.DomainModelLayer.Models
{
    public abstract class AggregateRoot : Entity,IAggregateRoot
    {
        protected AggregateRoot(Guid id)
            : base(id)
        {
        }
    }
}