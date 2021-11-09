using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Parameters;
using CatalogoAPI.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoAPI.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutosPaginate(ProdutosParameters produtosParameters)
        {
            return Get().OrderBy(p => p.Nome)
                        .Skip((produtosParameters.PageNumber -1) * produtosParameters.PageSize)
                        .Take(produtosParameters.PageSize)
                        .ToList();
        }
    }

}
