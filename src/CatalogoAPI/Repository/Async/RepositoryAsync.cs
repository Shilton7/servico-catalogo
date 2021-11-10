using CatalogoAPI.Context;
using CatalogoAPI.Repository.Async.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CatalogoAPI.Repository.Async
{
    public abstract class RepositoryAsync<T> : IRepositoryAsync<T> where T : class
    {
        private readonly AppDbContext _context;

        public RepositoryAsync(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAsync()
        {
            //Método Set<T> retorna uma instãncia DbSet<t> para acesso a entidade X
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().SingleOrDefaultAsync(predicate);
        }

        public void AddAsync(T entity)
        {
            _context.Set<T>().AddAsync(entity);
        }

        public void RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
        }

    }
}
