using System;
using System.Linq;
using System.Linq.Expressions;

namespace CatalogoAPI.Repository.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetById(Expression<Func<T,bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
        
    }
}
