using CatalogoAPI.Context;
using CatalogoAPI.Models;

namespace CatalogoAPIxUnitTests
{
    class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer()
        {
        }

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
            (new Categoria { CategoriaId = 1, Nome = "Categoria 1", ImagemUrl = "img-1.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 2, Nome = "Categoria 2", ImagemUrl = "img-2.jpg" });

            context.Categorias.Add
            (new Categoria { CategoriaId = 3, Nome = "Categoria 3", ImagemUrl = "img-3.jpg" });
        }

    }

}
