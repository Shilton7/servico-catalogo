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
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.AsNoTracking().ToList();
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao listar os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutoPorId")]
        public ActionResult<Produto> GetById(int id)
        {
            try
            {
                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto com o id({id} informado não existe.");

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao buscar o produto.");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Produto produtoRequest)
        {
            try
            {
                _context.Produtos.Add(produtoRequest);
                _context.SaveChanges();

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
        public ActionResult Put(int id, [FromBody] Produto produtoRequest)
        {
            try
            {
                if (id != produtoRequest.ProdutoId) return BadRequest();

                var produto = _context.Produtos.AsNoTracking().FirstOrDefault(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id} não foi encontrado.");

                _context.Entry(produtoRequest).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar atualizar este produto.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id} não foi encontrado.");

                _context.Produtos.Remove(produto);
                _context.SaveChanges();

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
