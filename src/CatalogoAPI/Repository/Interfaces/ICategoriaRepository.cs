using CatalogoAPI.Configuration.Pagination;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;

namespace CatalogoAPI.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriasProdutos(CategoriasParameters produtosParameters);
        Categoria GetCategoriaProdutoById(int id);
    }
}
