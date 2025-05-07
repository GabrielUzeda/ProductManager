using AutoMapper;
using ProductManager.Application.DTOs;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            // Adicione outros mapeamentos aqui conforme necessário
        }
    }
}
