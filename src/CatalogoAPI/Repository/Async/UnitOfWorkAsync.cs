using CatalogoAPI.Context;
using CatalogoAPI.Repository.Async;
using CatalogoAPI.Repository.Async.Interfaces;
using System.Threading.Tasks;

namespace CatalogoAPI.Repository
{
    public class UnitOfWorkAsync : IUnitOfWorkAsync
    {
        private ProdutoRepositoryAsync _produtoRepoAsync;
        private AppDbContext _context;

        public UnitOfWorkAsync(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepositoryAsync ProdutoRepositoryAsync
        {
            get
            {
                return _produtoRepoAsync = _produtoRepoAsync ?? new ProdutoRepositoryAsync(_context);
            }
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
           await _context.DisposeAsync();
        }

    }
}
