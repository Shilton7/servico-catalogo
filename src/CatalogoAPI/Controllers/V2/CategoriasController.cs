using AutoMapper;
using CatalogoAPI.Controllers.Base;
using CatalogoAPI.DTOs;
using CatalogoAPI.Filters;
using CatalogoAPI.Models;
using CatalogoAPI.Pagination;
using CatalogoAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CatalogoAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/v{version:apiVersion}/categorias")]
    public class CategoriasController : MainController
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
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
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetCategoriasProdutos(categoriasParameters);
                var metadata = new
                {
                    categoria.TotalCount,
                    categoria.PageSize,
                    categoria.CurrentPage,
                    categoria.TotalPages,
                    categoria.HasNext,
                    categoria.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriaDTO = _mapper.Map<IEnumerable<CategoriaDTO>> (categoria);

                return Ok(categoriaDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias juntamente com os produtos.");
            }

        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoriaPorId")]
        public ActionResult<CategoriaDTO> GetById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        [HttpGet("produtos/{id:int:min(1)}", Name = "ObterCategoriaProdutoPorId")]
        public ActionResult<CategoriaDTO> GetCategoriaProdutoById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetCategoriaProdutoById(id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        [ServiceFilter(typeof(ApiLoggingFilter))]
        [HttpPost]
        public ActionResult Post([FromBody] CategoriaDTO categoriaRequest)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaRequest);

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoriaPorId", new { id = categoriaDTO.CategoriaId },
                    categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao tentar criar a categoria.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult Put(int id, [FromBody] CategoriaDTO categoriaRequest)
        {
            try
            {
                
                if (id != categoriaRequest.CategoriaId) return BadRequest();

                var categoriaExists = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                if (categoriaExists == null) return NotFound("A categoria informada não foi encontrada.");

                var categoria = _mapper.Map<Categoria>(categoriaRequest);

                _uof.CategoriaRepository.Update(categoria);
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
