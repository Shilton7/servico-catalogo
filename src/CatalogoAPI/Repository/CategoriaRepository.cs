using CatalogoAPI.Context;
using CatalogoAPI.Models;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoAPI.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().AsNoTracking()
                        .Include(c => c.Produtos)
                        .OrderBy(c => c.Nome)
                        .ToList();
        }

        public Categoria GetCategoriaProdutoById(int id)
        {
            return Get().AsNoTracking()
                        .Include(c => c.Produtos)
                        .FirstOrDefault(c => c.CategoriaId == id);
        }

    }
}
