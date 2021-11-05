using CatalogoAPI.Context;
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
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            try
            {
                var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao listar os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutoPorId")]
        public async Task<ActionResult<Produto>> GetByIdAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto com o id({id}) informado não existe.");

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao buscar o produto.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] Produto produtoRequest)
        {
            try
            {
                await _context.Produtos.AddAsync(produtoRequest);
                await _context.SaveChangesAsync();

                return new CreatedAtRouteResult("ObterProdutoPorId", new { id = produtoRequest.ProdutoId },
                    produtoRequest);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar criar o produto.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Produto produtoRequest)
        {
            try
            {
                if (id != produtoRequest.ProdutoId) return BadRequest();

                var produto = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                _context.Entry(produtoRequest).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar atualizar este produto.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar excluir esse produto.");
            }
        }

    }
}
