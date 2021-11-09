using AutoMapper;
using CatalogoAPI.DTOs;
using CatalogoAPI.Models;

namespace CatalogoAPI.Configuration.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
        }
    }
}
