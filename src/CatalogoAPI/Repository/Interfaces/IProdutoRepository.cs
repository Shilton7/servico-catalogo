using CatalogoAPI.Configuration.Pagination;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutosPaginate(ProdutosParameters produtosParameters);
    }
}
