using CatalogoAPI.Configuration.Pagination;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository.Interfaces;
using System.Linq;

namespace CatalogoAPI.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Produto> GetProdutosPaginate(ProdutosParameters produtosParameters)
        {
            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }
    }

}
