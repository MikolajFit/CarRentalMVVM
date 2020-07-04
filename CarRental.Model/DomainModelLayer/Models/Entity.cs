using System;

namespace CarRental.Model.DomainModelLayer.Models
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
