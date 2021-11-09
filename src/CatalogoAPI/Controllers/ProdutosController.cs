using AutoMapper;
using CatalogoAPI.DTOs;
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
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
                var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

                return Ok(produtosDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao listar os produtos");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProdutoPorId")]
        public ActionResult<ProdutoDTO> GetById(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto com o id({id}) informado não existe.");

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return Ok(produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao buscar o produto.");
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdutoDTO produtoRequest)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoRequest);

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProdutoPorId", new { id = produtoDTO.ProdutoId },
                    produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar criar o produto.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoRequest)
        {
            try
            {
                if (id != produtoRequest.ProdutoId) return BadRequest();

                var produtoExists = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produtoExists == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                var produto = _mapper.Map<Produto>(produtoRequest);
                _uof.ProdutoRepository.Update(produto);
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
