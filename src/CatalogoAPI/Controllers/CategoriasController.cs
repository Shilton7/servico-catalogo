using CatalogoAPI.Context;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoAPI.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _context.Categorias.AsNoTracking().ToList();
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
                var categoria = _context.Categorias.Include(c => c.Produtos).ToList();
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
            var categoria =  _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [HttpGet("produtos/{id:int:min(1)}", Name = "ObterCategoriaProdutoPorId")]
        public ActionResult<Categoria> GetCategoriaProdutoById(int id)
        {
            var categoria = _context.Categorias.Include(c => c.Produtos).FirstOrDefault(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoriaRequest)
        {
            try
            {
                _context.Categorias.Add(categoriaRequest);
                _context.SaveChanges();

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

                var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(c => c.CategoriaId == id);
                if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

                _context.Entry(categoriaRequest).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok($"Categoria com id ({id}) foi atualizada com sucesso!");
            }
            catch (Exception)
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
                var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
                if (categoria == null) return NotFound($"A categoria com id={id} não foi encontrada.");

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar excluir essa categoria");
            }
        }

    }
}
