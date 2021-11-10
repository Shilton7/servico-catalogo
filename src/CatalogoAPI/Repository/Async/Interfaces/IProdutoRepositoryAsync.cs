using CatalogoAPI.Configuration.Pagination.Async;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using System.Threading.Tasks;

namespace CatalogoAPI.Repository.Async.Interfaces
{
    public interface IProdutoRepositoryAsync : IRepositoryAsync<Produto>
    {
        Task<PagedListAsync<Produto>> GetProdutosPaginateAsync(ProdutosParameters produtosParameters);
    }
}
