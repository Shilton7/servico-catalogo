using CatalogoAPI.Models;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CatalogoAPI.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
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
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
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
        public ActionResult Post([FromBody] Produto produtoRequest)
        {
            try
            {
                _uof.ProdutoRepository.Add(produtoRequest);
                _uof.Commit();

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

                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                _uof.ProdutoRepository.Update(produtoRequest);
                _uof.Commit();

                return Ok();
            }
            catch (Exception ex)
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
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                _uof.ProdutoRepository.Remove(produto);
                _uof.Commit();

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
