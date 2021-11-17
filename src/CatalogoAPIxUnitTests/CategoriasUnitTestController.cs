using AutoMapper;
using CatalogoAPI.Configuration.AutoMapper;
using CatalogoAPI.Context;
using CatalogoAPI.Controllers.V2;
using CatalogoAPI.DTOs;
using CatalogoAPI.Repository;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;

namespace CatalogoAPIxUnitTests
{
    public class CategoriasUnitTestController
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }
        public static string mySqlConnectionTestStr =
            "Server=localhost;Port=3306;DataBase=DBCatalogo;User=root;Password=shilton@70;Persist Security Info=False;Connect Timeout=300";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseMySql(mySqlConnectionTestStr, ServerVersion.AutoDetect(mySqlConnectionTestStr))
               .Options;
        }

        public CategoriasUnitTestController()
        {
            var configMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = configMapper.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            /* Caso fosse utilizar outro banco de dados mocado */
                //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
                //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        [Fact]
        public void GetCategorias_Return_OKResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert 
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }
            
    }

}
