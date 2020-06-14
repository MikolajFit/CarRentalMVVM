using System;

namespace DDD.Base.DomainModelLayer.Models
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity(Guid id)
        {
            this.Id = id;
        }
    }
}
