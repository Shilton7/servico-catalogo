using CatalogoAPI.Filters;
using CatalogoAPI.Models;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CatalogoAPI.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public CategoriasController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _uof.CategoriaRepository.Get();
                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias");
            }

        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetCategoriasProdutos();
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias juntamente com os produtos.");
            }

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoriaPorId")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [HttpGet("produtos/{id:int:min(1)}", Name = "ObterCategoriaProdutoPorId")]
        public ActionResult<Categoria> GetCategoriaProdutoById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetCategoriaProdutoById(id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [ServiceFilter(typeof(ApiLoggingFilter))]
        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoriaRequest)
        {
            try
            {
                _uof.CategoriaRepository.Add(categoriaRequest);
                _uof.Commit();

                return new CreatedAtRouteResult("ObterCategoriaPorId", new { id = categoriaRequest.CategoriaId },
                    categoriaRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar criar a categoria.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] Categoria categoriaRequest)
        {
            try
            {
                
                if (id != categoriaRequest.CategoriaId) return BadRequest();

                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria == null) return NotFound("A categoria informada não foi encontrada.");
                
                _uof.CategoriaRepository.Update(categoriaRequest);
                _uof.Commit();

                return Ok($"Categoria com id ({id}) foi atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao atualizar esta categoria.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoria == null) return NotFound($"A categoria com id={id} não foi encontrada.");

                _uof.CategoriaRepository.Remove(categoria);
                _uof.Commit();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar excluir essa categoria");
            }
        }

        [HttpGet("/test/global-exception")]
        public string GlobalException()
        {
            throw new Exception("Testing the return global exception");
        }

    }
}
