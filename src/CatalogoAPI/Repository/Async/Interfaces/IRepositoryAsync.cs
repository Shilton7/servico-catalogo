using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CatalogoAPI.Repository.Async.Interfaces
{
    public interface IRepositoryAsync<T>
    {
        IQueryable<T> GetAsync();
        Task<T> GetByIdAsync(Expression<Func<T,bool>> predicate);
        void AddAsync(T entity);
        void RemoveAsync(T entity);
        void UpdateAsync(T entity);
    }

}
