using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository.Async.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogoAPI.Controllers
{
    [Route("api/produtos")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWorkAsync _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWorkAsync context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAsync([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                var produtos = await _uof.ProdutoRepositoryAsync.GetProdutosPaginateAsync(produtosParameters);
                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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
        public async Task<ActionResult<ProdutoDTO>> GetByIdAsync(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepositoryAsync.GetByIdAsync(p => p.ProdutoId == id);
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
        public async Task<ActionResult> PostAsync([FromBody] ProdutoDTO produtoRequest)
        {
            try
            {
                var produto = _mapper.Map<Produto>(produtoRequest);

                _uof.ProdutoRepositoryAsync.AddAsync(produto);
                await _uof.CommitAsync();

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
        public async Task<ActionResult> PutAsync(int id, [FromBody] ProdutoDTO produtoRequest)
        {
            try
            {
                if (id != produtoRequest.ProdutoId) return BadRequest();

                var produtoExists = await _uof.ProdutoRepositoryAsync.GetByIdAsync(p => p.ProdutoId == id);
                if (produtoExists == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                var produto = _mapper.Map<Produto>(produtoRequest);
                _uof.ProdutoRepositoryAsync.UpdateAsync(produto);
                await _uof.CommitAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar atualizar este produto.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var produto = await _uof.ProdutoRepositoryAsync.GetByIdAsync(p => p.ProdutoId == id);
                if (produto == null) return NotFound($"O produto de id({id}) não foi encontrado.");

                _uof.ProdutoRepositoryAsync.RemoveAsync(produto);
                await _uof.CommitAsync();

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
