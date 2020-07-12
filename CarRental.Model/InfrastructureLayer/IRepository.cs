using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CarRental.Model.DomainModelLayer.Models;

namespace CarRental.Model.InfrastructureLayer
{
    public interface IRepository<TEntity>
        where TEntity : AggregateRoot
    {
        TEntity Get(Guid id);
        IList<TEntity> GetAll();
        IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
    }
}
