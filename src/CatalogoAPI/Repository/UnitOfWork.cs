using CatalogoAPI.Context;
using CatalogoAPI.Repository.Interfaces;

namespace CatalogoAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private CategoriaRepository _categoriaRepo;
        private AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepo = _categoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
