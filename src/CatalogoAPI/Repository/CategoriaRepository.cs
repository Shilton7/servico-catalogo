using CatalogoAPI.Configuration.Pagination;
using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CatalogoAPI.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategoriasProdutos(CategoriasParameters categoriasParameters)
        {
            return PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.CategoriaId),
                categoriasParameters.PageNumber, categoriasParameters.PageSize);

        }

        public Categoria GetCategoriaProdutoById(int id)
        {
            return Get().AsNoTracking()
                        .Include(c => c.Produtos)
                        .FirstOrDefault(c => c.CategoriaId == id);
        }

    }
}
