using CatalogoAPI.Models;
using System.Collections.Generic;

namespace CatalogoAPI.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetCategoriasProdutos();
        Categoria GetCategoriaProdutoById(int id);

    }
}
