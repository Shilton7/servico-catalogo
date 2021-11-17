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
using System.Linq;

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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            try
            {
                var categorias = _uof.CategoriaRepository.Get().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias");
            }

        }

        [HttpGet("produtos")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categoria);

                return Ok(categoriasDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao listar as categorias juntamente com os produtos.");
            }

        }

        /// <summary>
        /// Obtem uma categoria pelo seu Id
        /// </summary>
        /// <param name="id">código da categoria</param>
        /// <returns>Um Objeto de categoria</returns>
        [HttpGet("{id:int:min(1)}", Name = "ObterCategoriaPorId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoriaDTO> GetById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        [HttpGet("produtos/{id:int:min(1)}", Name = "ObterCategoriaProdutoPorId")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CategoriaDTO> GetCategoriaProdutoById(int id)
        {
            var categoria = _uof.CategoriaRepository.GetCategoriaProdutoById(id);
            if (categoria == null) return NotFound("A categoria informada não foi encontrada.");

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDTO);
        }

        /// <summary>
        /// Inclui uma nova categoria
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/v2/categorias
        ///     {
        ///         "nome": "Celulares 2",
        ///         "imagemUrl": "http://123.jpg"
        ///     }
        /// </remarks>
        /// <param name="categoriaRequest">Um Objeto do tipo CategoriaDTO</param>
        /// <returns>O objeto Categoria foi incluido</returns>
        /// <remarks>Retorna um objeto Categoria incluído</remarks>
        [ServiceFilter(typeof(ApiLoggingFilter))]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(CategoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao atualizar esta categoria.");
            }
        }

        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string GlobalException()
        {
            throw new Exception("Testing the return global exception");
        }

    }
}
