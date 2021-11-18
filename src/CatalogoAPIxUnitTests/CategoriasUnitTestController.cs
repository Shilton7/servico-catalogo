using AutoMapper;
using CatalogoAPI.Configuration.AutoMapper;
using CatalogoAPI.Context;
using CatalogoAPI.Controllers.V2;
using CatalogoAPI.DTOs;
using CatalogoAPI.Repository;
using CatalogoAPI.Repository.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        [Fact]
        public void GetCategorias_Return_BadRequestResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert 
            Assert.IsType<BadRequestResult>(data.Result);
        }

        [Fact]
        public void GetCategorias_MatchResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert 
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var categorias = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal(1, categorias[0].CategoriaId);
            Assert.Equal("Celulares", categorias[0].Nome);

            Assert.Equal(4, categorias[3].CategoriaId);
            Assert.Equal("Saldão", categorias[3].Nome);
        }

        [Fact]
        public void GetCategoriaPorId_Return_OkObjectResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            int CategoriaId = 1;
            var data = controller.GetById(CategoriaId);
            Console.WriteLine(data);

            //Assert 
            Assert.IsType<OkObjectResult>(data.Result);
        }

        [Fact]
        public void GetCategoriaById_Return_NotFoundObjectResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var CategoriaId = 9999;

            //Act  
            var data = controller.GetById(CategoriaId);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }

        [Fact]
        public void PostCategoria_Return_CreatedAtRouteResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var categoriaRequest = new CategoriaDTO
            {
                Nome = "Brinquedos",
                ImagemUrl = "brinquedos.png"
            };

            //Act  
            var data = controller.Post(categoriaRequest);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
        }

    }

}
