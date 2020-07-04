using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CarRental.Model.DomainModelLayer.Models;
using DDD.Base.DomainModelLayer.Models;

namespace CarRental.Model.DomainModelLayer.Interfaces
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
