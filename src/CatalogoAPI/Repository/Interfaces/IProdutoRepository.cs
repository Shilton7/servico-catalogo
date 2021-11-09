using CatalogoAPI.Models;
using CatalogoAPI.Parameters;
using System.Collections.Generic;

namespace CatalogoAPI.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPaginate(ProdutosParameters produtosParameters);
    }
}
