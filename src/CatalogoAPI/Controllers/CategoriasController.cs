using CatalogoAPI.Context;
using CatalogoAPI.Filters;
using CatalogoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAsync()
        {
            try
            {
                var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
                return Ok(categorias);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias");
            }

        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriasProdutosAsync()
        {
            try
            {
                var categoria = await _context.Categorias.Include(c => c.Produtos).ToListAsync();
                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias juntamente com os produtos.");
            }
            
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoriaPorId")]
        public async Task<ActionResult<Categoria>> GetByIdAsync(int id)
        {
            var categoria =  await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [HttpGet("produtos/{id:int:min(1)}", Name = "ObterCategoriaProdutoPorId")]
        public async Task<ActionResult<Categoria>> GetCategoriaProdutoByIdAsync(int id)
        {
            var categoria = await _context.Categorias.Include(c => c.Produtos).FirstOrDefaultAsync(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            return Ok(categoria);
        }

        [ServiceFilter(typeof(ApiLoggingFilter))]
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Categoria categoriaRequest)
        {
            try
            {
               await _context.Categorias.AddAsync(categoriaRequest);
               await _context.SaveChangesAsync();

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
        public async Task<ActionResult> PutAsync(int id, [FromBody] Categoria categoriaRequest)
        {
            try
            {
                if (id != categoriaRequest.CategoriaId) return BadRequest();

                var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.CategoriaId == id);
                if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

                _context.Entry(categoriaRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok($"Categoria com id ({id}) foi atualizada com sucesso!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao atualizar esta categoria.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.CategoriaId == id);
                if (categoria == null) return NotFound($"A categoria com id={id} não foi encontrada.");

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();

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
