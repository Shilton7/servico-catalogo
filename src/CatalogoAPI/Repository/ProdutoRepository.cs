using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repository.Interfaces;

namespace CatalogoAPI.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

    }

}
