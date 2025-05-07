using AutoMapper;
using ProductManager.Application.DTOs;
using ProductManager.Application.Features.Products.Commands.CreateProduct;
using ProductManager.Application.Features.Products.Commands.UpdateProduct;
using ProductManager.Domain.Entities;

namespace ProductManager.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de e para DTO
            CreateMap<Product, ProductDto>();
            
            // Mapeamento de comandos para entidade
            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
                
            CreateMap<UpdateProductCommand, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
