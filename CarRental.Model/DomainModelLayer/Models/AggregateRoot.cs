using System;
using DDD.Base.DomainModelLayer.Models;

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