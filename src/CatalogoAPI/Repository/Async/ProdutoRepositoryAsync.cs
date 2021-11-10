using CatalogoAPI.Configuration.Pagination;
using CatalogoAPI.Configuration.Pagination.Async;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository.Async.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoAPI.Repository.Async
{
    public class ProdutoRepositoryAsync : RepositoryAsync<Produto>, IProdutoRepositoryAsync
    {

        public ProdutoRepositoryAsync(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedListAsync<Produto>> GetProdutosPaginateAsync(ProdutosParameters produtosParameters)
        {
            return await PagedListAsync<Produto>.ToPagedListAsync(GetAsync().OrderBy(on => on.ProdutoId),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }
    }

}
