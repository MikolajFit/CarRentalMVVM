using System;

namespace DDD.Base.DomainModelLayer.Models
{
    public abstract class AggregateRoot : Entity,IAggregateRoot
    {
        protected AggregateRoot(Guid id)
            : base(id)
        {
        }
    }
}